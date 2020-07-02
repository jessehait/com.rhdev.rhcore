﻿using System.Collections;
using UnityEngine;

namespace Fiber
{
    public class CoroutineHandler : MonoBehaviour, ICoroutineHandler
    {
        public new Coroutine StartCoroutine(IEnumerator method)
        {
            return base.StartCoroutine(method);
        }

        public new void StopCoroutine(Coroutine coroutine)
        {
            base.StopCoroutine(coroutine);
        }
    }
}