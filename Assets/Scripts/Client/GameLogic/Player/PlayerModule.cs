using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	public class PlayerModule : AModule<PlayerData, PlayerController>
	{
		public override ModuleType ModuleType => ModuleType.Player;

		public override void RegisterViewConfig()
		{
			RegisterView<UIPlayerInfo, OnCloseUIPlayerInfoEvent>(OnCloseUIPlayerInfoEvent.Create);
		}
	}
}
