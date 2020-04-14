using System;
using UILayouTaro;
using UnityEngine;


public class AsyncTextElement : LTAsyncElement, ILayoutableText
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.AsyncText;
    }

    public string TextContent;

    public static AsyncTextElement GO(string text)
    {
        var prefabName = "LayouTaroPrefabs/AsyncText";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<AsyncTextElement>();

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

    public override void OnMissingCharFound<T>(string fontName, char[] chars, float x, float y, Action<T> onInput, Action onIgnore)
    {
        throw new NotImplementedException();
    }
}