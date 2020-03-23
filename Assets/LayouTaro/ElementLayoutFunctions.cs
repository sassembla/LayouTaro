using System.Collections.Generic;
using UnityEngine;

namespace UILayouTaro
{
    public static class ElementLayoutFunctions
    {
        /*
            改行、行継続に関するレイアウトコントロール系
        */
        public static float LineFeed(ref float x, ref float y, float currentElementHeight, ref float currentLineMaxHeight, ref List<RectTransform> linedElements)
        {
            // 列の概念の中で最大の高さを持つ要素を中心に、それより小さい要素をy軸に対して整列させる
            var lineHeight = Mathf.Max(currentElementHeight, currentLineMaxHeight);
            foreach (var rectTrans in linedElements)
            {
                var elementHeight = rectTrans.sizeDelta.y;
                var isParentRoot = rectTrans.parent.GetComponent<LTElement>() is LTRootElement;
                if (isParentRoot)
                {
                    rectTrans.anchoredPosition = new Vector2(
                        rectTrans.anchoredPosition.x,// xは維持
                        y - (lineHeight - elementHeight) / 2// yは行の高さから要素の高さを引いて/2したものをセット(縦の中央揃え)
                    );
                }
                else
                {
                    // 親がRootElementではない場合、なんらかの子要素なので、行の高さは合うが、上位の単位であるoriginYとの相性が悪すぎる。なので、独自の計算系で合わせる。
                    rectTrans.anchoredPosition = new Vector2(
                        rectTrans.anchoredPosition.x,// xは維持
                        rectTrans.anchoredPosition.y - (currentLineMaxHeight - elementHeight) / 2
                    );
                }
            }
            linedElements.Clear();

            x = 0;
            y -= lineHeight;
            currentLineMaxHeight = 0f;

            // 純粋にその行の中でどの要素が最も背が高かったのかを判別するために、計算結果による変数の初期化に関係なくこの値が必要な箇所がある。
            return lineHeight;
        }

        public static void ContinueLine(ref float x, float newX, float currentElementHeight, ref float currentLineMaxHeight)
        {
            x += newX;
            currentLineMaxHeight = Mathf.Max(currentElementHeight, currentLineMaxHeight);
        }

    }
}