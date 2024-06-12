using Common;

namespace Client
{
	public class PlayerData : AModel
	{
		public override void ClearData()
		{
			UID = 0;
			Account = 0;
			Name = string.Empty;
			Password = string.Empty;
			Gender = GenderType.None;
			ModelType = PlayerModelType.None;
			Life = 0;
			Hunger = 0;
			Thirst = 0;
			Exp = 0;
			Level = 0;
			Scene = 0;
		}

		/// <summary>
		/// 用户唯一ID
		/// </summary>
		public long UID { get; set; }
		/// <summary>
		/// 用户账号
		/// </summary>
		public long Account { get; set; }
		/// <summary>
		/// 用户名字
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 用户密码
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// 性别
		/// </summary>
		public GenderType Gender { get; set; }

		/// <summary>
		/// 模型ID
		/// </summary>
		public PlayerModelType ModelType { get; set; }

		/// <summary>
		/// 生命值
		/// </summary>
		public double Life { get; set; }
		/// <summary>
		/// 饥饿值
		/// </summary>
		public double Hunger { get; set; }
		/// <summary>
		/// 口渴值
		/// </summary>
		public double Thirst { get; set; }
		/// <summary>
		/// 经验值
		/// </summary>
		public long Exp { get; set; }

		public long NextLevelExp { get; set; }
		/// <summary>
		/// 等级
		/// </summary>
		public int Level { get; set; }
		/// <summary>
		/// 所在场景
		/// </summary>
		public SceneType Scene { get; set; }
	}
}
