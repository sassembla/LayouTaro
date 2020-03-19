using System;
using UnityEngine;
using UnityEngine.UI;

public class ElementBox : MonoBehaviour, ILayoutElement
{
    public LayoutElementType GetLayoutElementType()
    {
        return global::LayoutElementType.Box;
    }

    public ILayoutElement[] Elements;
    public Image BGImage;// 9パッチにすると良さそう


    // この関数をnew thisで実現できるといいなあー。
    public static ElementBox GO(Image bg, params ILayoutElement[] elements)
    {
        var prefabName = "ElementBox";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<ElementBox>();

        r.BGImage = bg;
        r.Elements = elements;

        return r;
    }

    public GameObject Update(Image p, string v)
    {
        foreach (var a in Elements)
        {
            switch (a.GetLayoutElementType())
            {
                case LayoutElementType.Image:
                    var i = (ImageElement)a;
                    i.Image = p;
                    break;
            }
        }

        var go = LayouTaro.Relayout(this.gameObject);
        return go;
    }
}