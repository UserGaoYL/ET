namespace ET
{
    /// <summary>UIEvent���ԣ�����ΪUI���ͣ�����ͳһ����UI�������ں���</summary>
    public class UIEventAttribute: BaseAttribute
    {
        /// <summary>UI���ͣ�UIö�٣�</summary>
        public string UIType { get; }

        public UIEventAttribute(string uiType)
        {
            this.UIType = uiType;
        }
    }
}