using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;






public class LayouTaroSample : MonoBehaviour
{
    public Canvas canvas;
    void Start()
    {
        // generate your own data structure with parameters for UI.
        var box = BoxElement.GO(
            null,// UI bg with image
            () =>
            {
                Debug.Log("root box element is tapped.");
            },
            TextElement.GO("hannin is yasu! this is public problem\U0001F60A! gooooooooooooood "),// text.
            ImageElement.GO(null),// image.
            ButtonElement.GO(null, () => { Debug.Log("button is tapped."); })
        );

        // generate the layouter which you want to use for layout.
        var layouter = new MyLayouter();

        // set the default size of content.
        var size = new Vector2(600, 100);

        // do layout with LayouTaro. the GameObject will be returned with layouted structure.
        var go = box.gameObject;
        go = LayouTaro.Layout<BoxElement>(
            canvas.transform,
            size,
            go,
            layouter
        );

        go.transform.SetParent(canvas.transform);
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // update element values and re-layout with same GameObject.
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