using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;

public class BoxElement : LTRootElement
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.Image;
    }

    public override LTElement[] GetLTElements()
    {
        return elements;
    }

    private LTElement[] elements;

    public Image BGImage;// 9パッチにすると良さそう


    // この関数をnew thisで実現できるといいなあー。
    public static BoxElement GO(Image bg, params LTElement[] elements)
    {
        var prefabName = "Box";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<BoxElement>();

        r.BGImage = bg;
        r.elements = elements;

        return r;
    }
}