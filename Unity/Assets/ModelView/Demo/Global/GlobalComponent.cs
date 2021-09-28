using UnityEngine;

namespace ET
{
    /// <summary>客户端入口组件 GameObject</summary>
    public class GlobalComponent: Entity
    {
        public static GlobalComponent Instance;

        /// <summary>根节点</summary>
        public Transform Global;
        /// <summary>unit 父节点</summary>
        public Transform Unit;
        /// <summary>ui 父节点</summary>
        public Transform UI;
    }
}