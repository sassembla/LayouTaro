using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UILayouTaro
{
    public class LayouTaro
    {

        public static GameObject Layout<T>(Transform parent, Vector2 size, GameObject rootObject, ILayouter layouter) where T : LTRootElement
        {
            Debug.Assert(parent.GetComponent<Canvas>() != null, "should set parent transform which contains Canvas. this limitation is caused by spec of TextMesh Pro.");
            var originX = 0f;
            var originY = 0f;

            var rootElement = rootObject.GetComponent<T>();
            var elements = rootElement.GetLTElements();

            var rootRect = rootObject.GetComponent<RectTransform>();

            // 親の基礎サイズをセット
            rootRect.sizeDelta = size;

            // set parent.
            foreach (var element in elements)
            {
                element.gameObject.transform.SetParent(rootObject.transform);
            }

            // ここでCanvas要素にセットしちゃう(でないとTMProのinfoが取れない。)
            rootObject.transform.SetParent(parent);

            var lineContents = new List<RectTransform>();// 同じ行に入っている要素を整列させるために使用するリスト
            var currentLineMaxHeight = 0f;

            layouter.Layout(size, out originX, out originY, rootObject, rootElement, elements, ref currentLineMaxHeight, ref lineContents);

            lineContents.Clear();

            // var lastHeight = originY + elements[elements.Length - 1].GetComponent<RectTransform>().sizeDelta.y;

            return rootObject;
        }

        public static GameObject RelayoutWithUpdate<T>(Vector2 size, GameObject rootObject, Dictionary<LTElementType, object> updateValues, ILayouter layouter) where T : LTRootElement
        {
            var originX = 0f;
            var originY = 0f;

            var rootElement = rootObject.GetComponent<T>();
            var elements = rootElement.GetLTElements();
            foreach (var element in elements)
            {
                if (element is ILayoutableText)
                {
                    if (0 < element.transform.childCount)
                    {
                        var count = element.transform.childCount;
                        for (var i = 0; i < count; i++)
                        {
                            // get first child.
                            var child = element.transform.GetChild(0);
                            child.gameObject.SetActive(false);
                            child.transform.SetParent(null);
                            GameObject.Destroy(child.gameObject);
                        }
                    }
                }

                var rectTrans = element.GetComponent<RectTransform>();
                rectTrans.anchoredPosition = Vector2.zero;
            }

            layouter.UpdateValues(elements, updateValues);

            var lineContents = new List<RectTransform>();// 同じ行に入っている要素を整列させるために使用するリスト
            var currentLineMaxHeight = 0f;

            layouter.Layout(size, out originX, out originY, rootObject, rootElement, elements, ref currentLineMaxHeight, ref lineContents);

            lineContents.Clear();

            return rootObject;
        }

        internal static Action<char[]> _OnMissingCharacter = chs => { };

        public static void SetOnMissingCharacterFound(Action<char[]> OnMissingCharacterFound)
        {
            _OnMissingCharacter = OnMissingCharacterFound;
        }


        public static IEnumerator LayoutAsync<T>(Transform parent, Vector2 size, GameObject rootObject, ILayouterAsync layouter) where T : LTRootElement
        {
            Debug.Assert(parent.GetComponent<Canvas>() != null, "should set parent transform which contains Canvas. this limitation is caused by spec of TextMesh Pro.");
            var originX = 0f;
            var originY = 0f;

            var rootElement = rootObject.GetComponent<T>();
            var elements = rootElement.GetLTElements();

            var rootRect = rootObject.GetComponent<RectTransform>();

            // 親の基礎サイズをセット
            rootRect.sizeDelta = size;

            // set parent.
            foreach (var element in elements)
            {
                element.gameObject.transform.SetParent(rootObject.transform);
            }

            // ここでCanvas要素にセットしちゃう(でないとTMProのinfoが取れない。)
            rootObject.transform.SetParent(parent);

            var lineContents = new List<RectTransform>();// 同じ行に入っている要素を整列させるために使用するリスト
            var currentLineMaxHeight = 0f;

            var opId = Guid.NewGuid().ToString();

            // この下のレイヤーで全ての非同期layout処理を集める。
            var layoutOps = layouter.LayoutAsync(size, out originX, out originY, rootObject, rootElement, elements, ref currentLineMaxHeight, ref lineContents);

            var layouted = false;
            RefObject resultRefObject = null;

            Debug.LogWarning("layoutOps[0]をここでいきなりいっぺん回す、というのができるといい気はしている。");

            AsyncLayoutRunner.LaunchLayoutOps(
                opId,
                layoutOps,
                pos =>
                {
                    layouted = true;
                    resultRefObject = pos;
                }
            );

            while (!layouted)
            {
                yield return null;
            }

            layouter.AfterLayout(size, resultRefObject.originX, resultRefObject.originY, rootObject, rootElement, elements, ref resultRefObject.currentLineMaxHeight, ref resultRefObject.lineContents);

            lineContents.Clear();

            yield break;
        }

    }
}