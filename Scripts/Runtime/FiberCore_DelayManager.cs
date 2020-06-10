﻿using System;
using System.Collections;
using UnityEngine;

namespace Fiber.Core
{
    public sealed class FiberCore_DelayManager : Manager, IDelayManager
    {

        public void WaitSeconds(float seconds, Action onComplete)
        {
            FiberCore.CoroutineHandler.StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return new WaitForSeconds(seconds);

                onComplete?.Invoke();
            }
        }

        public void WaitUntil(Func<bool> condition, Action onComplete)
        {
            FiberCore.CoroutineHandler.StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return new WaitUntil(condition);

                onComplete?.Invoke();
            }
        }
    }
}