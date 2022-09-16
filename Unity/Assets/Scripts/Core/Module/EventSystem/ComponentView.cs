#if ENABLE_VIEW && UNITY_EDITOR
using UnityEngine;

namespace ET
{
    /// <summary>
    /// Editor模式下，用于显示Entity信息
    /// </summary>
    public class ComponentView: MonoBehaviour
    {
        public Entity Component
        {
            get;
            set;
        }
    }
}
#endif