using System.Collections;
using UnityEngine;
using UILayouTaro;
using Miyamasu;
using System;
using System.Text;
using NUnit.Framework;

public class AsyncSameTimeTests : MiyamasuTestRunner
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
    public IEnumerator AsyncMethodSameTime()
    {
        var done0 = false;
        var done1 = false;
        var done2 = false;
        {
            var box = AsyncBoxElement.GO(
                null,// bg画像
                () =>
                {
                    Debug.Log("ルートがタップされた");
                },
                AsyncTextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
                AsyncImageElement.GO(null),// 画像
                AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
                AsyncTextElement.GO("h")
            );

            // レイアウトに使うクラスを生成する
            var layouter = new BasicAsyncLayouter();

            // コンテンツのサイズをセットする
            var size = new Vector2(600, 50);

            // レイアウトを行う

            LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
                canvas.transform,
                size,
                box,
                layouter,
                () =>
                {
                    done0 = true;

                    var rectTrans = box.GetComponent<RectTransform>();
                    rectTrans.anchoredPosition3D = new Vector3(0, -160 * 0, 0);
                    rectTrans.localScale = Vector3.one;
                }
            );
        }

        {
            var box = AsyncBoxElement.GO(
                null,// bg画像
                () =>
                {
                    Debug.Log("ルートがタップされた");
                },
                AsyncTextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
                AsyncImageElement.GO(null),// 画像
                AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
                AsyncTextElement.GO("h")
            );

            // レイアウトに使うクラスを生成する
            var layouter = new BasicAsyncLayouter();

            // コンテンツのサイズをセットする
            var size = new Vector2(600, 50);

            // レイアウトを行う

            LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
                canvas.transform,
                size,
                box,
                layouter,
                () =>
                {
                    done1 = true;

                    var rectTrans = box.GetComponent<RectTransform>();
                    rectTrans.anchoredPosition3D = new Vector3(0, -160 * 1, 0);
                    rectTrans.localScale = Vector3.one;
                }
            );
        }

        {
            var box = AsyncBoxElement.GO(
                null,// bg画像
                () =>
                {
                    Debug.Log("ルートがタップされた");
                },
                AsyncTextElement.GO("hannin is yasu! this is public problem! gooooooooooooood"),// テキスト
                AsyncImageElement.GO(null),// 画像
                AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
                AsyncTextElement.GO("h")
            );

            // レイアウトに使うクラスを生成する
            var layouter = new BasicAsyncLayouter();

            // コンテンツのサイズをセットする
            var size = new Vector2(600, 50);

            // レイアウトを行う

            LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
                canvas.transform,
                size,
                box,
                layouter,
                () =>
                {
                    done2 = true;

                    var rectTrans = box.GetComponent<RectTransform>();
                    rectTrans.anchoredPosition3D = new Vector3(0, -160 * 2, 0);
                    rectTrans.localScale = Vector3.one;
                }
            );
        }

        while (!(done0 && done1 && done2))
        {
            yield return null;
        }

        while (false)
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

    [MTest]
    public IEnumerator AsyncMethodWithEmojiSameTime()
    {
        var done0 = false;
        var done1 = false;
        var done2 = false;
        {
            var box = AsyncBoxElement.GO(
                null,// bg画像
                () =>
                {
                    Debug.Log("ルートがタップされた");
                },
                AsyncTextElement.GO("hannin is yasu!\U0001F60A this is public problem! goooooooooooooad"),
                AsyncTextElement.GO("\U0001F971\U0001F60A"),// emoji and mark. mark is missing by default.
                AsyncImageElement.GO(null),// 画像
                AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
                AsyncTextElement.GO("h")
            );

            // レイアウトに使うクラスを生成する
            var layouter = new BasicAsyncLayouter();

            // コンテンツのサイズをセットする
            var size = new Vector2(600, 50);

            // レイアウトを行う

            LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
                canvas.transform,
                size,
                box,
                layouter,
                () =>
                {
                    done0 = true;

                    var rectTrans = box.GetComponent<RectTransform>();
                    rectTrans.anchoredPosition3D = new Vector3(0, -160 * 0, 0);
                    rectTrans.localScale = Vector3.one;
                }
            );
        }

        {
            var box = AsyncBoxElement.GO(
                null,// bg画像
                () =>
                {
                    Debug.Log("ルートがタップされた");
                },
                AsyncTextElement.GO("hannin is yasu!\U0001F60A this is public problem! goooooooooooooad"),
                AsyncTextElement.GO("\U0001F971\U0001F60A"),// emoji and mark. mark is missing by default.
                AsyncImageElement.GO(null),// 画像
                AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
                AsyncTextElement.GO("h")
            );

            // レイアウトに使うクラスを生成する
            var layouter = new BasicAsyncLayouter();

            // コンテンツのサイズをセットする
            var size = new Vector2(600, 50);

            // レイアウトを行う

            LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
                canvas.transform,
                size,
                box,
                layouter,
                () =>
                {
                    done1 = true;

                    var rectTrans = box.GetComponent<RectTransform>();
                    rectTrans.anchoredPosition3D = new Vector3(0, -160 * 1, 0);
                    rectTrans.localScale = Vector3.one;
                }
            );
        }

        {
            var box = AsyncBoxElement.GO(
                null,// bg画像
                () =>
                {
                    Debug.Log("ルートがタップされた");
                },
                AsyncTextElement.GO("hannin is yasu!\U0001F60A this is public problem! goooooooooooooad"),
                AsyncTextElement.GO("\U0001F971\U0001F60A"),// emoji and mark. mark is missing by default.
                AsyncImageElement.GO(null),// 画像
                AsyncButtonElement.GO(null, () => { Debug.Log("ボタンがタップされた"); }),
                AsyncTextElement.GO("h")
            );

            // レイアウトに使うクラスを生成する
            var layouter = new BasicAsyncLayouter();

            // コンテンツのサイズをセットする
            var size = new Vector2(600, 50);

            // レイアウトを行う

            LayouTaro.LayoutAsync<BasicMissingSpriteCache>(
                canvas.transform,
                size,
                box,
                layouter,
                () =>
                {
                    done2 = true;

                    var rectTrans = box.GetComponent<RectTransform>();
                    rectTrans.anchoredPosition3D = new Vector3(0, -160 * 2, 0);
                    rectTrans.localScale = Vector3.one;
                }
            );
        }

        while (!(done0 && done1 && done2))
        {
            yield return null;
        }

        ScreenCapture.CaptureScreenshot("./images/" + methodName);
        yield break;
    }

}
