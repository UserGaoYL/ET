namespace ET
{
    /// <summary>UI�������ڷ���</summary>
    public abstract class AUIEvent
    {
        /// <summary>UI������ʱ���÷���</summary>
        public abstract ETTask<UI> OnCreate(UIComponent uiComponent);
        /// <summary>UI���Ƴ�ʱ���÷���</summary>
        public abstract void OnRemove(UIComponent uiComponent);
    }
}