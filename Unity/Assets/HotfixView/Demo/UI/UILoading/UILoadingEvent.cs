using System;
using UnityEngine;

namespace ET
{
    /// <summary>Loading界面相关事件</summary>
    [UIEvent(UIType.UILoading)]
    public class UILoadingEvent: AUIEvent
    {
		/// <summary>
		/// 创建Loading界面
		/// </summary>
		/// <param name="uiComponent">父节点</param>
		/// <returns>LoadingUI</returns>
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
	        try
	        {
		        await ETTask.CompletedTask;
				//	Loading界面是首个界面，资源不在AB包，直接从Resources文件夹加载
				GameObject bundleGameObject = ((GameObject)Resources.Load("KV")).Get<GameObject>(UIType.UILoading);
				//	实例化LoadingUI，并设置为UI Layer
				GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
				go.layer = LayerMask.NameToLayer(LayerNames.UI);
				//	创建UI实体，父节点为uiComponent
				UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.UILoading, go);
				//	绑定UI组件
				ui.AddComponent<UILoadingComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}

        public override void OnRemove(UIComponent uiComponent)
        {
			//	因为不是AB包资源，不需要卸载
        }
    }
}