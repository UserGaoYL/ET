using UnityEngine;

namespace ET
{
    /// <summary>登录界面相关事件</summary>
    [UIEvent(UIType.UILogin)]
    public class UILoginEvent: AUIEvent
    {
        /// <summary>创建登录界面</summary>
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            //  登录界面，是可热更新的AB包资源，需要从资源组件加载
            await ResourcesComponent.Instance.LoadBundleAsync(UIType.UILogin.StringToAB());
            //  加载完毕获取对应资源的Object
            GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset(UIType.UILogin.StringToAB(), UIType.UILogin);
            //  实例化对象
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            //  创建UI实体，父节点为uiComponent
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.UILogin, gameObject);
            //  绑定UI组件
            ui.AddComponent<UILoginComponent>();
            return ui;
        }

        /// <summary>界面移除</summary>
        public override void OnRemove(UIComponent uiComponent)
        {
            //  需要卸载对应的AB包资源
            ResourcesComponent.Instance.UnloadBundle(UIType.UILogin.StringToAB());
        }
    }
}