using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public enum BuildingType
	{
		None = 0,

		/// <summary>
		/// 松树
		/// </summary>
		Pine,
		
		/// <summary>
		/// 石头
		/// </summary>
		Stone,

		/// <summary>
		/// 小草
		/// </summary>
		Grass,

		/// <summary>
		/// 灌木
		/// </summary>
		Shrub,

		/// <summary>
		/// 铁矿石
		/// </summary>
		IronOre,

		/// <summary>
		/// 煤矿石
		/// </summary>
		CoalOre,

		/// <summary>
		/// 小箱子
		/// </summary>
		SmallBox,

		/// <summary>
		/// 中箱子
		/// </summary>
		MediumBox,

		/// <summary>
		/// 大箱子
		/// </summary>
		BigBox,

		/// <summary>
		/// 货架
		/// </summary>
		GoodsShelf,
	}
}
