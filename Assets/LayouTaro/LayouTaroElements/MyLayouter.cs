
using System;
using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;

public class MyLayouter : ILayouter
{
    public void Layout(Vector2 size, out float originX, out float originY, GameObject rootObject, LTRootElement rootElement, LTElement[] elements)
    {
        originX = 0f;
        originY = 0f;

        var viewWidh = size.x;

        // boxのInsetとかを作ると、上下左右の余白とか作れそうだなー。
        // ここは起点になるので、必要であれば起点をいじっていこう。

        var currentLineMaxHeight = 0f;

        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];

            var rectTrans = element.GetComponent<RectTransform>();
            var restWidth = viewWidh - originX;

            var type = element.GetLTElementType();
            switch (type)
            {
                case LTElementType.Image:
                    var imageElement = (ImageElement)element;

                    var rectSize = imageElement.RectSize();

                    if (restWidth < rectSize.x)// 同じ列にレイアウトできないので次の列に行く。
                    {
                        LineFeed(ref originX, ref originY, rectSize.y, ref currentLineMaxHeight);
                    }

                    // 位置をセット
                    rectTrans.anchoredPosition = new Vector2(originX, originY);

                    // ジャストで埋まったら、次の行に行く。
                    if (restWidth == rectSize.x)
                    {
                        LineFeed(ref originX, ref originY, rectTrans.sizeDelta.y, ref currentLineMaxHeight);
                        continue;
                    }

                    ContinueLine(ref originX, rectSize.x, rectTrans.sizeDelta.y, ref currentLineMaxHeight);
                    break;
                case LTElementType.Text:
                    var textElement = (TextElement)element;
                    var contentText = textElement.Text();

                NextLine:
                    var textComponent = textElement.GetComponent<TMPro.TextMeshProUGUI>();
                    var textPrefabHeight = textComponent.rectTransform.sizeDelta.y;

                    // prefabでAlignmentをセットしていても、prefabからロードした段階でLeft + Middleになるという設定が消えている。ここでは、Leftをセットすることで、なんでかLeft + Middleに戻るようにする。
                    textComponent.alignment = TMPro.TextAlignmentOptions.Left;

                    // wordWrappingを可能にすると、表示はともかく実際にこの行にどれだけの文字が入っているか判断できる。
                    textComponent.enableWordWrapping = true;
                    textComponent.text = contentText;

                    textComponent.rectTransform.sizeDelta = new Vector2(restWidth, float.PositiveInfinity);
                    var textInfos = textComponent.GetTextInfo(contentText);

                    var tmGeneratorLines = textInfos.lineInfo;
                    var lineSpacing = textComponent.lineSpacing;
                    var tmLineCount = textInfos.lineCount;

                    var firstLineWidth = tmGeneratorLines[0].length;

                    // 文字を中央ゾロ絵ではなく適正な位置にセットするためにラッピングを解除する。
                    textComponent.enableWordWrapping = false;

                    var isHeadOfLine = originX == 0;
                    var isMultiLined = 1 < tmLineCount;

                    var status = GetTextLayoutStatus(isHeadOfLine, isMultiLined);
                    switch (status)
                    {
                        case TextLayoutStatus.HeadAndSingle:
                        case TextLayoutStatus.NotHeadAndSingle:
                            // 全文を表示して終了
                            textComponent.text = contentText;
                            textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                            textComponent.rectTransform.sizeDelta = new Vector2(firstLineWidth, textPrefabHeight);

                            ContinueLine(ref originX, firstLineWidth, textPrefabHeight, ref currentLineMaxHeight);
                            break;
                        case TextLayoutStatus.HeadAndMulti:
                            // 41から引くと、9。これを2倍すると、18。かなり近い値になる。このへんかなー。

                            var lineCount = textInfos.lineCount;
                            var lineStrs = new string[lineCount];

                            // 改行を入れる
                            for (var j = 0; j < lineCount; j++)
                            {
                                var lInfo = textInfos.lineInfo[j];
                                lineStrs[j] = contentText.Substring(lInfo.firstCharacterIndex, lInfo.lastCharacterIndex - lInfo.firstCharacterIndex);
                            }

                            Debug.Log("textInfos.lineInfo[0].lineHeight:" + textInfos.lineInfo[0].lineHeight);
                            // たぶんもっと難しい式だ。これは諦めるかなー。フォントによって違うだろうし。
                            // textComponent.lineSpacing = (textPrefabHeight - textInfos.lineInfo[0].lineHeight) * 2;

                            var totalHeight = textPrefabHeight * lineCount;

                            textComponent.text = string.Join("\n", lineStrs);
                            textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                            textComponent.rectTransform.sizeDelta = new Vector2(restWidth, totalHeight);

                            // ここでの高さ系はミスってるな
                            originX = textInfos.lineInfo[lineCount - 1].length;
                            originY -= textPrefabHeight * lineCount;
                            break;
                        case TextLayoutStatus.NotHeadAndMulti:
                            var height = textInfos.lineInfo[0].lineHeight;
                            var baseLine = textInfos.lineInfo[0].baseline;
                            var asscend = textInfos.lineInfo[0].ascender;

                            // これは2コンテンツにできる
                            var nextLineTextIndex = tmGeneratorLines[1].firstCharacterIndex;
                            var nextLineText = contentText.Substring(nextLineTextIndex);

                            // 現在の行のセット
                            textComponent.text = contentText.Substring(0, nextLineTextIndex);
                            textComponent.rectTransform.anchoredPosition = new Vector2(originX, originY);
                            textComponent.rectTransform.sizeDelta = new Vector2(firstLineWidth, textPrefabHeight);

                            LineFeed(ref originX, ref originY, textPrefabHeight, ref currentLineMaxHeight);// 文字コンテンツの高さ分改行する

                            // gotoを使って次の行に行くので、計算に使う残り幅をビュー幅へとセットする。
                            restWidth = viewWidh;

                            // 次の行のコンテンツを入れる
                            contentText = nextLineText;
                            textElement = TextElement.GO(contentText);
                            textElement.transform.SetParent(rootObject.transform);
                            goto NextLine;
                    }
                    break;
                case LTElementType.Box:
                    throw new Exception("unsupported layout:" + type);

                case LTElementType.Button:
                    Debug.LogError("まだない");
                    break;
                default:
                    break;
            }
        }
    }

    private void LineFeed(ref float x, ref float y, float currentElementHeight, ref float currentLineMaxHeight)
    {
        x = 0;
        y -= Mathf.Max(currentElementHeight, currentLineMaxHeight);
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