using System.Collections.Generic;
using System.IO;

namespace Client
{
	public class MenuData : AModel
	{
		private List<FileInfo> _fileInfos = new();
		public List<FileInfo> FileInfos
		{
			get
			{
				if (_fileInfos.Count == 0)
				{
					GetSavingsFiles(ref _fileInfos);
				}
				return _fileInfos;
			}
		}
		public override void ClearData()
		{
			_fileInfos.Clear();
		}

		public string GameDatabaseDirectory
		{
			get
			{
#if UNITY_EDITOR
				var path = Path.GetFullPath("Bundles/Savings");
#else
				var path = $"{UnityEngine.Application.persistentDataPath}/Savings";
#endif
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				return path;
			}
		}

		public List<string> GetSavingsPath()
		{
			if (!Directory.Exists(GameDatabaseDirectory))
			{
				Directory.CreateDirectory(GameDatabaseDirectory);
				return new List<string>();
			}
			var allFile = Directory.GetFiles(GameDatabaseDirectory);
			var result = new List<string>();
			foreach (var file in allFile)
			{
				if (Path.GetExtension(file).ToLower() == ".db")
				{
					result.Add(file);
				}
			}
			return result;
		}

		public FileInfo GetSavingsFileInfo(string fileName)
		{
			if (!File.Exists(fileName))
			{
				return null;
			}
			return new FileInfo(fileName);
		}

		private void GetSavingsFiles(ref List<FileInfo> list)
		{
			var savings = GetSavingsPath();
			foreach (var file in savings)
			{
				list.Add(GetSavingsFileInfo(file));
			}
		}
	}
}
