using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Singleton
{
	public interface ISingletonManager
	{
		public void AddSingleton<T>() where T : Singleton<T>, new();
	}
}
