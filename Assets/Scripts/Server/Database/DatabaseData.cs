using Mono.Data.Sqlite;
using Common;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using UnityGameFramework;
using System.Data.Common;
using GameFramework;
using System.Text;

namespace Server
{
	internal class DatabaseData
	{
		private const string Int = "INTEGER";
		private const string Long = "BIGINT";
		private const string Str = "TEXT";
		private const string Bool = "BOOLEAN";
		private const string Double = "DOUBLE";
		private const string Key = "PRIMARY KEY";
		private const string Unique = "UNIQUE";

		private SqliteConnection _connection;

		internal DatabaseData(string database)
		{
			if (!File.Exists(database))
			{
				Log.Info(database);
				CreateNewDatabase(database);
			}
			else
			{
				_connection = new SqliteConnection($"Data Source={database}");
				_connection.Open();
			}
		}

		private void CreateNewDatabase(string database)
		{
			CreateDatabase(database);
			_connection = new SqliteConnection($"Data Source={database}");
			_connection.Open();
			CreateTable("Version", $"Value {Int} {Key}, CreateTime {Long}, LastSaveTime {Long}, NextUID {Long}, Name {Str}, Level {Int}, ModelType {Int}");
			CreateTable("Accounts", $"Account {Long} {Key}, Name {Str}, Password {Str}, UID {Long} {Unique}, Gender {Int}, MainAccount {Bool}");
			CreateTable("PlayerInfo", $"UID {Long} {Key}, ModelType {Int}, Life {Double}, Hunger {Double}, Thirst {Double}, Exp {Long}, Level {Int}, Scene {Int}");
			CreateTable("Containers", $"ID {Long} {Key}, OwnerUID {Long}, Type {Int}, RemainCapacity {Int}, Items {Str}");
			CreateTable("Room", $"ID {Long} {Key}, OwnerUID  {Long}, RoomType {Int}, Buildings {Str}, Zombies {Str}, Players {Str}");
			CreateTable("Transport", $"UID {Long} {Key}, StartTime {Long}, EndTime {Long}, OriginScene {Int}, DestinationScene {Int}");
			Add("Version", "Value, CreateTime, LastSaveTime, NextUID, Name, Level, ModelType", GetInsertValueString(Version.CurrentVersion, TimeInfo.LocalTimeTicks, TimeInfo.LocalTimeTicks, 100000000, "name", 0, 0));
		}

		internal (int Value, long CreateTime, long LastSaveTime, long NextUID, string Name, int Level, PlayerModelType ModelType) GetVersion()
		{
			using var reader = Search("Version", "*");
			while (reader.Read())
			{
				switch (Version.CurrentVersion)
				{
					case Version.Version1:
					{
						(int Value, long CreateTime, long LastSaveTime, long NextUID, string Name, int Level, PlayerModelType ModelType) versionInfo = new()
						{
							Value = reader.GetInt32(0),
							CreateTime = reader.GetInt64(1),
							LastSaveTime = reader.GetInt64(2),
							NextUID = reader.GetInt64(3),
							Name = reader.GetString(4),
							Level = reader.GetInt32(5),
							ModelType = (PlayerModelType)reader.GetInt32(6),
						};
						return versionInfo;
					}
				}

			}
			return default;
		}

		internal bool SaveVersion(Version data)
		{
			return Update("Version", $"LastSaveTime = {TimeInfo.LocalTimeTicks}, NextUID = {data.NextUID}, Name = {data.Name}, Level = {data.Level}, ModelType = {(int)data.ModelType}", $"Value = {Version.CurrentVersion}");
		}

		internal bool AddNewPlayer(Player p)
		{
			if (!Add("Accounts", "Account, Name, Password, UID, Gender, MainAccount", GetInsertValueString(p.Account, p.Name, p.Password, p.UID, p.Gender, p.MainAccount)))
			{
				return false;
			}
			if (!Add("PlayerInfo", "UID, ModelType, Life, Hunger, Thirst, Exp, Level, Scene", GetInsertValueString(p.UID, p.ModelType, p.Life, p.Hunger, p.Thirst, p.Exp, p.Level, p.Scene)))
			{
				return false;
			}
			return true;
		}

