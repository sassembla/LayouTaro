using System;
using UnityEngine;

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
}