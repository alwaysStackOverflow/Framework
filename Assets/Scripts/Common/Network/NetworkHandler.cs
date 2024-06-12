using GameFramework;
using GameFramework.Network;
using GameFramework.Timer;
using System;
using System.Net;
using UnityEngine;
using UnityGameFramework;

namespace Common
{
	public class NetworkHandler : GameFrameworkComponent
	{
		private INetworkManager m_NetworkManager = null;
		private bool m_IsConnected = false;
		/// <summary>
		/// 游戏框架组件初始化。
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_NetworkManager = GameFrameworkEntry.GetModule<INetworkManager>();
			if (m_NetworkManager == null)
			{
				Log.Fatal("Network manager is invalid.");
				return;
			}
		}

		private void Start()
		{
			m_NetworkManager.AcceptCallback += OnAcceptCallback;
			m_NetworkManager.ReadCallback += OnReceiveCallback;
			m_NetworkManager.ErrorCallback += OnErrorCallback;
		}

		private void OnDestroy()
		{
			m_NetworkManager.AcceptCallback -= OnAcceptCallback;
			m_NetworkManager.ReadCallback -= OnReceiveCallback;
			m_NetworkManager.ErrorCallback -= OnErrorCallback;
		}

		private void SendHeartBeat()
		{
			if(m_IsConnected)
			{
				m_NetworkManager.SendHeartbeat();
			}
		}

		private void OnAcceptCallback()
		{
			m_IsConnected = true;
			CommonEntry.Timer.Remove("PingRequest");
			CommonEntry.Timer.Add(Timer.Create("PingRequest", -1, 1, true, SendHeartBeat));
			CommonEntry.Event.Fire(ConnectionAcceptEventArgs.Create());
		}

		private void OnReceiveCallback(MemoryBuffer steam)
		{
			var buffer = steam.GetBuffer();
			var protocolID = BitConverter.ToInt32(buffer, 0);
			if (ProtocolTypeReference.TryGet(protocolID, out Type type))
			{
				var message = NetworkHelper.Deserialize(type, buffer.AsSpan(4, buffer.Length - 4));
				Log.Info($"Client Receive <color=#00ff00>ID:{protocolID} Code:{message.Code}</color> \n Message:{message}");
				CommonEntry.Event.TriggerNow(message);
			}
		}

		private void OnErrorCallback(int errorCode)
		{
			Log.Error($"OnErrorCallback Code:{errorCode}");
			switch(errorCode)
			{
				case ErrorCode.ERR_PeerDisconnect:
					m_IsConnected = false;
					break;
			}
		}

		public void Connect(IPEndPoint ipEndPointV6, IPEndPoint ipEndPointV4)
		{
			m_NetworkManager.Connect(ipEndPointV6, ipEndPointV4);
		}

		public void Disconnect()
		{
			m_NetworkManager.Disconnect();
		}

		public void Send(ProtoObject data)
		{
			m_NetworkManager.Send(data);
		}
	}
}
