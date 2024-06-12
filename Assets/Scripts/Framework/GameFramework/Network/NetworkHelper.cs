using MemoryPack;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace GameFramework.Network
{
	public static class NetworkHelper
	{
		public static IPEndPoint Clone(this EndPoint endPoint)
		{
			IPEndPoint ip = (IPEndPoint)endPoint;
			ip = new IPEndPoint(ip.Address, ip.Port);
			return ip;
		}

		/// <summary>
		/// 获取主机Ip地址, ipv6优先
		/// </summary>
		public static IPAddress GetHostAddress(string host, bool getIpV6 = true)
		{
			var addresList = Dns.GetHostAddresses(host);
			IPAddress result = default;
			foreach (var addres in addresList)
			{
				result = addres;
				if(getIpV6)
				{
					if (addres.AddressFamily == AddressFamily.InterNetworkV6)
					{
						return result;
					}
				}
				else
				{
					if (addres.AddressFamily == AddressFamily.InterNetwork)
					{
						return result;
					}
				}
			}
			return result;
		}

		/// <summary>
		/// 获取本地Ip地址, ipv6优先
		/// </summary>
		public static IPAddress GetOsAddress(bool getIpV6 = true)
		{
			var hostName = Dns.GetHostName();
			var addresList = Dns.GetHostAddresses(hostName);
			IPAddress result = default;
			foreach (var addres in addresList)
			{
				result = addres;
				if(getIpV6)
				{
					if (addres.AddressFamily == AddressFamily.InterNetworkV6)
					{
						return result;
					}
				}
				else
				{
					if (addres.AddressFamily == AddressFamily.InterNetwork)
					{
						return result;
					}
				}
			}
			return result;
		}


		public static IPEndPoint ConvertToIPEndPoint(string host, int port)
		{
			return new IPEndPoint(IPAddress.Parse(host), port);
		}

		public static IPEndPoint ConvertToIPEndPoint(string address)
		{
			int index = address.LastIndexOf(':');
			return ConvertToIPEndPoint(address[..index], int.Parse(address[(index + 1)..]));
		}

		public static void SetSioUdoConnectReset(Socket socket)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				const uint IOC_IN = 0x80000000;
				const uint IOC_VENDOR = 0x18000000;
				const int SIO_UDP_CONNRESET = unchecked((int)(IOC_IN | IOC_VENDOR | 12));
				socket.IOControl(SIO_UDP_CONNRESET, new[] { Convert.ToByte(false) }, null);
			}
		}

		public static ProtocolType GetOsServiceType()
		{
			bool ipv6Supported = Socket.OSSupportsIPv6;
			return ipv6Supported ? ProtocolType.UdpV6 : ProtocolType.UdpV4;
		}

		public static ProtoObject Deserialize(Type type, Span<byte> buffer)
		{
			object protoObject = ReferencePool.Acquire(type);
			MemoryPackSerializer.Deserialize(type, buffer, ref protoObject);
			if (protoObject is ISupportInitialize supportInitialize)
			{
				supportInitialize.EndInit();
			}
			return protoObject as ProtoObject;
		}

		public static void Serialize(object message, MemoryBuffer stream)
		{
			if (message is ISupportInitialize supportInitialize)
			{
				supportInitialize.BeginInit();
			}
			MemoryPackSerializer.Serialize(message.GetType(), stream, message);
		}

		public static void MessageToStream(MemoryBuffer stream, ProtoObject message)
		{
			stream.Seek(0, SeekOrigin.Begin);
			stream.SetLength(4);
			stream.GetBuffer().WriteTo(0, message.ProtocolID);
			stream.Seek(4, SeekOrigin.Begin);
			Serialize(message, stream);
		}
	}
}
