using GameFramework;
using System.Collections.Generic;

namespace Server
{
	internal class ServerRoom : IReference
	{
		public const byte MaxPlayer = 10;
		public long RoomID { get; set; }
		public long OwnerID { get; set; }
		public long KeeperId { get; set; }
		public Dictionary<long, BuildingObject> _buildings = new();
		public Dictionary<long, PlayerObject> _players = new();
		public Dictionary<long, ZombieObject> _zombies = new();
		public Dictionary<long, BuildingObject> Buildings => _buildings;
		public Dictionary<long, PlayerObject> Players => _players;
		public Dictionary<long, ZombieObject> Zombies => _zombies;
		public void Clear()
		{
			RoomID = 0;
			OwnerID = 0;
			KeeperId = 0;
			_buildings.Clear();
			_players.Clear();
			_zombies.Clear();
		}

		public static ServerRoom Create()
		{
			var view = ReferencePool.Acquire<ServerRoom>();
			return view;
		}


		public void AsNewRoom()
		{

		}
	}
}
