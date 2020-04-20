using System;
using System.Collections;
using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;
using UnityEngine.Networking;

public class BasicMissingSpriteCache : IMissingSpriteCache
{
    private Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
    private List<string> loadingKeys = new List<string>();

    public BasicMissingSpriteCache()
    {
        // これまでに作成された文字を辞書として読み出す、とか。まああんまいらないか。
        // ローカルにあるならURLCaching使えば楽。
    }

    public void LoadMissingText(string fontName, float fontSize, float requestWidth, float requestHeight, string text, Action<IEnumerator> onRequest, Action<Texture2D> onSucceeded, Action onFailed)
    {
        var cacheKey = fontName + "_" + fontSize + "_" + text;
        if (textureDict.ContainsKey(cacheKey))
        {
            var tex = textureDict[cacheKey];
            onSucceeded(tex);
            return;
        }

        if (loadingKeys.Contains(cacheKey))
        {
            // 同じ文字のリクエストが多重になってるので、リクエストせずに空転して待つ。
            IEnumerator wait()
            {
                while (loadingKeys.Contains(cacheKey))
                {
                    yield return null;
                }

                if (textureDict.ContainsKey(cacheKey))
                {
                    var tex = textureDict[cacheKey];
                    onSucceeded(tex);
                    yield break;
                }

                onFailed();
            }
            onRequest(wait());
            return;
        }

        loadingKeys.Add(cacheKey);

        IEnumerator load()
        {
            var url = "https://dummyimage.com/" + requestWidth + "x" + requestHeight;
            var req = UnityWebRequestTexture.GetTexture(url);

            var p = req.SendWebRequest();
            while (!p.isDone)
            {
                yield return null;
            }

            if (req.isNetworkError || req.isHttpError)
            {
                onFailed();
                loadingKeys.Remove(cacheKey);
                yield break;
            }

            if (200 <= req.responseCode && req.responseCode < 299)
            {
                // テクスチャ作成
                var tex = DownloadHandlerTexture.GetContent(req);
                onSucceeded(tex);
                loadingKeys.Remove(cacheKey);
                yield break;
            }

            onFailed();
        }

        var cor = load();
        onRequest(cor);
    }

    public void LoadMissingEmojiOrMark(string fontName, float fontSize, float requestWidth, float requestHeight, uint codePoint, Action<IEnumerator> onRequest, Action<Texture2D> onSucceeded, Action onFailed)
    {
        var cacheKey = fontName + "_" + fontSize + "_" + codePoint;
        if (textureDict.ContainsKey(cacheKey))
        {
            var tex = textureDict[cacheKey];
            onSucceeded(tex);
            return;
        }

        if (loadingKeys.Contains(cacheKey))
        {
            // 同じ文字のリクエストが多重になってるので、リクエストせずに空転して待つ。
            IEnumerator wait()
            {
                while (loadingKeys.Contains(cacheKey))
                {
                    yield return null;
                }

                if (textureDict.ContainsKey(cacheKey))
                {
                    var tex = textureDict[cacheKey];
                    onSucceeded(tex);
                    yield break;
                }

                onFailed();
            }
            onRequest(wait());
            return;
        }

        loadingKeys.Add(cacheKey);

        IEnumerator load()
        {
            var url = "https://dummyimage.com/" + requestWidth + "x" + requestHeight;
            var req = UnityWebRequestTexture.GetTexture(url);

            var p = req.SendWebRequest();
            while (!p.isDone)
            {
                yield return null;
            }

            if (req.isNetworkError || req.isHttpError)
            {
                onFailed();
                loadingKeys.Remove(cacheKey);
                yield break;
            }

            if (200 <= req.responseCode && req.responseCode < 299)
            {
                // テクスチャ作成
                var tex = DownloadHandlerTexture.GetContent(req);
                onSucceeded(tex);
                loadingKeys.Remove(cacheKey);
                yield break;
            }

            onFailed();
        }

        var cor = load();
        onRequest(cor);
    }
}