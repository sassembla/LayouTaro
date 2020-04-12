using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;

namespace UILayouTaro
{

    public class AsyncLayoutOperation
    {
        public readonly RectTransform rectTrans;
        public readonly Func<(bool, ParameterReference)> MoveNext;
        public ParameterReference refs;

        public AsyncLayoutOperation(RectTransform rectTrans, ParameterReference refs, Func<(bool, ParameterReference)> moveNext)
        {
            this.rectTrans = rectTrans;
            this.refs = refs;
            this.MoveNext = moveNext;
        }

        public void UpdateRef(ParameterReference baseRefs)
        {
            this.refs.id = baseRefs.id;

            this.refs.originX = baseRefs.originX;
            this.refs.originY = baseRefs.originY;
            this.refs.restWidth = baseRefs.restWidth;
            this.refs.currentLineMaxHeight = baseRefs.currentLineMaxHeight;
            this.refs.lineContents = baseRefs.lineContents;
        }
    }


    public static class AsyncLayoutExecutor
    {
        private class OpsGroup
        {
            public readonly Action<ParameterReference> onDone;

            public readonly IEnumerator<(bool, ParameterReference)> cor;

            public OpsGroup(string opsId, List<AsyncLayoutOperation> ops, Action<ParameterReference> onDone)
            {
                this.onDone = onDone;// グループ全体のDone扱い、上位側で調整した後に実行される。
                this.cor = RunLayout(opsId, ops);
            }

            // このオブジェクトの中でopsを回して、ParamRefを引きまわす。
            private IEnumerator<(bool, ParameterReference)> RunLayout(string opsId, List<AsyncLayoutOperation> ops)
            {
                // レイアウトの起点
                var baseRefs = ops[0].refs;
                Debug.Log("first id:" + baseRefs.id + " now frameCount:" + Time.frameCount + " baseRefs:" + baseRefs.ToString());

                while (true)
                {
                    // ここでRectTransformを取り出し、refsのlinedにセットする必要がある。これめちゃくちゃだなあ。
                    baseRefs.lineContents.Add(ops[0].rectTrans);

                    var (cont, refs) = ops[0].MoveNext();

                    Debug.Log("cont:" + cont + " refs.id:" + refs.id + " baseRefs:" + baseRefs.ToString());

                    baseRefs = refs;
                    if (!cont)
                    {
                        Debug.Log("このOpsのCor終了！");

                        // 処理が終了したOperationを取り除く。
                        ops.RemoveAt(0);

                        // このopsGroupが空になったら、trueを返す
                        if (ops.Count == 0)
                        {
                            break;
                        }

                        // refsを更新する
                        ops[0].UpdateRef(baseRefs);

                        // 開始位置に戻り、次のOpsのMoveNextを行う
                        continue;
                    }

                    // まだ実行中のopsがある場合、yieldで抜ける。
                    Debug.Log("継続中 opGroup.ops:" + ops.Count);// このログがブロック版でも出ちゃうのなんかあるな。まあはい。
                    yield return (true, refs);
                }

                yield return (false, baseRefs);
            }
        }


        private static List<OpsGroup> rootOps = new List<OpsGroup>();

        public static void LaunchLayoutOps(string opsId, List<AsyncLayoutOperation> newOps, Action<ParameterReference> onDone)
        {
            // 最初の一つだったらRunnerを追加する
            if (rootOps.Count == 0)
            {
                var layoutUpdateSystem = new PlayerLoopSystem()
                {
                    type = typeof(AsyncLayoutOperation),
                    updateDelegate = LayoutRunner
                };

                var playerLoop = PlayerLoop.GetDefaultPlayerLoop();

                // updateのシステムを取得する
                var updateSystem = playerLoop.subSystemList[4];
                var subSystem = new List<PlayerLoopSystem>(updateSystem.subSystemList);

                // updateブロックを追加する
                subSystem.Add(layoutUpdateSystem);
                updateSystem.subSystemList = subSystem.ToArray();
                playerLoop.subSystemList[4] = updateSystem;

                // セット
                PlayerLoop.SetPlayerLoop(playerLoop);
            }

            rootOps.Add(new OpsGroup(opsId, newOps, onDone));
        }

        private static void LayoutRunner()
        {
            var rootStartCount = rootOps.Count;
            for (var i = 0; i < rootStartCount; i++)
            {
                var opsGroup = rootOps[i];

                // 実行
                opsGroup.cor.MoveNext();

                var (cont, pos) = opsGroup.cor.Current;
                if (!cont)
                {
                    // このグループが終了したので取り除く
                    rootOps.RemoveAt(i);
                    rootStartCount--;

                    // 終了実行
                    opsGroup.onDone(pos);

                    // 全てのopsGroupが消えたら、Runner自体を削除する。
                    if (rootOps.Count == 0)
                    {
                        RemoveLayoutRunner();
                    }
                }
            }
        }

        private static void RemoveLayoutRunner()
        {
            var playerLoop = PlayerLoop.GetDefaultPlayerLoop();

            // updateのシステムを取得する
            var updateSystem = playerLoop.subSystemList[4];
            var subSystem = new List<PlayerLoopSystem>(updateSystem.subSystemList);

            // remove AsyncLayoutOperation.
            var count = subSystem.Count;
            for (var i = 0; i < count; i++)
            {
                var item = subSystem[i];
                if (item.type == typeof(AsyncLayoutOperation))
                {
                    subSystem.RemoveAt(i);
                    break;
                }
            }

            // セット
            updateSystem.subSystemList = subSystem.ToArray();
            playerLoop.subSystemList[4] = updateSystem;

            PlayerLoop.SetPlayerLoop(playerLoop);
        }
    }
}