using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UILayouTaro
{
    public class InternalAsyncEmojiRect : LTAsyncElement, ILayoutableRect
    {
        private Vector2 Size;
        public static GameObject GO<T>(T parentTextElement, Char[] chars) where T : LTAsyncElement, ILayoutableText
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
            var (isExist, codePoint) = ChechIfEmojiOrMarkExist(emojiOrMarkStr);
            if (isExist)
            {
                // 最低一つ要素が作られているはずなので、そのSptiteの位置情報を冷雨後に合致するように調整する。
                if (rectTrans.childCount == 1)
                {
                    var emojiRectTrans = rectTrans.GetChild(0).GetComponent<RectTransform>();
                    emojiRectTrans.pivot = new Vector2(0, 1);
                    emojiRectTrans.anchorMin = new Vector2(0, 1);
                    emojiRectTrans.anchorMax = new Vector2(0, 1);
                    emojiRectTrans.anchoredPosition = Vector2.zero;
                }
                else
                {
                    Debug.LogError("絵文字かマークがある状態だが、このcodePointの文字を示すspriteがロードされない codePoint:" + codePoint);
                }
            }
            else
            {
                Debug.LogWarning("この辺にキャッシュヒットが欲しい、codePointでいいので。");

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

        private static (bool, uint) ChechIfEmojiOrMarkExist(string emojiOrMarkStr)
        {
            uint codePoint = 0;
            for (var i = 0; i < emojiOrMarkStr.Length; i++)
            {
                codePoint = (uint)char.ConvertToUtf32(emojiOrMarkStr, i);

                // indexで切り分けられるようであれば、この時点で判断を行う。
                // 現状では2文字ずつしかsurrogateで追わないため、このブロックに処理がくることはない。
                if (char.IsSurrogatePair(emojiOrMarkStr, i))
                {
                    i++;
                }
            }

            var spriteAsset = TMPro.TMP_Settings.GetSpriteAsset();
            var table = spriteAsset.spriteCharacterTable;
            if (-1 < spriteAsset.GetSpriteIndexFromUnicode(codePoint))
            {
                // 絵文字か記号が既存のSpriteAssetに存在する
                return (true, codePoint);
            }

            // fallbackに登録されているSpriteAssetsも見る
            foreach (var sAsset in spriteAsset.fallbackSpriteAssets)
            {
                if (-1 < sAsset.GetSpriteIndexFromUnicode(codePoint))
                {
                    return (true, codePoint);
                }
            }

            // 存在しないのでfalseを返す
            return (false, 0);
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