using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayouTaro
{
    public static GameObject Layout(ElementBox box)
    {
        var boxGO = box.gameObject;


        // 
        Debug.Log("box:" + box + " boxG:" + box.gameObject);
        // parse
        var originX = 0;
        var originY = 0;

        // boxのInsetとかを作ると、上下左右の余白とか作れそうだなー。
        // ここは起点になるので、起点をいじっていこう。


        var elements = box.Elements;
        foreach (var element in elements)
        {
            var type = element.GetLayoutElementType();
            switch (type)
            {
                case LayoutElementType.Image:
                    var imageElement = (ImageElement)element;

                    var rectSize = imageElement.RectSize();
                    Debug.Log("rectSize:" + rectSize);

                    imageElement.gameObject.transform.SetParent(boxGO.transform);
                    break;
                case LayoutElementType.Text:
                    var textElement = (TextElement)element;
                    var contentText = textElement.Text();
                    Debug.Log("contentText:" + contentText);

                    textElement.gameObject.transform.SetParent(boxGO.transform);
                    break;
                case LayoutElementType.Box:
                    throw new Exception("unsupported layout:" + type);
                case LayoutElementType.Button:
                    Debug.LogError("まだない");
                    break;
                default:
                    break;
            }
        }

        return box.gameObject;
    }

    public static GameObject Relayout(GameObject elementGameObj)
    {
        var l = elementGameObj.GetComponent<ILayoutElement>();
        // relayoutする

        return elementGameObj;
    }
}
