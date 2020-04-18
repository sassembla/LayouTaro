using System;
using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;

public class BasicAsyncLayouter : ILayouterAsync
{
    /*
        子要素をレイアウトし、親要素が余白ありでそれを包む。
        outを使いたいから、非同期な計算を行う実行体をここから返すようにする。
    */
    public List<AsyncLayoutOperation> LayoutAsync(Vector2 viewSize, out float originX, out float originY, GameObject rootObject, LTAsyncRootElement rootElement, LTAsyncElement[] elements, ref float currentLineMaxHeight, ref List<RectTransform> lineContents)
    {
        var outsideSpacing = 10f;

        originX = 0f;
        originY = 0f;

        var originalViewWidth = viewSize.x;

        var viewWidth = viewSize.x - outsideSpacing * 2;// 左右の余白分を引く

        // MyLayoutはrootとしてboxがくる前提で作られている、という想定のサンプル
        var root = rootObject.GetComponent<AsyncBoxElement>();
        var rootTrans = root.GetComponent<RectTransform>();

        var layoutOps = new List<AsyncLayoutOperation>();

        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];

            var currentElementRectTrans = element.GetComponent<RectTransform>();
            var restWidth = viewWidth - originX;

            var type = element.GetLTElementType();
            switch (type)
            {
                case LTElementType.AsyncImage:
                    var imageElement = (AsyncImageElement)element;

                    // 概念的に、後でレイアウトする対象をレイアウト処理順にAddしている。
                    layoutOps.Add(
                        BasicAsyncLayoutFunctions.RectLayoutAsync(
                            imageElement,
                            currentElementRectTrans,
                            imageElement.RectSize(),
                            viewWidth,
                            ref originX,
                            ref originY,
                            ref restWidth,
                            ref currentLineMaxHeight,
                            ref lineContents
                        )
                    );
                    break;
                case LTElementType.AsyncText:
                    var newTailTextElement = (AsyncTextElement)element;
                    var contentText = newTailTextElement.Text();

                    layoutOps.Add(
                        BasicAsyncLayoutFunctions.TextLayoutAsync(
                            newTailTextElement,
                            contentText,
                            currentElementRectTrans,
                            viewWidth,
                            ref originX,
                            ref originY,
                            ref restWidth,
                            ref currentLineMaxHeight,
                            ref lineContents
                        )
                    );
                    break;
                case LTElementType.AsyncButton:
                    var buttonElement = (AsyncButtonElement)element;

                    layoutOps.Add(
                        BasicAsyncLayoutFunctions.RectLayoutAsync(
                            buttonElement,
                            currentElementRectTrans,
                            buttonElement.RectSize(),
                            viewWidth,
                            ref originX,
                            ref originY,
                            ref restWidth,
                            ref currentLineMaxHeight,
                            ref lineContents
                        )
                    );

                    break;

                case LTElementType.AsyncBox:
                    throw new Exception("unsupported layout:" + type);// 子のレイヤーにBoxが来るのを許可しない。

                default:
                    Debug.LogError("unsupported element type:" + type);
                    break;
            }
        }

        return layoutOps;
    }

    /*
        layout後、LayouTaroから呼ばれる
    */
    public void AfterLayout(Vector2 viewSize, float originX, float originY, GameObject rootObject, LTAsyncRootElement rootElement, LTAsyncElement[] elements, ref float currentLineMaxHeight, ref List<RectTransform> lineContents)
    {
        // 最終行の整列を行う
        BasicLayoutFunctions.LayoutLastLine(ref originY, currentLineMaxHeight, ref lineContents);

        var outsideSpacing = 10f;

        var rootTrans = rootObject.GetComponent<RectTransform>();

        // boxのサイズを調整する
        rootTrans.sizeDelta = new Vector2(viewSize.x, Mathf.Abs(originY) + outsideSpacing * 2);// オリジナル幅で、高さに対して2倍分の余白を足す。

        // 子要素の余白分の移動
        foreach (var e in elements)
        {
            var rectTrans = e.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x + outsideSpacing, rectTrans.anchoredPosition.y - outsideSpacing);// ルートの下のエレメントの要素をスペース分移動する。yは-なので-する。
        }
    }
}