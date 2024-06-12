

using GameFramework;
using GameFramework.Network;

namespace Client
{
	public abstract class AView
	{
		public T CreateProtocolObject<T>() where T : ProtoObject, IReference, new()
		{
			var request = ReferencePool.Acquire<T>();
			request.Code = 0;
			return request;
		}

		public void SendNetworkMessage<T>(T request) where T : ProtoObject
		{
			ClientEntry.Network.Send(request);
		}
	}
}
