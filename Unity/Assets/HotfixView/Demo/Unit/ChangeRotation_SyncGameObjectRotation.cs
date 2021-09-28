using UnityEngine;

namespace ET
{
    /// <summary>对象旋转变化事件</summary>
    public class ChangeRotation_SyncGameObjectRotation: AEvent<EventType.ChangeRotation>
    {
        protected override async ETTask Run(EventType.ChangeRotation args)
        {
            //  获取单元身上的GameObject组件
            GameObjectComponent gameObjectComponent = args.Unit.GetComponent<GameObjectComponent>();
            if (gameObjectComponent == null)
            {
                return;
            }
            //  设置GameObject的tranform旋转信息
            Transform transform = gameObjectComponent.GameObject.transform;
            transform.rotation = args.Unit.Rotation;
            await ETTask.CompletedTask;
        }
    }
}