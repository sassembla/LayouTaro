using System.Collections;
using System.Collections.Generic;
using UILayouTaro;
using UnityEngine;
using UnityEngine.UI;






public class LayouTaroSample : MonoBehaviour
{
    public Canvas canvas;
    IEnumerator Start()
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
        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        box.gameObject.transform.SetParent(canvas.transform);
        box.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        yield return null;

        // update element values and re-layout with same GameObject.
        box = LayouTaro.RelayoutWithUpdate(
            size,
            box,
            new Dictionary<LTElementType, object> {
                {LTElementType.Image, null},
                {LTElementType.Text, "relayout\U0001F60A!"}
            },
            layouter
        );

        yield return null;
    }
}