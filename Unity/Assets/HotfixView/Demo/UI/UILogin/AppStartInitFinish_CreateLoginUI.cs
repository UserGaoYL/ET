

namespace ET
{
    /// <summary>App启动结束，创建登录界面</summary>
    public class AppStartInitFinish_CreateLoginUI: AEvent<EventType.AppStartInitFinish>
	{
		protected override async ETTask Run(EventType.AppStartInitFinish args)
		{
			//	创建登录界面
			await UIHelper.Create(args.ZoneScene, UIType.UILogin);
		}
	}
}
