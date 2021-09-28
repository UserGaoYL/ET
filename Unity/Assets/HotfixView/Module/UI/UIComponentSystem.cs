namespace ET
{
    /// <summary>UIComponentAwake事件(UIManager Awake)</summary>
    public class UIComponentAwakeSystem : AwakeSystem<UIComponent>
	{
		public override void Awake(UIComponent self)
		{
		}
	}
	
	/// <summary>
	/// 管理Scene上的UI(UIManager 逻辑)
	/// </summary>
	public static class UIComponentSystem
	{
		/// <summary>
		/// 创建UI
		/// </summary>
		/// <param name="self">UI管理自身</param>
		/// <param name="uiType">UI类型</param>
		/// <returns>UI</returns>
		public static async ETTask<UI> Create(this UIComponent self, string uiType)
		{
			UI ui = await UIEventComponent.Instance.OnCreate(self, uiType);
			self.UIs.Add(uiType, ui);
			return ui;
		}

		/// <summary>
		/// 移除UI
		/// </summary>
		/// <param name="self">UI管理自身</param>
		/// <param name="uiType">UI类型</param>
		public static void Remove(this UIComponent self, string uiType)
		{
			if (!self.UIs.TryGetValue(uiType, out UI ui))
			{
				return;
			}
			//	调用UI生命周期OnRemove
			UIEventComponent.Instance.OnRemove(self, uiType);
			
			self.UIs.Remove(uiType);
			ui.Dispose();
		}

		/// <summary>
		/// 获得UI
		/// </summary>
		/// <param name="self">UI管理自身</param>
		/// <param name="name">UI类型</param>
		/// <returns>UI</returns>
		public static UI Get(this UIComponent self, string name)
		{
			UI ui = null;
			self.UIs.TryGetValue(name, out ui);
			return ui;
		}
	}
}