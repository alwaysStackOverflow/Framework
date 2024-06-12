using System.Collections.Generic;

namespace Server
{
	internal class Container
	{
		/// <summary>
		/// 箱子编号
		/// </summary>
		public long ID { get; set; }

		/// <summary>
		/// 剩余容量
		/// </summary>
		public int RemainCapacity { get; set;}

		public List<Item> Items{ get; set; }


		public bool AddItem(Item item, int count)
		{
			var tocalVolume = item.Volume * count;
			if(RemainCapacity < tocalVolume)
			{
				return false;
			}
			RemainCapacity -= tocalVolume;
			var index = Items.BinarySearch(item);
			if(index < 0)
			{
				Items.Add(new Item()
				{
					ID = item.ID,
					MaxStack = item.MaxStack,
					Stack = count,
					Volume = item.Volume
				});
			}
			else
			{
				Items[index].Stack += count;
				if(Items[index].Stack > Items[index].MaxStack)
				{
					Items.Add(new Item()
					{
						ID = item.ID,
						MaxStack = item.MaxStack,
						Stack = Items[index].Stack - Items[index].MaxStack,
						Volume = item.Volume
					});
					Items[index].Stack = Items[index].MaxStack;
				}
			}
			Items.Sort();
			return true;
		}

		public void Sort()
		{
			Items.Sort();
		}
	}
}
