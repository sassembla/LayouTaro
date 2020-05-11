using System.Collections;
using UnityEngine;
using UILayouTaro;
using Miyamasu;
using System;
using System.Text;
using NUnit.Framework;

public class WrappedSizeTests : MiyamasuTestRunner
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
    public IEnumerator WrappedSize()
    {
        /*
            すべてのcontentが入っている箱のサイズが欲しい。
        */
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
        yield return LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
            canvas.transform,
            size,
            box,
            layouter
        );

        var rectTrans = box.GetComponent<RectTransform>();
        rectTrans.anchoredPosition3D = Vector3.zero;
        rectTrans.localScale = Vector3.one;

        yield return null;
        Debug.Log("まだ。");
        // ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }
}
