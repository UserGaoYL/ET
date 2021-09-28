namespace ET
{
    /// <summary>AppStart事件监听</summary>
    public class AppStart_Init: AEvent<EventType.AppStart>
    {
        /// <summary>App启动入口</summary>
        protected override async ETTask Run(EventType.AppStart args)
        {
            //  计时器组件
            Game.Scene.AddComponent<TimerComponent>();
            //  协程锁组件
            Game.Scene.AddComponent<CoroutineLockComponent>();

            //  加载配置
            Game.Scene.AddComponent<ResourcesComponent>();
            //  加载AB包
            ResourcesComponent.Instance.LoadBundle("config.unity3d");
            //  配置组件(加载所有ConfigAttribute属性的配置)
            Game.Scene.AddComponent<ConfigComponent>();
            await ConfigComponent.Instance.LoadAsync();
            //  卸载AB包
            ResourcesComponent.Instance.UnloadBundle("config.unity3d");
            //  操作码类型解析组件
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            //  消息分发组件
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            //  网络线程组件
            Game.Scene.AddComponent<NetThreadComponent>();
            //  区域场景管理组件
            Game.Scene.AddComponent<ZoneSceneManagerComponent>();
            //  全局组件
            Game.Scene.AddComponent<GlobalComponent>();
            //  AI分发组件
            Game.Scene.AddComponent<AIDispatcherComponent>();

            ResourcesComponent.Instance.LoadBundle("unit.unity3d");
            //  场景Game场景
            Scene zoneScene = await SceneFactory.CreateZoneScene(1, "Game", Game.Scene);
            //  发布app启动完成消息
            await Game.EventSystem.Publish(new EventType.AppStartInitFinish() { ZoneScene = zoneScene });
        }
    }
}
