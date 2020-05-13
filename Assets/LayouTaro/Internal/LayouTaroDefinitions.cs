using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UILayouTaro
{
    public abstract class LTElement : MonoBehaviour
    {
        public abstract LTElementType GetLTElementType();
    }

    public abstract class LTRootElement : LTElement
    {
        public abstract LTElement[] GetLTElements();
    }

    public interface ILayouter
    {
        void Layout(Vector2 size, out float originX, out float originY, GameObject rootObject, LTRootElement rootElement, LTElement[] elements, ref float currentLineMaxHeight, ref List<RectTransform> lineContents);
        void UpdateValues(LTElement[] elements, Dictionary<LTElementType, object> updateValues);
    }


    public abstract class LTAsyncElement : MonoBehaviour
    {
        public abstract LTElementType GetLTElementType();
    }

    public abstract class LTAsyncLoadableElement : LTAsyncElement
    {
        public bool IsLoading;
    }

    public abstract class LTAsyncRootElement : LTAsyncElement
    {
        public abstract LTAsyncElement[] GetLTElements();
    }


    public interface IAsyncLayouter
    {
        List<AsyncLayoutOperation> LayoutAsync<T>(ref Vector2 size, out float originX, out float originY, GameObject rootObject, LTAsyncRootElement rootElement, LTAsyncElement[] elements, ref float currentLineMaxHeight, ref List<RectTransform> lineContents) where T : IMissingSpriteCache, new();
        void AfterLayout(Vector2 viewSize, float originX, float originY, GameObject rootObject, LTAsyncRootElement rootElement, LTAsyncElement[] elements, ref float currentLineMaxHeight, ref List<RectTransform> lineContents);
        void UpdateValuesAsync(LTAsyncElement[] elements, Dictionary<LTElementType, object> updateValues);
    }

    public interface ILayoutableRect
    {
        Vector2 RectSize();
    }

    public interface ILayoutableText
    {
        string Text();

        GameObject GenerateGO(string text);
    }

    public interface IMissingSpriteCache
    {
        void LoadMissingText(
            string fontName,
            float fontSize,
            float requestWidth,
            float requestHeight,
            string text,
            Action<IEnumerator> onRequest,
            Action<Texture2D> onSucceeded,
            Action onFailed
        );

        void LoadMissingEmojiOrMark(
            string fontName,
            float fontSize,
             float requestWidth,
             float requestHeight,
             uint codePoint,
            Action<IEnumerator> onRequest,
            Action<Texture2D> onSucceeded,
            Action onFailed
        );
    }
}