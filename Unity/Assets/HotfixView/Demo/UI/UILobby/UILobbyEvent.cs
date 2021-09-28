using UnityEngine;

namespace ET
{
    /// <summary>大厅界面相关事件</summary>
    [UIEvent(UIType.UILobby)]
    public class UILobbyEvent: AUIEvent
    {
        /// <summary>创建大厅界面</summary>
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            await ETTask.CompletedTask;
            //  这里用的同步加载AB包资源
            ResourcesComponent.Instance.LoadBundle(UIType.UILobby.StringToAB());
            //  加载完毕获取对应资源的Object
            GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset(UIType.UILobby.StringToAB(), UIType.UILobby);
            //  实例化对象
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            //  创建UI实体，父节点为uiComponent
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.UILobby, gameObject);
            //  绑定UI组件
            ui.AddComponent<UILobbyComponent>();
            return ui;
        }

        /// <summary>移除大厅界面</summary>
        public override void OnRemove(UIComponent uiComponent)
        {
            //  卸载大厅界面的AB资源
            ResourcesComponent.Instance.UnloadBundle(UIType.UILobby.StringToAB());
        }
    }
}