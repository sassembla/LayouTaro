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
    public class InternalAsyncMissingTextRect : LTAsyncElement, ILayoutableRect
    {
        private Vector2 Size;
        public static GameObject GO<T>(T parentTextElement, string text) where T : LTAsyncElement, ILayoutableText
        {
            var go = parentTextElement.GenerateGO(text);

            // TMProのレイアウトをするためには、ここでCanvasに乗っている親要素の上に載せるしかない。
            go.transform.SetParent(parentTextElement.transform, false);

            var missingTextRect = go.AddComponent<InternalAsyncMissingTextRect>();

            // 文字をセットする場所としてRectTransformを取得、レイアウトのために高さに無限値をセット
            var rectTrans = go.GetComponent<RectTransform>();
            // フォント情報を取得するためにT型をセットし、そこからTMProのcomponentを取り出す。そこに絵文字をセットし、絵文字画像を得る。
            var textComponent = go.GetComponent<TextMeshProUGUI>();

            textComponent.enableWordWrapping = true;
            textComponent.text = text;
            rectTrans.sizeDelta = Vector2.positiveInfinity;
            var textInfos = textComponent.GetTextInfo(text);
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

            // この文字オブジェクト自体の位置、サイズを規定する
            rectTrans.anchoredPosition = Vector2.zero;
            rectTrans.sizeDelta = size;

            // 後ほど取得できるサイズを決定する
            missingTextRect.Size = size;



            /*
                ポイント数
                フォント名
                表示幅
                表示高さ
                コードポイント
            */
            var fontSize = textComponent.fontSize;
            var fontName = textComponent.font.name;
            var requestWidth = size.x;
            var requestHeight = size.y;

            // これらの要素をもとに、インターフェースを切ろう。ここではtextがくる。
            Debug.LogWarning("キャッシュヒットを作るならここ。");

            // ここで条件があればキャッシュヒットが成立する。
            /*
                Coroutineがあれば継続かどうか判断できる。
                なので、終了条件は特に必要ない。
                Corがnullなのはやめて欲しいけど、サイズと画像をセットできればそれでいいはずなんだよな。
                要素の方に寄せるにはどうすればいい。要素にセットするComponentを返すか。いや、別に自分自身がスタートした方が速いな。
            */

            IEnumerator load()
            {
                // この部分に独自でURLを渡せればいいよね。
                var url = "https://dummyimage.com/" + size.x + "x" + size.y;
                var req = UnityWebRequestTexture.GetTexture(
                    url
                );

                var p = req.SendWebRequest();
                while (!p.isDone)
                {
                    yield return null;
                }

                // ローディングフラグを下げる
                missingTextRect.IsLoading = false;

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
                        // TMProの文字(カラ)が置いてあるコンポーネントを削除する
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
                    missingTextRect.Size = rectTrans.sizeDelta;
                }
            }

            var cor = load();

            Debug.LogWarning("ここで開始しちゃって構わないんだけど、なんかいい方法ないかな、まあキャッシュヒットとかをどう盛り込むかで変わっちゃうのか");
            missingTextRect.StartCoroutine(cor);

            // ローディングフラグを立てる
            missingTextRect.IsLoading = true;
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