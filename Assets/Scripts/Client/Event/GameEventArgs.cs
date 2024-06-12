using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	public abstract class GameEventArgs : BaseEventArgs
	{
		public void Invoke()
		{
			ClientEntry.Event.Fire(this);
		}
	}
}
