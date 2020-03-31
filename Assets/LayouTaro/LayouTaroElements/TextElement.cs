using UILayouTaro;
using UnityEngine;


public class TextElement : LTElement, ILayoutableText
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.Text;
    }

    public string TextContent;

    public static TextElement GO(string text)
    {
        var prefabName = "LayouTaroPrefabs/Text";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<TextElement>();

        r.TextContent = text;
        return r;
    }

    public string Text()
    {
        return TextContent;
    }

    public GameObject GenerateGO(string text)
    {
        var element = GO(text);
        return element.gameObject;
    }
}