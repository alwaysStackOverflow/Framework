using Common;
using UnityEngine;
using System.Threading;
using UnityGameFramework;
using System.Runtime.InteropServices;
using System.Net;
using GameFramework.Network;

namespace Server
{
	public class ServerEntry : MonoBehaviour
	{
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
		private static extern uint TimeBeginPeriod(uint uMilliseconds);

		[DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
		private static extern uint TimeEndPeriod(uint uMilliseconds);
#endif

		private static bool _changed = false;
		private static void ChangeThreadAccurac(uint milliseconds)
		{
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
			//if (!_changed)
			//{
			//	TimeBeginPeriod(milliseconds);
			//	_changed = true;
			//}
#endif
		}

		private static void FallbackThreadAccurac(uint milliseconds)
		{
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
			//if (_changed)
			//{
			//	TimeEndPeriod(milliseconds);
			//	_changed = false;
			//}
#endif
		}

		public static EventComponent Event
		{
			get;
			private set;
		}

		internal static NetworkEventManager NetworkEvent
		{
			get;
			private set;
		}

		private Thread _thread;
		private bool _run = false;
		private Server _server;


		private void Awake()
		{
			_thread = null;
			_server = null;
			NetworkEvent = new();
			DontDestroyOnLoad(this);
		}

		private void Start()
		{
			Event = GameEntry.GetComponent<EventComponent>();

			Event.Subscribe<AwakeServerEvent>(AwakeServerEvent.EventId, CreateServerThread);
			Event.Subscribe<ShutdownServerEvent>(ShutdownServerEvent.EventId, ShutDownServer);
		}

		private void OnDestroy()
		{
			Event.Unsubscribe<AwakeServerEvent>(AwakeServerEvent.EventId, CreateServerThread);
			Event.Unsubscribe<ShutdownServerEvent>(ShutdownServerEvent.EventId, ShutDownServer);
			_run = false;
		}

		private void OnApplicationQuit()
		{
			try
			{
				_thread?.Abort();
				_server?.Shutdown();
				NetworkEvent?.Shutdown();
				ChangeThreadAccurac(1);
			}
			catch
			{

			}
		}

		private void CreateServerThread(AwakeServerEvent args)
		{
			if (_server != null)
			{
				return;
			}
			_server = new Server(args.Database);
			_server.Init();
			var v6EndPoint = new IPEndPoint(NetworkHelper.GetOsAddress(true), PortType.ServerUdpV6);
			var v4EndPoint = new IPEndPoint(NetworkHelper.GetOsAddress(false), PortType.ServerUdpV6);
			InitServerFinshEvent.Fire(v6EndPoint, v4EndPoint, _server.MainCountConnenctionToken);
			ChangeThreadAccurac(1);
			_run = true;
			_thread = new Thread(ThreadRun)
			{
				IsBackground = true,
			};
			_thread.Start();
		}

		private void ThreadRun()
		{
			while (_run)
			{
				_server.Update();
				NetworkEvent.Update();
				Thread.Sleep(1);
			}
			NetworkEvent.Shutdown();
			_server.Shutdown();
			_thread = null;
			_server = null;
			FallbackThreadAccurac(1);
		}

		private void ShutDownServer(ShutdownServerEvent e)
		{
			_run = false;
		}
	}
}