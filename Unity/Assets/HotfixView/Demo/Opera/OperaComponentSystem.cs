using System;
using UnityEngine;

namespace ET
{
    /// <summary>操作组件初始化</summary>
    public class OperaComponentAwakeSystem : AwakeSystem<OperaComponent>
    {
        public override void Awake(OperaComponent self)
        {
            //  设置地图LayerMask
            self.mapMask = LayerMask.GetMask("Map");
        }
    }

    /// <summary>操作组件刷新</summary>
    public class OperaComponentUpdateSystem : UpdateSystem<OperaComponent>
    {
        public override void Update(OperaComponent self)
        {
            self.Update();
        }
    }

    /// <summary>操作组件逻辑类</summary>
    public static class OperaComponentSystem
    {
        public static void Update(this OperaComponent self)
        {
            //  鼠标右键
            if (Input.GetMouseButtonDown(1))
            {
                //  获取鼠标点击在主相机下的射线
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //  射线检测是否点击到MapLayer
                if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
                {
                    //  设置点击信息
                    self.ClickPoint = hit.point;
                    self.frameClickMap.X = self.ClickPoint.x;
                    self.frameClickMap.Y = self.ClickPoint.y;
                    self.frameClickMap.Z = self.ClickPoint.z;
                    //  从主域Domain获取会话组件，发送点击消息
                    self.DomainScene().GetComponent<SessionComponent>().Session.Send(self.frameClickMap);
                }
            }
        }
    }
}