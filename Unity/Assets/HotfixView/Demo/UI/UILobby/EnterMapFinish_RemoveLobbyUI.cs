namespace ET
{
    /// <summary>进入地图结束，移除大厅界面事件</summary>
    public class EnterMapFinish_RemoveLobbyUI: AEvent<EventType.EnterMapFinish>
	{
		protected override async ETTask Run(EventType.EnterMapFinish args)
		{
			//	加载场景资源
			await ResourcesComponent.Instance.LoadBundleAsync("map.unity3d");
			//	切换到map场景
			using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
			{
				await sceneChangeComponent.ChangeSceneAsync("Map");
			}
			//	添加操作组件
            args.ZoneScene.AddComponent<OperaComponent>();
			//	最后移除大厅界面
            await UIHelper.Remove(args.ZoneScene, UIType.UILobby);
		}
	}
}
