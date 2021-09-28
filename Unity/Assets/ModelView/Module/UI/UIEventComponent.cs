using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	/// <summary>
	/// 管理所有UI GameObject 生命周期、Layer
	/// </summary>
	public class UIEventComponent: Entity
	{
		public static UIEventComponent Instance;

        /// <summary>UI生命周期事件字典</summary>
        public Dictionary<string, AUIEvent> UIEvents = new Dictionary<string, AUIEvent>();
        /// <summary>UI不同层级父节点字典(UILayer Dictionary)</summary>
        public Dictionary<int, Transform> UILayers = new Dictionary<int, Transform>();
	}
}