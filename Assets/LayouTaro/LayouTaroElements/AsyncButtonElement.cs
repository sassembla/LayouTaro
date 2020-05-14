using System;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;


public class AsyncButtonElement : LTAsyncElement, ILayoutableRect
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.AsyncButton;
    }

    public Image Image;
    public Action OnTapped;

    public static AsyncButtonElement GO(Image image, Action onTapped)
    {
        // prefab名を固定してGOを作ってしまおう
        var prefabName = "LayouTaroPrefabs/AsyncButton";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<AsyncButtonElement>();

        r.Image = image;
        r.OnTapped = onTapped;

        // このへんでレシーバセットする
        var button = r.GetComponent<Button>();
        button.onClick.AddListener(() => r.OnTapped());

        return r;
    }

    public Vector2 RectSize()
    {
        // ここで、最低でもこのサイズ、とか、ロード失敗したらこのサイズ、とかができる。
        var imageRect = this.GetComponent<RectTransform>().sizeDelta;
        return imageRect;
    }
}