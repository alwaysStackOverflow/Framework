using Common;
using UnityEngine;

namespace Server
{
	public class ZombieObject : AGameObject
	{
		public override long Id { get; set; }
		public override Vector3 Position { get; set; }
		public override double Life { get; set; }
		public ZombieType Type { get; set; }
		public float AttackPower { get; set; }
		public float AttackDuration { get; set; }
		public float MoveSpeed { get; set; }
		public override Quaternion Rotation { get; set; }

		public override void BeAttacked(WeaponType weaponType, int power)
		{
			
		}
	}
}
