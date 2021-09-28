using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    /// <summary>Loading界面Awake</summary>
    public class UiLoadingComponentAwakeSystem : AwakeSystem<UILoadingComponent>
    {
        public override void Awake(UILoadingComponent self)
        {
            //  获取提示文本
            self.text = self.GetParent<UI>().GameObject.Get<GameObject>("Text").GetComponent<Text>();
            //  启动进度条加载
            self.StartAsync().Coroutine();
        }
    }

    /// <summary>Loading界面逻辑</summary>
    public static class UiLoadingComponentSystem
    {
        public static async ETVoid StartAsync(this UILoadingComponent self)
        {
            long instanceId = self.InstanceId;
            while (true)
            {
                await TimerComponent.Instance.WaitAsync(1000);

                if (self.InstanceId != instanceId)
                {
                    return;
                }
            }
        }
    }

}