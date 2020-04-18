using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UILayouTaro
{
    public class InternalAsyncEmojiRect : LTAsyncElement, ILayoutableRect
    {
        private Vector2 Size;
        public static GameObject GO<T>(T parentTextElement, string rawText, Char[] chars) where T : LTAsyncElement, ILayoutableText
        {
            var emojiOrMarkStr = new string(chars);

            var go = parentTextElement.GenerateGO(emojiOrMarkStr);

            // TMProのレイアウトをするためには、ここでCanvasに乗っている親要素の上に載せるしかない。
            go.transform.SetParent(parentTextElement.transform, false);


            var emojiRect = go.AddComponent<InternalAsyncEmojiRect>();

            // 文字をセットする場所としてRectTransformを取得、レイアウトのために高さに無限値をセット
            var rectTrans = go.GetComponent<RectTransform>();
            // フォント情報を取得するためにT型をセットし、そこからTMProのcomponentを取り出す。そこに絵文字をセットし、絵文字画像を得る。
            var textComponent = go.GetComponent<TextMeshProUGUI>();

            textComponent.enableWordWrapping = true;
            textComponent.text = emojiOrMarkStr;
            rectTrans.sizeDelta = Vector2.positiveInfinity;
            var textInfos = textComponent.GetTextInfo(emojiOrMarkStr);
            textComponent.enableWordWrapping = false;

            var lines = textInfos.lineCount;
            if (lines != 1)
            {
                throw new Exception("unsupported emoji/mark pattern.");
            }

            var lineInfo = textInfos.lineInfo[0];
            var lineWidth = lineInfo.length;
            var lineHeight = lineInfo.lineHeight;

            // サイズの更新
            var size = new Vector2(lineWidth, lineHeight);

            // 絵文字/記号部分を左上アンカーにすると、高さがNanにならずに絵文字/記号スプライトが表示される。
            if (0 < rectTrans.childCount)
            {
                var emojiRectTrans = rectTrans.GetChild(0).GetComponent<RectTransform>();
                emojiRectTrans.pivot = new Vector2(0, 1);
                emojiRectTrans.anchorMin = new Vector2(0, 1);
                emojiRectTrans.anchorMax = new Vector2(0, 1);
                emojiRectTrans.anchoredPosition = Vector2.zero;
            }

            // この文字オブジェクト自体の位置、サイズを規定する
            rectTrans.anchoredPosition = Vector2.zero;
            rectTrans.sizeDelta = size;

            // 後ほど取得できるサイズを決定する
            emojiRect.Size = size;

            /*
                ここでこの要素のサイズが決定される。
                文字の有無をもってリクエストを行い、レイアウト時までに取得が終わっていればレイアウトができる。
                失敗した場合でもfallbackを指定できる。まあ、そういうのはしょうがない。

                ここでリクエストが決定し、レスポンスが来た時に叩かれるメソッドの設定ができるのが良い。
                インターフェースは何も返さなくていいな。終了がわかればいい。なので、ハンドラを放り込む機構が存在すればいいのか。
            */

            var has0 = textComponent.font.HasCharacters(emojiOrMarkStr);


            // var has1 = textComponent.font.HasCharacter(res);

            // Debug.Log("has0:" + has0 + " has1:" + has1 + " res:" + res);
            var clist = new List<char>();
            foreach (var a in chars)
            {
                // // var ww = (int)Char.GetNumericValue(a);
                // var s = textComponent.font.HasCharacters(emojiOrMarkStr, out clist);
                // var table = textComponent.font.fallbackFontAssetTable;
                // foreach (var t in table)
                // {
                //     var tableHas = t.HasCharacters(emojiOrMarkStr, out clist);
                //     Debug.Log("tableHas:" + tableHas + " clist:" + clist.Count);
                // }

                string characters = TMP_FontAsset.GetCharacters(textComponent.font);
                Debug.Log("characters:" + characters.Length);
                if (characters.IndexOf(a) >= 0)
                {
                    Debug.Log("have!");
                }
                else
                {
                    var have = false;
                    List<TMP_FontAsset> fallbackFontAssets = textComponent.font.fallbackFontAssetTable;
                    for (int i = 0; i < fallbackFontAssets.Count; i++)
                    {
                        characters = TMP_FontAsset.GetCharacters(fallbackFontAssets[i]);
                        if (characters.IndexOf(a) >= 0)
                        {
                            Debug.Log("have!");
                            have = true;
                        }
                    }

                    Debug.Log("isHave:" + have);
                }

            }




            // var has2 = textComponent.font.HasCharacters(emojiOrMarkStr);
            // var has3 = textComponent.font.HasCharacters(emojiOrMarkStr);
            // emojiOrMarkStr
            // List<char> list;
            // if (!textComponent.font.HasCharacters(rawText, out list))
            // この検査方法がおかしい。なるほど、通常の絵文字もヒットしてしまう。
            // えー、おかしくない、、うーん、、
            // if (list.Contains(chars[0]))
            // {
            //     Debug.Log("ヒット、これはmissing、、は？ list:" + list.Count);// これはかならず4を返してくる、手元に存在する絵文字すら、missingを返してくるっぽい。
            // }

            // この方法は採用したくないなー、
            if (0 < rectTrans.childCount)
            {
                var emojiRectTrans = rectTrans.GetChild(0).GetComponent<RectTransform>();
                emojiRectTrans.pivot = new Vector2(0, 1);
                emojiRectTrans.anchorMin = new Vector2(0, 1);
                emojiRectTrans.anchorMax = new Vector2(0, 1);
                emojiRectTrans.anchoredPosition = Vector2.zero;
            }
            else
            {
                Debug.Log("何もつくり出せてない状態");
                Debug.LogWarning("この辺にキャッシュヒットが欲しい");

                IEnumerator load()
                {
                    var url = "https://dummyimage.com/" + size.x + "x" + size.y + "/2cb6d1/000000";
                    var req = UnityWebRequestTexture.GetTexture(
                        url
                    );

                    var p = req.SendWebRequest();
                    while (!p.isDone)
                    {
                        yield return null;
                    }

                    // ローディングフラグを下げる
                    emojiRect.IsLoading = false;

                    if (req.isNetworkError || req.isHttpError)
                    {
                        Debug.Log("サイズを変更せずにロード処理が終わる");
                        yield break;
                    }

                    if (200 <= req.responseCode && req.responseCode < 299)
                    {
                        // テクスチャ作成
                        var tex = DownloadHandlerTexture.GetContent(req);

                        // スプライトを取り出せるようになった、うーん、はい。どのレイヤーで切り出すのがいいんだろう。
                        var spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);

                        // サイズを更新
                        rectTrans.sizeDelta = new Vector2(tex.width, tex.height);

                        // tmProのコンポーネントを排除する(子供があるとそれを描画しようとしてエラーが出る)
                        GameObject.Destroy(textComponent);

                        if (0 < rectTrans.childCount)
                        {
                            // TMProの絵文字(カラ)が置いてあるコンポーネントを削除し、代わりにSpriteを置く
                            var emojiChild = rectTrans.GetChild(0);
                            GameObject.Destroy(emojiChild.gameObject);
                        }

                        // スプライトを作って入れる
                        var spriteObject = new GameObject("sprite");
                        var spriteComponent = spriteObject.AddComponent<Image>();

                        var childRectTrans = spriteComponent.GetComponent<RectTransform>();
                        childRectTrans.pivot = new Vector2(0, 1);
                        childRectTrans.anchorMin = new Vector2(0, 1);
                        childRectTrans.anchorMax = new Vector2(0, 1);
                        spriteComponent.transform.SetParent(rectTrans, false);

                        spriteComponent.sprite = spr;
                        spriteComponent.SetNativeSize();

                        // 決定後のサイズを入力する
                        emojiRect.Size = rectTrans.sizeDelta;
                    }
                }

                var cor = load();

                Debug.LogWarning("ここで開始しちゃって構わないんだけど、なんかいい方法ないかな、まあキャッシュヒットとかをどう盛り込むかで変わっちゃうのか");
                emojiRect.StartCoroutine(cor);

                // ローディングフラグを立てる
                emojiRect.IsLoading = true;
            }

            return go;
        }

        public override LTElementType GetLTElementType()
        {
            throw new System.NotImplementedException();
        }

        public Vector2 RectSize()
        {
            return Size;
        }

        public override void OnMissingCharFound<T>(string fontName, char[] chars, float width, float height, Action<T> onInput, Action onIgnore)
        {
            throw new NotImplementedException();
        }
    }
}