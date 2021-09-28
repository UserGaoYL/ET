using System;
using UnityEngine;

namespace ET
{
    /// <summary>UI事件管理Awake</summary>
    public class UIEventComponentAwakeSystem : AwakeSystem<UIEventComponent>
	{
		public override void Awake(UIEventComponent self)
		{
			//	绑定单例
			UIEventComponent.Instance = self;
			
			GameObject uiRoot = GameObject.Find("/Global/UI");
			ReferenceCollector referenceCollector = uiRoot.GetComponent<ReferenceCollector>();
			
			//	添加各自类型的Layer节点
			self.UILayers.Add((int)UILayer.Hidden, referenceCollector.Get<GameObject>(UILayer.Hidden.ToString()).transform);
			self.UILayers.Add((int)UILayer.Low, referenceCollector.Get<GameObject>(UILayer.Low.ToString()).transform);
			self.UILayers.Add((int)UILayer.Mid, referenceCollector.Get<GameObject>(UILayer.Mid.ToString()).transform);
			self.UILayers.Add((int)UILayer.High, referenceCollector.Get<GameObject>(UILayer.High.ToString()).transform);

			//	获取当前程序集UIEvent属性的类
			var uiEvents = Game.EventSystem.GetTypes(typeof (UIEventAttribute));
			foreach (Type type in uiEvents)
			{
				object[] attrs = type.GetCustomAttributes(typeof(UIEventAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				//	key-UI类型属性
				UIEventAttribute uiEventAttribute = attrs[0] as UIEventAttribute;

				//	value-UI生命周期事件类
				AUIEvent aUIEvent = Activator.CreateInstance(type) as AUIEvent;

				//	key-value 添加到UIEvent管理
				self.UIEvents.Add(uiEventAttribute.UIType, aUIEvent);
			}
		}
	}
	
	/// <summary>
	/// 管理所有UI GameObject 以及UI事件
	/// </summary>
	public static class UIEventComponentSystem
	{
		/// <summary>
		/// UI 创建时
		/// </summary>
		/// <param name="self">UI事件管理自身</param>
		/// <param name="uiComponent">UI管理自身</param>
		/// <param name="uiType">UI类型</param>
		/// <returns>UI</returns>
		public static async ETTask<UI> OnCreate(this UIEventComponent self, UIComponent uiComponent, string uiType)
		{
			try
			{
				//	根据key-uiType获取到value-UIEvent，uiComponent作为UI的父节点，调用OnCreate
				UI ui = await self.UIEvents[uiType].OnCreate(uiComponent);
				//	获取到创建的UILayer
				UILayer uiLayer = ui.GameObject.GetComponent<UILayerScript>().UILayer;
				//	根据Layer获取到GameObject，并设置为UI GameObject的父节点
				ui.GameObject.transform.SetParent(self.UILayers[(int)uiLayer]);
				return ui;
			}
			catch (Exception e)
			{
				throw new Exception($"on create ui error: {uiType}", e);
			}
		}

		/// <summary>
		/// UI 移除时
		/// </summary>
		/// <param name="self">UI事件管理自身</param>
		/// <param name="uiComponent">UI管理自身</param>
		/// <param name="uiType">UI类型</param>
		public static void OnRemove(this UIEventComponent self, UIComponent uiComponent, string uiType)
		{
			try
			{
				//	调用对应的UIEvent的OnRemove
				self.UIEvents[uiType].OnRemove(uiComponent);
			}
			catch (Exception e)
			{
				throw new Exception($"on remove ui error: {uiType}", e);
			}
			
		}
	}
}