using System;
using System.Net;

namespace GameFramework.Network
{
	public enum ConnectionType
	{
		Connect,
		Accept,
	}

	public abstract class AConnection : IDisposable
	{
		public uint LocalConnectionId { get; protected set; }
		public uint RemoteConnectionId { get; set; }

		public string RemoteIPAddress => RemoteIPEndPoint.Address.ToString();

		public IPEndPoint RemoteIPEndPoint { get; set; }

		public ConnectionType ConnectionType { get; protected set; }

		public int Code { get; set; }

		public bool IsDisposed => LocalConnectionId == 0;

		public abstract void Dispose();
	}

}
