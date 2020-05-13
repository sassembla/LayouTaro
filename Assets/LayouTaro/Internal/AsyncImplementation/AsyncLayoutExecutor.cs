using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;

namespace UILayouTaro
{
    public static class AsyncLayoutExecutor
    {
        private class OpsGroup
        {
            public readonly Action<ParameterReference> onDone;

            public readonly IEnumerator<(bool, ParameterReference)> cor;

            public OpsGroup(string opsId, float viewWidth, List<AsyncLayoutOperation> ops, Action<ParameterReference> onDone)
            {
                this.onDone = onDone;// グループ全体のDone扱い、上位側で調整した後に実行される。
                this.cor = RunLayout(opsId, viewWidth, ops);
            }

            // このオブジェクトの中でopsを回して、ParamRefを引きまわす。
            private IEnumerator<(bool, ParameterReference)> RunLayout(string opsId, float viewWidth, List<AsyncLayoutOperation> ops)
            {
                var targeOp = ops[0];

                // レイアウトの起点
                var baseRefs = targeOp.refs;

                // ここでRectTransformを取り出し、refsのlinedにセットする必要がある。
                baseRefs.lineContents.Add(targeOp.rectTrans);

                while (true)
                {
                    var (cont, refs) = targeOp.MoveNext();

                    baseRefs = refs;

                    // 終了検知
                    if (!cont)
                    {
                        // 処理が終了したOperationを取り除く。
                        ops.RemoveAt(0);

                        // このopsGroupが空になったら、ループを抜けて最終的なレスポンスを返す。
                        if (ops.Count == 0)
                        {
                            break;
                        }

                        var endOpsOriginX = baseRefs.originX;

                        targeOp = ops[0];

                        baseRefs.restWidth = viewWidth - endOpsOriginX;
                        baseRefs.lineContents.Add(targeOp.rectTrans);

                        // refsを更新する
                        targeOp.UpdateRef(baseRefs);

                        // 開始位置に戻り、次のOpsのMoveNextを行う
                        continue;
                    }

                    // まだ実行中のopsがある場合、yieldで抜ける。
                    yield return (true, refs);
                }

                yield return (false, baseRefs);
            }
        }


        private static List<OpsGroup> rootOps = new List<OpsGroup>();

        public static void LaunchLayoutOps(string opsId, float viewWidth, List<AsyncLayoutOperation> newOps, Action<ParameterReference> onDone)
        {
            var opsGroup = new OpsGroup(opsId, viewWidth, newOps, onDone);
            var opsCont = opsGroup.cor.MoveNext();
            var (cont, pos) = opsGroup.cor.Current;
            if (!cont)
            {
                opsGroup.onDone(pos);
                return;
            }

            // 最初の一つだったらRunnerを追加する
            if (rootOps.Count == 0)
            {
                var layoutUpdateSystem = new PlayerLoopSystem()
                {
                    type = typeof(AsyncLayoutExecutor),
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

            rootOps.Add(opsGroup);
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
                if (item.type == typeof(AsyncLayoutExecutor))
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