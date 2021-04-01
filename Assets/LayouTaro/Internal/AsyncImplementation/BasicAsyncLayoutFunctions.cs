
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UILayouTaro
{
    public class ParameterReference
    {
        public string id;// 判別用

        public float originX;
        public float originY;
        public float restWidth;
        public float currentLineMaxHeight;
        public List<RectTransform> lineContents;

        public Vector2 wrappedSize;

        public ParameterReference(float originX, float originY, float restWidth, float currentLineMaxHeight, List<RectTransform> lineContents)
        {
            this.id = Guid.NewGuid().ToString();

            this.originX = originX;
            this.originY = originY;
            this.restWidth = restWidth;
            this.currentLineMaxHeight = currentLineMaxHeight;
            this.lineContents = lineContents;
        }

        public override string ToString()
        {
            return " originX:" + originX + " originY:" + originY + " restWidth:" + restWidth + " currentLineMaxHeight:" + currentLineMaxHeight;
        }
    }


    public static class BasicAsyncLayoutFunctions
    {
        public static AsyncLayoutOperation TextLayoutAsync<T, U>(T textElement, string contentText, RectTransform rectTrans, float viewWidth, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents) where T : LTAsyncElement, ILayoutableText where U : IMissingSpriteCache, new()
        {
            Debug.Assert(rectTrans.pivot.x == 0 && rectTrans.pivot.y == 1 && rectTrans.anchorMin.x == 0 && rectTrans.anchorMin.y == 1 && rectTrans.anchorMax.x == 0 && rectTrans.anchorMax.y == 1, "rectTransform for BasicAsyncLayoutFunctions.TextLayoutAsync should set pivot to 0,1 and anchorMin 0,1 anchorMax 0,1.");
            Debug.Assert(textElement.transform.childCount == 0, "BasicAsyncLayoutFunctions.TextLayoutAsync not allows text element which has child.");

            var refs = new ParameterReference(originX, originY, restWidth, currentLineMaxHeight, lineContents);

            // 文字がnullか空の場合、空のAsyncOpを返す。
            if (string.IsNullOrEmpty(contentText))
            {
                return new AsyncLayoutOperation(
                    rectTrans,
                    refs,
                    () =>
                    {
                        return (false, refs);
                    }
                );
            }

            var cor = _TextLayoutAsync<T, U>(textElement, contentText, rectTrans, viewWidth, refs);
            return new AsyncLayoutOperation(
                rectTrans,
                refs,
                () =>
                {
                    var cont = cor.MoveNext();
                    return (cont, refs);
                }
            );
        }

        public static AsyncLayoutOperation RectLayoutAsync<T>(LTAsyncElement rectElement, RectTransform rectTrans, Vector2 rectSize, float viewWidth, ref float originX, ref float originY, ref float restWidth, ref float currentLineMaxHeight, ref List<RectTransform> lineContents) where T : IMissingSpriteCache, new()
        {
            var refs = new ParameterReference(originX, originY, restWidth, currentLineMaxHeight, lineContents);

            var cor = _RectLayoutAsync<T>(rectElement, rectTrans, rectSize, viewWidth, refs);
            return new AsyncLayoutOperation(
                rectTrans,
                refs,
                () =>
                {
                    var cont = cor.MoveNext();
                    return (cont, refs);
                }
            );
        }

        /*
            内部的な実装部
            どこかから先をstaticではないように作ると良さそう。キャッシュが切れる。まあTextにセットしてあるフォント単位でキャッシュ作っちゃうけどね。
        */
        private static IEnumerator _TextLayoutAsync<T, U>(T textElement, string contentText, RectTransform rectTrans, float viewWidth, ParameterReference refs) where T : LTAsyncElement, ILayoutableText where U : IMissingSpriteCache, new()
        {
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
                textComponent.rectTransform.sizeDelta = new Vector2(refs.restWidth, Screen.height);
                textInfos = textComponent.GetTextInfo(contentText);

                // 文字を中央揃えではなく適正な位置にセットするためにラッピングを解除する。
                textComponent.enableWordWrapping = false;
            }

            // サロゲート文字が含まれている場合、画像と文字に分けてレイアウトを行う。
            if (BasicAsyncLayoutFunctions.IsContainsSurrogatePairOrSprite(contentText))
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
                refs.lineContents.RemoveAt(refs.lineContents.Count - 1);

                textComponent.rectTransform.sizeDelta = new Vector2(refs.restWidth, 0);// 高さが0で問題ない。

                // この内部で全てのレイアウトを終わらせる。
                var cor = LayoutContentWithEmojiAsync<T, U>(textElement, contentText, viewWidth, refs);
                while (cor.MoveNext())
                {
                    yield return null;
                }

                yield break;
            }

            var fontAsset = textComponent.font;
            if (!TextLayoutDefinitions.TMPro_CheckIfTextCharactersExist(fontAsset, contentText))
            {
                textComponent.text = string.Empty;

                // 今後のレイアウトに自分自身を巻き込まないように、レイアウトから自分自身を取り外す
                refs.lineContents.RemoveAt(refs.lineContents.Count - 1);

                textComponent.rectTransform.sizeDelta = new Vector2(refs.restWidth, 0);// 高さが0で問題ない。

                // missingを含んでいるので、内部でテキストとmissingに分解、レイアウトする。
                var cor = LayoutContentWithMissingTextAsync<T, U>(textElement, contentText, viewWidth, refs);
                while (cor.MoveNext())
                {
                    yield return null;
                }

                yield break;
            }

            var tmGeneratorLines = textInfos.lineInfo;
            var lineSpacing = textComponent.lineSpacing;
            var tmLineCount = textInfos.lineCount;

            var firstLine = tmGeneratorLines[0];
            var currentFirstLineWidth = firstLine.length;
            var currentFirstLineHeight = firstLine.lineHeight;

            var isHeadOfLine = refs.originX == 0;
            var isMultiLined = 1 < tmLineCount;

            /*
                予定している1行目の文字の幅が予定幅を超えている = オーバーフローしている場合、次のケースがある
                ・文字列の1行目の末尾がたまたま幅予算を超えてレイアウトされた

                この場合、溢れたケースとして文字列の長さを調整してレイアウトを行う。
            */
            var isTextOverflow = (viewWidth < refs.originX + currentFirstLineWidth);


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
                var totalWidthWithSpaces = refs.originX + currentFirstLineWidth + (singleWidthOfWhiteSpace * numEndWhiteSpaceCount);

                // 画面の幅と比較し、小さい方をとる。
                // これは、whitespaceを使ってレイアウトした場合、TMProがリクエスト幅を超えたぶんのwhitespaceの計算をサボって、whitespaceではない文字が来た時点でその文字を頭とする改行を行うため。
                // 最大でも画面幅になるようにする。
                var maxWidth = Mathf.Min(viewWidth, totalWidthWithSpaces);

                // 行送りが発生しているため、この行の値の幅の更新はもう起きない。そのため、ここでwrappedSize.xを更新する。
                refs.wrappedSize.x = Mathf.Max(refs.wrappedSize.x, maxWidth);
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
                            refs.restWidth = viewWidth - currentFirstLineWidth;
                            refs.currentLineMaxHeight = currentFirstLineHeight;
                        }
                        else
                        {
                            textComponent.rectTransform.anchoredPosition = new Vector2(refs.originX, refs.originY);
                        }

                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        ContinueLine(ref refs.originX, refs.originY, currentFirstLineWidth, currentFirstLineHeight, ref refs.currentLineMaxHeight, ref refs.wrappedSize);
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

                        textComponent.rectTransform.anchoredPosition = new Vector2(refs.originX, refs.originY);
                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        var childOriginX = refs.originX;
                        var currentTotalLineHeight = LineFeed<LTAsyncRootElement>(ref refs.originX, ref refs.originY, currentFirstLineHeight, ref refs.currentLineMaxHeight, ref refs.lineContents, ref refs.wrappedSize);// 文字コンテンツの高さ分改行する

                        // 次の行のコンテンツをこのコンテンツの子として生成するが、レイアウトまでを行わず次の行の起点の計算を行う。
                        // ここで全てを計算しない理由は、この処理の結果、複数種類のレイアウトが発生するため、ここで全てを書かない方が変えやすい。
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            refs.restWidth = viewWidth;

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

                            // X表示位置を原点にずらす、Yは次のコンテンツの開始Y位置 = LineFeed<LTAsyncRootElement>で変更された親の位置に依存し、親の位置からoriginYを引いた値になる。
                            var newTailTextElementRectTrans = nextLineTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(-childOriginX, yPosFromLinedParentY);

                            // テキスト特有の継続したコンテンツ扱いする。
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            refs.lineContents.Add(newTailTextElementRectTrans);

                            // 上書きを行う
                            textElement = nextLineTextElement;
                            contentText = nextLineText;
                            goto NextLine;
                        }
                    }

                case TextLayoutStatus.HeadAndMulti:
                    {
                        // このコンテンツは矩形で、行揃えの影響を受けないため、明示的に行から取り除く。
                        refs.lineContents.RemoveAt(refs.lineContents.Count - 1);

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

                        textComponent.rectTransform.sizeDelta = new Vector2(refs.restWidth, rectHeight);

                        // 幅の最大値を取得
                        var max = Mathf.Max(textComponent.rectTransform.sizeDelta.x, textComponent.preferredWidth);

                        // サイズを更新する。
                        refs.wrappedSize.x = Mathf.Max(refs.wrappedSize.x, max);

                        // なんらかの続きの文字コンテンツである場合、そのコンテンツの子になっているので位置情報を調整しない。最終行を分割する。
                        if (continueContent)
                        {
                            // 別のコンテンツから継続している行はじめの処理なので、子をセットする前にここまでの分の改行を行う。
                            LineFeed<LTAsyncRootElement>(ref refs.originX, ref refs.originY, rectHeight, ref refs.currentLineMaxHeight, ref refs.lineContents, ref refs.wrappedSize);

                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            refs.restWidth = viewWidth;

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
                            refs.lineContents.Add(newBrotherTailTextElementRectTrans);

                            // 上書きを行う
                            textElement = nextLineTextElement;
                            contentText = lastLineText;
                            goto NextLine;
                        }


                        // 誰かの子ではないので、独自に自分の位置をセットする
                        textComponent.rectTransform.anchoredPosition = new Vector2(refs.originX, refs.originY);

                        // 最終行のコンテンツを入れる
                        var newTextElement = textElement.GenerateGO(lastLineText).GetComponent<T>();
                        newTextElement.transform.SetParent(rectTrans, false);// 消しやすくするため、この新規コンテンツを現在の要素の子にする

                        // 残りの行のサイズは最大化する
                        refs.restWidth = viewWidth;

                        // 次の行の行頭になる
                        refs.originX = 0;

                        // yは親の分移動する
                        refs.originY -= rectHeight;

                        {
                            // 新規オブジェクトはそのy位置を親コンテンツの高さを加えた値にセットする。
                            var newTailTextElementRectTrans = newTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(refs.originX, -rectHeight);

                            // 残りのデータをテキスト特有の継続したコンテンツ扱いする。
                            continueContent = true;

                            // 生成したコンテンツを次の行の要素へと追加する
                            refs.lineContents.Add(newTailTextElementRectTrans);

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

                        textComponent.rectTransform.anchoredPosition = new Vector2(refs.originX, refs.originY);
                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        var childOriginX = refs.originX;
                        var currentTotalLineHeight = LineFeed<LTAsyncRootElement>(ref refs.originX, ref refs.originY, currentFirstLineHeight, ref refs.currentLineMaxHeight, ref refs.lineContents, ref refs.wrappedSize
                        );// 文字コンテンツの高さ分改行する

                        // 次の行のコンテンツをこのコンテンツの子として生成するが、レイアウトまでを行わず次の行の起点の計算を行う。
                        // ここで全てを計算しない理由は、この処理の結果、複数種類のレイアウトが発生するため、ここで全てを書かない方が変えやすい。
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            refs.restWidth = viewWidth;

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
                            refs.lineContents.Add(newTailTextElementRectTrans);

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

                        textComponent.rectTransform.anchoredPosition = new Vector2(refs.originX, refs.originY);
                        textComponent.rectTransform.sizeDelta = new Vector2(currentFirstLineWidth, currentFirstLineHeight);

                        var childOriginX = refs.originX;
                        var currentTotalLineHeight = LineFeed<LTAsyncRootElement>(ref refs.originX, ref refs.originY, currentFirstLineHeight, ref refs.currentLineMaxHeight, ref refs.lineContents, ref refs.wrappedSize);// 文字コンテンツの高さ分改行する

                        // 次の行のコンテンツをこのコンテンツの子として生成するが、レイアウトまでを行わず次の行の起点の計算を行う。
                        // ここで全てを計算しない理由は、この処理の結果、複数種類のレイアウトが発生するため、ここで全てを書かない方が変えやすい。
                        {
                            // 末尾でgotoを使って次の行頭からのコンテンツの設置に行くので、計算に使う残り幅をビュー幅へとセットする。
                            refs.restWidth = viewWidth;

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
                            refs.lineContents.Add(newTailTextElementRectTrans);

                            // 上書きを行う
                            textElement = nextLineTextElement;
                            contentText = restText;
                            goto NextLine;
                        }
                    }
            }
            yield break;
        }

        private static IEnumerator LayoutContentWithEmojiAsync<T, U>(T textElement, string contentText, float viewWidth, ParameterReference refs) where T : LTAsyncElement, ILayoutableText where U : IMissingSpriteCache, new()
        {
            /*
                絵文字が含まれている文字列を、絵文字と矩形に分解、再構成を行う。絵文字を単に画像が入る箱としてRectLayoutに放り込む。

                自分自身を書き換えて、一連のコマンドを実行するようにする。
                文字がどう始まるかも含めて、今足されているlinedからは一度離反する。その上で一つ目のコンテンツを追加する。
            */
            var elementsWithEmoji = CollectSpriteAndTextElement<T, U>(textElement, contentText);

            for (var i = 0; i < elementsWithEmoji.Count; i++)
            {
                var element = elementsWithEmoji[i];
                var rectTrans = element.GetComponent<RectTransform>();
                refs.restWidth = viewWidth - refs.originX;
                refs.lineContents.Add(rectTrans);

                if (element is InternalAsyncEmojiRect)
                {
                    // emojiRectが入っている
                    var internalRectElement = (InternalAsyncEmojiRect)element;
                    var cor = _EmojiRectLayoutAsync<U>(internalRectElement, rectTrans, viewWidth, refs);

                    while (cor.MoveNext())
                    {
                        yield return null;
                    }

                    continue;
                }

                // ここに来るということは、T型が入っている。
                var internalTextElement = (T)element;
                var internalContentText = internalTextElement.Text();

                var textCor = _TextLayoutAsync<T, U>(internalTextElement, internalContentText, rectTrans, viewWidth, refs);
                while (textCor.MoveNext())
                {
                    yield return null;
                }
            }

            yield break;
        }

        // TODO: 名前変える
        private static List<LTAsyncElement> CollectSpriteAndTextElement<T, U>(T textElement, string contentText) where T : LTAsyncElement, ILayoutableText where U : IMissingSpriteCache, new()
        {
            var elementsWithEmoji = new List<LTAsyncElement>();

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
                        var emojiElement = InternalAsyncEmojiRect.New<T, U>(textElement, new Char[] { firstChar, nextChar });
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
                        var emojiElement = InternalAsyncEmojiRect.New<T, U>(textElement, new Char[] { firstChar });
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
                            var emojiElement = InternalAsyncEmojiRect.New<T, U>(textElement, new Char[] { firstChar });
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




        private static IEnumerator LayoutContentWithMissingTextAsync<T, U>(T textElement, string contentText, float viewWidth, ParameterReference refs) where T : LTAsyncElement, ILayoutableText where U : IMissingSpriteCache, new()
        {
            /*
                missingな文字が含まれている文字列を、文字と矩形に分解、再構成を行う。missing文字を単に画像が入る箱としてRectLayoutに放り込む。
                文字がどう始まるかも含めて、今足されているlinedからは一度離反する。その上で一つ目のコンテンツを追加する。
            */
            var elementsWithMissing = CollectMissingAndTextElement<T, U>(textElement, contentText);
            for (var i = 0; i < elementsWithMissing.Count; i++)
            {
                var element = elementsWithMissing[i];
                var rectTrans = element.GetComponent<RectTransform>();
                refs.restWidth = viewWidth - refs.originX;
                refs.lineContents.Add(rectTrans);

                if (element is InternalAsyncMissingTextRect)
                {
                    // missingTextRectが入っている
                    var internalRectElement = (InternalAsyncMissingTextRect)element;
                    var cor = _MissingTextRectLayoutAsync<U>(internalRectElement, rectTrans, viewWidth, refs);

                    while (cor.MoveNext())
                    {
                        yield return null;
                    }

                    continue;
                }

                // ここに来るということは、T型が入っている。
                var internalTextElement = (T)element;
                var internalContentText = internalTextElement.Text();

                var textCor = _TextLayoutAsync<T, U>(internalTextElement, internalContentText, rectTrans, viewWidth, refs);
                while (textCor.MoveNext())
                {
                    yield return null;
                }
            }

            yield break;
        }

        private static List<LTAsyncElement> CollectMissingAndTextElement<T, U>(T textElement, string contentText) where T : LTAsyncElement, ILayoutableText where U : IMissingSpriteCache, new()
        {
            var elementsWithMissing = new List<LTAsyncElement>();
            var font = textElement.GetComponent<TextMeshProUGUI>().font;

            var textStartIndex = 0;
            var length = 0;
            for (var i = 0; i < contentText.Length; i++)
            {
                var firstChar = contentText[i];

                var isExist = TextLayoutDefinitions.TMPro_CheckIfTextCharacterExist(font, firstChar);
                if (!isExist)
                {
                    // missingにぶち当たった。ここまでに用意されているテキストを取り出す
                    if (0 < length)
                    {
                        var currentText = contentText.Substring(textStartIndex, length);
                        var newTextElement = textElement.GenerateGO(currentText).GetComponent<T>();
                        newTextElement.transform.SetParent(textElement.transform, false);
                        elementsWithMissing.Add(newTextElement);
                    }

                    length = 0;

                    // missing文字確定。なので、箱的な要素として扱い、次の文字を飛ばす処理を行う。
                    var missingTextElement = InternalAsyncMissingTextRect.New<T, U>(textElement, contentText.Substring(i, 1));
                    elementsWithMissing.Add(missingTextElement);

                    // 文字は次から始まる、、かもしれない。
                    textStartIndex = i + 1;
                    continue;
                }

                // missingではないので文字として扱う
                length++;
            }

            // 残りの文字を足す
            if (0 < length)
            {
                var lastText = contentText.Substring(textStartIndex, length);
                var lastTextElement = textElement.GenerateGO(lastText).GetComponent<T>();
                lastTextElement.transform.SetParent(textElement.transform, false);
                elementsWithMissing.Add(lastTextElement);
            }

            return elementsWithMissing;
        }




        // TODO: Spriteにrenameする。
        private static IEnumerator _EmojiRectLayoutAsync<T>(InternalAsyncEmojiRect rectElement, RectTransform transform, float viewWidth, ParameterReference refs) where T : IMissingSpriteCache, new()
        {
            // ここでサイズを確定させている。レイアウト対象の画像が存在していれば、GOを作成したタイミングでサイズが確定されているが、
            // もし対象が見つかっていない場合、このelementは既に通信を行っている。その場合、ここで完了を待つことができる。

            while (rectElement.IsLoading)
            {
                yield return null;
            }

            var rectSize = rectElement.RectSize();
            var cor = _RectLayoutAsync<T>(rectElement, transform, rectSize, viewWidth, refs);
            while (cor.MoveNext())
            {
                yield return null;
            }
        }

        private static IEnumerator _MissingTextRectLayoutAsync<T>(InternalAsyncMissingTextRect rectElement, RectTransform transform, float viewWidth, ParameterReference refs) where T : IMissingSpriteCache, new()
        {
            // ここでサイズを確定させている。レイアウト対象の画像が存在していれば、GOを作成したタイミングでサイズが確定されているが、
            // もし対象が見つかっていない場合、このelementは既に通信を行っている。その場合、ここで完了を待つことができる。

            while (rectElement.IsLoading)
            {
                yield return null;
            }

            var rectSize = rectElement.RectSize();
            var cor = _RectLayoutAsync<T>(rectElement, transform, rectSize, viewWidth, refs);
            while (cor.MoveNext())
            {
                yield return null;
            }
        }

        private static IEnumerator _RectLayoutAsync<T>(LTAsyncElement rectElement, RectTransform transform, Vector2 rectSize, float viewWidth, ParameterReference refs) where T : IMissingSpriteCache, new()
        {
            Debug.Assert(transform.pivot.x == 0 && transform.pivot.y == 1 && transform.anchorMin.x == 0 && transform.anchorMin.y == 1 && transform.anchorMax.x == 0 && transform.anchorMax.y == 1, "rectTransform for LayouTaro should set pivot to 0,1 and anchorMin 0,1 anchorMax 0,1.");

            if (refs.restWidth < rectSize.x)// 同じ列にレイアウトできないので次の列に行く。
            {
                // 現在最後の追加要素である自分自身を取り出し、整列させる。
                refs.lineContents.RemoveAt(refs.lineContents.Count - 1);
                LineFeed<LTAsyncRootElement>(ref refs.originX, ref refs.originY, refs.currentLineMaxHeight, ref refs.currentLineMaxHeight, ref refs.lineContents, ref refs.wrappedSize);
                refs.lineContents.Add(transform);

                // 位置をセット
                transform.anchoredPosition = new Vector2(refs.originX, refs.originY);
            }
            else
            {
                // 位置をセット
                transform.anchoredPosition = new Vector2(refs.originX, refs.originY);
            }

            // ジャストで埋まったら、次の行を作成する。
            if (refs.restWidth == rectSize.x)
            {
                LineFeed<LTAsyncRootElement>(ref refs.originX, ref refs.originY, transform.sizeDelta.y, ref refs.currentLineMaxHeight, ref refs.lineContents, ref refs.wrappedSize);

                refs.restWidth = viewWidth;
                yield break;
            }

            ContinueLine(ref refs.originX, refs.originY, rectSize.x, transform.sizeDelta.y, ref refs.currentLineMaxHeight, ref refs.wrappedSize);
            refs.restWidth = viewWidth - refs.originX;
            yield break;
        }

        /*
            改行、行継続に関するレイアウトコントロール系
        */
        private static float LineFeed<T>(ref float x, ref float y, float currentElementHeight, ref float currentLineMaxHeight, ref List<RectTransform> linedElements, ref Vector2 wrappedSize) where T : LTAsyncRootElement
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


        public static void LayoutLastLine<T>(ref float y, float currentLineMaxHeight, ref List<RectTransform> linedRectTransforms) where T : LTAsyncRootElement
        {
            var x = 0f;
            var w = Vector2.zero;
            LineFeed<T>(ref x, ref y, currentLineMaxHeight, ref currentLineMaxHeight, ref linedRectTransforms, ref w);
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
    }
}
