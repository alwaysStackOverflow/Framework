using UnityGameFramework;
using GameFramework.Procedure;
using GameFramework.Localization;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Client
{
    public class AppStartProcedure : ProcedureBase
    {
		protected override void OnInit(ProcedureOwner procedureOwner)
		{
			base.OnInit(procedureOwner);
		}

		protected override void OnEnter(ProcedureOwner procedureOwner)
        {
			Log.Debug("游戏开始");
			ClientEntry.BuiltinData.InitBuildInfo();
			//InitLanguageSettings();
			//InitCurrentVariant();
			//ClientEntry.BuiltinData.InitDefaultDictionary();
			base.OnEnter(procedureOwner);
        }

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

			ChangeState<SplashProcedure>(procedureOwner);
		}

		protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
		{
			base.OnLeave(procedureOwner, isShutdown);
		}

		protected override void OnDestroy(ProcedureOwner procedureOwner)
		{
			base.OnDestroy(procedureOwner);
		}

		private void InitLanguageSettings()
		{
			if (ClientEntry.Base.EditorResourceMode && ClientEntry.Base.EditorLanguage != Language.Unspecified)
			{
				// 编辑器资源模式直接使用 Inspector 上设置的语言
				//ClientEntry.Setting.SetInt(Constant.Setting.Language, (int)ClientEntry.Base.EditorLanguage);
				ClientEntry.Setting.Save();
				return;
			}

			Language language = Language.English;// (Language)ClientEntry.Setting.GetInt(Constant.Setting.Language, (int)Language.English);

			if (language != Language.English
				&& language != Language.ChineseSimplified
				&& language != Language.ChineseTraditional)
			{
				// 若是暂不支持的语言，则使用英语
				language = Language.English;

				//ClientEntry.Setting.SetString(Constant.Setting.Language, language.ToString());
				ClientEntry.Setting.Save();
			}

			ClientEntry.Localization.Language = language;

			Log.Info("Init language settings complete, current language is '{0}'.", language.ToString());
		}

		private void InitCurrentVariant()
		{
			if (ClientEntry.Base.EditorResourceMode)
			{
				// 编辑器资源模式不使用 AssetBundle，也就没有变体了
				return;
			}

			string currentVariant = ClientEntry.Localization.Language switch
			{
				Language.English => "en-us",
				Language.ChineseSimplified => "zh-cn",
				Language.ChineseTraditional => "zh-tw",
				_ => "en-us",
			};
			ClientEntry.Resource.SetCurrentVariant(currentVariant);
			Log.Info("Init current variant complete.current variant :{0}.", currentVariant);
		}
	}
}
