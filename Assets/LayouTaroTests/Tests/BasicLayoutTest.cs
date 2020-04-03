using System.Collections;
using UnityEngine;
using UILayouTaro;
using Miyamasu;
using System;

public class BasicLayoutTest : MiyamasuTestRunner
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
        Debug.Log("canvas:" + canvas);
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
        var layouter = new MyLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
        var go = box.gameObject;
        go = LayouTaro.Layout<BoxElement>(
            canvas.transform,
            size,
            go,
            layouter
        );

        var rectTrans = go.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;


        ScreenCapture.CaptureScreenshot("./images/0_BasicPattern");
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
        var layouter = new MyLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
        var go = box.gameObject;
        go = LayouTaro.Layout<BoxElement>(
            canvas.transform,
            size,
            go,
            layouter
        );

        var rectTrans = go.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;


        ScreenCapture.CaptureScreenshot("./images/1_ComplexPattern");
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
        var layouter = new MyLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
        var go = box.gameObject;
        go = LayouTaro.Layout<BoxElement>(
            canvas.transform,
            size,
            go,
            layouter
        );

        var rectTrans = go.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;


        ScreenCapture.CaptureScreenshot("./images/2_ComplexPattern2");
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
        var layouter = new MyLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
        var go = box.gameObject;
        go = LayouTaro.Layout<BoxElement>(
            canvas.transform,
            size,
            go,
            layouter
        );

        var rectTrans = go.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/3_WithEmoji");

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
            TextElement.GO("hannin is yasu!\U0001F60A this is public problem! goooooooooooooad")// 59から絵文字を1文字削ると？2文字減る。文字数カウント的にはUTF8と同じ扱いなのか。うーん

        // 事前にサイズを取得、インデックスポイントを送り込んで、取り除いて、という形にするか。
        // どうすればできる？載せ替える四角を成立させればいいか。単純に文字列コンテンツを絵文字でぶった切るか！！これは手な気がする。
        // 他になんかないかな。
        );

        // レイアウトに使うクラスを生成する
        var layouter = new MyLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
        var go = box.gameObject;
        go = LayouTaro.Layout<BoxElement>(
            canvas.transform,
            size,
            go,
            layouter
        );

        var rectTrans = go.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/4_WithEmoji");
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
        var layouter = new MyLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        // レイアウトを行う
        var go = box.gameObject;
        go = LayouTaro.Layout<BoxElement>(
            canvas.transform,
            size,
            go,
            layouter
        );

        var rectTrans = go.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        ScreenCapture.CaptureScreenshot("./images/5_WithEmoji");

        yield break;
    }
}
