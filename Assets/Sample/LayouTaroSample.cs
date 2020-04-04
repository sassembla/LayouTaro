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

        /*
            ・マルチ文字列、ラストが1ラインなら別にレイアウト巻き込まれていいよね
            ・同じマルチ文字列で結果が違うやつがいる
        */

        // データ構造を作る、自由に構造を書いていい。
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("hannin is yasu! this is public problem! \U0001F60A gooooooooooooood "),// テキスト
            ImageElement.GO(null),// 画像
            ButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); })
        );

        // レイアウトに使うクラスを生成する
        var layouter = new MyLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

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
        // // goからBoxを取得し、Relayoutを行うと、中身の値が変わったgoインスタンスが返ってくる。
        // go = LayouTaro.RelayoutWithUpdate<BoxElement>(
        //     size,
        //     go,
        //     new Dictionary<LTElementType, object> {
        //         {LTElementType.Image, null},
        //         {LTElementType.Text, "relayout!"}
        //     },
        //     layouter
        // );
    }
}