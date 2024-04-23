namespace GameFramework
{
	public abstract class AActionArgsData
	{
		public abstract void Invoke();
	}

	public class ActionArgsData : AActionArgsData
	{
		private readonly GameFrameworkAction _method;

		public ActionArgsData(GameFrameworkAction method)
		{
			_method = method;
		}

		public override void Invoke()
		{
			_method();
		}
	}

	public class ActionArgsData<T1> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1> _method;
		private readonly T1 _arg1;

		public ActionArgsData(GameFrameworkAction<T1> method, T1 arg1 = default)
		{
			_method = method;
			_arg1 = arg1;
		}

		public override void Invoke()
		{
			_method(_arg1);
		}
	}

	public class ActionArgsData<T1, T2> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;

		public ActionArgsData(GameFrameworkAction<T1, T2> method, T1 arg1 = default, T2 arg2 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
		}

		public override void Invoke()
		{
			_method(_arg1, _arg2);
		}
	}

	public class ActionArgsData<T1, T2, T3> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;

		public ActionArgsData(GameFrameworkAction<T1, T2, T3> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
		}

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
		}

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
		}

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
		}

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6);
		}
	}


	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _method;
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

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _method;
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

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default, T11 arg11 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _method;
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

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default, T11 arg11 = default, T12 arg12 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> _method;
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

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default, T11 arg11 = default, T12 arg12 = default, T13 arg13 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12, _arg13);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> _method;
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

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default, T11 arg11 = default, T12 arg12 = default, T13 arg13 = default, T14 arg14 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12, _arg13, _arg14);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> _method;
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

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default, T11 arg11 = default, T12 arg12 = default, T13 arg13 = default, T14 arg14 = default, T15 arg15 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12, _arg13, _arg14, _arg15);
		}
	}

	public class ActionArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : AActionArgsData
	{
		private readonly GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> _method;
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

		public ActionArgsData(GameFrameworkAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default, T11 arg11 = default, T12 arg12 = default, T13 arg13 = default, T14 arg14 = default, T15 arg15 = default, T16 arg16 = default)
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

		public override void Invoke()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12, _arg13, _arg14, _arg15, _arg16);
		}
	}
}