		internal List<(long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount)> GetAllPlayerAccount()
		{
			List<(long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount)> playerInfos = new();
			using var reader = Search("Accounts", "*");
			while (reader.Read())
			{
				switch (Version.CurrentVersion)
				{
					case Version.Version1:
					{
						(long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount) playerInfo = new()
						{
							Account = reader.GetInt64(0),
							Name = reader.GetString(1),
							Password = reader.GetString(2),
							UID = reader.GetInt64(3),
							Gender = (GenderType)reader.GetInt32(4),
							MainAccount = reader.GetBoolean(5),
						};
						playerInfos.Add(playerInfo);
						break;
					}
				}

			}
			return playerInfos;
		}

		internal (bool, (long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount)) GetPlayerAccount(long account)
		{
			using var reader = Search("Accounts", "*", $"Account = {account}");
			while (reader.Read())
			{
				switch (Version.CurrentVersion)
				{
					case Version.Version1:
					{
						(long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount) info = new()
						{
							Account = reader.GetInt64(0),
							Name = reader.GetString(1),
							Password = reader.GetString(2),
							UID = reader.GetInt64(3),
							Gender = (GenderType)reader.GetInt32(4),
							MainAccount = reader.GetBoolean(5),
						};
						return (true, info);
					}
				}
			}
			return (false, default);
		}

		internal (bool, (long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount)) GetMainAccount()
		{
			using var reader = Search("Accounts", "*", $"MainAccount = {true}");
			while (reader.Read())
			{
				switch (Version.CurrentVersion)
				{
					case Version.Version1:
					{
						(long Account, string Name, string Password, long UID, GenderType Gender, bool MainAccount) info = new()
						{
							Account = reader.GetInt64(0),
							Name = reader.GetString(1),
							Password = reader.GetString(2),
							UID = reader.GetInt64(3),
							Gender = (GenderType)reader.GetInt32(4),
							MainAccount = reader.GetBoolean(5),
						};
						return (true, info);
					}
				}
			}
			return (false, default);
		}

		internal void DeletePlayerAccount(long uid)
		{
			Delete("Accounts", $"UID = {uid}");
		}

		internal List<(long UID, PlayerModelType ModelType, double Life, double Hunger, double Thirst, long Exp, int Level, SceneType Scene)> GetAllPlayerInfo()
		{
			List<(long UID, PlayerModelType ModelType, double Life, double Hunger, double Thirst, long Exp, int Level, SceneType Scene)> playerInfos = new();
			using var reader = Search("PlayerInfo", "*");
			while (reader.Read())
			{
				switch (Version.CurrentVersion)
				{
					case Version.Version1:
					{
						(long UID, PlayerModelType ModelType, double Life, double Hunger, double Thirst, long Exp, int Level, SceneType Scene) playerInfo = new()
						{
							UID = reader.GetInt64(0),
							ModelType = (PlayerModelType)reader.GetInt32(1),
							Life = reader.GetDouble(2),
							Hunger = reader.GetDouble(3),
							Thirst = reader.GetDouble(4),
							Exp = reader.GetInt64(5),
							Level = reader.GetInt32(6),
							Scene = (SceneType)reader.GetInt32(7),
						};
						playerInfos.Add(playerInfo);
						break;
					}
				}

			}
			return playerInfos;
		}

		internal (bool, (long UID, PlayerModelType ModelType, double Life, double Hunger, double Thirst, long Exp, int Level, SceneType Scene)) GetPlayerInfo(long uid)
		{
			using var reader = Search("PlayerInfo", "*", $"UID = {uid}");
			while (reader.Read())
			{
				switch (Version.CurrentVersion)
				{
					case Version.Version1:
					{
						(long UID, PlayerModelType ModelType, double Life, double Hunger, double Thirst, long Exp, int Level, SceneType Scene) info = new()
						{
							UID = reader.GetInt64(0),
							ModelType = (PlayerModelType)reader.GetInt32(1),
							Life = reader.GetDouble(2),
							Hunger = reader.GetDouble(3),
							Thirst = reader.GetDouble(4),
							Exp = reader.GetInt64(5),
							Level = reader.GetInt32(6),
							Scene = (SceneType)reader.GetInt32(7),
						};
						return (true, info);
					}
				}
			}
			return (false, default);
		}

