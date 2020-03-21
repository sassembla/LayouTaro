using UILayouTaro;
using UnityEngine;


public class TextElement : LTElement, ILayoutableText
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.Text;
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
        return TextContent;
    }
}