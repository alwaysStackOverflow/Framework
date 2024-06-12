using System;
using System.Collections.Generic;

namespace Server
{
	public class Item : IComparable<Item>, IComparer<Item>
	{
		/// <summary>
		/// 物品id
		/// </summary>
		public int ID { get; set; }
		/// <summary>
		/// 物品最大堆叠数量
		/// </summary>
		public int MaxStack { get; set; }
		/// <summary>
		/// 物品堆叠数量
		/// </summary>
		public int Stack { get; set; }
		/// <summary>
		/// 物品体积
		/// </summary>
		public int Volume { get; set; }

		public int Compare(Item x, Item y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			if (x.ID == y.ID)
			{
				return x.Stack - y.Stack;
			}
			return x.ID - y.ID;
		}

		public int CompareTo(Item other)
		{
            if (other == null)
			{
				return 1;
			}
            return Compare(this, other);
		}
	}
}
