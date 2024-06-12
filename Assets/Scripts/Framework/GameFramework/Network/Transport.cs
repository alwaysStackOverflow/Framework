using System;
using System.Net;
using System.Net.Sockets;
namespace GameFramework.Network
{
	public interface IKcpTransport : IDisposable
	{
		void Send(byte[] bytes, int index, int length, EndPoint endPoint);
		int Receive(byte[] buffer, ref EndPoint endPoint);
		int Available();
		void Update();
		void OnError(long id, int error);
	}


	public class UdpTransport : IKcpTransport
	{
		public Socket Socket { get; private set; }

		public UdpTransport(IPEndPoint endPoint)
		{
			Socket = new Socket(endPoint.AddressFamily, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp)
			{
				SendBufferSize = Kcp.OneM * 64,
				ReceiveBufferSize = Kcp.OneM * 64
			};
			try
			{
				Socket.Bind(endPoint);
			}
			catch(Exception e)
			{
				Socket.Close();
				throw new Exception($"bind error: {endPoint}", e);
			}
			NetworkHelper.SetSioUdoConnectReset(Socket);
		}

		public int Available()
		{
			return Socket.Available;
		}
		public int Receive(byte[] buffer, ref EndPoint endPoint)
		{
			return Socket.ReceiveFrom(buffer, ref endPoint);
		}

		public void Send(byte[] bytes, int index, int length, EndPoint endPoint)
		{
			Socket.SendTo(bytes, index, length, SocketFlags.None, endPoint);
		}
		public void Dispose()
		{
			Socket?.Dispose();
		}

		public void OnError(long id, int error)
		{
			
		}

		public void Update()
		{
			
		}
	}
}

