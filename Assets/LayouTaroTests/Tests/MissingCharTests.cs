using System.Collections;
using UnityEngine;
using UILayouTaro;
using Miyamasu;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class MissingCharTests : MiyamasuTestRunner
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
    public IEnumerator GetMissingChar()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("あいうえお\U00011580")// missingになる梵字
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        var missingDetected = false;
        LayouTaro.SetOnMissingCharacterFound(
            missingChars =>
            {
                missingDetected = true;
            }
        );

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

        Assert.True(missingDetected);

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }


    [MTest]
    public IEnumerator GetMissingChar2()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("a\U00011580a")// missingになる梵字
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        var missingDetected = false;
        LayouTaro.SetOnMissingCharacterFound(
            missingChars =>
            {
                missingDetected = true;
            }
        );

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

        Assert.True(missingDetected);

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator GetMissingChar3()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("aaaaaaaaaaaaabbbbbbbbb💚🎮✨✨cccccccccccccccccccccccccccccccddddddddddddddddddddd")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        var missingDetected = false;
        LayouTaro.SetOnMissingCharacterFound(
            missingChars =>
            {
                missingDetected = true;
            }
        );

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

        Assert.True(missingDetected);

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator GetMissingChar3Relayout()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("aaaaaaaaaaaaabbbbbbbbb💚🎮✨✨cccccccccccccccccccccccccccccccddddddddddddddddddddd")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        var missingDetected = false;
        LayouTaro.SetOnMissingCharacterFound(
            missingChars =>
            {
                missingDetected = true;
            }
        );

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

        Assert.True(missingDetected);

        box = LayouTaro.RelayoutWithUpdate(
            size,
            box,
            new Dictionary<LTElementType, object>()
            {
                {LTElementType.Text, "aaaaaaaaaaaaabbbbbbbbb💚🎮✨✨cccccccccccccccccccccccccccccccddddddddddddddddddddd"}
            },
            layouter
        );

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator GetMissingCharAsync()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("あいうえお\U00011580")// missingになる梵字
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        var missingDetected = false;
        var cache = InternalCachePool.Get<BasicMissingSpriteCache>();
        cache.Debug_OnMissingCharacter(
            () =>
            {
                missingDetected = true;
            }
        );

        // レイアウトを行う

        yield return LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
            canvas.transform,
            size,
            box,
            layouter
        );


        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        Assert.True(missingDetected);

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }


    [MTest]
    public IEnumerator GetMissingChar2Async()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("a\U00011580a")// missingになる梵字
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        var missingDetected = false;
        var cache = InternalCachePool.Get<BasicMissingSpriteCache>();
        cache.Debug_OnMissingCharacter(
            () =>
            {
                missingDetected = true;
            }
        );

        // レイアウトを行う

        yield return LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        Assert.True(missingDetected);

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }


    [MTest]
    public IEnumerator GetMissingChar3Async()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("aaaaaaaaaaaaabbbbbbbbb💚🎮✨✨cccccccccccccccccccccccccccccccddddddddddddddddddddd")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        var missingDetected = false;

        var cache = InternalCachePool.Get<BasicMissingSpriteCache>();
        cache.Debug_OnMissingCharacter(
            () =>
            {
                missingDetected = true;
            }
        );

        // レイアウトを行う

        yield return LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        Assert.True(missingDetected);

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator GetMissingChar3RelayoutAsync()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("aaaaaaaaaaaaabbbbbbbbb💚🎮✨✨cccccccccccccccccccccccccccccccddddddddddddddddddddd")
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

        var missingDetected = false;
        var cache = InternalCachePool.Get<BasicMissingSpriteCache>();
        cache.Debug_OnMissingCharacter(
            () =>
            {
                missingDetected = true;
            }
        );

        // レイアウトを行う

        yield return LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.gameObject.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        Assert.True(missingDetected);

        yield return LayouTaro.RelayoutWithUpdateAsync<BasicMissingSpriteCache>(
            size,
            box,
            new Dictionary<LTElementType, object>()
            {
                {LTElementType.AsyncText, "aaaaaaaaaaaaabbbbbbbbb💚🎮✨✨cccccccccccccccccccccccccccccccddddddddddddddddddddd"}
            },
            layouter
        );

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

}
