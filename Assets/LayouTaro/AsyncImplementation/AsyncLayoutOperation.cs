using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;

namespace UILayouTaro
{

    public class AsyncLayoutOperation
    {
        public readonly Func<bool> MoveNext;

        public AsyncLayoutOperation(Func<bool> moveNext)
        {
            this.MoveNext = moveNext;
        }
    }


    public static class AsyncLayoutRunner
    {
        private class OpsGroup
        {
            public readonly string opsId;
            public readonly List<AsyncLayoutOperation> ops;
            public readonly Action onDone;
            public OpsGroup(string opsId, List<AsyncLayoutOperation> ops, Action onDone)
            {
                this.opsId = opsId;
                this.ops = ops;
                this.onDone = onDone;
            }
        }
        private static List<OpsGroup> rootOps = new List<OpsGroup>();

        public static void Add(string opsId, List<AsyncLayoutOperation> newOps, Action onDone)
        {
            // 最初の一つだったらRunnerを追加する
            if (rootOps.Count == 0)
            {
                var chUpdateSystem = new PlayerLoopSystem()
                {
                    type = typeof(AsyncLayoutOperation),
                    updateDelegate = LayoutRunner
                };

                var playerLoop = PlayerLoop.GetDefaultPlayerLoop();

                // updateのシステムを取得する
                var updateSystem = playerLoop.subSystemList[4];
                var subSystem = new List<PlayerLoopSystem>(updateSystem.subSystemList);

                // updateブロックを追加する
                subSystem.Insert(0, chUpdateSystem);
                updateSystem.subSystemList = subSystem.ToArray();
                playerLoop.subSystemList[4] = updateSystem;

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

                var cont = OpsGroupMoveNext(opsGroup);
                if (!cont)
                {
                    // このグループが終了したので取り除く
                    rootOps.RemoveAt(i);
                    rootStartCount--;

                    // 終了実行
                    opsGroup.onDone();

                    // 全てのopsGroupが消えたら、Runner自体を削除する。
                    if (rootOps.Count == 0)
                    {
                        RemoveLayoutRunner();
                    }
                }
            }
        }

        private static bool OpsGroupMoveNext(OpsGroup opGroup)
        {
        next:
            var done = opGroup.ops[0].MoveNext();
            if (done)
            {
                // 処理が終了したOperationを取り除く。
                opGroup.ops.RemoveAt(0);

                // このopsGroupが空になったら、trueを返す
                if (opGroup.ops.Count == 0)
                {
                    return true;
                }

                // 続いて次の要素のMoveNextを実行する。
                // 終わればさらに次を。
                // もし一回実行して終わらなかった場合、ブロックを抜けてfalseを返す。
                goto next;
            }

            // まだ実行中のopsがある
            Debug.Log("継続中 opGroup.ops:" + opGroup.ops.Count);
            return false;
        }

        private static void RemoveLayoutRunner()
        {
            var playerLoop = PlayerLoop.GetDefaultPlayerLoop();

            // updateのシステムを取得する
            var updateSystem = playerLoop.subSystemList[4];
            var subSystem = new List<PlayerLoopSystem>(updateSystem.subSystemList);

            // updateブロックを削除する
            subSystem.RemoveAt(0);
            updateSystem.subSystemList = subSystem.ToArray();
            playerLoop.subSystemList[4] = updateSystem;

            PlayerLoop.SetPlayerLoop(playerLoop);
        }
    }
}