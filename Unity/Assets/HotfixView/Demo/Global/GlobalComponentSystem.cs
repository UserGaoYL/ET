using UnityEngine;

namespace ET
{
    /// <summary>全局GameObject组件Awake</summary>
    public class GlobalComponentAwakeSystem: AwakeSystem<GlobalComponent>
    {
        public override void Awake(GlobalComponent self)
        {
            //  初始化单例
            GlobalComponent.Instance = self;
            
            //  赋值Global
            self.Global = GameObject.Find("/Global").transform;
            //  赋值Unit根节点
            self.Unit = GameObject.Find("/Global/Unit").transform;
            //  赋值UI根据点
            self.UI = GameObject.Find("/Global/UI").transform;
        }
    }
}