using System.Collections;
using UnityEngine;
using UILayouTaro;
using Miyamasu;
using System;
using System.Text;
using NUnit.Framework;

public class AsyncMethodTests : MiyamasuTestRunner
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
    public IEnumerator AsyncMethod()
    {
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

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 50);

        // レイアウトを行う

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

        while (!done)
        {
            yield return null;
        }

        var rectTrans = box.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator AsyncMethodComplexPattern()
    {

        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("hannidjkfajfaoooood"),// テキスト
            AsyncImageElement.GO(null),// 画像
            AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
            AsyncTextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
            AsyncImageElement.GO(null)// 画像
                                      // TextElement.GO("hannidjkfajfaoooood2")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
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

        while (!done)
        {
            yield return null;
        }

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }


    [MTest]
    public IEnumerator AsyncMethodComplexPattern2()
    {

        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("hannidjkfajfaoooood"),// テキスト
            AsyncImageElement.GO(null),// 画像
            AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
            AsyncTextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
            AsyncImageElement.GO(null),// 画像
            AsyncImageElement.GO(null),// 画像
            AsyncTextElement.GO("hannidjkfajfaoooood2")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
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

        while (!done)
        {
            yield return null;
        }

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    /*
        文字を加えると高さ位置がずれる。なんでなんだ
        そもそも横にずれてない。文字の後ろに行かない、何かある。
    */
    [MTest]
    public IEnumerator AsyncMethodSimpleEmoji()
    {

        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("a\U0001F60A")// 絵文字
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
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

        while (!done)
        {
            yield return null;
        }

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
    public IEnumerator AsyncMethodWithEmoji()
    {

        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("yasu \U0001F60A is public prob!")// テキスト
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
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

        while (!done)
        {
            yield return null;
        }

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
    public IEnumerator AsyncMethodWithEmojiComplex()
    {

        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("hannidjkfajfaoooood"),// テキスト
            AsyncImageElement.GO(null),// 画像
            AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
            AsyncTextElement.GO("hannin is yasu!\U0001F60A\U0001F60B this is public problem! goooooooooooooad")// 59から絵文字を1文字削ると？2文字減る。文字数カウント的にはUTF8と同じ扱いなのか。うーん

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
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
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

        while (!done)
        {
            yield return null;
        }

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        yield break;
    }

    [MTest]
    public IEnumerator AsyncMethodWithEmojiComplex2()
    {

        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("hannidjkfajfaoooood"),// テキスト
            AsyncImageElement.GO(null),// 画像
            AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
            AsyncTextElement.GO("hannin is yasu!\U0001F60A this is public problem! goooooooooooooad"),// テキスト
            AsyncImageElement.GO(null),// 画像
            AsyncImageElement.GO(null),// 画像
            AsyncTextElement.GO("hannidjkfajfaoooood2")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
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

        while (!done)
        {
            yield return null;
        }

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        yield break;
    }



    [MTest]
    public IEnumerator AsyncMethodDetectMissingEmoji()
    {
        // generate your own data structure with parameters for UI.
        var box = AsyncBoxElement.GO(
            null,// UI bg with image
            () =>
            {
                Debug.Log("root box element is tapped.");
            },
            AsyncTextElement.GO("\U0001F971\U0001F60A"),// emoji and mark. mark is missing by default.
            AsyncImageElement.GO(null),// image.
            AsyncButtonElement.GO(null, () => { Debug.Log("button is tapped."); })
        );

        // generate the layouter which you want to use for layout.
        var layouter = new BasicAsyncLayouter();

        // set the default size of content.
        var size = new Vector2(600, 100);

        // do layout with LayouTaro. the GameObject will be returned with layouted structure.
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

        while (!done)
        {
            yield return null;
        }

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
    public IEnumerator AsyncMethodMark()
    {
        // generate your own data structure with parameters for UI.
        var box = AsyncBoxElement.GO(
            null,// UI bg with image
            () =>
            {
                Debug.Log("root box element is tapped.");
            },
            AsyncTextElement.GO("\u26A1"),// text.
            AsyncImageElement.GO(null),// image.
            AsyncButtonElement.GO(null, () => { Debug.Log("button is tapped."); })
        );

        // generate the layouter which you want to use for layout.
        var layouter = new BasicAsyncLayouter();

        // set the default size of content.
        var size = new Vector2(600, 100);

        // do layout with LayouTaro. the GameObject will be returned with layouted structure.
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

        while (!done)
        {
            yield return null;
        }

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        yield break;
    }

    [MTest]
    public IEnumerator AsyncMethodImageAndButton()
    {
        // generate your own data structure with parameters for UI.
        var box = AsyncBoxElement.GO(
            null,// UI bg with image
            () =>
            {
                Debug.Log("root box element is tapped.");
            },
            AsyncImageElement.GO(null),// image.
            AsyncButtonElement.GO(null, () => { Debug.Log("button is tapped."); })
        );

        // generate the layouter which you want to use for layout.
        var layouter = new BasicAsyncLayouter();

        // set the default size of content.
        var size = new Vector2(600, 100);

        // do layout with LayouTaro. the GameObject will be returned with layouted structure.
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

        while (!done)
        {
            yield return null;
        }

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);

        yield break;
    }
}
