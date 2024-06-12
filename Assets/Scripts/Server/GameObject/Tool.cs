namespace Server
{
	internal class Tool : Item
	{
		/// <summary>
		/// 耐久值，乘以10000保存，客户端要除以10000再使用
		/// </summary>
		public long Durability { get; set; }
	}
}
