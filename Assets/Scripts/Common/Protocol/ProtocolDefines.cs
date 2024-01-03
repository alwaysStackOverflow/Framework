using ProtoBuf;
using Newtonsoft.Json;

public class ProtoObject
{
	public object Clone()
	{
		byte[] bytes = ProtobufHelper.Serialize(this);
		return ProtobufHelper.Deserialize(GetType(), bytes, 0, bytes.Length);
	}

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}

[ProtoContract]
public partial class LoginRequest : ProtoObject
{
	[ProtoMember(1)]
	public int Id;
}

public partial class LoginReply : ProtoObject
{
	[ProtoMember(1)]
	public int Id;
}