using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>Debug显示路线</summary>
    public class GizmosDebug: MonoBehaviour
    {
        public static GizmosDebug Instance { get; private set; }

        //  寻路点列表
        public List<Vector3> Path;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            if (this.Path.Count < 2)
            {
                return;
            }
            for (int i = 0; i < Path.Count - 1; ++i)
            {
                //  绘制寻路点连线
                Gizmos.DrawLine(Path[i], Path[i + 1]);
            }
        }
    }
}