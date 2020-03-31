
using System.Collections.Generic;
using UnityEngine;

namespace UILayouTaro
{
    public static class BasicLayoutFunctions
    {

        public static void TextLayout(LTElement textElement, string contentText, RectTransform transform, float viewWidth, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents)
        {
            Debug.Assert(transform.pivot.x == 0 && transform.pivot.y == 1 && transform.anchorMin.x == 0 && transform.anchorMin.y == 1 && transform.anchorMax.x == 0 && transform.anchorMax.y == 1, "rectTransform for LayouTaro should set pivot to 0,1 and anchorMin 0,1 anchorMax 0,1.");
            var continueContent = false;

        NextLine:
            var textComponent = textElement.GetComponent<TMPro.TextMeshProUGUI>();

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
                        // textComponent.text = "<indent=" + originX + "pixels>"  でindentを付けられるが、これの恩恵を素直に受けられるケースが少ない。
                        // この行をレイアウトした際、行頭の文字のレイアウトが変わるようであれば、利用できない。
                        // 最適化としてはケースが複雑なので後回しにする。

                        // これは、この行 + 追加のHead ~ 系の複数のコンテンツに分割する。
                        var nextLineTextIndex = tmGeneratorLines[1].firstCharacterIndex;
                        var nextLineText = contentText.Substring(nextLineTextIndex);

                        // 現在の行のセット
                        textComponent.text = contentText.Substring(0, nextLineTextIndex);
                        textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        Debug.Log("textComponent.text:" + textComponent.text);

                        var childOriginX = originX;
                        var currentTotalLineHeight = ElementLayoutFunctions.LineFeed(ref originX, ref originY, currentFirstLineHeight, ref currentLineMaxHeight, ref lineContents);// 文字コンテンツの高さ分改行する

                        // 次の行のコンテンツをこのコンテンツの子として生成するが、レイアウトまでを行わず次の行の起点の計算を行う。
                        // ここで全てを計算しない理由は、この処理の結果、複数種類のレイアウトが発生するため、ここで全てを書かない方が変えやすい。

                        // if (false)
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidth;

                            // 次の行のコンテンツを入れる
                            Debug.Log("nextLineText:" + nextLineText);
                            var newTextElement = TextElement.GO(nextLineText);
                            newTextElement.gameObject.name = "は？";
                            newTextElement.transform.SetParent(textElement.transform);// 消しやすくするため、この新規コンテンツを子にする

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
                            var newTailTextElementRectTrans = newTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(-childOriginX, yPosFromLinedParentY);

                            // テキスト特有の継続したコンテンツ扱いする。
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            lineContents.Add(newTailTextElementRectTrans);
                            goto NextLine;
                        }
                        break;
                    }

