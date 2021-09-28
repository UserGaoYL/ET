using UnityEngine;

namespace ET
{
    /// <summary>实体单元创建完毕事件，添加渲染Object（动画）</summary>
    public class AfterUnitCreate_CreateUnitView: AEvent<EventType.AfterUnitCreate>
    {
        protected override async ETTask Run(EventType.AfterUnitCreate args)
        {
            // Unit View层
            // 这里可以改成异步加载，demo就不搞了
            GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset("Unit.unity3d", "Unit");
            GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");
	        
            //  实例化动作对象
            GameObject go = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            go.transform.position = args.Unit.Position;
            //  绑定GameObject组件，并赋值GameObject
            args.Unit.AddComponent<GameObjectComponent>().GameObject = go;
            //  添加动作组件
            args.Unit.AddComponent<AnimatorComponent>();
            await ETTask.CompletedTask;
        }
    }
}