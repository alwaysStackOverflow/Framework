//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
using System;
using System.ComponentModel;
using MemoryPack;
using Newtonsoft.Json;

namespace GameFramework.Network
{
	/// <summary>
	/// 网络消息包基类。
	/// </summary>
	public interface IPacket
	{
		public int ProtocolID
		{
			get;
		}

		public int Code
		{
			get; set;
		}
	}

	public abstract class ProtoObject : IPacket, IReference
	{
		protected int _;
		public abstract int ProtocolID { get;}
		public abstract int Code { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}

		public byte[] Serialize()
		{
			return MemoryPackSerializer.Serialize(this);
		}

		public abstract void Clear();

		public void Recycle()
		{
			ReferencePool.Release(this);
		}

		public static T Create<T>() where T : ProtoObject, IReference, new()
		{
			var request = ReferencePool.Acquire<T>();
			request.Code = 0;
			return request;
		}
	}
}
