using System.Collections.Generic;

namespace GameFramework
{
	public abstract class AActionArgsData : IReference
	{
		public abstract void Clear();
		public abstract void Invoke();
		public abstract void Invoke(params object[] args);
	}

	public class ActionArgsData : AActionArgsData
	{
		public GameFrameworkAction _method;

		public override void Invoke()
		{
			_method?.Invoke();
		}

		public override void Invoke(params object[] args)
		{
			_method?.Invoke();
		}

		public static ActionArgsData Create(GameFrameworkAction method)
		{
			var data = ReferencePool.Acquire<ActionArgsData>();
			data._method = method;
			return data;
		}

		public override void Clear()
		{
			_method = null;
		}
	}

	public class ActionArgsData<T1> : AActionArgsData
	{
		public GameFrameworkAction<T1> _method;
		private T1 _arg1;

		public override void Invoke()
		{
			_method?.Invoke(_arg1);
		}

		private readonly List<object> _args = new(1) { null };
		public override void Invoke(params object[] args)
		{
			ClearList();
			InitList(args);
			_arg1 = _args[0] != null ? (T1)_args[0] : default;
			Invoke();
		}

		private void InitList(object[] args)
		{
			for (int i = 0; i < args.Length; ++i)
			{
				_args[i] = args[i];
			}
		}

		private void ClearList()
		{
			for (int i = 0; i < _args.Count; ++i)
			{
				_args[i] = null;
			}
		}

		public static ActionArgsData<T1> Create(GameFrameworkAction<T1> method, T1 arg1 = default)
		{
			var data = ReferencePool.Acquire<ActionArgsData<T1>>();
			data._method = method;
			data._arg1 = arg1;
			return data;
		}

		public override void Clear()
		{
			_method = null;
			_arg1 = default;
			ClearList();
		}
	}

	public class ActionArgsData<T1, T2> : AActionArgsData
	{
		public GameFrameworkAction<T1, T2> _method;
		private T1 _arg1;
		private T2 _arg2;

		public static ActionArgsData<T1, T2> Create(GameFrameworkAction<T1, T2> method, T1 arg1 = default, T2 arg2 = default)
		{
			var data = ReferencePool.Acquire<ActionArgsData<T1, T2>>();
			data._method = method;
			data._arg1 = arg1;
			data._arg2 = arg2;
			return data;
		}

		public override void Invoke()
		{
			_method?.Invoke(_arg1, _arg2);
		}

		private readonly List<object> _args = new(2) { null, null };
		public override void Invoke(params object[] args)
		{
			ClearList();
			InitList(args);
			_arg1 = _args[0] != null ? (T1)_args[0] : default;
			_arg2 = _args[1] != null ? (T2)_args[1] : default;
			Invoke();
		}

		private void InitList(object[] args)
		{
			for (int i = 0; i < args.Length; ++i)
			{
				_args[i] = args[i];
			}
		}

		private void ClearList()
		{
			for (int i = 0; i < _args.Count; ++i)
			{
				_args[i] = null;
			}
		}

		public override void Clear()
		{
			_method = null;
			_arg1 = default;
			_arg2 = default;
			ClearList();
		}
	}

	public class ActionArgsData<T1, T2, T3> : AActionArgsData
	{
		public GameFrameworkAction<T1, T2, T3> _method;
		private T1 _arg1;
		private T2 _arg2;
		private T3 _arg3;

		public override void Invoke()
		{
			_method?.Invoke(_arg1, _arg2, _arg3);
		}
		public static ActionArgsData<T1, T2, T3> Create(GameFrameworkAction<T1, T2, T3> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default)
		{
			var data = ReferencePool.Acquire<ActionArgsData<T1, T2, T3>>();
			data._method = method;
			data._arg1 = arg1;
			data._arg2 = arg2;
			data._arg3 = arg3;
			return data;
		}

		private readonly List<object> _args = new(3) { null, null, null };
		public override void Invoke(params object[] args)
		{
			ClearList();
			InitList(args);
			_arg1 = _args[0] != null ? (T1)_args[0] : default;
			_arg2 = _args[1] != null ? (T2)_args[1] : default;
			_arg3 = _args[2] != null ? (T3)_args[2] : default;
			Invoke();
		}

