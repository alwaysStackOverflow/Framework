namespace Common
{
	public static class Protocol
	{
		public const int CheckAccountRequest = 1;
		public const int CheckAccountReply = 2;

		public const int MainAccountLoginRequest = 3;
		public const int NormalAccountLoginRequest = 4;
		public const int MainAccountRegisterRequest = 5;
		public const int NormalAccountRegisterRequest = 6;
		public const int AccountLoginReply = 7;

		public const int EnterRoomRequest = 8;
		public const int EnterRoomReply = 9;
	}
}