namespace ET
{
    /// <summary>UIEvent属性，参数为UI类型，用于统一调用UI生命周期函数</summary>
    public class UIEventAttribute: BaseAttribute
    {
        /// <summary>UI类型（UI枚举）</summary>
        public string UIType { get; }

        public UIEventAttribute(string uiType)
        {
            this.UIType = uiType;
        }
    }
}