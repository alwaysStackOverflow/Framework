using Common;
using GameFramework;
using UnityEngine;

namespace Server
{
	public abstract class AGameObject : IReference
	{
		public virtual void Clear()
		{
			Position = Vector3.zero;
			Life = 0;
			Id = 0;
		}

		public abstract long Id { get; set; }
		public abstract Vector3 Position { get; set; }
		public abstract Quaternion Rotation { get; set; }
		public abstract double Life { get; set; }


		public abstract void BeAttacked(WeaponType weaponType, int power);

		public void Release()
		{
			ReferencePool.Release(this);
		}
	}
}
