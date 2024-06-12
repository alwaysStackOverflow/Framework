using MemoryPack;
using GameFramework.Network;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace Common
{
	[MemoryPackable]
	public partial class CheckAccountRequest : ProtoObject
	{
		public override int ProtocolID => Protocol.CheckAccountRequest;

		[MemoryPackOrder(0)]
		public override int Code { get; set; }

		[MemoryPackOrder(1)]
		public string Token { get; set; }

		public override void Clear()
		{
			Code = 0;
			Token = null;
		}
	}

	[MemoryPackable]
	public partial class CheckAccountReply : ProtoObject
	{
		public override int ProtocolID => Protocol.CheckAccountReply;

		[MemoryPackOrder(0)]
		public override int Code { get; set; }

		[MemoryPackOrder(1)]
		public bool MainAccountExist { get; set; }

		[MemoryPackOrder(2)]
		public string Token { get; set; }

		public override void Clear()
		{
			Code = 0;
			MainAccountExist = false;
			Token = null;
		}
	}

	[MemoryPackable]
	public partial class MainAccountLoginRequest : ProtoObject
	{
		public override int ProtocolID => Protocol.MainAccountLoginRequest;

		[MemoryPackOrder(0)]
		public override int Code { get; set; }

		[MemoryPackOrder(1)]
		public string Token { get; set; }

		public override void Clear()
		{
			Code = 0;
			Token = null;
		}
	}



	[MemoryPackable]
	public partial class NormalAccountLoginRequest : ProtoObject
	{
		public override int ProtocolID => Protocol.NormalAccountLoginRequest;

		[MemoryPackOrder(0)]
		public override int Code { get; set; }

		[MemoryPackOrder(1)]
		public long Account { get; set; }

		[MemoryPackOrder(2)]
		public string Password { get; set; }

		public override void Clear()
		{
			Code = 0;
			Account = 0;
			Password = null;
		}
	}


	[MemoryPackable]
	public partial class MainAccountRegisterRequest : ProtoObject
	{
		public override int ProtocolID => Protocol.MainAccountRegisterRequest;

		[MemoryPackOrder(0)]
		public override int Code { get; set; }

		[MemoryPackOrder(1)]
		public string Token { get; set; }

		[MemoryPackOrder(2)]
		public string Name { get; set; }

		[MemoryPackOrder(3)]
		public GenderType Gender { get; set; }

		[MemoryPackOrder(4)]
		public PlayerModelType ModelType { get; set; }

		public override void Clear()
		{
			Code = 0;
			Token = null;
			Name = null;
			Gender = GenderType.None;
			ModelType = PlayerModelType.None;
		}
	}

	[MemoryPackable]
	public partial class NormalAccountRegisterRequest : ProtoObject
	{
		public override int ProtocolID => Protocol.NormalAccountRegisterRequest;

		[MemoryPackOrder(0)]
		public override int Code { get; set; }

		[MemoryPackOrder(1)]
		public string Token { get; set; }

		[MemoryPackOrder(2)]
		public string Name { get; set; }

		[MemoryPackOrder(3)]
		public GenderType Gender { get; set; }

		[MemoryPackOrder(4)]
		public PlayerModelType ModelType { get; set; }
		[MemoryPackOrder(5)]

		public long Account { get; set; }
		[MemoryPackOrder(6)]

		public string Password { get; set; }

		public override void Clear()
		{
			Code = 0;
			Token = null;
			Name = null;
			Gender = GenderType.None;
			ModelType = PlayerModelType.None;
			Account = 0;
			Password = null;
		}
	}

	[MemoryPackable]
	public partial class AccountLoginReply : ProtoObject
	{
		public override int ProtocolID => Protocol.AccountLoginReply;

		[MemoryPackOrder(0)]
		public override int Code { get; set; }

		/// <summary>
		/// 用户唯一ID
		/// </summary>
		[MemoryPackOrder(1)]
		public long UID { get; set; }
		/// <summary>
		/// 用户账号
		/// </summary>
		[MemoryPackOrder(2)]
		public long Account { get; set; }
		/// <summary>
		/// 用户名字
		/// </summary>
		[MemoryPackOrder(3)]
		public string Name { get; set; }
		/// <summary>
		/// 用户密码
		/// </summary>
		[MemoryPackOrder(4)]
		public string Password { get; set; }

		/// <summary>
		/// 性别
		/// </summary>
		[MemoryPackOrder(5)]
		public GenderType Gender { get; set; }

		/// <summary>
		/// 模型ID
		/// </summary>
		[MemoryPackOrder(6)]
		public PlayerModelType ModelType { get; set; }

		/// <summary>
		/// 生命值
		/// </summary>
		[MemoryPackOrder(7)]
		public double Life { get; set; }
		/// <summary>
		/// 饥饿值
		/// </summary>
		[MemoryPackOrder(8)]
		public double Hunger { get; set; }
		/// <summary>
		/// 口渴值
		/// </summary>
		[MemoryPackOrder(9)]
		public double Thirst { get; set; }
		/// <summary>
		/// 经验值
		/// </summary>
		[MemoryPackOrder(10)]
		public long Exp { get; set; }

		/// <summary>
		/// 经验值
		/// </summary>
		[MemoryPackOrder(11)]
		public long NextLevelExp { get; set; }
		/// <summary>
		/// 等级
		/// </summary>
		[MemoryPackOrder(12)]
		public int Level { get; set; }
		/// <summary>
		/// 所在场景
		/// </summary>
		[MemoryPackOrder(13)]
		public SceneType Scene { get; set; }

		public override void Clear()
		{
			Code = 0;
			Name = null;
			Password = null;
			UID = 0;
		}
	}

	[MemoryPackable]
	public partial class EnterRoomRequest : ProtoObject
	{
		public override int ProtocolID => Protocol.EnterRoomRequest;

		[MemoryPackOrder(0)]
		public override int Code { get; set; }

		[MemoryPackOrder(1)]
		public SceneType RoomType { get; set; }

		[MemoryPackOrder(2)]
		public long UID { get; set; }

		public override void Clear()
		{
			Code = 0;
			RoomType = SceneType.None;
			UID = 0;
		}
	}

	[MemoryPackable]
	public partial class Zombie : IReference
	{
		[MemoryPackOrder(0)]
		public long Id { get; set; }
		[MemoryPackOrder(1)]
		public Vector3 Position { get; set; }
		[MemoryPackOrder(2)]
		public Quaternion Rotation { get; set; }
		[MemoryPackOrder(3)]
		public double Life { get; set; }
		[MemoryPackOrder(4)]
		public ZombieType Type { get; set; }
		[MemoryPackOrder(5)]
		public float AttackPower { get; set; }
		[MemoryPackOrder(6)]
		public float AttackDuration { get; set; }
		[MemoryPackOrder(7)]
		public float MoveSpeed { get; set; }

		public void Clear()
		{
			Id = 0;
			Position = Vector3.zero;
			Rotation = Quaternion.identity;
			Life = 0;
			Type = ZombieType.Normal;
			AttackPower = 0;
			AttackDuration = 0;
			MoveSpeed = 0;
		}
	}

	[MemoryPackable]
	public partial class PlayerInfo : IReference
	{
		[MemoryPackOrder(0)]
		public long Id { get; set; }
		[MemoryPackOrder(1)]
		public Vector3 Position { get; set; }
		[MemoryPackOrder(2)]
		public Quaternion Rotation { get; set; }
		[MemoryPackOrder(3)]
		public double Life { get; set; }
		[MemoryPackOrder(5)]
		public string Name { get; set; }
		[MemoryPackOrder(7)]
		public GenderType Gender { get; set; }
		[MemoryPackOrder(9)]
		public PlayerModelType ModelType { get; set; }
		[MemoryPackOrder(15)]
		public float MoveSpeed { get; set; }
		[MemoryPackOrder(16)]
		public float AtackPower { get; set; }
		[MemoryPackOrder(17)]
		public float AtackDuration { get; set; }

		public void Clear()
		{
			Id = 0;
		}
	}

	[MemoryPackable]
	public partial class BuildingInfo
	{
		[MemoryPackOrder(0)]
		public long ID { get; set; }
		[MemoryPackOrder(1)]
		public BuildingType BuildingType { get; set; }
		[MemoryPackOrder(2)]
		public int Life { get; set; }

		[MemoryPackOrder(3)]
		public long Owner { get; set; }

		[MemoryPackOrder(4)]
		public Vector3 Position { get; set; }
		[MemoryPackOrder(5)]
		public Quaternion Rotation { get; set; }
		[MemoryPackOrder(6)]
		public List<WeaponType> CanBeAttackedWeaponList { get; set; }
	}

	[MemoryPackable]
	public partial class EnterRoomReply : ProtoObject
	{
		public override int ProtocolID => Protocol.EnterRoomReply;
		[MemoryPackOrder(0)]
		public override int Code { get; set; }
		[MemoryPackOrder(1)]
		public long RoomID { get; set; }
		[MemoryPackOrder(2)]
		public SceneType RoomType { get; set; }
		[MemoryPackOrder(3)]
		public long OwnerID { get; set; }
		[MemoryPackOrder(4)]
		public long RoomKeeper { get; set; }
		[MemoryPackOrder(5)]
		public List<BuildingInfo> Buildings { get; set; }
		[MemoryPackOrder(6)]
		public List<PlayerInfo> Players { get; set; }
		[MemoryPackOrder(7)]
		public List<Zombie> Zombies { get; set; }

		public override void Clear()
		{
			Code = 0;
			Buildings = null;
			Players = null;
			Zombies = null;
		}
	}
}