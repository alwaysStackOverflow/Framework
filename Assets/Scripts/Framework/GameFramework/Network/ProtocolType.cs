namespace GameFramework.Network
{
    /// <summary>
    /// 网络服务类型。
    /// </summary>
    public enum ProtocolType : byte
    {
		/// <summary>
		/// Upd、IPv4 协议
		/// </summary>
		UdpV4 = 5,

		/// <summary>
		/// Upd、IPv6 协议
		/// </summary>
		UdpV6 = 4,

		/// <summary>
		/// Tcp、IPv4 协议
		/// </summary>
		TcpV4 = 6,

		/// <summary>
		/// Tcp、IPv6 协议
		/// </summary>
		TcpV6 = 7,
	}

	public static class PortType
	{
        public const ushort ClientUdpV6 = 55001;
		public const ushort ClientUdpV4 = 55002;
		public const ushort ClientTcpV6 = 55003;
		public const ushort ClientTcpV4 = 55004;

		public const ushort ServerUdpV6 = 56001;
		public const ushort ServerUdpV4 = 56002;
		public const ushort ServerTcpV6 = 56003;
		public const ushort ServerTcpV4 = 56004;
	}

	public enum ServiceType
	{
		Client,
		Server,
	}

	public static class NetworkMessageType
	{
		public const byte None = 0;
		public const byte ClientConnect = 1;
		public const byte ServerAccept = 2;
		public const byte Disconnect = 3;
		public const byte GameMessage = 4;
		public const byte PingRequest = 5;
		public const byte PingReply = 6;
	}
}
