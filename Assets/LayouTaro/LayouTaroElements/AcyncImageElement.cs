using System;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;


public class AsyncImageElement : LTAsyncElement, ILayoutableRect
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.AsyncImage;
    }

    public Image Image;

    public static AsyncImageElement GO(Image image)
    {
        // prefab名を固定してGOを作ってしまおう
        var prefabName = "LayouTaroPrefabs/AsyncImage";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<AsyncImageElement>();

        r.Image = image;
        return r;
    }

    public Vector2 RectSize()
    {
        // ここで、最低でもこのサイズ、とか、ロード失敗したらこのサイズ、とかができる。
        var imageRect = this.GetComponent<RectTransform>().sizeDelta;
        return imageRect;
    }

    public override void OnMissingCharFound<T>(string fontName, char[] chars, float x, float y, Action<T> onInput, Action onIgnore)
    {
        throw new NotImplementedException();
    }
}