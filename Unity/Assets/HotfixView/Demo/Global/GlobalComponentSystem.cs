using UnityEngine;

namespace ET
{
    /// <summary>ȫ��GameObject���Awake</summary>
    public class GlobalComponentAwakeSystem: AwakeSystem<GlobalComponent>
    {
        public override void Awake(GlobalComponent self)
        {
            //  ��ʼ������
            GlobalComponent.Instance = self;
            
            //  ��ֵGlobal
            self.Global = GameObject.Find("/Global").transform;
            //  ��ֵUnit���ڵ�
            self.Unit = GameObject.Find("/Global/Unit").transform;
            //  ��ֵUI���ݵ�
            self.UI = GameObject.Find("/Global/UI").transform;
        }
    }
}