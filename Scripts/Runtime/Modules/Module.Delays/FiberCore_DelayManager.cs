﻿using System;
using System.Collections;
using UnityEngine;

namespace Fiber
{
    public sealed class FiberCore_DelayManager : Manager, IDelayManager
    {
        public override void Initialize()
        {

        }

        public void WaitSeconds(float seconds, Action onComplete)
        {
            FiberCore.Coroutine.Start(Routine());

            IEnumerator Routine()
            {
                yield return new WaitForSeconds(seconds);

                onComplete?.Invoke();
            }
        }

        public void WaitUntil(Func<bool> condition, Action onComplete)
        {
            FiberCore.Coroutine.Start(Routine());

            IEnumerator Routine()
            {
                yield return new WaitUntil(condition);

                onComplete?.Invoke();
            }
        }
    }
}