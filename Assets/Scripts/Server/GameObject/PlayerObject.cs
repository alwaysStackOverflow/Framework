using Common;
using UnityEngine;

namespace Server
{
	public class PlayerObject : AGameObject
	{
		public override long Id { get; set; }
		public override Vector3 Position { get; set; }
		public override double Life { get; set; }
		public string Name { get; set; }
		public GenderType Gender { get; set; }
		public PlayerModelType ModelType { get; set; }
		public double Hunger { get; set; }
		public double Thirst { get; set; }
		public long Exp { get; set; }
		public int Level { get; set; }
		public SceneType Scene { get; set; }
		public float MoveSpeed { get; set; }
		public float AtackPower { get; set; }
		public float AtackDuration { get; set; }
		public override Quaternion Rotation { get; set; }

		public override void BeAttacked(WeaponType weaponType, int power)
		{
			
		}
	}
}