		private void InitList(object[] args)
		{
			for (int i = 0; i < args.Length; ++i)
			{
				_args[i] = args[i];
			}
		}

		private void ClearList()
		{
			for (int i = 0; i < _args.Count; ++i)
			{
				_args[i] = null;
			}
		}

		public override void Clear()
		{
			_method = null;
			_arg1 = default;
			_arg2 = default;
			_arg3 = default;
			ClearList();
		}
	}

	public class ActionArgsData<T1, T2, T3, T4> : AActionArgsData
	{
		public GameFrameworkAction<T1, T2, T3, T4> _method;
		private T1 _arg1;
		private T2 _arg2;
		private T3 _arg3;
		private T4 _arg4;

		public override void Invoke()
		{
			_method?.Invoke(_arg1, _arg2, _arg3, _arg4);
		}

		public static ActionArgsData<T1, T2, T3, T4> Create(GameFrameworkAction<T1, T2, T3, T4> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default)
		{
			var data = ReferencePool.Acquire<ActionArgsData<T1, T2, T3, T4>>();
			data._method = method;
			data._arg1 = arg1;
			data._arg2 = arg2;
			data._arg3 = arg3;
			data._arg4 = arg4;
			return data;
		}

		private readonly List<object> _args = new(4) { null, null, null, null};

		public override void Invoke(params object[] args)
		{
			ClearList();
			InitList(args);
			_arg1 = _args[0] != null ? (T1)_args[0] : default;
			_arg2 = _args[1] != null ? (T2)_args[1] : default;
			_arg3 = _args[2] != null ? (T3)_args[2] : default;
			_arg4 = _args[3] != null ? (T4)_args[3] : default;
			Invoke();
		}

		private void InitList(object[] args)
		{
			for (int i = 0; i < args.Length; ++i)
			{
				_args[i] = args[i];
			}
		}

		private void ClearList()
		{
			for (int i = 0; i < _args.Count; ++i)
			{
				_args[i] = null;
			}
		}

