using System;
using UnityEngine;


// monobe付ければPrefab化できる、なんか適当にインターフェース作れるといいなー、やるか。
public class TextElement : MonoBehaviour, ILayoutElement, ILayoutableText
{
    public LayoutElementType GetLayoutElementType()
    {
        return global::LayoutElementType.Text;
    }

    public string TextContent;

    internal static TextElement GO(string text)
    {
        var prefabName = "Text";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<TextElement>();

        r.TextContent = text;
        return r;
    }

    public string Text()
    {
        return "here comes!";
    }
}