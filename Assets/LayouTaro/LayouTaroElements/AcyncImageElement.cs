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

        if (image != null)
        {
            var imageComponent = res.GetComponent<Image>();
            imageComponent.sprite = image.sprite;
        }

        r.Image = image;
        return r;
    }

    public static AsyncImageElement GO(float width, float height)
    {
        // prefab名を固定してGOを作ってしまおう
        var prefabName = "LayouTaroPrefabs/AsyncImage";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<AsyncImageElement>();

        var rectTras = r.GetComponent<RectTransform>();
        rectTras.sizeDelta = new Vector2(width, height);
        r.Image = null;
        return r;
    }

    public Vector2 RectSize()
    {
        // ここで、最低でもこのサイズ、とか、ロード失敗したらこのサイズ、とかができる。
        var imageRect = this.GetComponent<RectTransform>().sizeDelta;
        return imageRect;
    }
}