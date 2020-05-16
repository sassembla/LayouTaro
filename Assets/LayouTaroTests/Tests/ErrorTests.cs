using System.Collections;
using UnityEngine;
using UILayouTaro;
using Miyamasu;
using System;
using System.Text;
using NUnit.Framework;
using System.Collections.Generic;

public class ErrorTests : MiyamasuTestRunner
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
    public IEnumerator EmptyStringError()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO(null)// テキスト
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
    public IEnumerator EmptyStringErrorAsync()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO(null)// テキスト
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

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


        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }


    [MTest]
    public IEnumerator MarkContinues()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("\u26A1\u26A1")// 連続する記号
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
    public IEnumerator MarkContinuesAsync()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("\u26A1\u26A1")// 連続する記号
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

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

        yield return null;

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }




    [MTest]
    public IEnumerator SpacesAndLayout()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("a  ab  bc   cd   d\naxxabxxbcxxxcdxxxd日本語")// 連続するスペースと文字
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
    public IEnumerator SpacesAndLayoutAsync()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncTextElement.GO("a  ab  bc   cd   d\naxxabxxbcxxxcdxxxd日本語")// 連続するスペースと文字
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

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

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }



    [MTest]
    public IEnumerator SpacesAndLayoutRelayout()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            TextElement.GO("a  ab  bc   cd   d\naxxabxxbcxxxcdxxxd日本語")// 連続するスペースと文字
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

        LayouTaro.RelayoutWithUpdate(
            size,
            box,
            new Dictionary<LTElementType, object> {
                {LTElementType.Text, "a  ab  bc   cd   d\naxxabxxbcxxxcdxxxd日本語"}
            },
            layouter
        );

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }





    [MTest]
    public IEnumerator SpacesAndLayout2()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            ButtonElement.GO(null, () => { }),
            TextElement.GO("ここから　　　　　　　　　　　　　　　　　　　　・　　　　　　　　　　　　　ここまで")// 連続するスペースと文字
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
    public IEnumerator SpacesAndLayout2Relayout()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            ButtonElement.GO(null, () => { }),
            TextElement.GO("ここから　　　　　　　　　　　　　　　　　　　　・　　　　　　　　　　　　　ここまで")// 連続するスペースと文字
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

        LayouTaro.RelayoutWithUpdate(
            size,
            box,
            new Dictionary<LTElementType, object> {
                {LTElementType.Text, "ここから　　　　　　　　　　　　　　　　　　　　・　　　　　　　　　　　　　ここまで"}
            },
            layouter
        );

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator SpacesAndLayout2Async()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncButtonElement.GO(null, () => { }),
            AsyncTextElement.GO("ここから　　　　　　　　　　　　　　　　　　　　・　　　　　　　　　　　　　ここまで")// 連続するスペースと文字
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

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

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator SpacesAndLayout2RelayoutAsync()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncButtonElement.GO(null, () => { }),
            AsyncTextElement.GO("ここから　　　　　　　　　　　　　　　　　　　　・　　　　　　　　　　　　　ここまで")// 連続するスペースと文字
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

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

        yield return LayouTaro.RelayoutWithUpdateAsync<BasicMissingSpriteCache>(
            size,
            box,
            new Dictionary<LTElementType, object> {
                {LTElementType.AsyncText, "ここから　　　　　　　　　　　　　　　　　　　　・　　　　　　　　　　　　　ここまで"}
            },
            layouter
        );

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }




    [MTest]
    public IEnumerator SpacesAndLayout3()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            ButtonElement.GO(null, () => { }),
            TextElement.GO(
                "ここから　　　　　　　　　" +// 全角スペースが9文字あり、かなりのサイズになる。
                "・")// 連続するスペースと文字、・の直前で改行が発生する。
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
    public IEnumerator SpacesAndLayout3Relayout()
    {
        var box = BoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            ButtonElement.GO(null, () => { }),
            TextElement.GO(
                "ここから　　　　　　　　　" +// 全角スペースが9文字あり、かなりのサイズになる。
                "・"
                )// 連続するスペースと文字、・の直前で改行が発生する。
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

        box = LayouTaro.RelayoutWithUpdate(
            size,
            box,
            new Dictionary<LTElementType, object>{
                {
                    LTElementType.Text,"ここから　　　　　　　　　" +// 全角スペースが9文字あり、かなりのサイズになる。
                    "・"
                }
            },
            layouter
        );

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }


    [MTest]
    public IEnumerator SpacesAndLayout3Async()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncButtonElement.GO(null, () => { }),
            AsyncTextElement.GO(
                "ここから　　　　　　　　　" +// 全角スペースが9文字あり、かなりのサイズになる。
                "・")// 連続するスペースと文字、・の直前で改行が発生する。
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

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

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator SpacesAndLayout3RelayoutAsync()
    {
        var box = AsyncBoxElement.GO(
            null,// bg画像
            () =>
            {
                Debug.Log("ルートがタップされた");
            },
            AsyncButtonElement.GO(null, () => { }),
            AsyncTextElement.GO(
                "ここから　　　　　　　　　" +// 全角スペースが9文字あり、かなりのサイズになる。
                "・")// 連続するスペースと文字、・の直前で改行が発生する。
        );

        // レイアウトに使うクラスを生成する
        var layouter = new BasicAsyncLayouter();

        // コンテンツのサイズをセットする
        var size = new Vector2(600, 100);

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

        yield return LayouTaro.RelayoutWithUpdateAsync<BasicMissingSpriteCache>(
            size,
            box,
            new Dictionary<LTElementType, object>{
                {
                    LTElementType.AsyncText,"ここから　　　　　　　　　" +// 全角スペースが9文字あり、かなりのサイズになる。
                    "・"
                }
            },
            layouter
        );

        yield return null;

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }
}
