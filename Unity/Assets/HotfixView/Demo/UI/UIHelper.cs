namespace ET
{
    public static class UIHelper
    {
        /// <summary>
        /// 在指定场景创建UI
        /// </summary>
        /// <param name="scene">场景</param>
        /// <param name="uiType">UI类型</param>
        /// <returns>UI</returns>
        public static async ETTask<UI> Create(Scene scene, string uiType)
        {
            return await scene.GetComponent<UIComponent>().Create(uiType);
        }
        
        /// <summary>
        /// 移除指定场景UI
        /// </summary>
        /// <param name="scene">场景</param>
        /// <param name="uiType">UI类型</param>
        /// <returns></returns>
        public static async ETTask Remove(Scene scene, string uiType)
        {
            scene.GetComponent<UIComponent>().Remove(uiType);
            await ETTask.CompletedTask;
        }
    }
}