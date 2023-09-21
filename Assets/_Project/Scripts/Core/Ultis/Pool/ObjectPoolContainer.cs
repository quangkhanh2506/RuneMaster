
namespace Core
{	
	public class ObjectPoolContainer<T>
	{
		private T _item;
		private ObjectPoolContainerType _type;

		public bool Used { get; private set; }

		public void Consume()
		{
			Used = true;
		}
		
		public T Item
		{
			get => _item;
			set => _item = value;
		}

		public ObjectPoolContainerType Type
		{
			get => _type;
			set => _type = value;
		}

		public void Release()
		{
			Used = false;
		}
	}
}
