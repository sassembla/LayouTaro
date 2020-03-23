
using System.Collections.Generic;
using UnityEngine;

namespace UILayouTaro
{
    public static class BasicLayoutFunctions
    {

        public static void TextLayout(TextElement newTailTextElement, string contentText, RectTransform transform, float viewWidth, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents)
        {
            Debug.Assert(transform.pivot.x == 0 && transform.pivot.y == 1 && transform.anchorMin.x == 0 && transform.anchorMin.y == 1 && transform.anchorMax.x == 0 && transform.anchorMax.y == 1, "rectTransform for LayouTaro should set pivot to 0,1 and anchorMin 0,1 anchorMax 0,1.");
            var continueContent = false;

        NextLine:
            var textComponent = newTailTextElement.GetComponent<TMPro.TextMeshProUGUI>();

            // wordWrappingを可能にすると、表示はともかく実際にこの行にどれだけの文字が入っているか判断できる。
            textComponent.enableWordWrapping = true;
            textComponent.text = contentText;

            textComponent.rectTransform.sizeDelta = new Vector2(restWidth, float.PositiveInfinity);
            var textInfos = textComponent.GetTextInfo(contentText);

            var tmGeneratorLines = textInfos.lineInfo;
            var lineSpacing = textComponent.lineSpacing;
            var tmLineCount = textInfos.lineCount;

            var currentFirstLineWidth = tmGeneratorLines[0].length;

            // 文字を中央揃えではなく適正な位置にセットするためにラッピングを解除する。
            textComponent.enableWordWrapping = false;

            var currentFirstLineHeight = tmGeneratorLines[0].lineHeight;

            var isHeadOfLine = originX == 0;
            var isMultiLined = 1 < tmLineCount;
            var isLayoutedOutOfView = viewWidth < originX + currentFirstLineWidth;

            var status = TextLayoutDefinitions.GetTextLayoutStatus(isHeadOfLine, isMultiLined, isLayoutedOutOfView);
            switch (status)
            {
                case TextLayoutStatus.NotHeadAndSingle:
                case TextLayoutStatus.HeadAndSingle:
                    {
                        // 全文を表示して終了
                        textComponent.text = contentText;
                        if (continueContent)
                        {
                            continueContent = false;
                            restWidth = viewWidth - currentFirstLineWidth;
                            currentLineMaxHeight = currentFirstLineHeight;
                        }
                        else
                        {
                            textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                        }

                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        ElementLayoutFunctions.ContinueLine(ref originX, currentFirstLineWidth, currentFirstLineHeight, ref currentLineMaxHeight);
                        break;
                    }

                case TextLayoutStatus.NotHeadAndMulti:
                    {
                        // これは、この行 + 追加のHead ~ 系の最大2コンテンツにできる。
                        var nextLineTextIndex = tmGeneratorLines[1].firstCharacterIndex;
                        var nextLineText = contentText.Substring(nextLineTextIndex);

                        // 現在の行のセット
                        textComponent.text = contentText.Substring(0, nextLineTextIndex);
                        textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        var childOriginX = originX;
                        var currentTotalLineHeight = ElementLayoutFunctions.LineFeed(ref originX, ref originY, currentFirstLineHeight, ref currentLineMaxHeight, ref lineContents);// 文字コンテンツの高さ分改行する

                        // 次の行のコンテンツをこのコンテンツの子として生成するが、レイアウトまでを行わず次の行の起点の計算を行う。
                        // ここで全てを計算しない理由は、この処理の結果、複数種類のレイアウトが発生するため、ここで全てを書かない方が変えやすい。
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidth;

                            // 次の行のコンテンツを入れる
                            contentText = nextLineText;
                            newTailTextElement = TextElement.GO(contentText);
                            newTailTextElement.transform.SetParent(transform);// 消しやすくするため、この新規コンテンツを子にする

                            // xは-に、yは親の直下に置く。yは特に、「親が親の行上でどのように配置されたのか」を加味する必要がある。
                            // 例えば親行の中で親が最大の背の高さのコンテンツでない場合、改行すべき値は 親の背 + (行の背 - 親の背)/2 になる。

                            var yPosFromLinedParentY =
                                -(// 下向きがマイナスな座標系なのでマイナス
                                    currentFirstLineHeight// 現在の文字の高さと行の高さを比較し、文字の高さ + 上側の差の高さ(差/2)を足して返す。
                                    + (
                                        currentTotalLineHeight
                                        - currentFirstLineHeight
                                    )
                                    / 2
                                );

                            // X表示位置を原点にずらす、Yは次のコンテンツの開始Y位置 = LineFeedで変更された親の位置に依存し、親の位置からoriginYを引いた値になる。
                            var newTailTextElementRectTrans = newTailTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(-childOriginX, yPosFromLinedParentY);// ここかー

                            // テキスト特有の継続したコンテンツ扱いする。
                            continueContent = true;

                            // 追加する(次の処理で確実に消されるが、足しておかないと次の行頭複数行で-されるケースがあり詰む)
                            lineContents.Add(newTailTextElementRectTrans);
                            goto NextLine;
                        }
                    }

                case TextLayoutStatus.HeadAndMulti:
                    {
                        // この形式のコンテンツは明示的に行に追加しない。末行が行中で終わるケースがあるため、追加するとコンテンツ全体が上下してしまう。
                        lineContents.RemoveAt(lineContents.Count - 1);

                        var lineCount = textInfos.lineCount;
                        var lineStrs = new string[lineCount];

                        var totalHeight = 0f;
                        // totalHeightを出す + 改行を入れる
                        for (var j = 0; j < lineCount; j++)
                        {
                            var lInfo = textInfos.lineInfo[j];
                            totalHeight += lInfo.lineHeight;
                            lineStrs[j] = contentText.Substring(lInfo.firstCharacterIndex, lInfo.lastCharacterIndex - lInfo.firstCharacterIndex + 1);
                        }

                        // 改行コードを入れ、複数行での表示をいい感じにする。
                        textComponent.text = string.Join("\n", lineStrs);
                        textComponent.rectTransform.sizeDelta = new Vector2(restWidth, totalHeight);

                        // 最終行関連のデータを揃える
                        var lastLineWidth = textInfos.lineInfo[lineCount - 1].length;
                        var lastLineHeight = textInfos.lineInfo[lineCount - 1].lineHeight;
                        var contentHeightWithoutLastLine = totalHeight - lastLineHeight;

                        // なんらかの続きの文字コンテンツである場合、そのコンテンツの子になっているので位置情報を調整しない。
                        if (continueContent)
                        {
                            continueContent = false;
                        }
                        else
                        {
                            textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                        }

                        originX = lastLineWidth;// 最終ラインのx位置を次のコンテンツに使う
                        originY -= contentHeightWithoutLastLine;// Y位置の継続ポイントとして、最終行の高さを引いたコンテンツの高さを足す
                        restWidth = viewWidth - lastLineWidth;// この行に入る残りのコンテンツの幅を初期化する
                        currentLineMaxHeight = lastLineHeight;// 最終行の高さを使ってコンテンツの高さを初期化する

                        break;
                    }
                case TextLayoutStatus.NotHeadAndOutOfView:
                    {
                        // 次の行にコンテンツを置き、継続する

                        // 現在最後の追加要素である自分自身を取り出し、ここまでの行の要素を整列させる。
                        lineContents.RemoveAt(lineContents.Count - 1);
                        ElementLayoutFunctions.LineFeed(ref originX, ref originY, currentLineMaxHeight, ref currentLineMaxHeight, ref lineContents);

                        // レイアウト対象のビューサイズを新しい行のものとして更新する
                        restWidth = viewWidth;
                        lineContents.Add(textComponent.rectTransform);
                        goto NextLine;
                    }
            }
        }

