namespace ET
{
    /// <summary>配置组件初始化</summary>
    public class ConfigComponent_SetConfigLoader_Awake: AwakeSystem<ConfigComponent>
    {
        public override void Awake(ConfigComponent self)
        {
            //  创建配置加载器
            self.ConfigLoader = new ConfigLoader();
        }
    }
}