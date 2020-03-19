using UnityEngine;
using UnityEngine.UI;






public class LayouTaroSample : MonoBehaviour
{
    void Start()
    {
        // データのインスタンスを元に、レイアウトを開始する。
        Debug.Log("レイアウト開始！");
        var box = ElementBox.GO(
            null,// bg画像
            ImageElement.GO(null),// 画像
            TextElement.GO("本名はヤス")// テキスト
        );

        // 8
        var go = LayouTaro.Layout(box);


        // goからBox
        var boxMo = go.GetComponent<ElementBox>();
        boxMo.Update(null, "taro");
    }
}