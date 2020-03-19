using UnityEngine;
using UnityEngine.UI;


// monobe付ければPrefab化できる、Prefabを放り込むべきなのか。


public class ImageElement : MonoBehaviour, ILayoutElement, ILayoutableImage
{
    public LayoutElementType GetLayoutElementType()
    {
        return global::LayoutElementType.Image;
    }

    public Image Image;

    public static ImageElement GO(Image image)
    {
        // prefab名を固定してGOを作ってしまおう
        var prefabName = "Image";
        var res = Resources.Load(prefabName) as GameObject;
        Debug.Log("res:" + res);
        var r = Instantiate(res).AddComponent<ImageElement>();

        r.Image = image;
        return r;
    }

    public Vector2 RectSize()
    {
        return new Vector2(100, 100);
    }
}