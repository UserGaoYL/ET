using UnityEngine;

namespace ET
{
    /// <summary>�ͻ��������� GameObject</summary>
    public class GlobalComponent: Entity
    {
        public static GlobalComponent Instance;

        /// <summary>���ڵ�</summary>
        public Transform Global;
        /// <summary>unit ���ڵ�</summary>
        public Transform Unit;
        /// <summary>ui ���ڵ�</summary>
        public Transform UI;
    }
}