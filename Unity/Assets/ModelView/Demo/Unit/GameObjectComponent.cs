using UnityEngine;

namespace ET
{
    /// <summary>Unit实体关联的GameObject数据结构</summary>
    public class GameObjectComponent: Entity
    {
        /// <summary>Unit GameObject(用于获取动画和其他组件信息)</summary>
        public GameObject GameObject;
    }
}