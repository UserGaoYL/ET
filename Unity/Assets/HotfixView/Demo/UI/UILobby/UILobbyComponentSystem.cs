using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    /// <summary>大厅界面Awake</summary>
    public class UILobbyComponentAwakeSystem : AwakeSystem<UILobbyComponent>
    {
        public override void Awake(UILobbyComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			//  获取相对应的控件
            self.enterMap = rc.Get<GameObject>("EnterMap");
            //  绑定进入地图按钮点击事件
            self.enterMap.GetComponent<Button>().onClick.AddListener(self.EnterMap);
            self.text = rc.Get<GameObject>("Text").GetComponent<Text>();
        }
    }

    /// <summary>大厅界面逻辑</summary>
    public static class UILobbyComponentSystem
    {
        /// <summary>进入地图</summary>
        public static void EnterMap(this UILobbyComponent self)
        {
            //  进入地图参数：Scene
            EnterMapHelper.EnterMapAsync(self.ZoneScene()).Coroutine();
        }
    }
}
