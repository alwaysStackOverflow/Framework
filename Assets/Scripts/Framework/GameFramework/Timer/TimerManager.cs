using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log = GameFramework.GameFrameworkLog;

namespace GameFramework.Timer
{
	internal sealed class TimerManager : GameFrameworkModule, ITimerManager
	{
		private class TimerData : IReference
		{
			public string ID {  get; set; }
			public Timer Timer { get; set; }

			public double RemainTime { get; set; }

			public int RemainExcuteCount { get; set; }

			public bool IsRemoved { get; set; }

			public void Clear()
			{
				ID = null;
				if(Timer is not null)
				{
					ReferencePool.Release(Timer);
				}
				Timer = null;
				RemainExcuteCount = 0;
				RemainTime = 0;
				IsRemoved = true;
			}

			public static TimerData Create(Timer timer)
			{
				var data = ReferencePool.Acquire<TimerData>();
				data.Timer = timer;
				data.ID = timer.ID;
				data.RemainTime = timer.Interval;
				data.RemainExcuteCount = timer.LoopCount;
				data.IsRemoved = false;
				return data;
			}
		}

		private readonly ConcurrentDictionary<string, TimerData> _timers = new();
		private readonly ConcurrentQueue<TimerData> _executeQueue = new();
		private readonly ConcurrentQueue<TimerData> _cacheQueue = new();

		internal override int Priority => TimerManagerPriority;

		internal override void Shutdown()
		{
			_timers.Clear();
			_executeQueue.Clear();
			_cacheQueue.Clear();
		}

		internal override void Update(float elapseSeconds, float realElapseSeconds)
		{
			while (_executeQueue.Count > 0)
			{
				if (!_executeQueue.TryDequeue(out TimerData data))
				{
					continue;
				}
				if(data.IsRemoved)
				{
					ReferencePool.Release(data);
					continue;
				}
				data.RemainTime -= elapseSeconds;
				if(data.RemainTime <= 0)
				{
					data.Timer.Func.Invoke();
					data.RemainExcuteCount = data.RemainExcuteCount == -1 ? -1 : data.RemainExcuteCount - 1;
					if (data.RemainExcuteCount == 0)
					{
						_timers.TryRemove(data.ID, out var _);
						ReferencePool.Release(data);
						continue;
					}
					data.RemainTime += data.Timer.Interval;
				}
				_cacheQueue.Enqueue(data);
			}

			while (_cacheQueue.Count > 0)
			{
				if(_cacheQueue.TryDequeue(out TimerData data))
				{
					_executeQueue.Enqueue(data);
				}
			}
			_cacheQueue.Clear();
		}

		public void Add(Timer timer)
		{
			if (_timers.ContainsKey(timer.ID))
			{
				Log.Error($"Add Timer Failed, ID:{timer.ID} already exist");
				return;
			}
			if (timer.LoopCount == 0)
			{
				Log.Error($"Timer LoopCount == 0");
				return;
			}
			var data = TimerData.Create(timer);
			if (data.Timer.IsExecuteImmediately)
			{
				data.Timer.Func.Invoke();
				data.RemainExcuteCount = data.RemainExcuteCount == -1 ? -1 : data.RemainExcuteCount - 1;
			}
			
			if (data.RemainExcuteCount == 0)
			{
				ReferencePool.Release(data);
				return;
			}
			_timers.TryAdd(data.ID, data);
			_executeQueue.Enqueue(data);
		}

		public void Remove(Timer timer)
		{
			if(timer is null)
			{
				return;
			}
			if(_timers.TryGetValue(timer.ID, out TimerData data))
			{
				data.IsRemoved = true;
				_timers.TryRemove(data.ID, out var _);
			}
		}

		public void Remove(string id)
		{
			if (_timers.TryGetValue(id, out TimerData data))
			{
				data.IsRemoved = true;
				_timers.TryRemove(data.ID, out var _);
			}
		}
	}
}