                case TextLayoutStatus.HeadAndMulti:
                    {
                        return;
                        // このコンテンツは矩形で、行揃えの影響を受けないため、明示的に行から取り除く。
                        lineContents.RemoveAt(lineContents.Count - 1);

                        var lineCount = textInfos.lineCount;
                        var lineStrs = new string[lineCount];

                        var totalHeight = 0f;

                        // このコンテンツのtotalHeightを出す + 改行を入れる
                        for (var j = 0; j < lineCount; j++)
                        {
                            var lInfo = textInfos.lineInfo[j];
                            totalHeight += lInfo.lineHeight;
                            lineStrs[j] = contentText.Substring(lInfo.firstCharacterIndex, lInfo.lastCharacterIndex - lInfo.firstCharacterIndex + 1);
                        }

                        // 矩形になるテキスト = 最終行を取り除いたテキストを得る
                        var rectTexts = new string[lineStrs.Length - 1];
                        for (var i = 0; i < rectTexts.Length; i++)
                        {
                            rectTexts[i] = lineStrs[i];
                        }

                        // 最終行のテキストを得る
                        var lastLineText = lineStrs[lineStrs.Length - 1];
                        Debug.Log("lastLineText:" + lastLineText);

                        // rectの高さの取得
                        var lastLineHeight = textInfos.lineInfo[lineCount - 1].lineHeight;
                        var rectHeight = totalHeight - lastLineHeight;

                        // 改行コードを入れ、複数行での表示をいい感じにする。
                        textComponent.text = string.Join("\n", rectTexts);

                        Debug.Log("textComponent.text:" + textComponent.text);
                        textComponent.rectTransform.sizeDelta = new Vector2(restWidth, rectHeight);

                        // なんらかの続きの文字コンテンツである場合、そのコンテンツの子になっているので位置情報を調整しない。最終行を分割する。
                        if (continueContent)
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidth;

                            // 最終行のコンテンツを入れる
                            var newBrotherTextElement = TextElement.GO(lastLineText);
                            newBrotherTextElement.transform.SetParent(transform.parent);// 消しやすくするため、この新規コンテンツを現在の要素の親の子にする

                            // 次の行の行頭になる = 続いている要素と同じxを持つ
                            var childX = textComponent.rectTransform.anchoredPosition.x;

                            // yは親の分移動する
                            var childY = textComponent.rectTransform.anchoredPosition.y - rectHeight;

                            var newBrotherTailTextElementRectTrans = newBrotherTextElement.GetComponent<RectTransform>();
                            newBrotherTailTextElementRectTrans.anchoredPosition = new Vector2(childX, childY);

                            // 継続させる
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            lineContents.Add(newBrotherTailTextElementRectTrans);
                            // goto NextLine;
                            return;
                        }


                        // 誰かの子ではないので、独自に自分の位置をセットする
                        textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);

                        // 最終行のコンテンツを入れる
                        var newTextElement = TextElement.GO(lastLineText);
                        newTextElement.transform.SetParent(transform);// 消しやすくするため、この新規コンテンツを現在の要素の子にする

                        // 残りの行のサイズは最大化する
                        restWidth = viewWidth;

                        // 次の行の行頭になる
                        originX = 0;

                        // yは親の分移動する
                        originY -= rectHeight;

                        var newTailTextElementRectTrans = newTextElement.GetComponent<RectTransform>();
                        newTailTextElementRectTrans.anchoredPosition = new Vector2(originX, originY);

                        // 残りのデータをテキスト特有の継続したコンテンツ扱いする。
                        continueContent = true;

                        // 生成したコンテンツを次の行の要素へと追加する
                        lineContents.Add(newTailTextElementRectTrans);
                        // goto NextLine;
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

        public static void LayoutLastLine(ref float originY, float currentLineMaxHeight, ref List<RectTransform> lineContents)
        {
            // レイアウト終了後、最後の列の要素を並べる。
            foreach (var element in lineContents)
            {
                var rectTrans = element.GetComponent<RectTransform>();
                var elementHeight = rectTrans.sizeDelta.y;
                var isParentRoot = rectTrans.parent.GetComponent<LTElement>() is LTRootElement;
                if (isParentRoot)
                {
                    rectTrans.anchoredPosition = new Vector2(
                        rectTrans.anchoredPosition.x,
                        originY - (currentLineMaxHeight - elementHeight) / 2
                    );
                }
                else
                {
                    // 親がRootElementではない場合、なんらかの子要素なので、行の高さは合うが、上位の単位であるoriginYとの相性が悪すぎる。なので、独自の計算系で合わせる。
                    rectTrans.anchoredPosition = new Vector2(
                        rectTrans.anchoredPosition.x,
                        rectTrans.anchoredPosition.y -// 子要素は親からの絶対的な距離を独自に保持しているので、それ + 行全体を整頓した際の高さの隙間、という計算を行う。
                            (
                                currentLineMaxHeight// この行全体の高さからこの要素の高さを引いて/2して、「要素の上の方の隙間高さ」を手に入れる
                                - elementHeight
                            )
                            / 2
                        );
                }
            }

            // 最終的にyを更新する。
            originY -= currentLineMaxHeight;
        }
    }
}