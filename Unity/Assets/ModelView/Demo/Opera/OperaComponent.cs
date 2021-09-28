using System;

using UnityEngine;

namespace ET
{
    /// <summary>用户操作实体数据结构</summary>
	public class OperaComponent: Entity
    {
        /// <summary>点击坐标</summary>
        public Vector3 ClickPoint;

        /// <summary>地图对应的MaskLayer</summary>
	    public int mapMask;

        /// <summary>Client to Map 点击操作数据结构</summary>
	    public readonly C2M_PathfindingResult frameClickMap = new C2M_PathfindingResult();
    }
}
