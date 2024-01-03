namespace GameFramework
{
	public abstract class ADelegatesArgsData
	{
		public abstract void Fire();
	}

	public class ArgsData : ADelegatesArgsData
	{
		private readonly Method _method;

		public ArgsData(Method method)
		{
			_method = method;
		}

		public override void Fire()
		{
			_method();
		}
	}

	public class ArgsData<T1> : ADelegatesArgsData
	{
		private readonly Method<T1> _method;
		private readonly T1 _arg1;

		public ArgsData(Method<T1> method, T1 arg1 = default)
		{
			_method = method;
			_arg1 = arg1;
		}

		public override void Fire()
		{
			_method(_arg1);
		}
	}

	public class ArgsData<T1, T2> : ADelegatesArgsData
	{
		private readonly Method<T1, T2> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;

		public ArgsData(Method<T1, T2> method, T1 arg1 = default, T2 arg2 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
		}

		public override void Fire()
		{
			_method(_arg1, _arg2);
		}
	}

	public class ArgsData<T1, T2, T3> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;

		public ArgsData(Method<T1, T2, T3> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
		}

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3);
		}
	}

	public class ArgsData<T1, T2, T3, T4> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;

		public ArgsData(Method<T1, T2, T3, T4> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
		}

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4);
		}
	}

	public class ArgsData<T1, T2, T3, T4, T5> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4, T5> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;

		public ArgsData(Method<T1, T2, T3, T4, T5> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
		}

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5);
		}
	}

	public class ArgsData<T1, T2, T3, T4, T5, T6> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4, T5, T6> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;

		public ArgsData(Method<T1, T2, T3, T4, T5, T6> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default)
		{
			_method = method;
			_arg1 = arg1;
			_arg2 = arg2;
			_arg3 = arg3;
			_arg4 = arg4;
			_arg5 = arg5;
			_arg6 = arg6;
		}

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6);
		}
	}


	public class ArgsData<T1, T2, T3, T4, T5, T6, T7> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4, T5, T6, T7> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;

		public ArgsData(Method<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default)
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

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7);
		}
	}

	public class ArgsData<T1, T2, T3, T4, T5, T6, T7, T8> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4, T5, T6, T7, T8> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;

		public ArgsData(Method<T1, T2, T3, T4, T5, T6, T7, T8> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default)
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

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8);
		}
	}

	public class ArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4, T5, T6, T7, T8, T9> _method;
		private readonly T1 _arg1;
		private readonly T2 _arg2;
		private readonly T3 _arg3;
		private readonly T4 _arg4;
		private readonly T5 _arg5;
		private readonly T6 _arg6;
		private readonly T7 _arg7;
		private readonly T8 _arg8;
		private readonly T9 _arg9;

		public ArgsData(Method<T1, T2, T3, T4, T5, T6, T7, T8, T9> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default)
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

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9);
		}
	}

	public class ArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> _method;
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

		public ArgsData(Method<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default)
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

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10);
		}
	}

	public class ArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> _method;
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

		public ArgsData(Method<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default, T11 arg11 = default)
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

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11);
		}
	}

	public class ArgsData<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ADelegatesArgsData
	{
		private readonly Method<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> _method;
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

		public ArgsData(Method<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> method, T1 arg1 = default, T2 arg2 = default, T3 arg3 = default, T4 arg4 = default, T5 arg5 = default, T6 arg6 = default, T7 arg7 = default, T8 arg8 = default, T9 arg9 = default, T10 arg10 = default, T11 arg11 = default, T12 arg12 = default)
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

		public override void Fire()
		{
			_method(_arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg7, _arg8, _arg9, _arg10, _arg11, _arg12);
		}
	}
}
