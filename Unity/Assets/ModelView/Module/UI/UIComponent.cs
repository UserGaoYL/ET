using System.Collections.Generic;

namespace ET
{
	/// <summary>
	/// 管理Scene上的UI(UIManager 数据)
	/// </summary>
	public class UIComponent: Entity
	{
        /// <summary>所有打开的UI Entity</summary>
        public Dictionary<string, UI> UIs = new Dictionary<string, UI>();
	}
}