using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework.Timer
{
	public class Timer : IReference
	{
		public void Clear()
		{
			ID = null;
			LoopCount = 0;
			Interval = 0;
			IsExecuteImmediately = false;
			Func = null;
		}

		public string ID { get; private set; }
		public int LoopCount { get; private set; }
		public double Interval { get; private set; }
		public bool IsExecuteImmediately { get; private set; }
		public GameFrameworkAction Func { get; private set; }

		/// <summary>
		/// 创建一个Timer
		/// </summary>
		/// <param name="id">Timer的id,用于标识</param>
		/// <param name="loopCount">循环次数，-1代表无限次</param>
		/// <param name="interval">执行的间隔时间</param>
		/// <param name="isExecuteImmediately">是否立即执行，ture代表Timer加进去就会执行一次，false代表加进去过了interval的时间才会执行一次</param>
		/// <param name="func">要执行的函数</param>
		/// <returns></returns>
		public static Timer Create(string id, int loopCount, double interval, bool isExecuteImmediately, GameFrameworkAction func)
		{
			var timer = ReferencePool.Acquire<Timer>();
			timer.ID = id;
			timer.LoopCount = loopCount < 0 ? -1 : loopCount;
			timer.Interval = interval;
			timer.IsExecuteImmediately = isExecuteImmediately;
			timer.Func = func;
			return timer;
		}
	}
}
