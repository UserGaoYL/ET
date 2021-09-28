using UnityEngine;

namespace ET
{
    //  UI组件Awake添加的组件，自定义Inspector显示
    public class ComponentView: MonoBehaviour
    {
        /// <summary>绑定的UI Entity</summary>
        public object Component
        {
            get;
            set;
        }
    }
}