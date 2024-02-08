using UnityEngine;
using UnityGameFramework;

namespace Client
{
    public class ClientEntry : MonoBehaviour
    {
		#region Builtin
		/// <summary>
		/// ��ȡ��Ϸ���������
		/// </summary>
		public static BaseComponent Base
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static ConfigComponent Config
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���ݽ�������
		/// </summary>
		public static DataNodeComponent DataNode
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���ݱ������
		/// </summary>
		public static DataTableComponent DataTable
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static DebuggerComponent Debugger
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static DownloadComponent Download
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡʵ�������
		/// </summary>
		public static EntityComponent Entity
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ�¼������
		/// </summary>
		public static EventComponent Event
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ�ļ�ϵͳ�����
		/// </summary>
		public static FileSystemComponent FileSystem
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ����״̬�������
		/// </summary>
		public static FsmComponent Fsm
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���ػ������
		/// </summary>
		public static LocalizationComponent Localization
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static NetworkComponent Network
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ����������
		/// </summary>
		public static ObjectPoolComponent ObjectPool
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static ProcedureComponent Procedure
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ��Դ�����
		/// </summary>
		public static ResourceComponent Resource
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static SceneComponent Scene
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static SettingComponent Setting
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static SoundComponent Sound
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static UIComponent UI
		{
			get;
			private set;
		}

		/// <summary>
		/// ��ȡ���������
		/// </summary>
		public static WebRequestComponent WebRequest
		{
			get;
			private set;
		}

		private static void InitBuiltinComponents()
		{
			Base = GameEntry.GetComponent<BaseComponent>();
			Config = GameEntry.GetComponent<ConfigComponent>();
			DataNode = GameEntry.GetComponent<DataNodeComponent>();
			DataTable = GameEntry.GetComponent<DataTableComponent>();
			Debugger = GameEntry.GetComponent<DebuggerComponent>();
			Download = GameEntry.GetComponent<DownloadComponent>();
			Entity = GameEntry.GetComponent<EntityComponent>();
			Event = GameEntry.GetComponent<EventComponent>();
			FileSystem = GameEntry.GetComponent<FileSystemComponent>();
			Fsm = GameEntry.GetComponent<FsmComponent>();
			Localization = GameEntry.GetComponent<LocalizationComponent>();
			Network = GameEntry.GetComponent<NetworkComponent>();
			ObjectPool = GameEntry.GetComponent<ObjectPoolComponent>();
			Procedure = GameEntry.GetComponent<ProcedureComponent>();
			Resource = GameEntry.GetComponent<ResourceComponent>();
			Scene = GameEntry.GetComponent<SceneComponent>();
			Setting = GameEntry.GetComponent<SettingComponent>();
			Sound = GameEntry.GetComponent<SoundComponent>();
			UI = GameEntry.GetComponent<UIComponent>();
			WebRequest = GameEntry.GetComponent<WebRequestComponent>();
		}
		#endregion Builtin

		#region Custom
		public static BuiltinDataComponent BuiltinData
		{
			get;
			private set;
		}

		//public static ItemComponent Item
		//{
		//	get;
		//	private set;
		//}

		//public static DataComponent Data
		//{
		//	get;
		//	private set;
		//}


		private static void InitCustomComponents()
		{
			BuiltinData = GameEntry.GetComponent<BuiltinDataComponent>();
			//Item = GameEntry.GetComponent<ItemComponent>();
			//Data = GameEntry.GetComponent<DataComponent>();
		}
		#endregion Custom

		private void Start()
		{
			InitBuiltinComponents();
			InitCustomComponents();
		}
	}
}
