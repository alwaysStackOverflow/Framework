using Common;

namespace Server
{
	internal class Version
	{
		public const int Version1 = 1;

		public static readonly int CurrentVersion = Version1;
		public int VersionValue {  get; set; }

		public long CreateTime { get; set; }

		public long LastSaveTime { get; set; }

		public long NextUID { get; set; }

		public string Name { get; set; }

		public int Level { get; set; }

		public PlayerModelType ModelType { get; set; }
	}
}
