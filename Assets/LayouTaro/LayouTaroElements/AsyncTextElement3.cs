using UILayouTaro;
using UnityEngine;

public class AsyncTextElement3 : LTAsyncElement, ILayoutableText
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.AsyncText3;
    }

    public string TextContent;

    public static AsyncTextElement3 GO(string text)
    {
        var prefabName = "LayouTaroPrefabs/AsyncText";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<AsyncTextElement3>();

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