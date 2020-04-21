# LayouTaro

The UI layout library for Unity. 

![demo image](https://raw.githubusercontent.com/sassembla/LayouTaro/master/screenshot.png "demo image")


This library helps:
* Defining your original elements of UI.
* Layout it with your original Layouter with the support of BasicLayoutFunctions.


## Example

clone this repo and open Sample.scene, then Play unity.  above looking of UI will appear.

see LayouTaroSample.cs for knowing what's happen.

```csharp
// Box, Text, Image and Button elenents are already implemented in this sample. Also you can add or edit or delete these definition.

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

// generate the layouter which you want to layout your structures.
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

go.transform.SetParent(canvas.transform);
go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

// update element values and re-layout with same GameObject.
// box = LayouTaro.RelayoutWithUpdate(
//     size,
//     box,
//     new Dictionary<LTElementType, object> {
//         {LTElementType.Image, null},
//         {LTElementType.Text, "relayout!"}
//     },
//     layouter
// );
```

this code shows the layouted UI. looks simple!. 

### Async version

LayouTaro now supports async layout.

```csharp
// ready async elements.
var box = AsyncBoxElement.GO(
    null,// bg画像
    () =>
    {
        Debug.Log("ルートがタップされた");
    },
    AsyncTextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
    AsyncImageElement.GO(null),// 画像
    AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); })
);

// generate async layouter.
var layouter = new BasicAsyncLayouter();

// set size of content.
var size = new Vector2(600, 50);

// do async layout with BasicMissingSpriteCache cache.
var done = false;

LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
    canvas.transform,
    size,
    box,
    layouter,
    () =>
    {
        done = true;
    }
);
```

that's all.

## Usage

1. Use already defined element definition or define your original element of UI in LayouTaro/LayouTaroElements/LayoutElementType.cs.

```csharp
namespace UILayouTaro
{
    // add your own element type and then implement the class which extends LTElement or LTRootElement. 
    public enum LTElementType
    {
        Box,
        Image,
        Text,
        Button
    }
}
```

2. Define the type which extends LTElement (and adopt ILayoutableText | ILayoutableRect if need to express the element is rect or text.) 

for example, TextElement is defined for example of the kind of UI text.

```csharp
public class TextElement : LTElement, ILayoutableText
{
    public override LTElementType GetLTElementType()// set the LTElementType which you defined or already defined for detect "which is this element" by Layouter code.
    {
        return LTElementType.Text;
    }

    public string TextContent;

    public static TextElement GO(string text)
    {
        var prefabName = "LayouTaroPrefabs/Text";
        var res = Resources.Load(prefabName) as GameObject;
        var r = Instantiate(res).AddComponent<TextElement>();

        r.TextContent = text;
        return r;
    }

    /*
        Text() and GenerateGO methods are required by ILayoutableText.

        this methods are available for your custom layouter.
    */
    public string Text()
    {
        return TextContent;
    }

    public GameObject GenerateGO(string text)
    {
        var element = GO(text);
        return element.gameObject;
    }
}
```

in async version, you should use LTAsyncElement and LTRootAsyncElement instead of LTElement and LTRootElement.

3. Define your original Layouter or use already defined sample Layouter(MyLayouter.cs) with implementing ILayouter interface.

here is example for implementation.

```csharp
public class YourLayouter : ILayouter
{
    /*
        Layout method will be called when the LayouTaro.Layout is called.
        this requires layouting the root element and it's child elements.
    */
    public void Layout(Vector2 viewSize, out float originX, out float originY, GameObject rootObject, LTRootElement rootElement, LTElement[] elements, ref float currentLineMaxHeight, ref List<RectTransform> lineContents)
    {
        // start with initialize element pos.
        originX = 0f;
        originY = 0f;

        var originalViewWidth = viewSize.x;

        var viewWidth = viewSize.x;

        // get root element instance from rootObject.
        var root = rootObject.GetComponent<YourRootElement>();
        var rootTrans = root.GetComponent<RectTransform>();

        // layout child elements of root element.
        for (var i = 0; i < elements.Length; i++)
        {
            var element = elements[i];

            var currentElementRectTrans = element.GetComponent<RectTransform>();
            var restWidth = viewWidth - originX;

            lineContents.Add(currentElementRectTrans);

            // easy to load the type of element using GetLTElementType() method.
            var type = element.GetLTElementType();
            switch (type)
            {
                case LTElementType.YourImage:
                    var yourImageElement = (YourImageElement)element;
                    
                    // do layout here. for example, BasicLayoutFunctions has some helper methods for layout. in this case, if YourImageElement implements ILayoutableRect, BasicLayoutFunctions helps layout for your rect element.
                    BasicLayoutFunctions.RectLayout(
                        yourImageElement,
                        currentElementRectTrans,
                        yourImageElement.RectSize(),
                        viewWidth,
                        ref originX,
                        ref originY,
                        ref restWidth,
                        ref currentLineMaxHeight,
                        ref lineContents
                    );
                    break;
                case LTElementType.YourText:
                    var yourTextElement = (YourTextElement)element;
                    var contentText = youtTextElement.Text();

                    // do layout here...
                    break;
                case LTElementType.YourButton:
                    var yourButtonElement = (YourButtonElement)element;

                    // do layout here...
                    break;

                case LTElementType.YourRoot:
                    // do layout here...
                    break;
                default:
                    Debug.LogError("unsupported element type:" + type);
                    break;
            }
        }

        // layout last line if need. LayoutLastLine layouts the element which located the last line of all elements.
        BasicLayoutFunctions.LayoutLastLine(ref originY, currentLineMaxHeight, ref lineContents);

        // if you want to resize the root element to it's containing element size, you can do that here.
        rootTrans.sizeDelta = new Vector2(originalViewWidth, Mathf.Abs(originY));}
    }

    /*
        UpdateValues method will be called when the LayouTaro.RelayoutWithUpdate is called.
        this requires you to parse the updateValues which you passed to LayouTaro.RelayoutWithUpdate and need to set it to the once layouted elements.
        
        Updated elements will be re-layouted with updated values after the end of this method.
    */
    public void UpdateValues(LTElement[] elements, Dictionary<LTElementType, object> updateValues)
    {
        foreach (var e in elements)
        {
            switch (e.GetLTElementType())
            {
                case LTElementType.Image:
                    var i = (ImageElement)e;

                    // get value from updateValues and cast to the type what you set.
                    var p = updateValues[LTElementType.Image] as Image;
                    i.Image = p;
                    break;
                case LTElementType.Text:
                    var t = (TextElement)e;

                    // get value from updateValues and cast to the type what you set.
                    var tVal = updateValues[LTElementType.Text] as string;
                    t.TextContent = tVal;
                    break;

                default:
                    break;
            }
        }
    }
```


## Detecting missing Character/Emoji/Mark
use async version and set IMissingSpriteCache-implemented class to LayoutAsync method.

BasicMissingSpriteCache is the example of implementation of IMissingSpriteCache.

```csharp
LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
    canvas.transform,
    size,
    box,
    layouter,
    () =>
    {
        done = true;
    }
);
```

you can control the behaviour when missing emoji/mark or text comming.



## About BasicLayoutFunction
BasicLayoutFunction are implemented inside of the LayouTaro.

This functions helps:
* automatic text line feed.
* layouts rect with the rest of the width of view.
* centerlize the elements by the highest element in the line of elements.

and alsp async version do same things asynchronously.

## Install

use [Releases.](https://github.com/sassembla/LayouTaro/releases)

## License

see [here](https://raw.githubusercontent.com/sassembla/LayouTaro/master/LICENSE)