		public override void Clear()
		{
			_method = null;
			_arg1 = default;
			_arg2 = default;
			_arg3 = default;
			_arg4 = default;
			ClearList();
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5> : AActionArgsData
	{
		public GameFrameworkAction<T1, T2, T3, T4, T5> _method;
		private T1 _arg1;
		private T2 _arg2;
		private T3 _arg3;
		private T4 _arg4;
		private T5 _arg5;

		public override void Invoke()
		{
			_method?.Invoke(_arg1, _arg2, _arg3, _arg4, _arg5);
		}

		public static ActionArgsData<T1, T2, T3, T4, T5> Create(GameFrameworkAction<T1, T2, T3, T4, T5> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default)
		{
			var data = ReferencePool.Acquire<ActionArgsData<T1, T2, T3, T4, T5>>();
			data._method = method;
			data._arg1 = arg1;
			data._arg2 = arg2;
			data._arg3 = arg3;
			data._arg4 = arg4;
			data._arg5 = arg5;
			return data;
		}

		private readonly List<object> _args = new(5) { null, null, null, null, null };

		public override void Invoke(params object[] args)
		{
			ClearList();
			InitList(args);
			_arg1 = _args[0] != null ? (T1)_args[0] : default;
			_arg2 = _args[1] != null ? (T2)_args[1] : default;
			_arg3 = _args[2] != null ? (T3)_args[2] : default;
			_arg4 = _args[3] != null ? (T4)_args[3] : default;
			_arg5 = _args[4] != null ? (T5)_args[4] : default;
			Invoke();
		}

		private void InitList(object[] args)
		{
			for (int i = 0; i < args.Length; ++i)
			{
				_args[i] = args[i];
			}
		}

		private void ClearList()
		{
			for (int i = 0; i < _args.Count; ++i)
			{
				_args[i] = null;
			}
		}

		public override void Clear()
		{
			_method = null;
			_arg1 = default;
			_arg2 = default;
			_arg3 = default;
			_arg4 = default;
			_arg5 = default;
			ClearList();
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6> : AActionArgsData
	{
		public GameFrameworkAction<T1, T2, T3, T4, T5, T6> _method;
		private T1 _arg1;
		private T2 _arg2;
		private T3 _arg3;
		private T4 _arg4;
		private T5 _arg5;
		private T6 _arg6;

		public override void Invoke()
		{
			_method?.Invoke(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6);
		}

		public static ActionArgsData<T1, T2, T3, T4, T5, T6> Create(GameFrameworkAction<T1, T2, T3, T4, T5, T6> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default)
		{
			var data = ReferencePool.Acquire<ActionArgsData<T1, T2, T3, T4, T5, T6>>();
			data._method = method;
			data._arg1 = arg1;
			data._arg2 = arg2;
			data._arg3 = arg3;
			data._arg4 = arg4;
			data._arg5 = arg5;
			data._arg6 = arg6;
			return data;
		}

		private readonly List<object> _args = new(6) { null, null, null, null, null, null };

		public override void Invoke(params object[] args)
		{
			ClearList();
			InitList(args);
			_arg1 = _args[0] != null ? (T1)_args[0] : default;
			_arg2 = _args[1] != null ? (T2)_args[1] : default;
			_arg3 = _args[2] != null ? (T3)_args[2] : default;
			_arg4 = _args[3] != null ? (T4)_args[3] : default;
			_arg5 = _args[4] != null ? (T5)_args[4] : default;
			_arg6 = _args[5] != null ? (T6)_args[5] : default;
		}

		private void InitList(object[] args)
		{
			for (int i = 0; i < args.Length; ++i)
			{
				_args[i] = args[i];
			}
		}

		private void ClearList()
		{
			for (int i = 0; i < _args.Count; ++i)
			{
				_args[i] = null;
			}
		}

		public override void Clear()
		{
			_method = null;
			_arg1 = default;
			_arg2 = default;
			_arg3 = default;
			_arg4 = default;
			_arg5 = default;
			_arg6 = default;
			ClearList();
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7> : AActionArgsData
	{
		public GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7> _method;
		private T1 _arg1;
		private T2 _arg2;
		private T3 _arg3;
		private T4 _arg4;
		private T5 _arg5;
		private T6 _arg6;
		private T7 _arg7;

		public override void Invoke()
		{
			_method?.Invoke(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7);
		}

		public static ActionArgsData<T1, T2, T3, T4, T5, T6, T7> Create(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default)
		{
			var data = ReferencePool.Acquire<ActionArgsData<T1, T2, T3, T4, T5, T6, T7>>();
			data._method = method;
			data._arg1 = arg1;
			data._arg2 = arg2;
			data._arg3 = arg3;
			data._arg4 = arg4;
			data._arg5 = arg5;
			data._arg6 = arg6;
			data._arg7 = arg7;
			return data;
		}

		private readonly List<object> _args = new(7) { null, null, null, null, null, null, null };

		public override void Invoke(params object[] args)
		{
			ClearList();
			InitList(args);
			_arg1 = _args[0] != null ? (T1)_args[0] : default;
			_arg2 = _args[1] != null ? (T2)_args[1] : default;
			_arg3 = _args[2] != null ? (T3)_args[2] : default;
			_arg4 = _args[3] != null ? (T4)_args[3] : default;
			_arg5 = _args[4] != null ? (T5)_args[4] : default;
			_arg6 = _args[5] != null ? (T6)_args[5] : default;
			_arg7 = _args[6] != null ? (T7)_args[6] : default;
		}

		private void InitList(params object[] args)
		{
			for (int i = 0; i < args.Length; ++i)
			{
				_args[i] = args[i];
			}
		}

		private void ClearList()
		{
			for (int i = 0; i < _args.Count; ++i)
			{
				_args[i] = null;
			}
		}

		public override void Clear()
		{
			_method = null;
			_arg1 = default;
			_arg2 = default;
			_arg3 = default;
			_arg4 = default;
			_arg5 = default;
			_arg6 = default;
			_arg7 = default;
			ClearList();
		}
	}
}
