using UnityEngine;

namespace ET
{
    /// <summary>场景切换实体数据结构</summary>
    public class SceneChangeComponent: Entity
    {
        /// <summary>加载地图异步操作</summary>
        public AsyncOperation loadMapOperation;
        /// <summary>ETTask</summary>
        public ETTask tcs;
    }
}