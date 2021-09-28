
namespace ET
{
    /// <summary>登录结束</summary>
    public class LoginFinish_RemoveLoginUI: AEvent<EventType.LoginFinish>
	{
		protected override async ETTask Run(EventType.LoginFinish args)
		{
			//	移除登录界面
			await UIHelper.Remove(args.ZoneScene, UIType.UILogin);
		}
	}
}
