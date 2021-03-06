﻿using Fiber.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fiber
{
    public class FiberCore_PoolManager :Manager, IPoolManager
    {
        private Dictionary<Type, IPool> _pools = new Dictionary<Type, IPool>();
        private GameObject              _root;


        public override void Initialize()
        {
            FiberCore.Coroutine.Start(CleanUpCycle());
        }


        public IPool PutElement<T>(T prefab, int count = 1) where T : PoolElement
        {
            IPool pool;

            if (Exists(typeof(T)))
            {
                pool = GetPool<T>();
            }
            else
            {
                pool = CreatePool<T>(prefab);
            }

            pool.PutElement(prefab, count);

            return pool;
        }


        public IPool PutElement<T>(T prefab, int count = 1, bool startCleaningCycle = false) where T : PoolElement
        {
            IPool pool = PutElement(prefab, count);

            ((Pool)pool).CleanUp(startCleaningCycle);

            return pool;
        }


        public T GetElement<T>(Transform newParent = null) where T : PoolElement
        {
            IPool pool;

            if (Exists(typeof(T)))
            {
                pool = GetPool<T>();
            }
            else
            {
                Debug.Log("There is no pool of type : \"" + typeof(T).Name + "\". Please Put something of this type.");
                return null;
            }

            return pool.GetElements<T>(1, newParent)[0];
        }


        public T[] GetElements<T>(int count = 1, Transform newParent = null) where T : PoolElement
        {
            IPool pool;

            if (Exists(typeof(T)))
            {
                pool = GetPool<T>();
            }
            else
            {
                Debug.LogError("There is no pool of type : \"" + typeof(T).Name + "\". Please Put something of this type.");
                return null;
            }

            return pool.GetElements<T>(count, newParent);
        }


        public IPool GetPool<T>() where T : PoolElement
        {
            return _pools[typeof(T)];
        }


        private void CreateManagerRoot()
        {
            _root = new GameObject("[Module]: PoolManager");
            _root.transform.SetParent(FiberCore.Root);
        }


        private bool Exists(Type type)
        {
            return _pools.ContainsKey(type);
        }


        private IPool CreatePool<T>(PoolElement originalPrefab) where T : PoolElement
        {
            var type = typeof(T);

            var poolRoot = new GameObject("[Pool of type]: " + type.Name);

            if (!_root)
            {
                CreateManagerRoot();
            }

            poolRoot.transform.SetParent(_root.transform);

            var pool = poolRoot.AddComponent<Pool>();

            pool.Initialize(originalPrefab);

            _pools.Add(type, pool);

            return pool;
        }


        private IEnumerator CleanUpCycle()
        {
            while (FiberCore.Configurations.PoolCleanUpRate > 0)
            {
                yield return new WaitForEndOfFrame();

                foreach (var pool in _pools)
                {
                    ((Pool)pool.Value).CleanCycle();
                }
            }
        }

    }
}
