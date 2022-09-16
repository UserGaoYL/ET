namespace ET
{
    public static class SceneHelper
    {
        public static int DomainZone(this Entity entity)
        {
            return ((Scene) entity.Domain)?.Zone ?? 0;
        }

        /// <summary>
        /// 获取Entity所在Scene
        /// </summary>
        /// <param name="entity">定位对象</param>
        /// <returns>所在Scene</returns>
        public static Scene DomainScene(this Entity entity)
        {
            return (Scene) entity.Domain;
        }
    }
}