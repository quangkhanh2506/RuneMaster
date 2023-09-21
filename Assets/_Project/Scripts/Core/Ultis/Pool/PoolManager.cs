using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Core
{
    public class PoolManager : SingletonMono<PoolManager>
    {
        public bool LogStatus;

        private bool _hasInitialized = false;
        private GameObject _poolRoot;
        private bool _dirty = false;

        private Dictionary<GameObject, ObjectPool<GameObject>> _prefabLookup;
        private Dictionary<GameObject, ObjectPool<GameObject>> _instanceLookup;

        void Awake()
        {
            _prefabLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();
            _instanceLookup = new Dictionary<GameObject, ObjectPool<GameObject>>();

            Initialize();
        }

        void Update()
        {
            if (LogStatus && _dirty)
            {
                PrintStatus();
                _dirty = false;
            }
        }

        private void Initialize()
        {
            if (_hasInitialized) return;

            _poolRoot = new GameObject("[PoolRoot]");
            _poolRoot.AddComponent<PoolRoot>();

            _hasInitialized = true;
        }

        public void WarmPool(GameObject prefab, int size, ObjectPoolContainerType type)
        {
            if (_prefabLookup.ContainsKey(prefab))
            {
                throw new Exception("Pool for prefab " + prefab.name + " has already been created");
            }

            var pool = new ObjectPool<GameObject>(() => InstantiatePrefab(prefab), size, type);
            _prefabLookup[prefab] = pool;

            _dirty = true;
        }

        public GameObject SpawnObject(GameObject prefab, ObjectPoolContainerType type)
        {
            return SpawnObject(prefab, type, Vector3.zero, Quaternion.identity);
        }

        public GameObject SpawnObject(GameObject prefab, Vector3 position,
            Quaternion rotation, Transform parent = null, bool worldPositionStays = false)
        {
            return SpawnObject(prefab, ObjectPoolContainerType.General, position, rotation, parent, worldPositionStays);
        }

        public GameObject SpawnObject(GameObject prefab, ObjectPoolContainerType type, Vector3 position,
            Quaternion rotation, Transform parent = null, bool worldPositionStays = false)
        {
            if (!_prefabLookup.ContainsKey(prefab))
            {
                WarmPool(prefab, 1, type);
            }

            var pool = _prefabLookup[prefab];

            var clone = pool.GetItem(type);
            if (parent)
            {
                clone.transform.SetParent(parent, worldPositionStays);

                var rectTrans = clone.GetComponent<RectTransform>();
                if (rectTrans)
                {
                    clone.GetComponent<RectTransform>().anchoredPosition = position;
                }
                else
                {
                    clone.transform.localPosition = position;
                    clone.transform.localRotation = rotation;
                }
            }
            else
            {
                clone.transform.SetParent(_poolRoot.transform, worldPositionStays);
                clone.transform.SetPositionAndRotation(position, rotation);
            }

            clone.SetActive(true);

            _instanceLookup.Add(clone, pool);
            _dirty = true;
            return clone;
        }

        public void ReleaseObject(GameObject clone)
        {
            clone.SetActive(false);

            if (_instanceLookup.ContainsKey(clone))
            {
                _instanceLookup[clone].ReleaseItem(clone);
                _instanceLookup.Remove(clone);
                _dirty = true;
            }
            else
            {
                Debug.LogWarning("No pool contains the object: " + clone.name);
            }
        }

        public void ReleasePoolType(ObjectPoolContainerType type)
        {
            foreach (var lookup in _prefabLookup)
            {
                ReleasePoolType(lookup.Value, type);
            }
        }

        private void ReleasePoolType(ObjectPool<GameObject> pool, ObjectPoolContainerType type)
        {
            for (int i = pool.Count - 1; i >= 0; i--)
            {
                var item = pool.GetPoolItem(i);
                if (item.Type == type)
                {
                    ReleaseObject(item.Item);
                }
            }
        }


        public void DestroyPool()
        {
            foreach (var lookup in _prefabLookup)
            {
                DestroyPool(_prefabLookup[lookup.Key]);
            }

            _prefabLookup.Clear();
        }

        public void DestroyPool(GameObject prefab)
        {
            if (_prefabLookup.ContainsKey(prefab))
            {
                var pool = _prefabLookup[prefab];
                DestroyPool(pool);

                _prefabLookup.Remove(prefab);
            }
        }

        private void DestroyPool(ObjectPool<GameObject> pool)
        {
            for (int i = pool.Count - 1; i >= 0; i--)
            {
                var item = pool.GetPoolItem(i);
                if (item.Item == null)
                {
                    Debug.LogWarning($"GameObject was destroyed {pool}");
                }

                ReleaseObject(item.Item);
                Destroy(item.Item);
            }

            pool.ReleaseAllItem();
        }

        public void DestroyPoolUnused()
        {
            foreach (var lookup in _prefabLookup)
            {
                DestroyPoolUnused(lookup.Value);
            }
        }

        public void DestroyPoolUnused(GameObject prefab)
        {
            if (_prefabLookup.ContainsKey(prefab))
            {
                DestroyPoolUnused(_prefabLookup[prefab]);
            }
        }

        private void DestroyPoolUnused(ObjectPool<GameObject> pool)
        {
            for (int i = pool.Count - 1; i >= 0; i--)
            {
                var item = pool.GetPoolItem(i);
                if (item.Used == false)
                {
                    Destroy(item.Item);
                    pool.Remove(item);
                }
            }
        }

        public void DestroyPoolType(ObjectPoolContainerType type)
        {
            foreach (var lookup in _prefabLookup)
            {
                DestroyPoolType(lookup.Value, type);
            }
        }

        public void DestroyPoolType(GameObject prefab, ObjectPoolContainerType type)
        {
            if (_prefabLookup.ContainsKey(prefab))
            {
                DestroyPoolType(_prefabLookup[prefab], type);
            }
        }

        private void DestroyPoolType(ObjectPool<GameObject> pool, ObjectPoolContainerType type)
        {
            for (int i = pool.Count - 1; i >= 0; i--)
            {
                var item = pool.GetPoolItem(i);
                if (item.Type == type)
                {
                    Destroy(item.Item);
                    pool.Remove(item);
                }
            }
        }

        public void OnPoolRootDestroyed()
        {
            if (_hasInitialized)
            {
                foreach (var instance in _instanceLookup)
                {
                    Destroy(instance.Key);
                }

                _instanceLookup.Clear();

                _poolRoot = null;

                _hasInitialized = false;
            }
        }


        private GameObject InstantiatePrefab(GameObject prefab)
        {
            var go = Instantiate(prefab, _poolRoot.transform, true);
            return go;
        }

        public void PrintStatus()
        {
            foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in _prefabLookup)
            {
                Debug.Log(string.Format("Object Pool for Prefab: {0} In Use: {1} Total {2}", keyVal.Key.name,
                    keyVal.Value.CountUsedItems, keyVal.Value.Count));
            }
        }
    }
}