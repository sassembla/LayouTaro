
using System;
using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;

public class MyLayouter : ILayouter
{
    /*
        子要素をレイアウトし、親要素が余白ありでそれを包む。
    */
    public void Layout(Vector2 viewSize, out float originX, out float originY, GameObject rootObject, LTRootElement rootElement, LTElement[] elements, ref float currentLineMaxHeight, ref List<RectTransform> lineContents)
    {
        var outsideSpacing = 10f;
        originX = 0f;
        originY = 0f;

        var originalViewWidth = viewSize.x;

        var viewWidth = viewSize.x - outsideSpacing * 2;// 左右の余白分を引く

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
                        viewWidth,
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
                        viewWidth,
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

        // 最終行のレイアウトを行う
        BasicLayoutFunctions.LayoutLastLine(ref originY, currentLineMaxHeight, ref lineContents);

        // boxのサイズを調整する
        rootTrans.sizeDelta = new Vector2(originalViewWidth, Mathf.Abs(originY) + outsideSpacing * 2);// オリジナル幅で、高さに対して2倍分の余白を足す。

        // 子要素の余白分の移動
        foreach (var e in elements)
        {
            var rectTrans = e.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x + outsideSpacing, rectTrans.anchoredPosition.y - outsideSpacing);// ルートの下のエレメントの要素をスペース分移動する。yは-なので-する。
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