        public static void RectLayout(LTElement rectElement, RectTransform transform, Vector2 rectSize, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents)
        {
            Debug.Assert(transform.pivot.x == 0 && transform.pivot.y == 1 && transform.anchorMin.x == 0 && transform.anchorMin.y == 1 && transform.anchorMax.x == 0 && transform.anchorMax.y == 1, "rectTransform for LayouTaro should set pivot to 0,1 and anchorMin 0,1 anchorMax 0,1.");
            if (restWidth < rectSize.x)// 同じ列にレイアウトできないので次の列に行く。
            {
                // 現在最後の追加要素である自分自身を取り出し、整列させる。
                lineContents.RemoveAt(lineContents.Count - 1);
                ElementLayoutFunctions.LineFeed(ref originX, ref originY, currentLineMaxHeight, ref currentLineMaxHeight, ref lineContents);
                lineContents.Add(transform);

                // 位置をセット
                transform.anchoredPosition = new Vector2(originX, originY);
            }
            else
            {
                // 位置をセット
                transform.anchoredPosition = new Vector2(originX, originY);
            }

            // ジャストで埋まったら、次の行を作成する。
            if (restWidth == rectSize.x)
            {
                ElementLayoutFunctions.LineFeed(ref originX, ref originY, transform.sizeDelta.y, ref currentLineMaxHeight, ref lineContents);
                return;
            }

            ElementLayoutFunctions.ContinueLine(ref originX, rectSize.x, transform.sizeDelta.y, ref currentLineMaxHeight);
        }
    }
}