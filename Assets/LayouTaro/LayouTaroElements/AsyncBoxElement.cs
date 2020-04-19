using System;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;

public class AsyncBoxElement : LTAsyncRootElement
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.AsyncBox;
    }

    public override LTAsyncElement[] GetLTElements()
    {
        return elements;
    }

    private LTAsyncElement[] elements;

    public Action OnTapped;

    public Image BGImage;// 9パッチにすると良さそう

    /*
        ユーザーはこのような
        ・prefabを持ってきて
        ・instantiateし
        ・LTElement型をAddComponentし
        ・初期値をセットして
        ・LTElement型を返す
        という所作を求められる。

        本当は、MonoBehaviour自体が独自定義なコンストラクタでnewできれば、
        そしてMonoBeをnewしたタイミングでGameObjectが勝手に生成されてMonoBeがAddされれば、
        とてもいいインターフェースが作れたんだが。まあはい。
    */
    public static AsyncBoxElement GO(Image bg, Action onTapped, params LTAsyncElement[] elements)
    {
        var prefabName = "LayouTaroPrefabs/AsyncBox";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<AsyncBoxElement>();

        r.BGImage = bg;
        r.elements = elements;
        r.OnTapped = onTapped;

        // このへんでレシーバセットする
        var button = r.GetComponent<Button>();
        button.onClick.AddListener(() => r.OnTapped());


        return r;
    }
}