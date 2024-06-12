using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	public class MenuModule : AModule<MenuData, MenuController>
	{
		public override ModuleType ModuleType => ModuleType.Menu;

		public override void RegisterViewConfig()
		{
			RegisterView<UIMenu, OnCloseUIMenuEvent>(OnCloseUIMenuEvent.Create);
			RegisterView<UISavingsForm, OnCloseUISavingsFormEvent>(OnCloseUISavingsFormEvent.Create);
		}
	}
}
