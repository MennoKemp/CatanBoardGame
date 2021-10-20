using Auxilia.Extensions;
using Auxilia.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace CatanBoardGame
{
	public class ResourceCollection : IEnumerable<ResourceType>, INotifyCollectionChanged
	{
		private readonly Dictionary<ResourceType, int> _resources = EnumUtils.GetValues<ResourceType>().Except(ResourceType.Generic).ToDictionary(r => r, _ => 0);

		public ResourceCollection()
		{
		}
		public ResourceCollection(IEnumerable<ResourceType> resources)
		{
			resources.Execute(r => this[r]++);	
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public int Count
		{
			get => _resources.Values.Sum();
		}

		public int this[ResourceType resource]
		{
			get => _resources[resource];
			set
			{
				_resources[resource] = value;
				CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		public void Add(ResourceType resource)
		{
			_resources[resource]++;
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
		public void Add(ResourceCollection resources)
		{
			resources.Execute(r => _resources[r]++);
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public void Remove(ResourceType resource)
		{
			_resources[resource]--;
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
		public void Remove(ResourceCollection resources)
		{
			resources.Execute(r => _resources[r]--);
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		public IEnumerator<ResourceType> GetEnumerator()
		{
			return _resources.SelectMany(r => Enumerable.Repeat(r.Key, r.Value)).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
