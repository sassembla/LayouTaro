using System;
using TMPro;
using UnityEngine;

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
            else
            {
                // target mark or emoji is missing,
                // LayouTaro._OnMissingCharacter(chars);
                Debug.LogError("来ないと思う");
            }

            // この文字オブジェクト自体の位置、サイズを規定する
            rectTrans.anchoredPosition = Vector2.zero;
            rectTrans.sizeDelta = size;

            // 後ほど取得できるサイズを決定する
            emojiRect.Size = size;
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

        public override void OnMissingCharFound<T>(string fontName, char[] chars, float x, float y, Action<T> onInput, Action onIgnore)
        {
            throw new NotImplementedException();
        }
    }
}