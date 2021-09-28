using System;
using UnityEngine;

namespace ET
{
    /// <summary>���������ʼ��</summary>
    public class OperaComponentAwakeSystem : AwakeSystem<OperaComponent>
    {
        public override void Awake(OperaComponent self)
        {
            //  ���õ�ͼLayerMask
            self.mapMask = LayerMask.GetMask("Map");
        }
    }

    /// <summary>�������ˢ��</summary>
    public class OperaComponentUpdateSystem : UpdateSystem<OperaComponent>
    {
        public override void Update(OperaComponent self)
        {
            self.Update();
        }
    }

    /// <summary>��������߼���</summary>
    public static class OperaComponentSystem
    {
        public static void Update(this OperaComponent self)
        {
            //  ����Ҽ�
            if (Input.GetMouseButtonDown(1))
            {
                //  ��ȡ�������������µ�����
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //  ���߼���Ƿ�����MapLayer
                if (Physics.Raycast(ray, out hit, 1000, self.mapMask))
                {
                    //  ���õ����Ϣ
                    self.ClickPoint = hit.point;
                    self.frameClickMap.X = self.ClickPoint.x;
                    self.frameClickMap.Y = self.ClickPoint.y;
                    self.frameClickMap.Z = self.ClickPoint.z;
                    //  ������Domain��ȡ�Ự��������͵����Ϣ
                    self.DomainScene().GetComponent<SessionComponent>().Session.Send(self.frameClickMap);
                }
            }
        }
    }
}