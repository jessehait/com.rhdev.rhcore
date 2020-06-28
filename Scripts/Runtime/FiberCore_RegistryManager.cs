﻿using FiberCore.Data;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace FiberCore
{
    public class FiberCore_RegistryManager : Manager, IRegistryManager
    {
        private BasicData _data;
        private const string DEFAULT_NAME = "fibercore_registry_data";

        public event Action OnSaveRequested;
        public event Action OnLoadRequested;

        public override void Initialize()
        {

        }

        public void RegisterType<T>() where T : BasicData, new()
        {
            Reset<T>();
        }

        public bool GetData<T>(out T data) where T : BasicData
        {
            try
            {
                data = _data as T;
                return true;
            }
            catch (Exception)
            {

                data = null;
                return false;
            }
        }

        public T GetData<T>() where T : BasicData
        {
            return _data as T;
        }

        public void Save(string name = DEFAULT_NAME)
        {
            OnSaveRequested?.Invoke();

            if (_data != null)
            {
                _data.Modify(DateTime.Now);
            }

            if (!PlayerPrefs.HasKey(name))
                _data.Create(name, DateTime.Now);

            PlayerPrefs.SetString(name, JsonUtility.ToJson(_data));
        }

        public async void SaveAsync(string name = DEFAULT_NAME, Action onComplete = null)
        {
            await Task.Run(() =>
            {
                Save(name);
            });

            onComplete?.Invoke();
        }

        public void Load(string name = DEFAULT_NAME)
        {
            if (PlayerPrefs.HasKey(name))
            {
                OnLoadRequested?.Invoke();
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(name), _data);
            }
        }

        public async void LoadAsync(string name = DEFAULT_NAME, Action onComplete = null)
        {
            await Task.Run(() =>
            {
                Load(name);
            });

            onComplete?.Invoke();
        }

        public void Reset<T>() where T : BasicData, new()
        {
            _data = new T();
        }
    }
}
