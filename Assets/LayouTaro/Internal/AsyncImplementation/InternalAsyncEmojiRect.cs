using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UILayouTaro
{
    // TODO: SpriteRectにrenameする
    public class InternalAsyncEmojiRect : LTAsyncLoadableElement, ILayoutableRect
    {
        private Vector2 Size;
        public static InternalAsyncEmojiRect New<T, U>(T parentTextElement, Char[] chars) where T : LTAsyncElement, ILayoutableText where U : IMissingSpriteCache, new()
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

            // サイズを一旦TMProの情報をもとに決定する
            emojiRect.Size = size;

            var (isExist, codePoint) = TextLayoutDefinitions.TMPro_ChechIfEmojiOrMarkExist(emojiOrMarkStr);
            if (isExist)
            {
                // 最低一つ要素が作られているはずなので、そのSptiteの位置情報をレイアウト後に合致するように調整する。
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
                    Debug.LogWarning("絵文字かマークがある状態だが、このcodePointの文字を示すspriteがロードされない codePoint:" + codePoint);
                }
            }
            else
            {
                // ローディングフラグを立てる
                emojiRect.IsLoading = true;

                /*
                    ポイント数
                    フォント名
                    表示幅
                    表示高さ
                    コードポイント
                */
                var fontName = textComponent.font.name;
                var fontSize = textComponent.fontSize;
                var requestWidth = size.x;
                var requestHeight = size.y;

                var cacheInstance = InternalCachePool.Get<U>();

                cacheInstance.LoadMissingEmoji(
                    fontName,
                    fontSize,
                    requestWidth,
                    requestHeight,
                    codePoint,
                    cor =>
                    {
                        emojiRect.StartCoroutine(cor);
                    },
                    data =>
                    {
                        emojiRect.IsLoading = false;

                        var spr = Sprite.Create(data, new Rect(0, 0, data.width, data.height), Vector2.zero);

                        // サイズを更新
                        rectTrans.sizeDelta = new Vector2(data.width, data.height);

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
                        emojiRect.Size = rectTrans.sizeDelta;
                    },
                    () =>
                    {
                        emojiRect.IsLoading = false;
                    }
                );
            }

            return emojiRect;
        }

        public override LTElementType GetLTElementType()
        {
            throw new System.NotImplementedException();
        }

        public Vector2 RectSize()
        {
            return Size;
        }
    }
}