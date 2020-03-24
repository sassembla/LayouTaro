
using System;
using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;

public class MyLayouter : ILayouter
{
    public void Layout(Vector2 viewSize, out float originX, out float originY, GameObject rootObject, LTRootElement rootElement, LTElement[] elements, ref float currentLineMaxHeight, ref List<RectTransform> lineContents)
    {
        originX = 0f;
        originY = 0f;

        var viewWidth = viewSize.x - 20;

        // MyLayputはrootとしてboxがくる前提で作られている、という想定のサンプル
        var root = rootObject.GetComponent<BoxElement>();
        var rootTrans = root.GetComponent<RectTransform>();

        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];

            var currentElementRectTrans = element.GetComponent<RectTransform>();
            var restWidth = viewWidth - originX;

            lineContents.Add(currentElementRectTrans);

            var type = element.GetLTElementType();
            switch (type)
            {
                case LTElementType.Image:
                    var imageElement = (ImageElement)element;

                    BasicLayoutFunctions.RectLayout(
                        imageElement,
                        currentElementRectTrans,
                        imageElement.RectSize(),
                        ref originX,
                        ref originY,
                        ref restWidth,
                        ref currentLineMaxHeight,
                        ref lineContents
                    );
                    break;
                case LTElementType.Text:
                    var newTailTextElement = (TextElement)element;
                    var contentText = newTailTextElement.Text();

                    BasicLayoutFunctions.TextLayout(
                        newTailTextElement,
                        contentText,
                        currentElementRectTrans,
                        viewWidth,
                        ref originX,
                        ref originY,
                        ref restWidth,
                        ref currentLineMaxHeight,
                        ref lineContents
                    );
                    break;
                case LTElementType.Button:
                    var buttonElement = (ButtonElement)element;

                    BasicLayoutFunctions.RectLayout(
                        buttonElement,
                        currentElementRectTrans,
                        buttonElement.RectSize(),
                        ref originX,
                        ref originY,
                        ref restWidth,
                        ref currentLineMaxHeight,
                        ref lineContents
                    );
                    break;

                case LTElementType.Box:
                    throw new Exception("unsupported layout:" + type);// 子のレイヤーにBoxが来るのを許可しない。

                default:
                    Debug.LogError("unsupported element type:" + type);
                    break;
            }
        }

        // 自分で最終行のレイアウトを行う
        BasicLayoutFunctions.LayoutLastLine(ref originY, currentLineMaxHeight, ref lineContents);

        // サイズを調整する
        rootTrans.sizeDelta = new Vector2(viewWidth + 10, Mathf.Abs(originY) + 20);

        foreach (var e in elements)
        {
            var rectTrans = e.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x + 10, rectTrans.anchoredPosition.y - 10);
        }
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
                case LTElementType.Text:
                    var t = (TextElement)e;

                    // get value from updateValues and cast to the type what you set.
                    var tVal = updateValues[LTElementType.Text] as string;
                    t.TextContent = tVal;
                    break;

                default:
                    break;
            }
        }
    }
}