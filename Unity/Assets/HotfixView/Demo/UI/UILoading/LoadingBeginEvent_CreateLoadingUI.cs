using UnityEngine;

namespace ET
{
    /// <summary>开始加载Loading事件</summary>
    public class LoadingBeginEvent_CreateLoadingUI : AEvent<EventType.LoadingBegin>
    {
        protected override async ETTask Run(EventType.LoadingBegin args)
        {
            //  创建Loading界面
            await UIHelper.Create(args.Scene, UIType.UILoading);
        }
    }
}
