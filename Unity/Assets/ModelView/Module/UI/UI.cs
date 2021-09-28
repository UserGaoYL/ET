using System.Collections.Generic;

using UnityEngine;

namespace ET
{
    /// <summary>UI Awake方法</summary>
    public class UIAwakeSystem : AwakeSystem<UI, string, GameObject>
	{
		/// <summary>
		/// 界面Awake
		/// </summary>
		/// <param name="self">UI</param>
		/// <param name="name">界面名字(UIType)</param>
		/// <param name="gameObject">UI预制体</param>
		public override void Awake(UI self, string name, GameObject gameObject)
		{
			self.Awake(name, gameObject);
		}
	}

    /// <summary>UI实体</summary>
    public sealed class UI: Entity
	{
        /// <summary>UI关联的Unity GameObject预制体</summary>
        public GameObject GameObject;

        /// <summary>UI名字</summary>
        public string Name { get; private set; }

        /// <summary>UI子节点</summary>
        public Dictionary<string, UI> nameChildren = new Dictionary<string, UI>();
		
		public void Awake(string name, GameObject gameObject)
		{
			this.nameChildren.Clear();
			//	绑定ComponetView组件，用于自定义显示Inspector
			gameObject.AddComponent<ComponentView>().Component = this;
			//	设置UI预制体所在层为UI层
			gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
			this.Name = name;
			this.GameObject = gameObject;
		}

        /// <summary>销毁方法</summary>
        public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			//	子节点销毁
			foreach (UI ui in this.nameChildren.Values)
			{
				ui.Dispose();
			}
			
			//	销毁UI绑定的GameObject
			UnityEngine.Object.Destroy(this.GameObject);
			this.nameChildren.Clear();
		}

        /// <summary>设置UI到最高层</summary>
        public void SetAsFirstSibling()
		{
			this.GameObject.transform.SetAsFirstSibling();
		}

        /// <summary>添加子节点</summary>
        public void Add(UI ui)
		{
			this.nameChildren.Add(ui.Name, ui);
			ui.Parent = this;
		}

		/// <summary>
		/// 移除子节点
		/// </summary>
		/// <param name="name">子节点名字</param>
		public void Remove(string name)
		{
			UI ui;
			if (!this.nameChildren.TryGetValue(name, out ui))
			{
				return;
			}
			this.nameChildren.Remove(name);
			ui.Dispose();
		}

		/// <summary>
		/// 获取节点
		/// </summary>
		/// <param name="name">节点名字</param>
		/// <returns></returns>
		public UI Get(string name)
		{
			UI child;
			//	优先检测UI子节点
			if (this.nameChildren.TryGetValue(name, out child))
			{
				return child;
			}

			//	如果没有子节点，再找gameObject子节点
			GameObject childGameObject = this.GameObject.transform.Find(name)?.gameObject;
			if (childGameObject == null)
			{
				return null;
			}

			//	如果没有，则创建UI节点，并添加到自身UI
			child = EntityFactory.Create<UI, string, GameObject>(this.Domain, name, childGameObject);
			this.Add(child);
			return child;
		}
	}
}