using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public class ObjectPool<T>
	{
		private List<ObjectPoolContainer<T>> list;
		private Dictionary<T, ObjectPoolContainer<T>> lookup;
		private Func<T> factoryFunc;
		private int lastIndex = 0;

		public ObjectPool(Func<T> factoryFunc, int initialSize, ObjectPoolContainerType type)
		{
			this.factoryFunc = factoryFunc;

			list = new List<ObjectPoolContainer<T>>(initialSize);
			lookup = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);

			Warm(initialSize, type);
		}

		private void Warm(int capacity, ObjectPoolContainerType type)
		{
			for (int i = 0; i < capacity; i++)
			{
				CreateContainer(type);
			}
		}

		private ObjectPoolContainer<T> CreateContainer(ObjectPoolContainerType type)
		{
			var container = new ObjectPoolContainer<T>
			{
				Item = factoryFunc(),
				Type = type
			};
			list.Add(container);
			return container;
		}

		public T GetItem(ObjectPoolContainerType type)
		{
			ObjectPoolContainer<T> container = null;
			for (int i = 0; i < list.Count; i++)
			{
				lastIndex++;
				if (lastIndex > list.Count - 1) lastIndex = 0;
				
				if (list[lastIndex].Used)
				{
					continue;
				}
				else
				{
					container = list[lastIndex];
					break;
				}
			}

			if (container == null)
			{
				container = CreateContainer(type);
			}

			container.Consume();
			lookup.Add(container.Item, container);
			return container.Item;
		}
		
		public void ReleaseAllItem()
		{
			foreach (var item in list)
			{
				ReleaseItem(item.Item);
			}
			list.Clear();
		}

		public ObjectPoolContainer<T> GetPoolItem(int index)
		{
			return list[index];
		}

		public void ReleaseItem(object item)
		{
			ReleaseItem((T) item);
		}

		public void ReleaseItem(T item)
		{
			if (lookup.ContainsKey(item))
			{
				var container = lookup[item];
				container.Release();
				lookup.Remove(item);
			}
			else
			{
				Debug.LogWarning("This object pool does not contain the item provided: " + item);
			}
		}

		public List<ObjectPoolContainer<T>> DestroyItemsUnused()
		{
			List<ObjectPoolContainer<T>> result = new List<ObjectPoolContainer<T>>();
			for(int i = list.Count - 1 ; i >= 0; i--)
			{
				if (list[i].Used == false)
				{
					result.Add(result[i]);
				}

				list.Remove(result[i]);
			}

			return result;
		}

		public bool IsUsed(ObjectPoolContainer<T> item)
		{
			return item.Used;
		}

		public void Remove(ObjectPoolContainer<T> item)
		{
			list.Remove(item);
		}

		public int Count
		{
			get { return list.Count; }
		}

		public int CountUsedItems
		{
			get { return lookup.Count; }
		}
	}
}
