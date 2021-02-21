using System;
using UnityEngine;
namespace UILayouTaro
{
    public class AsyncRunnerComponent : MonoBehaviour
    {
        private Action updateAct;
        public void Initialize(Action onUpdateAction)
        {
            // guard myself from destroy.
            DontDestroyOnLoad(this.gameObject);

            // set update action for every update.
            this.updateAct = onUpdateAction;
        }

        void Update()
        {
            updateAct();
        }
    }
}