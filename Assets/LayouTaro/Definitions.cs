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
        void Layout(Vector2 size, out float originX, out float originY, GameObject rootObject, LTRootElement rootElement, LTElement[] elements);
        void UpdateValues(LTElement[] elements, Dictionary<LTElementType, object> updateValues);
    }

    public interface ILayoutableImage
    {
        Vector2 RectSize();
    }

    public interface ILayoutableText
    {
        string Text();
    }
}