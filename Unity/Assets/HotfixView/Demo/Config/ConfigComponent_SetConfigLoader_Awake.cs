namespace ET
{
    /// <summary>���������ʼ��</summary>
    public class ConfigComponent_SetConfigLoader_Awake: AwakeSystem<ConfigComponent>
    {
        public override void Awake(ConfigComponent self)
        {
            //  �������ü�����
            self.ConfigLoader = new ConfigLoader();
        }
    }
}