using Common;
using GameFramework;
using GameFramework.Network;
using UnityGameFramework;

namespace Server
{
	internal class NetworkHandler : IReference
	{
		public long Type { get; private set; }
		public long Id { get; private set; }
		public uint ConnectionId { get; private set; }
		public CacheData Cache { get; private set; }
		public Server Server { get; private set; }

		public void Clear()
		{
			RemoveHandler();
			Type = 0;
			Id = 0;
			ConnectionId = 0;
			Cache = null;
			Server = null;
		}

		private void Listen<T>(int protocolId, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			ServerEntry.NetworkEvent.Listen(Id, protocolId, handler);
		}

		private void Unlisten<T>(int protocolId, GameFrameworkAction<T> handler) where T : ProtoObject
		{
			ServerEntry.NetworkEvent.Unlisten(Id, protocolId, handler);
		}

		public void Send(ProtoObject data)
		{
			Server.Send(Type, ConnectionId, data);
		}


		public static NetworkHandler Create(long protocolType, uint connectionId, Server server, CacheData cache)
		{
			var handler = ReferencePool.Acquire<NetworkHandler>();
			handler.Type = protocolType;
			handler.Id = protocolType + connectionId;
			handler.ConnectionId = connectionId;
			handler.Cache = cache;
			handler.Server = server;
			handler.AddHandler();
			return handler;
		}


		private void AddHandler()
		{
			Listen<CheckAccountRequest>(Protocol.CheckAccountRequest, OnCheckAccountRequest);
			Listen<MainAccountRegisterRequest>(Protocol.MainAccountRegisterRequest, OnMainAccountRegisterRequest);
			Listen<NormalAccountRegisterRequest>(Protocol.NormalAccountRegisterRequest, OnNormalAccountRegisterRequest);
			Listen<MainAccountLoginRequest>(Protocol.MainAccountLoginRequest, OnMainAccountLoginRequest);
			Listen<NormalAccountLoginRequest>(Protocol.NormalAccountLoginRequest, OnNormalAccountLoginRequest);
			Listen<EnterRoomRequest>(Protocol.EnterRoomRequest, OnEnterRoomRequest);
		}

		private void RemoveHandler()
		{
			Unlisten<CheckAccountRequest>(Protocol.CheckAccountRequest, OnCheckAccountRequest);
			Unlisten<MainAccountRegisterRequest>(Protocol.MainAccountRegisterRequest, OnMainAccountRegisterRequest);
			Unlisten<NormalAccountRegisterRequest>(Protocol.NormalAccountRegisterRequest, OnNormalAccountRegisterRequest);
			Unlisten<MainAccountLoginRequest>(Protocol.MainAccountLoginRequest, OnMainAccountLoginRequest);
			Unlisten<NormalAccountLoginRequest>(Protocol.NormalAccountLoginRequest, OnNormalAccountLoginRequest);
			Unlisten<EnterRoomRequest>(Protocol.EnterRoomRequest, OnEnterRoomRequest);
		}

		private void OnCheckAccountRequest(CheckAccountRequest data)
		{
			var reply = ProtoObject.Create<CheckAccountReply>();
			if (data.Token != Server.MainCountConnenctionToken)
			{
				reply.Code = ProtocolCode.MainAccountTokenError;
				Send(reply);
				return;
			}
			if (Cache.MainPlayer != null)
			{
				reply.MainAccountExist = true;
			}
			reply.Token = Server.MainCountConnenctionToken;
			Send(reply);
		}

		private void OnMainAccountRegisterRequest(MainAccountRegisterRequest data)
		{
			var reply = ProtoObject.Create<AccountLoginReply>();
			if (data.Token != Server.MainCountConnenctionToken)
			{
				reply.Code = ProtocolCode.MainAccountTokenError;
				Send(reply);
				return;
			}
			Player p;
			if (Cache.MainPlayer != null)
			{
				p = Cache.MainPlayer;
			}
			else
			{
				p = Cache.CreateNewPlayer(data.Name, data.Gender, data.ModelType);
			}
			if (p != null)
			{
				reply.UID = p.UID;
				reply.Name = p.Name;
				reply.Account = p.Account;
				reply.Password = p.Password;
				reply.Gender = p.Gender;
				reply.Life = p.Life;
				reply.ModelType = p.ModelType;
				reply.Hunger = p.Hunger;
				reply.Thirst = p.Thirst;
				reply.Exp = p.Exp;
				reply.Level = p.Level;
				var config = LevelConfig.Get(p.Level);
				reply.NextLevelExp = config != null ? config.LevelMaxExp : long.MaxValue;
				reply.Scene = p.Scene;
			}
			else
			{
				reply.Code = ProtocolCode.AccountRegisterError;
			}
			Send(reply);
		}

		private void OnNormalAccountRegisterRequest(NormalAccountRegisterRequest data)
		{

		}

		private void OnMainAccountLoginRequest(MainAccountLoginRequest data)
		{
			var reply = ProtoObject.Create<AccountLoginReply>();
			if (data.Token != Server.MainCountConnenctionToken)
			{
				reply.Code = ProtocolCode.MainAccountTokenError;
				Send(reply);
				return;
			}
			Player p;
			if (Cache.MainPlayer != null)
			{
				p = Cache.MainPlayer;
				reply.UID = p.UID;
				reply.Name = p.Name;
				reply.Account = p.Account;
				reply.Password = p.Password;
				reply.Gender = p.Gender;
				reply.Life = p.Life;
				reply.ModelType = p.ModelType;
				reply.Hunger = p.Hunger;
				reply.Thirst = p.Thirst;
				reply.Exp = p.Exp;
				reply.Level = p.Level;
				var config = LevelConfig.Get(p.Level);
				reply.NextLevelExp = config != null ? config.LevelMaxExp : long.MaxValue;
				reply.Scene = p.Scene;
			}
			else
			{
				reply.Code = ProtocolCode.AccountNotExist;
			}
			Send(reply);
		}

		private void OnNormalAccountLoginRequest(NormalAccountLoginRequest data)
		{

		}

		private void OnEnterRoomRequest(EnterRoomRequest data)
		{
			var reply = ProtoObject.Create<EnterRoomReply>();
			ServerRoom room = null;
			if (Cache.Rooms.TryGetValue(data.RoomType, out var rooms))
			{
				foreach (var serverRoom in rooms.Values)
				{
					if (serverRoom.Players.Count < ServerRoom.MaxPlayer)
					{
						room = serverRoom;
					}
				}
			}
			else
			{
				room = ServerRoom.Create();
				room.AsNewRoom();
				if (data.RoomType == SceneType.Home)
				{
					room.OwnerID = data.UID;
				}
				room.KeeperId = data.UID;
			}
			reply.Players = new();
			foreach (var player in room.Players.Values)
			{
				var p = new PlayerInfo()
				{
					Id = player.Id,
					Position = player.Position,
					Rotation = player.Rotation,
					Life = player.Life,
					Name = player.Name,
					Gender = player.Gender,
					ModelType = player.ModelType,
					MoveSpeed = player.MoveSpeed,
					AtackPower = player.AtackPower,
					AtackDuration = player.AtackDuration,
				};
				reply.Players.Add(p);
			}
			reply.Zombies = new();
			foreach (var zombie in room.Zombies.Values)
			{
				var z = new ZombieObject()
				{

				};
			}
			reply.Buildings = new();
			foreach (var building in room.Buildings.Values)
			{

			}
			Send(reply);
		}
	}
}
