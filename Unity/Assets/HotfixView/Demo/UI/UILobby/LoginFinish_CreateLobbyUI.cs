

namespace ET
{
    /// <summary>登录结束，创建大厅界面事件</summary>
    public class LoginFinish_CreateLobbyUI: AEvent<EventType.LoginFinish>
	{
		protected override async ETTask Run(EventType.LoginFinish args)
		{
			//	创建大厅界面
			await UIHelper.Create(args.ZoneScene, UIType.UILobby);
		}
	}
}
