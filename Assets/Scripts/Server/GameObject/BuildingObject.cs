using Common;
using UnityEngine;
using GameFramework;

namespace Server
{
	public class BuildingObject : AGameObject
	{
		public BuildingType Type { get; set; }
		public override long Id { get; set; }
		public long Owner { get; set; }
		public override Vector3 Position { get; set; }
		public override double Life { get; set; }
		public override Quaternion Rotation { get; set; }

		public static BuildingObject Create(BuildingType type, Vector3 position, long Owner)
		{
			var building = ReferencePool.Acquire<BuildingObject>();
			building.Type = type;
			building.Owner = Owner;
			building.Position = position;
			return building;
		}

		public override void Clear()
		{
			base.Clear();
			Type = BuildingType.None;
			Owner = 0;
		}

		public override void BeAttacked(WeaponType weaponType, int power)
		{
			switch (Type)
			{
				case BuildingType.Pine:
					var info = BuildingConfig.Get(Type);
					if (info != null && info.CanBeAttackedWeapon.Contains(weaponType))
					{
						Life -= power;
					}
					break;
				
			}
		}
	}
}
