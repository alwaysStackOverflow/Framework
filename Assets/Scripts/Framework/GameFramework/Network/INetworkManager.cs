//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Net;

namespace GameFramework.Network
{
    /// <summary>
    /// 网络管理器接口。
    /// </summary>
    public interface INetworkManager
    {
		public void Connect(IPEndPoint ipEndPointV6, IPEndPoint ipEndPointV4);
		public void Disconnect();
		public void SendHeartbeat();
		public void Send(ProtoObject data);


		public event Action AcceptCallback;
		public event Action<MemoryBuffer> ReadCallback;
		public event Action<int> ErrorCallback;
	}
}
