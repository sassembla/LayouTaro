using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;

public class BoxElement : LTRootElement
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.Image;
    }

    public override LTElement[] GetLTElements()
    {
        return elements;
    }

    private LTElement[] elements;

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
    public static BoxElement GO(Image bg, params LTElement[] elements)
    {
        var prefabName = "LayouTaroPrefabs/Box";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<BoxElement>();

        r.BGImage = bg;
        r.elements = elements;

        return r;
    }
}