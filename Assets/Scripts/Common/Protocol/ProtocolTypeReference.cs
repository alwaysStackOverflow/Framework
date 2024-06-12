using System;
using System.Collections.Generic;

namespace Common
{
	public static class ProtocolTypeReference
	{
		static ProtocolTypeReference()
		{
			Register(Protocol.CheckAccountRequest, typeof(CheckAccountRequest));
			Register(Protocol.CheckAccountReply, typeof(CheckAccountReply));
			Register(Protocol.MainAccountLoginRequest, typeof(MainAccountLoginRequest));
			Register(Protocol.NormalAccountLoginRequest, typeof(NormalAccountLoginRequest));
			Register(Protocol.MainAccountRegisterRequest, typeof(MainAccountRegisterRequest));
			Register(Protocol.NormalAccountRegisterRequest, typeof(NormalAccountRegisterRequest));
			Register(Protocol.AccountLoginReply, typeof(AccountLoginReply));
			Register(Protocol.EnterRoomRequest, typeof(EnterRoomRequest));
			Register(Protocol.EnterRoomReply, typeof(EnterRoomReply));
		}

		private static readonly Dictionary<int, Type> _protocolType = new();

		private static void Register(int id, Type t)
		{
			_protocolType.Add(id, t);
		}

		public static Type Get(int id)
		{
			if(_protocolType.TryGetValue(id, out var type))
			{
				return type;
			}
			return null;
		}

		public static bool TryGet(int id, out Type t)
		{
			if (_protocolType.TryGetValue(id, out t))
			{
				return true;
			}
			return false;
		}
	}
}
