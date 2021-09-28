using UnityEngine;

namespace ET
{
    /// <summary>对象坐标变化事件</summary>
    public class ChangePosition_SyncGameObjectPos: AEvent<EventType.ChangePosition>
    {
        protected override async ETTask Run(EventType.ChangePosition args)
        {
            //  获取单元身上的GameObject组件
            GameObjectComponent gameObjectComponent = args.Unit.GetComponent<GameObjectComponent>();
            if (gameObjectComponent == null)
            {
                return;
            }
            //  设置GameObject的tranform坐标信息
            Transform transform = gameObjectComponent.GameObject.transform;
            transform.position = args.Unit.Position;
            await ETTask.CompletedTask;
        }
    }
}