		internal void DeletePlayerInfo(long uid)
		{
			Delete("PlayerInfo", $"UID = {uid}");
		}


		internal void Dispose()
		{
			_connection.Close();
			_connection.Dispose();
			_connection = null;
		}

		private void CreateDatabase(string path)
		{
			SqliteConnection.CreateFile(path);
		}
		/// <summary>
		/// 创建表
		/// </summary>
		/// <remarks>
		/// CREATE TABLE table_name (column1 datatype,column2 datatype,...);
		/// </remarks>
		private bool CreateTable(string table, string columns)
		{
			try
			{
				using var command = _connection.CreateCommand();
				command.CommandText = $"CREATE TABLE IF NOT EXISTS {table} ({columns});";
				command.ExecuteNonQuery();
				return true;
			}
			catch (Exception e)
			{
				Log.Error($"Message:{e}\nStack{e.StackTrace}");
				return false;
			}
		}

		/// <summary>
		/// 插入数据
		/// </summary>
		/// <remarks>
		/// INSERT INTO table_name (column1, column2, ...) VALUES (value1, value2, ...);
		/// </remarks>
		private bool Add(string table, string columns, string values)
		{
			try
			{
				using var command = _connection.CreateCommand();
				command.CommandText = $"INSERT INTO {table} ({columns}) VALUES ({values});";
				command.ExecuteNonQuery();
				return true;
			}
			catch (Exception e)
			{
				Log.Error($"Message:{e}\nStack{e.StackTrace}");
				return false;
			}
		}

		/// <summary>
		/// 查找数据 
		/// </summary>
		/// <remarks>
		/// SELECT column1, column2, ... FROM table_name WHERE condition;
		/// </remarks>
		private DbDataReader Search(string table, string columns, string condition = "")
		{
			try
			{
				using var command = _connection.CreateCommand();
				command.CommandText = $"SELECT {columns} FROM {table}" + (string.IsNullOrWhiteSpace(condition) ? "" : $" WHERE {condition}") + ";";
				return command.ExecuteReader();
			}
			catch (Exception e)
			{
				Log.Error($"Message:{e}\nStack{e.StackTrace}");
				return null;
			}
		}

		/// <summary>
		/// 更新数据 
		/// </summary>
		/// <remarks>
		/// UPDATE table_name SET column1 = value1, column2 = value2, ... WHERE condition;
		/// </remarks>

		private bool Update(string table, string columnsEqualValue, string condition)
		{
			try
			{
				using var command = _connection.CreateCommand();
				command.CommandText = $"UPDATE {table} SET {columnsEqualValue} WHERE {condition};";
				command.ExecuteNonQuery();
				return true;
			}
			catch (Exception e)
			{
				Log.Error($"Message:{e}\nStack{e.StackTrace}");
				return false;
			}
		}

		/// <summary>
		/// 删除数据 
		/// </summary>
		/// <remarks>
		/// DELETE FROM table_name WHERE condition;
		/// </remarks>

		private bool Delete(string table, string condition)
		{
			try
			{
				using var command = _connection.CreateCommand();
				command.CommandText = $"DELETE FROM {table} WHERE {condition};";
				command.ExecuteNonQuery();
				return true;
			}
			catch (Exception e)
			{
				Log.Error($"Message:{e}\nStack{e.StackTrace}");
				return false;
			}
		}

		private readonly StringBuilder sb = new StringBuilder();
		private string GetInsertValueString(params object[] args)
		{
			sb.Clear();
			foreach (object arg in args)
			{
				if(arg is string str)
				{
					sb.Append($", \"{str}\"");
				}
				else if(arg is Enum)
				{
					sb.Append($", {(int)arg}");
				}
				else
				{
					sb.Append($", {arg}");
				}
			}
			sb.Remove(0, 2);
			return sb.ToString();
		}
	}
}
