using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UILayouTaro
{
    public class InternalAsyncMissingTextRect : LTAsyncLoadableElement, ILayoutableRect
    {
        private Vector2 Size;
        public static InternalAsyncMissingTextRect New<T, U>(T parentTextElement, string text) where T : LTAsyncElement, ILayoutableText where U : IMissingSpriteCache, new()
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

            // サイズを一旦TMProの情報をもとに決定する
            missingTextRect.Size = size;

            // ローディングフラグを立てる
            missingTextRect.IsLoading = true;

            /*
                フォント名
                ポイント数
                表示幅
                表示高さ
                文字
            */
            var fontName = textComponent.font.name;
            var fontSize = textComponent.fontSize;
            var requestWidth = size.x;
            var requestHeight = size.y;

            var cacheInstance = InternalCachePool.Get<U>();

            cacheInstance.LoadMissingText(
                fontName,
                fontSize,
                requestWidth,
                requestHeight,
                text,
                cor =>
                {
                    missingTextRect.StartCoroutine(cor);
                },
                data =>
                {
                    missingTextRect.IsLoading = false;

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
                    missingTextRect.Size = rectTrans.sizeDelta;
                },
                () =>
                {
                    missingTextRect.IsLoading = false;
                }
            );

            return missingTextRect;
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