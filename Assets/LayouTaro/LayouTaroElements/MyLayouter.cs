
using System;
using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;

public class MyLayouter : ILayouter
{
    List<RectTransform> lineContents = new List<RectTransform>();// 同じ行に入っている要素を整列させるために必要
    public void Layout(Vector2 size, out float originX, out float originY, GameObject rootObject, LTRootElement rootElement, LTElement[] elements)
    {
        originX = 0f;
        originY = 0f;

        var viewWidh = size.x;

        // boxのInsetとかを作ると、上下左右の余白とか作れそうだなー。
        // ここは起点になるので、必要であれば起点をいじっていこう。

        var currentLineMaxHeight = 0f;
        lineContents.Clear();

        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];

            var currentElementRectTrans = element.GetComponent<RectTransform>();
            var restWidth = viewWidh - originX;

            lineContents.Add(currentElementRectTrans);

            var type = element.GetLTElementType();
            switch (type)
            {
                case LTElementType.Image:
                    var imageElement = (ImageElement)element;

                    var rectSize = imageElement.RectSize();

                    if (restWidth < rectSize.x)// 同じ列にレイアウトできないので次の列に行く。
                    {
                        // 最後の追加要素である自分自身を取り出し、整列させる。
                        lineContents.RemoveAt(lineContents.Count - 1);
                        LineFeed(ref originX, ref originY, currentLineMaxHeight, ref currentLineMaxHeight, ref lineContents);
                        lineContents.Add(currentElementRectTrans);

                        // 位置をセット
                        currentElementRectTrans.anchoredPosition = new Vector2(originX, originY);
                    }
                    else
                    {
                        // 位置をセット
                        currentElementRectTrans.anchoredPosition = new Vector2(originX, originY);
                    }

                    // ジャストで埋まったら、次の行を作成する。
                    if (restWidth == rectSize.x)
                    {
                        LineFeed(ref originX, ref originY, currentElementRectTrans.sizeDelta.y, ref currentLineMaxHeight, ref lineContents);
                        continue;
                    }

                    ContinueLine(ref originX, rectSize.x, currentElementRectTrans.sizeDelta.y, ref currentLineMaxHeight);
                    break;
                case LTElementType.Text:
                    var newTailTextElement = (TextElement)element;
                    var contentText = newTailTextElement.Text();

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

                    var firstLineWidth = tmGeneratorLines[0].length;

                    // 文字を中央揃えではなく適正な位置にセットするためにラッピングを解除する。
                    textComponent.enableWordWrapping = false;

                    var currentFirstLineHeight = tmGeneratorLines[0].lineHeight;

                    var isHeadOfLine = originX == 0;
                    var isMultiLined = 1 < tmLineCount;


                    Debug.LogWarning("一文字も入らない、という可能性を考えてなかった、、、そのパターンもある。ミニマムサイズ決めちゃうか。その方が綺麗かも。");


                    var status = GetTextLayoutStatus(isHeadOfLine, isMultiLined);
                    switch (status)
                    {
                        case TextLayoutStatus.NotHeadAndSingle:
                        case TextLayoutStatus.HeadAndSingle:
                            // 全文を表示して終了
                            textComponent.text = contentText;
                            textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                            textComponent.rectTransform.sizeDelta = new Vector2(firstLineWidth, currentFirstLineHeight);

                            ContinueLine(ref originX, firstLineWidth, currentFirstLineHeight, ref currentLineMaxHeight);
                            break;

                        case TextLayoutStatus.NotHeadAndMulti:
                            // これは、この行 + HeadAndMultiの最大2コンテンツにできる。
                            var nextLineTextIndex = tmGeneratorLines[1].firstCharacterIndex;
                            var nextLineText = contentText.Substring(nextLineTextIndex);

                            // 現在の行のセット
                            textComponent.text = contentText.Substring(0, nextLineTextIndex);
                            textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                            textComponent.rectTransform.sizeDelta = new Vector2(firstLineWidth, currentFirstLineHeight);

                            var childOriginX = originX;

                            LineFeed(ref originX, ref originY, currentFirstLineHeight, ref currentLineMaxHeight, ref lineContents);// 文字コンテンツの高さ分改行する

                            // gotoを使って次の行に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidh;

                            // 次の行のコンテンツを入れる
                            contentText = nextLineText;
                            newTailTextElement = TextElement.GO(contentText);
                            newTailTextElement.transform.SetParent(element.transform);// 消しやすくするため、親にセットする

                            var yPosFromLinedParentY = -(Mathf.Abs(textComponent.rectTransform.anchoredPosition.y) + textComponent.rectTransform.sizeDelta.y);

                            // X表示位置を原点にずらす、Yは次のコンテンツの開始Y位置 = LineFeedで変更された親の位置に依存し、親の位置からoriginYを引いた値になる。
                            var newTailTextElementRectTrans = newTailTextElement.GetComponent<RectTransform>();
                            newTailTextElementRectTrans.anchoredPosition = new Vector2(-childOriginX, yPosFromLinedParentY);
                            continueContent = true;

                            // 追加する(次の処理で確実に消されるが、足しておかないと次の行頭複数行で-されるケースがあり詰む)
                            lineContents.Add(newTailTextElementRectTrans);
                            goto NextLine;

                        case TextLayoutStatus.HeadAndMulti:
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
                                lineStrs[j] = contentText.Substring(lInfo.firstCharacterIndex, lInfo.lastCharacterIndex - lInfo.firstCharacterIndex);
                            }

                            textComponent.text = string.Join("\n", lineStrs);

                            textComponent.rectTransform.sizeDelta = new Vector2(restWidth, totalHeight);

                            // なんらかの続きの文字コンテンツである場合、そのコンテンツの子になっているので位置情報を調整しない。
                            if (continueContent)
                            {
                                continueContent = false;

                                originX = textInfos.lineInfo[lineCount - 1].length;// 最終ラインのx位置を次のコンテンツに使う
                                originY -= (totalHeight - textInfos.lineInfo[lineCount - 1].lineHeight);// 最終行
                                restWidth = viewWidh - textInfos.lineInfo[lineCount - 1].length;
                                currentLineMaxHeight = textInfos.lineInfo[lineCount - 1].lineHeight;
                            }
                            else
                            {
                                textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                                // まだ適当
                                Debug.Log("まだ適当");
                            }

                            break;
                    }
                    break;
                case LTElementType.Box:
                    throw new Exception("unsupported layout:" + type);

