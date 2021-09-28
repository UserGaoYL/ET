namespace ET
{
    /// <summary>Loading结束事件</summary>
    public class LoadingFinishEvent_RemoveLoadingUI : AEvent<EventType.LoadingFinish>
    {
        protected override async ETTask Run(EventType.LoadingFinish args)
        {
            //  移除Loading界面
            await UIHelper.Remove(args.Scene, UIType.UILoading);
        }
    }
}
