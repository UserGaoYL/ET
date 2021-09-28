namespace ET
{
    /// <summary>UI生命周期方法</summary>
    public abstract class AUIEvent
    {
        /// <summary>UI被创建时调用方法</summary>
        public abstract ETTask<UI> OnCreate(UIComponent uiComponent);
        /// <summary>UI被移除时调用方法</summary>
        public abstract void OnRemove(UIComponent uiComponent);
    }
}