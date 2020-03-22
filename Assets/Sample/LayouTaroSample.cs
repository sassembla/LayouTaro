using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;






public class LayouTaroSample : MonoBehaviour
{
    public Canvas canvas;
    void Start()
    {
        // データのインスタンスを元に、レイアウトを開始する。
        Debug.Log("レイアウト開始！");

        // データ構造を作る、自由に構造を書いていい。初期化子で初期化できる。
        var box = BoxElement.GO(
            null,// bg画像
            TextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
            ImageElement.GO(null),// 画像
            TextElement.GO("dijklmno"),
            TextElement.GO("h"),
            ImageElement.GO(null),
            TextElement.GO("g"),
            ImageElement.GO(null)
        );

        // レイアウトに使うクラスを生成する
        var layouter = new MyLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(200, 100);

        // レイアウトを行う
        var go = box.gameObject;
        go = LayouTaro.Layout<BoxElement>(
            canvas.transform,
            size,
            go,
            layouter
        );

        go.transform.SetParent(canvas.transform);
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // // 値の更新とリレイアウトを行う
        // // goからBoxを取得し、Relayoutを行い、行った後のGameObjectを受け取る。同じインスタンスで値が変わったものが返ってくる。
        // go = LayouTaro.RelayoutWithUpdate<BoxElement>(
        //     size,
        //     go,
        //     new Dictionary<LayoutElementType, object> {
        //         {LayoutElementType.Image, null},
        //         {LayoutElementType.Text, "relayout!"}
        //     },
        //     layouter
        // );
    }
}