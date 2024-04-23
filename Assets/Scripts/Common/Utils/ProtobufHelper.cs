                                           using System;
using System.IO;
using ProtoBuf.Meta;
using System.ComponentModel;

public static class ProtobufHelper
{
	static ProtobufHelper()
	{

	}

	public static object Deserialize(Type type, byte[] bytes, int index, int count)
	{
		using MemoryStream stream = new(bytes, index, count);
		object o = ProtoBuf.Serializer.Deserialize(type, stream);
		if (o is ISupportInitialize supportInitialize)
		{
			supportInitialize.EndInit();
		}
		return o;
	}

	public static byte[] Serialize(object message)
	{
		using MemoryStream stream = new();
		ProtoBuf.Serializer.Serialize(stream, message);
		return stream.ToArray();
	}

	public static void Serialize(object message, Stream stream)
	{
		ProtoBuf.Serializer.Serialize(stream, message);
	}

	public static object Deserialize(Type type, Stream stream)
	{
		object o = ProtoBuf.Serializer.Deserialize(type, stream);
		if (o is ISupportInitialize supportInitialize)
		{
			supportInitialize.EndInit();
		}
		return o;
	}
}
