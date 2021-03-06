
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UILayouTaro
{
    public static class BasicLayoutFunctions
    {

        public static void TextLayout<T>(T textElement, string contentText, RectTransform rectTrans, float viewWidth, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents, ref Vector2 wrappedSize) where T : LTElement, ILayoutableText
        {
            // 文字列が空な場合、何もせずに返す。
            if (string.IsNullOrEmpty(contentText))
            {
                return;
            }

            Debug.Assert(rectTrans.pivot.x == 0 && rectTrans.pivot.y == 1 && rectTrans.anchorMin.x == 0 && rectTrans.anchorMin.y == 1 && rectTrans.anchorMax.x == 0 && rectTrans.anchorMax.y == 1, "rectTransform for BasicLayoutFunctions.TextLayout should set pivot to 0,1 and anchorMin 0,1 anchorMax 0,1.");
            Debug.Assert(textElement.transform.childCount == 0, "BasicLayoutFunctions.TextLayout not allows text element which has child.");
            var continueContent = false;

        NextLine:

            var textComponent = textElement.GetComponent<TextMeshProUGUI>();
            TMPro.TMP_TextInfo textInfos = null;
            {
                // wordWrappingを可能にすると、表示はともかく実際にこの行にどれだけの文字が入っているか判断できる。
                textComponent.enableWordWrapping = true;
                textComponent.text = contentText;
                if (0 < textComponent.transform.childCount)
                {
                    for (var i = 0; i < textComponent.transform.childCount; i++)
                    {
                        var childRectTrans = textComponent.transform.GetChild(i).GetComponent<RectTransform>();
                        childRectTrans.pivot = new Vector2(0, 1);
                        childRectTrans.anchorMin = new Vector2(0, 1);
                        childRectTrans.anchorMax = new Vector2(0, 1);
                        childRectTrans.anchoredPosition = Vector2.zero;
                    }
                }


                // 文字が入る箱のサイズを縦に無限にして、どの程度入るかのレイアウト情報を取得する。
                textComponent.rectTransform.sizeDelta = new Vector2(restWidth, Screen.height);
                textInfos = textComponent.GetTextInfo(contentText);

                // 文字を中央揃えではなく適正な位置にセットするためにラッピングを解除する。
                textComponent.enableWordWrapping = false;
            }

            // TODO: 直す
            // 絵文字が含まれている場合、画像と文字に分けてレイアウトを行う。
            if (IsContainsSurrogatePairOrSprite(contentText))
            {
                textComponent.text = string.Empty;

                // TMProが子オブジェクトを作っている場合があり、それらがあれば消す必要がある。
                // 同じフレーム内で同じ絵文字を作るようなことをする場合、作成されないケースがあるため、子供の有無を条件として絵文字の有無を判断することはできなかった。
                for (var i = 0; i < textComponent.transform.childCount; i++)
                {
                    GameObject.Destroy(textComponent.transform.GetChild(i).gameObject);
                }

                // 絵文字が含まれている。

                // 今後のレイアウトに自分自身を巻き込まないように、レイアウトから自分自身を取り外す
                lineContents.RemoveAt(lineContents.Count - 1);

                textComponent.rectTransform.sizeDelta = new Vector2(restWidth, 0);// 高さが0で問題ない。

                // この内部で全てのレイアウトを終わらせる。
                LayoutContentWithEmoji(textElement, contentText, viewWidth, ref originX, ref originY, ref restWidth, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);
                return;
            }

            var tmGeneratorLines = textInfos.lineInfo;
            var lineSpacing = textComponent.lineSpacing;
            var tmLineCount = textInfos.lineCount;

            var firstLine = tmGeneratorLines[0];
            var currentFirstLineWidth = firstLine.length;
            var currentFirstLineHeight = firstLine.lineHeight;

            var isHeadOfLine = originX == 0;
            var isMultiLined = 1 < tmLineCount;

            /*
                予定している1行目の文字の幅が予定幅を超えている = オーバーフローしている場合、次のケースがある
                ・文字列の1行目の末尾がたまたま幅予算を超えてレイアウトされた

                この場合、溢れたケースとして文字列の長さを調整してレイアウトを行う。
            */
            var isTextOverflow = (viewWidth < originX + currentFirstLineWidth);

            // TMProで末尾がwhitespaceで終わっている場合、正しい幅を出せていなく、そのくせ改行設定などは正しい。
            // 幅がわかる文字を用意し、スペース文字 + 文字をつくり、コンテンツの幅から文字の幅を引けば、スペースの幅が出せる。
            // それを1単位として、末尾にあるスペースの幅 x 個数をやれば、このコンテンツの正しい幅が出せる。
            if (Char.IsWhiteSpace(contentText[firstLine.lastCharacterIndex]))
            {
                // 行末の、whitespaceかそれに該当する非表示な要素の個数
                var numEndWhiteSpaceCount = firstLine.lastCharacterIndex - firstLine.lastVisibleCharacterIndex;

                // サンプリングする対象を今回の文字列の末尾から取得する。
                // このため、幅が異なるwhitespace扱いのものが混じっていても、正しい長さを返せない。
                var samplingWhiteSpace = contentText[firstLine.lastCharacterIndex];

                // 一時退避する
                var sourceText = textComponent.text;

                // 0の文字の幅を取得するためにtextComponentにセット、現在のフォントでの0の幅を計測する。
                textComponent.text = "0";
                var widthOf0 = textComponent.preferredWidth;

                // whitespace + 0の文字の幅を取得するためにtextComponentにセット、現在のフォントでのws + 0の幅を計測する。
                textComponent.text = samplingWhiteSpace + "0";

                // 差 = whitespaceの幅を取得する。
                var singleWidthOfWhiteSpace = textComponent.preferredWidth - widthOf0;

                // 退避したテキストを戻す
                textComponent.text = sourceText;

                // 現在のコンテンツのX起点 + whitespaceが含まれない幅 + whitespace幅 * 個数 を足し、この行の正しい幅を出す。
                var totalWidthWithSpaces = originX + currentFirstLineWidth + (singleWidthOfWhiteSpace * numEndWhiteSpaceCount);

                // 画面の幅と比較し、小さい方をとる。
                // これは、whitespaceを使ってレイアウトした場合、TMProがリクエスト幅を超えたぶんのwhitespaceの計算をサボって、whitespaceではない文字が来た時点でその文字を頭とする改行を行うため。
                // 最大でも画面幅になるようにする。
                var maxWidth = Mathf.Min(viewWidth, totalWidthWithSpaces);

                // 行送りが発生しているため、この行の値の幅の更新はもう起きない。そのため、ここでwrappedSize.xを更新する。
                wrappedSize.x = Mathf.Max(wrappedSize.x, maxWidth);
            }

            var status = TextLayoutDefinitions.GetTextLayoutStatus(isHeadOfLine, isMultiLined, isTextOverflow);
            switch (status)
            {
                case TextLayoutStatus.NotHeadAndSingle:
                case TextLayoutStatus.HeadAndSingle:
                    {
                        // 全文を表示して終了
                        textComponent.text = contentText;
                        if (0 < textComponent.transform.childCount)
                        {
                            for (var i = 0; i < textComponent.transform.childCount; i++)
                            {
                                var childRectTrans = textComponent.transform.GetChild(i).GetComponent<RectTransform>();
                                childRectTrans.pivot = new Vector2(0, 1);
                                childRectTrans.anchorMin = new Vector2(0, 1);
                                childRectTrans.anchorMax = new Vector2(0, 1);
                                childRectTrans.anchoredPosition = Vector2.zero;
                            }
                        }
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

                        ContinueLine(ref originX, originY, currentFirstLineWidth, currentFirstLineHeight, ref currentLineMaxHeight, ref wrappedSize);
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
                        if (0 < textComponent.transform.childCount)
                        {
                            for (var i = 0; i < textComponent.transform.childCount; i++)
                            {
                                var childRectTrans = textComponent.transform.GetChild(i).GetComponent<RectTransform>();
                                childRectTrans.pivot = new Vector2(0, 1);
                                childRectTrans.anchorMin = new Vector2(0, 1);
                                childRectTrans.anchorMax = new Vector2(0, 1);
                                childRectTrans.anchoredPosition = Vector2.zero;
                            }
                        }

                        textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        var childOriginX = originX;
                        var currentTotalLineHeight = LineFeed<LTRootElement>(ref originX, ref originY, currentFirstLineHeight, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);// 文字コンテンツの高さ分改行する

                        // 次の行のコンテンツをこのコンテンツの子として生成するが、レイアウトまでを行わず次の行の起点の計算を行う。
                        // ここで全てを計算しない理由は、この処理の結果、複数種類のレイアウトが発生するため、ここで全てを書かない方が変えやすい。
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidth;

                            // 次の行のコンテンツを入れる
                            var nextLineTextElement = textElement.GenerateGO(nextLineText).GetComponent<T>();
                            nextLineTextElement.transform.SetParent(textElement.transform, false);// 消しやすくするため、この新規コンテンツを子にする

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
                            var newTailTextElementRectTrans = nextLineTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(-childOriginX, yPosFromLinedParentY);

                            // テキスト特有の継続したコンテンツ扱いする。
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            lineContents.Add(newTailTextElementRectTrans);

                            // 上書きを行う
                            textElement = nextLineTextElement;
                            contentText = nextLineText;
                            goto NextLine;
                        }
                    }

                case TextLayoutStatus.HeadAndMulti:
                    {
                        // このコンテンツは矩形で、行揃えの影響を受けないため、明示的に行から取り除く。
                        lineContents.RemoveAt(lineContents.Count - 1);

                        var lineCount = textInfos.lineCount;
                        var lineStrs = new string[lineCount];

                        var totalHeight = 0f;
                        for (var j = 0; j < lineCount; j++)
                        {
                            var lInfo = textInfos.lineInfo[j];
                            totalHeight += lInfo.lineHeight;

                            // ここで、含まれている絵文字の数がわかれば、そのぶんを後ろに足すことで文字切れを回避できそう、、ではある、、
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

                        // rectの高さの取得
                        var lastLineHeight = textInfos.lineInfo[lineCount - 1].lineHeight;
                        var rectHeight = totalHeight - lastLineHeight;

                        // 改行コードを入れ、複数行での表示をいい感じにする。
                        textComponent.text = string.Join("\n", rectTexts);
                        if (0 < textComponent.transform.childCount)
                        {
                            for (var i = 0; i < textComponent.transform.childCount; i++)
                            {
                                var childRectTrans = textComponent.transform.GetChild(i).GetComponent<RectTransform>();
                                childRectTrans.pivot = new Vector2(0, 1);
                                childRectTrans.anchorMin = new Vector2(0, 1);
                                childRectTrans.anchorMax = new Vector2(0, 1);
                                childRectTrans.anchoredPosition = Vector2.zero;
                            }
                        }

                        textComponent.rectTransform.sizeDelta = new Vector2(restWidth, rectHeight);

                        // 幅の最大値を取得
                        var max = Mathf.Max(textComponent.rectTransform.sizeDelta.x, textComponent.preferredWidth);

                        // wrappedな幅の更新
                        wrappedSize.x = Mathf.Max(wrappedSize.x, max);

                        // なんらかの続きの文字コンテンツである場合、そのコンテンツの子になっているので位置情報を調整しない。最終行を分割する。
                        if (continueContent)
                        {
                            // 別のコンテンツから継続している行はじめの処理なので、子をセットする前にここまでの分の改行を行う。
                            LineFeed<LTRootElement>(ref originX, ref originY, rectHeight, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);

                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidth;

                            // 最終行のコンテンツを入れる
                            var nextLineTextElement = textElement.GenerateGO(lastLineText).GetComponent<T>();
                            nextLineTextElement.transform.SetParent(textElement.transform.parent, false);// 消しやすくするため、この新規コンテンツを現在の要素の親の子にする

                            // 次の行の行頭になる = 続いている要素と同じxを持つ
                            var childX = textComponent.rectTransform.anchoredPosition.x;

                            // yは親の分移動する
                            var childY = textComponent.rectTransform.anchoredPosition.y - rectHeight;

                            var newBrotherTailTextElementRectTrans = nextLineTextElement.GetComponent<RectTransform>();
                            newBrotherTailTextElementRectTrans.anchoredPosition = new Vector2(childX, childY);

                            // 継続させる
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            lineContents.Add(newBrotherTailTextElementRectTrans);

                            // 上書きを行う
                            textElement = nextLineTextElement;
                            contentText = lastLineText;
                            goto NextLine;
                        }

                        // 誰かの子ではないので、独自に自分の位置をセットする
                        textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);

                        // 最終行のコンテンツを入れる
                        var newTextElement = textElement.GenerateGO(lastLineText).GetComponent<T>();
                        newTextElement.transform.SetParent(rectTrans, false);// 消しやすくするため、この新規コンテンツを現在の要素の子にする

                        // 残りの行のサイズは最大化する
                        restWidth = viewWidth;

                        // 次の行の行頭になる
                        originX = 0;

                        // yは親の分移動する
                        originY -= rectHeight;

                        {
                            // 新規オブジェクトはそのy位置を親コンテンツの高さを加えた値にセットする。
                            var newTailTextElementRectTrans = newTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(originX, -rectHeight);

                            // 残りのデータをテキスト特有の継続したコンテンツ扱いする。
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            lineContents.Add(newTailTextElementRectTrans);

                            textElement = newTextElement;
                            contentText = lastLineText;
                            goto NextLine;
                        }
                    }
                case TextLayoutStatus.OutOfViewAndSingle:
                    {
                        // 1行ぶんのみのコンテンツで、行末が溢れている。入るように要素を切り、残りを継続してHeadAndSingleとして処理する。

                        // 超過している幅を収めるために、1文字ぶんだけ引いた文字を使ってレイアウトを行う。
                        var index = contentText.Length - 1;
                        var firstLineText = contentText.Substring(0, index);
                        var restText = contentText.Substring(index);

                        // 現在の行のセット
                        textComponent.text = firstLineText;
                        if (0 < textComponent.transform.childCount)
                        {
                            for (var i = 0; i < textComponent.transform.childCount; i++)
                            {
                                var childRectTrans = textComponent.transform.GetChild(i).GetComponent<RectTransform>();
                                childRectTrans.pivot = new Vector2(0, 1);
                                childRectTrans.anchorMin = new Vector2(0, 1);
                                childRectTrans.anchorMax = new Vector2(0, 1);
                                childRectTrans.anchoredPosition = Vector2.zero;
                            }
                        }

                        textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        var childOriginX = originX;
                        var currentTotalLineHeight = LineFeed<LTRootElement>(ref originX, ref originY, currentFirstLineHeight, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);// 文字コンテンツの高さ分改行する

                        // 次の行のコンテンツをこのコンテンツの子として生成するが、レイアウトまでを行わず次の行の起点の計算を行う。
                        // ここで全てを計算しない理由は、この処理の結果、複数種類のレイアウトが発生するため、ここで全てを書かない方が変えやすい。
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidth;

                            // 次の行のコンテンツを入れる
                            var nextLineTextElement = textElement.GenerateGO(restText).GetComponent<T>();
                            nextLineTextElement.transform.SetParent(textElement.transform, false);// 消しやすくするため、この新規コンテンツを子にする

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

                            // X表示位置を原点にずらす、Yは次のコンテンツの開始Y位置 = LineFeed<LTAsyncRootElement>で変更された親の位置に依存し、親の位置からoriginYを引いた値になる。
                            var newTailTextElementRectTrans = nextLineTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(-childOriginX, yPosFromLinedParentY);

                            // テキスト特有の継続したコンテンツ扱いする。
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            lineContents.Add(newTailTextElementRectTrans);

                            // 上書きを行う
                            textElement = nextLineTextElement;
                            contentText = restText;
                            goto NextLine;
                        }
                    }
                case TextLayoutStatus.OutOfViewAndMulti:
                    {
                        // 複数行があるのが確定していて、最初の行の内容が溢れている。入るように要素を切り、残りを継続してHeadAndMultiとして処理する。

                        // 複数行が既に存在するのが確定しているので、次の行のコンテンツをそのまま取得、分割する。
                        var nextLineTextIndex = tmGeneratorLines[1].firstCharacterIndex;

                        // 超過している幅を収めるために、1文字ぶんだけ引いた文字を使ってレイアウトを行う。
                        var index = nextLineTextIndex - 1;

                        var firstLineText = contentText.Substring(0, index);
                        var restText = contentText.Substring(index);

                        // 現在の行のセット
                        textComponent.text = firstLineText;
                        if (0 < textComponent.transform.childCount)
                        {
                            for (var i = 0; i < textComponent.transform.childCount; i++)
                            {
                                var childRectTrans = textComponent.transform.GetChild(i).GetComponent<RectTransform>();
                                childRectTrans.pivot = new Vector2(0, 1);
                                childRectTrans.anchorMin = new Vector2(0, 1);
                                childRectTrans.anchorMax = new Vector2(0, 1);
                                childRectTrans.anchoredPosition = Vector2.zero;
                            }
                        }

                        textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        var childOriginX = originX;
                        var currentTotalLineHeight = LineFeed<LTRootElement>(ref originX, ref originY, currentFirstLineHeight, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);// 文字コンテンツの高さ分改行する

                        // 次の行のコンテンツをこのコンテンツの子として生成するが、レイアウトまでを行わず次の行の起点の計算を行う。
                        // ここで全てを計算しない理由は、この処理の結果、複数種類のレイアウトが発生するため、ここで全てを書かない方が変えやすい。
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidth;

                            // 次の行のコンテンツを入れる
                            var nextLineTextElement = textElement.GenerateGO(restText).GetComponent<T>();
                            nextLineTextElement.transform.SetParent(textElement.transform, false);// 消しやすくするため、この新規コンテンツを子にする

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

                            // X表示位置を原点にずらす、Yは次のコンテンツの開始Y位置 = LineFeed<LTAsyncRootElement>で変更された親の位置に依存し、親の位置からoriginYを引いた値になる。
                            var newTailTextElementRectTrans = nextLineTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(-childOriginX, yPosFromLinedParentY);

                            // テキスト特有の継続したコンテンツ扱いする。
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            lineContents.Add(newTailTextElementRectTrans);

                            // 上書きを行う
                            textElement = nextLineTextElement;
                            contentText = restText;
                            goto NextLine;
                        }
                    }
            }
        }

        /*
            絵文字が入った文字列をintenalな矩形と文字としてレイアウトする。ここでやることで、TM Proによるレイアウトの破綻を避ける。
        */
        private static void LayoutContentWithEmoji<T>(T textElement, string contentText, float viewWidth, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents, ref Vector2 wrappedSize) where T : LTElement, ILayoutableText
        {
            /*
                絵文字が含まれている文字列を、絵文字と矩形に分解、再構成を行う。絵文字を単に画像が入る箱としてRectLayoutに放り込む。というのがいいのか、それとも独自にinternalを定義した方がいいのか。
                後者だなー、EmojiRectみたいなのを用意しよう。

                自分自身を書き換えて、一連のコマンドを実行するようにする。
                文字がどう始まるかも含めて、今足されているlinedからは一度離反する。その上で一つ目のコンテンツを追加する。
            */
            var elementsWithEmoji = CollectSpriteAndTextElement(textElement, contentText);

            for (var i = 0; i < elementsWithEmoji.Count; i++)
            {
                var element = elementsWithEmoji[i];
                var rectTrans = element.GetComponent<RectTransform>();
                restWidth = viewWidth - originX;
                lineContents.Add(rectTrans);

                if (element is InternalEmojiRect)
                {
                    // emojiRectが入っている
                    var internalRectElement = (InternalEmojiRect)element;
                    EmojiRectLayout(internalRectElement, rectTrans, viewWidth, ref originX, ref originY, ref restWidth, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);
                    continue;
                }

                // ここに来るということは、T型が入っている。
                var internalTextElement = (T)element;
                var internalContentText = internalTextElement.Text();

                TextLayout(internalTextElement, internalContentText, rectTrans, viewWidth, ref originX, ref originY, ref restWidth, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);
            }
        }

        public static bool IsContainsSurrogatePairOrSprite(string contentText)
        {
            for (var i = 0; i < contentText.Length; i++)
            {
                var firstChar = contentText[i];

                // \U0001F971
                var isSurrogate = Char.IsSurrogate(firstChar);
                if (isSurrogate)
                {
                    if (i == contentText.Length - 1)
                    {
                        continue;
                    }

                    var nextChar = contentText[i + 1];
                    var isSurrogatePair = Char.IsSurrogatePair(firstChar, nextChar);

                    if (isSurrogatePair)
                    {
                        return true;
                    }

                    // 後続の文字がsurrogateではなかった。
                    continue;
                }

                // spriteに含まれている文字になる場合
                var codePoint = (uint)char.ConvertToUtf32(firstChar.ToString(), 0);

                var spriteAsset = TMPro.TMP_Settings.GetSpriteAsset();
                if (-1 < spriteAsset.GetSpriteIndexFromUnicode(codePoint))
                {
                    // 絵文字か記号が既存のSpriteAssetに存在する
                    return true;
                }

                // fallbackに登録されているSpriteAssetsも見る
                foreach (var sAsset in spriteAsset.fallbackSpriteAssets)
                {
                    if (-1 < sAsset.GetSpriteIndexFromUnicode(codePoint))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static List<LTElement> CollectSpriteAndTextElement<T>(T textElement, string contentText) where T : LTElement, ILayoutableText
        {
            var elementsWithEmoji = new List<LTElement>();

            var textStartIndex = 0;
            var length = 0;
            for (var i = 0; i < contentText.Length; i++)
            {
                var firstChar = contentText[i];

                // \U0001F971
                var isSurrogate = Char.IsSurrogate(firstChar);
                if (isSurrogate)
                {
                    if (0 < length)
                    {
                        var currentText = contentText.Substring(textStartIndex, length);
                        var newTextElement = textElement.GenerateGO(currentText).GetComponent<T>();
                        newTextElement.transform.SetParent(textElement.transform, false);
                        elementsWithEmoji.Add(newTextElement);
                    }

                    length = 0;

                    if (i == contentText.Length - 1)
                    {
                        // 続きの文字がないのでサロゲートペアではない。無視する。
                        // 文字の開始インデックスを次の文字へとセットする。
                        textStartIndex = i + 1;
                        continue;
                    }

                    var nextChar = contentText[i + 1];
                    var isSurrogatePair = Char.IsSurrogatePair(firstChar, nextChar);

                    if (isSurrogatePair)
                    {
                        // サロゲートペア確定。なので、要素として扱い、次の文字を飛ばす処理を行う。
                        var emojiElement = InternalEmojiRect.GO(textElement, new Char[] { firstChar, nextChar }).GetComponent<InternalEmojiRect>();
                        elementsWithEmoji.Add(emojiElement);

                        // 文字は次の次から始まる、、かもしれない。
                        textStartIndex = i + 2;
                        i = i + 1;
                        continue;
                    }

                    // ペアではなかったので、無視して次の文字へと行く。
                    textStartIndex = i + 1;
                    continue;
                }

                // spriteに含まれている文字になる場合
                var codePoint = (uint)char.ConvertToUtf32(contentText, i);
                {
                    var spriteAsset = TMPro.TMP_Settings.GetSpriteAsset();
                    if (-1 < spriteAsset.GetSpriteIndexFromUnicode(codePoint))
                    {
                        if (0 < length)
                        {
                            var currentText = contentText.Substring(textStartIndex, length);
                            var newTextElement = textElement.GenerateGO(currentText).GetComponent<T>();
                            newTextElement.transform.SetParent(textElement.transform, false);
                            elementsWithEmoji.Add(newTextElement);
                        }

                        length = 0;

                        // Sprite確定。なので、要素として扱い、次の文字を飛ばす処理を行う。
                        var emojiElement = InternalEmojiRect.GO(textElement, new Char[] { firstChar }).GetComponent<InternalEmojiRect>();
                        elementsWithEmoji.Add(emojiElement);

                        // 文字は次から始まる、、かもしれない。
                        textStartIndex = i + 1;
                        continue;
                    }

                    // fallbackに登録されているSpriteAssetsも見る
                    var isFound = false;
                    foreach (var sAsset in spriteAsset.fallbackSpriteAssets)
                    {
                        if (-1 < sAsset.GetSpriteIndexFromUnicode(codePoint))
                        {
                            if (0 < length)
                            {
                                var currentText = contentText.Substring(textStartIndex, length);
                                var newTextElement = textElement.GenerateGO(currentText).GetComponent<T>();
                                newTextElement.transform.SetParent(textElement.transform, false);
                                elementsWithEmoji.Add(newTextElement);
                            }

                            length = 0;

                            // Sprite確定。なので、要素として扱い、次の文字を飛ばす処理を行う。
                            var emojiElement = InternalEmojiRect.GO(textElement, new Char[] { firstChar }).GetComponent<InternalEmojiRect>();
                            elementsWithEmoji.Add(emojiElement);

                            // 文字は次から始まる、、かもしれない。
                            textStartIndex = i + 1;
                            isFound = true;
                            break;// foreachを抜ける
                        }
                    }
                    if (isFound)
                    {
                        // 発見できたので消費する
                        continue;
                    }
                }

                // サロゲートやSpriteではないので文字として扱う
                length++;
            }

            // 残りの文字を足す
            if (0 < length)
            {
                var lastText = contentText.Substring(textStartIndex, length);
                var lastTextElement = textElement.GenerateGO(lastText).GetComponent<T>();
                lastTextElement.transform.SetParent(textElement.transform, false);
                elementsWithEmoji.Add(lastTextElement);
            }

            return elementsWithEmoji;
        }


        // TODO: Spriteにrenameする。
        private static void EmojiRectLayout(InternalEmojiRect rectElement, RectTransform transform, float viewWidth, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents, ref Vector2 wrappedSize)
        {
            var rectSize = rectElement.RectSize();
            RectLayout(rectElement, transform, rectSize, viewWidth, ref originX, ref originY, ref restWidth, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);
        }

        public static void RectLayout(LTElement rectElement, RectTransform transform, Vector2 rectSize, float viewWidth, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents, ref Vector2 wrappedSize)
        {
            Debug.Assert(transform.pivot.x == 0 && transform.pivot.y == 1 && transform.anchorMin.x == 0 && transform.anchorMin.y == 1 && transform.anchorMax.x == 0 && transform.anchorMax.y == 1, "rectTransform for LayouTaro should set pivot to 0,1 and anchorMin 0,1 anchorMax 0,1.");
            if (restWidth < rectSize.x)// 同じ列にレイアウトできないので次の列に行く。
            {
                // 現在最後の追加要素である自分自身を取り出し、整列させる。
                lineContents.RemoveAt(lineContents.Count - 1);
                LineFeed<LTRootElement>(ref originX, ref originY, currentLineMaxHeight, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);
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
                LineFeed<LTRootElement>(ref originX, ref originY, transform.sizeDelta.y, ref currentLineMaxHeight, ref lineContents, ref wrappedSize);
                return;
            }

            ContinueLine(ref originX, originY, rectSize.x, transform.sizeDelta.y, ref currentLineMaxHeight, ref wrappedSize);
        }


        /*
            改行、行継続に関するレイアウトコントロール系
        */
        private static float LineFeed<T>(ref float x, ref float y, float currentElementHeight, ref float currentLineMaxHeight, ref List<RectTransform> linedElements, ref Vector2 wrappedSize) where T : LTRootElement
        {
            // 列の概念の中で最大の高さを持つ要素を中心に、それより小さい要素をy軸に対して整列させる
            var maxHeight = Mathf.Max(currentElementHeight, currentLineMaxHeight);

            for (var i = 0; i < linedElements.Count; i++)
            {
                var rectTrans = linedElements[i];

                var elementHeight = rectTrans.sizeDelta.y;
                var isParentRoot = rectTrans.parent.GetComponent<T>() is T;
                if (isParentRoot)
                {
                    rectTrans.anchoredPosition = new Vector2(
                        rectTrans.anchoredPosition.x,// xは維持
                        y - (maxHeight - elementHeight) / 2// yは行の高さから要素の高さを引いて/2したものをセット(縦の中央揃え)
                    );
                }
                else
                {
                    // 親がRootElementではない場合、なんらかの子要素なので、行の高さは合うが、上位の単位であるoriginYとの相性が悪すぎる。なので、独自の計算系で合わせる。
                    rectTrans.anchoredPosition = new Vector2(
                        rectTrans.anchoredPosition.x,// xは維持
                        rectTrans.anchoredPosition.y - (maxHeight - elementHeight) / 2
                    );
                }

                // 最後の要素を使ってwrappedなコンテンツの幅の更新を行う
                if (i == linedElements.Count - 1)
                {
                    // wrappedな幅の更新
                    var rectTransOfLastElement = linedElements[linedElements.Count - 1];

                    var currentLineWidth = 0f;
                    if (isParentRoot)
                    {
                        currentLineWidth = rectTransOfLastElement.anchoredPosition.x + rectTransOfLastElement.sizeDelta.x;
                    }
                    else
                    {
                        // 子要素なので、親のx位置を足す。
                        currentLineWidth = rectTransOfLastElement.parent.GetComponent<RectTransform>().anchoredPosition.x + rectTransOfLastElement.anchoredPosition.x + rectTransOfLastElement.sizeDelta.x;
                    }

                    // wrappedな幅の更新
                    wrappedSize.x = Mathf.Max(wrappedSize.x, currentLineWidth);
                }
            }

            // 行の整列が終わったので初期化する
            linedElements.Clear();


            x = 0;
            y -= maxHeight;

            // wrappedな高さの更新
            wrappedSize.y = Mathf.Abs(y);

            // 記録してある行の最大高度のリセットを行う
            currentLineMaxHeight = 0f;

            // 純粋にその行の中でどの要素が最も背が高かったのかを判別するために、計算結果による変数の初期化に関係なくこの値が必要な箇所がある。
            return maxHeight;
        }

        private static void ContinueLine(ref float x, float currentY, float newX, float currentElementHeight, ref float currentLineMaxHeight, ref Vector2 wrappedSize)
        {
            // 継続するコンテンツのX位置の更新
            x += newX;

            // 現在の行の高さの更新
            currentLineMaxHeight = Mathf.Max(currentElementHeight, currentLineMaxHeight);

            // wrappedなサイズの更新
            wrappedSize.x = Mathf.Max(wrappedSize.x, x);
            wrappedSize.y = Mathf.Max(wrappedSize.y, Math.Abs(currentY) + currentLineMaxHeight);
        }


        public static void LayoutLastLine<T>(ref float y, float currentLineMaxHeight, ref List<RectTransform> linedRectTransforms) where T : LTRootElement
        {
            var x = 0f;
            var w = Vector2.zero;
            LineFeed<T>(ref x, ref y, currentLineMaxHeight, ref currentLineMaxHeight, ref linedRectTransforms, ref w);
        }
    }
}