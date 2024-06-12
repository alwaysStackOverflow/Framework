using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Timer
{
	public interface ITimerManager
	{
		public void Add(Timer timer);
		public void Remove(Timer timer);

		public void Remove(string id);
	}
}
