using Common;
using System.Collections.Concurrent;
using System.Collections.Generic;
namespace Server
{
	internal class CacheData
	{
		internal DatabaseData Database { get; private set; }
		internal Player MainPlayer { get; set; }
		/// <summary>
		/// 用ID查询
		/// </summary>
		internal Dictionary<long, Player> PlayerData { get; private set; }
		/// <summary>
		/// 用Account查询
		/// </summary>
		internal Dictionary<long, Player> PlayerInfo { get; private set; }
		internal Version VersionData { get; private set; }
		internal DoubleKeyMap<long, long, Container> ContainerData { get; private set; }
		internal ConcurrentDoubleKeyMap<SceneType, long, ServerRoom> Rooms { get; private set; }

		internal CacheData(DatabaseData database)
		{
			Database = database;
			PlayerData = new();
			PlayerInfo = new();
			VersionData = new();
			ContainerData = new();
		}

		internal void Dispose()
		{
			ContainerData.Clear();
			VersionData = null;
			PlayerData.Clear();
			PlayerInfo.Clear();
			MainPlayer = null;
		}

		internal void Init()
		{
			(int Value, long CreateTime, long LastSaveTime, long NextUID, string n, int l, PlayerModelType t) = Database.GetVersion();
			VersionData.VersionValue = Value;
			VersionData.CreateTime = CreateTime;
			VersionData.LastSaveTime = LastSaveTime;
			VersionData.NextUID = NextUID;
			VersionData.Name = n;
			VersionData.Level = l;
			VersionData.ModelType = t;
			(bool Exist, (long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount)) = Database.GetMainAccount();
			if (Exist)
			{
				(_, (_, PlayerModelType ModelType, double Life, double Hunger, double Thirst, long Exp, int Level, SceneType Scene)) = Database.GetPlayerInfo(UID);
				var player = new Player
				{
					MainAccount = MainAccount,
					UID = UID,
					Account = Account,
					Name = Name,
					Password = Password,
					Gender = Gender,
					Life = Life,
					ModelType = ModelType,
					Hunger = Hunger,
					Thirst = Thirst,
					Exp = Exp,
					Level = Level,
					Scene = Scene,
				};
				VersionData.Name = player.Name;
				VersionData.Level = player.Level;
				VersionData.ModelType = player.ModelType;
				MainPlayer = player;
				PlayerData.Add(player.UID, player);
				PlayerInfo.Add(player.Account, player);
			}
		}

		internal bool HasPlayer(long account)
		{
			if (PlayerInfo.TryGetValue(account, out _))
			{
				return true;
			}
			(bool Exist, (long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount)) = Database.GetPlayerAccount(account);
			if(Exist)
			{
				(_, (_, PlayerModelType ModelType, double Life, double Hunger, double Thirst, long Exp, int Level, SceneType Scene)) = Database.GetPlayerInfo(UID);
				var player = new Player
				{
					MainAccount = MainAccount,
					UID = UID,
					Account = Account,
					Name = Name,
					Password = Password,
					Gender = Gender,
					Life = Life,
					ModelType = ModelType,
					Hunger = Hunger,
					Thirst = Thirst,
					Exp = Exp,
					Level = Level,
					Scene = Scene,
				};
				PlayerData.Add(player.UID, player);
				PlayerInfo.Add(player.Account, player);
				return true;
			}
			return false;
		}

		internal Player CreateNewPlayer(string name, GenderType gender, PlayerModelType modelType, long account = 1000000000, string password = "pass")
		{
			if(PlayerInfo.TryGetValue(account, out var player))
			{
				return player;
			}
			player = new Player()
			{
				MainAccount = account == 1000000000,
				UID = ++VersionData.NextUID,
				Account = account,
				Name = name,
				Password = password,
				Gender = gender,
				Life = 100,
				ModelType = modelType,
				Hunger = 100,
				Thirst = 100,
				Exp = 0,
				Level = 0,
				Scene = SceneType.Home,
			};
			if (Database.AddNewPlayer(player))
			{
				PlayerInfo.Add(account, player);
				PlayerData.Add(player.UID, player);
				return player;
			}
			return null;
		}
	}
}
