using UnityEngine;

namespace ET
{
    /// <summary>UI分层</summary>
    public enum UILayer
    {
        Hidden = 0,
        Low = 10,
        Mid = 20,
        High = 30,
    }

    /// <summary>UI层级脚本，用于UI预制体设置</summary>
    public class UILayerScript: MonoBehaviour
    {
        public UILayer UILayer;
    }
}