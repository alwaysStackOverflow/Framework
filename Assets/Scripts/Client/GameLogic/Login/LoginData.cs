using Common;
using System.Collections.Generic;

namespace Client
{
	public class LoginData : AModel
	{
		public string Token { get; set; }

		public bool HasMainAccount { get; set; }

		public bool IsMainAccount { get; set; }

		public List<PlayerModelType> MaleList { get; private set; } = new()
		{
			PlayerModelType.WhiteMale,
			PlayerModelType.YellowMale,
			PlayerModelType.BrownMale,
			PlayerModelType.BlackMale,
		};

		public List<PlayerModelType> FemaleList { get; private set; } = new()
		{
			PlayerModelType.WhiteFemale,
			PlayerModelType.YellowFemale,
			PlayerModelType.BrownFemale,
			PlayerModelType.BlackFemale,
		};

		public override void ClearData()
		{
			Token = string.Empty;
			HasMainAccount = false;
			IsMainAccount = false;
		}
	}
}
