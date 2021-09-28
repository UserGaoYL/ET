namespace ET
{
    /// <summary>���򴴽�����¼�</summary>
    public class AfterCreateZoneScene_AddComponent: AEvent<EventType.AfterCreateZoneScene>
    {
        protected override async ETTask Run(EventType.AfterCreateZoneScene args)
        {
            //  ���UI�¼������UI����
            Scene zoneScene = args.ZoneScene;
            zoneScene.AddComponent<UIEventComponent>();
            zoneScene.AddComponent<UIComponent>();
            await ETTask.CompletedTask;
        }
    }
}