                case LTElementType.Button:
                    Debug.LogError("まだButtonがない");
                    break;
                default:
                    break;
            }
        }

        // レイアウト終了後、最後の列の要素を並べる。
        foreach (var element in lineContents)
        {
            var rectTrans = element.GetComponent<RectTransform>();
            var elementHeight = rectTrans.sizeDelta.y;
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, originY - (currentLineMaxHeight - elementHeight) / 2);
        }
        lineContents.Clear();

        Debug.LogWarning("この辺まとめ終わったら、抽象化すると良さそう。textとrectで抽象化できそう。");
    }

    private void LineFeed(ref float x, ref float y, float currentElementHeight, ref float currentLineMaxHeight, ref List<RectTransform> linedElements)
    {
        // 列の概念の中で最大の高さを持つ要素を中心に、それより小さい要素をy軸に対して整列させる
        var lineHeight = Mathf.Max(currentElementHeight, currentLineMaxHeight);
        foreach (var rectTrans in linedElements)
        {
            var elementHeight = rectTrans.sizeDelta.y;
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, -(lineHeight - elementHeight) / 2);
        }
        linedElements.Clear();

        x = 0;
        y -= lineHeight;
        currentLineMaxHeight = 0f;
    }

    private void ContinueLine(ref float x, float newX, float currentElementHeight, ref float currentLineMaxHeight)
    {
        x += newX;
        currentLineMaxHeight = Mathf.Max(currentElementHeight, currentLineMaxHeight);
    }

    private enum TextLayoutStatus
    {
        HeadAndSingle,
        HeadAndMulti,
        NotHeadAndSingle,
        NotHeadAndMulti
    }

    private TextLayoutStatus GetTextLayoutStatus(bool isHeadOfLine, bool isMultiLined)
    {
        if (isHeadOfLine && isMultiLined)
        {
            return TextLayoutStatus.HeadAndMulti;
        }
        else if (isHeadOfLine && !isMultiLined)
        {
            return TextLayoutStatus.HeadAndSingle;
        }
        else if (!isHeadOfLine && !isMultiLined)
        {
            return TextLayoutStatus.NotHeadAndSingle;
        }
        return TextLayoutStatus.NotHeadAndMulti;
    }

    public void UpdateValues(LTElement[] elements, Dictionary<LTElementType, object> updateValues)
    {
        foreach (var e in elements)
        {
            switch (e.GetLTElementType())
            {
                case LTElementType.Image:
                    var i = (ImageElement)e;

                    // get value from updateValues and cast to the type what you set.
                    var p = updateValues[LTElementType.Image] as Image;
                    i.Image = p;
                    break;

                default:
                    break;
            }
        }
    }
}