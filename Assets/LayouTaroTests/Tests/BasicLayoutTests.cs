using System.Collections;
using UnityEngine;
using UILayouTaro;
using Miyamasu;
using System;
using System.Text;
using NUnit.Framework;

public class BasicLayoutTests : MiyamasuTestRunner
{
    private Canvas canvas;

    [MSetup]
    public void Setup()
    {
        var canvasObj = GameObject.Find("Canvas");
        var cameraObj = GameObject.Find("Main Camera");
        if (canvasObj == null)
        {
            Camera camera = null;
            if (cameraObj == null)
            {
                camera = new GameObject("Main Camera").AddComponent<Camera>();
            }

            // イベントシステムを追加してもボタンとかは効かないが、まあ、はい。
            // new GameObject("EventSystem").AddComponent<EventSystem>();

            canvas = new GameObject("TestCanvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            return;
        }

        throw new Exception("不要なデータが残っている");
    }

    [MTeardown]
    public void Teardown()
    {
        var canvasObj = GameObject.Find("TestCanvas");
        var cameraObj = GameObject.Find("Main Camera");
        GameObject.Destroy(canvasObj);
        GameObject.Destroy(cameraObj);
    }

    [MTest]
    public IEnumerator BasicPattern()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
            ImageElement.GO(null),// 画像
            ButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); })
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;


        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator ComplexPattern()
    {
        // 最後のgooooo..dが分離されて浮くように。
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("hannidjkfajfaoooood"),// テキスト
            ImageElement.GO(null),// 画像
            ButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
            TextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
            ImageElement.GO(null)// 画像
                                 // TextElement.GO("hannidjkfajfaoooood2")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;


        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }


    [MTest]
    public IEnumerator ComplexPattern2()
    {
        // 最後のgooooo..dが分離されて浮くように。
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("hannidjkfajfaoooood"),// テキスト
            ImageElement.GO(null),// 画像
            ButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
            TextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
            ImageElement.GO(null),// 画像
            ImageElement.GO(null),// 画像
            TextElement.GO("hannidjkfajfaoooood2")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;


        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }


    [MTest]
    public IEnumerator WithEmoji()
    {
        // 最後のgooooo..dが分離されて浮くように。
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("yasu \U0001F60A is public prob!")// テキスト
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        while (false)
        {
            yield return null;
        }

        yield break;
    }


    [MTest]
    public IEnumerator WithEmojiComplex()
    {
        // 最後のgooooo..dが分離されて浮くように。
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("hannidjkfajfaoooood"),// テキスト
            ImageElement.GO(null),// 画像
            ButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
            TextElement.GO("hannin is yasu!\U0001F60A\U0001F60B this is public problem! goooooooooooooad")// 59から絵文字を1文字削ると？2文字減る。文字数カウント的にはUTF8と同じ扱いなのか。うーん

        // 事前にサイズを取得、インデックスポイントを送り込んで、取り除いて、という形にするか。
        // どうすればできる？載せ替える四角を成立させればいいか。単純に文字列コンテンツを絵文字でぶった切るか！！これは手な気がする。
        // 他になんかないかな。いけるな、
        // 文字列を最初に渡すときに、N個の絵文字が出た場合、\Uで検索して文字を取り出す。そんで、その文字がどんな画像になるか、っていうのを別途やる。そんで、
        // それぞれの文字を出力して、サイズを割り出し、文字列を再構成する。
        // 再構成した上でレイアウトすればいい。で、それぞれのレイアウト終了時に絵文字の画像を割り当てるか。
        // この方法であれば、まあ、そもそも画像を割り当てるような置換ができれば優勝できるな。

        // 割り出し方に問題が出るかな、例があると嬉しいな。ない。今はない。
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        while (false)
        {
            yield return null;
        }

        yield break;
    }

    [MTest]
    public IEnumerator WithEmojiComplex2()
    {
        // 最後のgooooo..dが分離されて浮くように。
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("hannidjkfajfaoooood"),// テキスト
            ImageElement.GO(null),// 画像
            ButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
            TextElement.GO("hannin is yasu!\U0001F60A this is public problem! goooooooooooooad"),// テキスト
            ImageElement.GO(null),// 画像
            ImageElement.GO(null),// 画像
            TextElement.GO("hannidjkfajfaoooood2")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        while (false)
        {
            yield return null;
        }

        yield break;
    }



    [MTest]
    public IEnumerator DetectMissingEmoji()
    {
        // generate your own data structure with parameters for UI.
        var box = BoxElement.GO(
            null,// UI bg with image
            () =>
            {
                Debug.Log("root box element is tapped.");
            },
            TextElement.GO("\U0001F971\U0001F60A"),// text.
            ImageElement.GO(null),// image.
            ButtonElement.GO(null, () => { Debug.Log("button is tapped."); })
        );

        // generate the layouter which you want to use for layout.
        var layouter = new BasicLayouter();

        // set the default size of content.
        var size = new Vector2(600, 100);

        var done = false;
        LayouTaro.SetOnMissingCharacterFound(chs =>
        {
            var bytes = Encoding.UTF8.GetBytes(chs);
            foreach (var ch in bytes)
            {
                Debug.Log("missing ch byte:" + ch);
            }
            done = true;
        });
        // do layout with LayouTaro. the GameObject will be returned with layouted structure.

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        Assert.True(done);
        yield break;
    }

    [MTest]
    public IEnumerator Mark()
    {
        // generate your own data structure with parameters for UI.
        var box = BoxElement.GO(
            null,// UI bg with image
            () =>
            {
                Debug.Log("root box element is tapped.");
            },
            TextElement.GO("\u26A1"),// text.
            ImageElement.GO(null),// image.
            ButtonElement.GO(null, () => { Debug.Log("button is tapped."); })
        );

        // generate the layouter which you want to use for layout.
        var layouter = new BasicLayouter();

        // set the default size of content.
        var size = new Vector2(600, 100);

        // do layout with LayouTaro. the GameObject will be returned with layouted structure.

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        yield break;
    }


    [MTest]
    public IEnumerator ImageAndButton()
    {
        // generate your own data structure with parameters for UI.
        var box = BoxElement.GO(
            null,// UI bg with image
            () =>
            {
                Debug.Log("root box element is tapped.");
            },
            ImageElement.GO(null),// image.
            ButtonElement.GO(null, () => { Debug.Log("button is tapped."); })
        );

        // generate the layouter which you want to use for layout.
        var layouter = new BasicLayouter();

        // set the default size of content.
        var size = new Vector2(600, 100);

        // do layout with LayouTaro. the GameObject will be returned with layouted structure.

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        yield return null;
        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        yield break;
    }

    [MTest]
    public IEnumerator LayoutOneLineTextDoesNotContainInTheEndOfLine()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("hannin is yasu! this is public problem! gooooooood"),// テキスト
            ImageElement.GO(null),// 画像
            ImageElement.GO(6.558f + 0.0002f, 10f),
            TextElement.GO("lllllllllllllx"),
            TextElement.GO("dijklmnoああああああああああああいえああああ")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator LayoutHeadOfText()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
            ImageElement.GO(null),// 画像
            TextElement.GO("dijklmnoああああああああああああいえああああ")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator MultilineAfterFilledOneLine()
    {
        // 行頭からの改行を行う要素がY位置ゼロよりも下方で発生する場合にレイアウトが狂うバグ
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("dijklmnoああああああああああああ"),
            TextElement.GO("dijklmnoああああああああああああいえああああ")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator MultilineAfterFilledOneLineLong()
    {
        // 行頭からの改行を行う要素がY位置ゼロよりも下方で発生する場合にレイアウトが狂うバグ
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("dijklmnoああああああああああああ"),
            TextElement.GO("dijklmnoああああああああああああいえあああああああああああああああああああああああああああああああああああああああ")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う

        box = LayouTaro.Layout(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }
}
