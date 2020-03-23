using System;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonElement : LTElement, ILayoutableImage
{
    public override LTElementType GetLTElementType()
    {
        return LTElementType.Button;
    }

    public Image Image;
    public Action OnTapped;

    public static ButtonElement GO(Image image, Action onTapped)
    {
        // prefab名を固定してGOを作ってしまおう
        var prefabName = "LayouTaroPrefabs/Button";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<ButtonElement>();

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