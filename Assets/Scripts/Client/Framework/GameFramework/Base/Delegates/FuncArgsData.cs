namespace GameFramework
{
	public abstract class AFuncArgsData<TResult>
	{
		public abstract TResult Invoke();
	}

	public class FuncArgsData<TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<TResult> _method;

		public FuncArgsData(GameFrameworkFunc<TResult> method)
		{
			_method = method;
		}

		public override TResult Invoke()
		{
			return _method();
		}
	}

	public class FuncArgsData<T1, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1,  TResult> _method;
		private readonly T1 _arg1;

		public FuncArgsData(GameFrameworkFunc<T1, TResult> method, T1 arg1)
		{
			_method = method;
			_arg1 = arg1;
		}

		public override TResult Invoke()
		{
			return _method(_arg1);
		}
	}

	public class FuncArgsData<T1, T2, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;

		public FuncArgsData(GameFrameworkFunc<T1, T2, TResult> method, T1 arg1, T2 arg2)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2);
		}
	}

	public class FuncArgsData<T1, T2, T3, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, TResult> method, T1 arg1, T2 arg2, T3 arg3)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
			_arg9 = arg9;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;
		private readonly T10 _arg10;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
			_arg9 = arg9;
			_arg10 = arg10;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;
		private readonly T10 _arg10;
		private readonly T11 _arg11;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
			_arg9 = arg9;
			_arg10 = arg10;
			_arg11 = arg11;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;
		private readonly T10 _arg10;
		private readonly T11 _arg11;
		private readonly T12 _arg12;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
			_arg9 = arg9;
			_arg10 = arg10;
			_arg11 = arg11;
			_arg12 = arg12;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;
		private readonly T10 _arg10;
		private readonly T11 _arg11;
		private readonly T12 _arg12;
		private readonly T13 _arg13;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
			_arg9 = arg9;
			_arg10 = arg10;
			_arg11 = arg11;
			_arg12 = arg12;
			_arg13 = arg13;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12, _arg13);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;
		private readonly T10 _arg10;
		private readonly T11 _arg11;
		private readonly T12 _arg12;
		private readonly T13 _arg13;
		private readonly T14 _arg14;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
			_arg9 = arg9;
			_arg10 = arg10;
			_arg11 = arg11;
			_arg12 = arg12;
			_arg13 = arg13;
			_arg14 = arg14;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12, _arg13, _arg14);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;
		private readonly T10 _arg10;
		private readonly T11 _arg11;
		private readonly T12 _arg12;
		private readonly T13 _arg13;
		private readonly T14 _arg14;
		private readonly T15 _arg15;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
			_arg9 = arg9;
			_arg10 = arg10;
			_arg11 = arg11;
			_arg12 = arg12;
			_arg13 = arg13;
			_arg14 = arg14;
			_arg15 = arg15;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12, _arg13, _arg14, _arg15);
		}
	}

	public class FuncArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> : AFuncArgsData<TResult>
	{
		private readonly GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;
		private readonly T10 _arg10;
		private readonly T11 _arg11;
		private readonly T12 _arg12;
		private readonly T13 _arg13;
		private readonly T14 _arg14;
		private readonly T15 _arg15;
		private readonly T16 _arg16;

		public FuncArgsData(GameFrameworkFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
			_arg7 = arg7;
			_arg8 = arg8;
			_arg9 = arg9;
			_arg10 = arg10;
			_arg11 = arg11;
			_arg12 = arg12;
			_arg13 = arg13;
			_arg14 = arg14;
			_arg15 = arg15;
			_arg16 = arg16;
		}

		public override TResult Invoke()
		{
			return _method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12, _arg13, _arg14, _arg15, _arg16);
		}
	}
}
