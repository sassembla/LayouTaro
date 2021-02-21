using System;
using System.Collections.Generic;
using UnityEngine;

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
                // 初期opを取り出す
                var targetOp = ops[0];

                // レイアウトの起点
                var baseRefs = targetOp.refs;

                // ここでRectTransformを取り出し、refsのlinedにセットする必要がある。
                baseRefs.lineContents.Add(targetOp.rectTrans);

                while (true)
                {
                    var (cont, refs) = targetOp.MoveNext();

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

                        // 次のopを取り出す
                        targetOp = ops[0];

                        // コンテンツが継続してレイアウトされるための残り幅を更新し渡す
                        baseRefs.restWidth = viewWidth - endOpsOriginX;

                        // 新たなOpを現在まで積まれている行配列に追加、行単位のy揃えに利用する。
                        baseRefs.lineContents.Add(targetOp.rectTrans);

                        // refsを更新する
                        targetOp.UpdateRef(baseRefs);

                        // 開始位置に戻り、次のOpのMoveNextを行う
                        continue;
                    }

                    // まだ実行中のopsがある場合、yieldで抜ける。
                    yield return (true, refs);
                }

                yield return (false, baseRefs);
            }
        }


        private static List<OpsGroup> rootOps = new List<OpsGroup>();

        private static AsyncRunnerComponent runnerComponent;

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

            // add runner for LayouTaro itself.
            // we stopped using RunLoop for this for avoiding confusion against main loop.
            if (runnerComponent == null)
            {
                var go = new GameObject();
                runnerComponent = go.AddComponent<AsyncRunnerComponent>();
                runnerComponent.Initialize(LayoutRunner);
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
                }
            }
        }
    }
}