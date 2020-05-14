using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;


public class ImageElement : LTElement, ILayoutableRect
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.Image;
    }

    public Image Image;

    public static ImageElement GO(Image image)
    {
        // prefab名を固定してGOを作ってしまおう
        var prefabName = "LayouTaroPrefabs/Image";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<ImageElement>();

        r.Image = image;
        return r;
    }

    public static ImageElement GO(float width, float height)
    {
        // prefab名を固定してGOを作ってしまおう
        var prefabName = "LayouTaroPrefabs/Image";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<ImageElement>();

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