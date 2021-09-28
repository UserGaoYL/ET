namespace ET
{
    public static class UIHelper
    {
        /// <summary>
        /// ��ָ����������UI
        /// </summary>
        /// <param name="scene">����</param>
        /// <param name="uiType">UI����</param>
        /// <returns>UI</returns>
        public static async ETTask<UI> Create(Scene scene, string uiType)
        {
            return await scene.GetComponent<UIComponent>().Create(uiType);
        }
        
        /// <summary>
        /// �Ƴ�ָ������UI
        /// </summary>
        /// <param name="scene">����</param>
        /// <param name="uiType">UI����</param>
        /// <returns></returns>
        public static async ETTask Remove(Scene scene, string uiType)
        {
            scene.GetComponent<UIComponent>().Remove(uiType);
            await ETTask.CompletedTask;
        }
    }
}