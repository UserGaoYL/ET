using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ConfigLoader: IConfigLoader
    {
        /// <summary>��ȡUnity���е�Config����</summary>
        public void GetAllConfigBytes(Dictionary<string, byte[]> output)
        {
            //  ͨ����Դ�����ȡ���е�Bundle����
            Dictionary<string, UnityEngine.Object> keys = ResourcesComponent.Instance.GetBundleAll("config.unity3d");

            //  ���������ı�
            foreach (var kv in keys)
            {
                TextAsset v = kv.Value as TextAsset;
                string key = kv.Key;
                output[key] = v.bytes;
            }
        }

        /// <summary>
        /// ��ȡָ��config����
        /// </summary>
        /// <param name="configName">config����</param>
        /// <returns></returns>
        public byte[] GetOneConfigBytes(string configName)
        {
            TextAsset v = ResourcesComponent.Instance.GetAsset("config.unity3d", configName) as TextAsset;
            return v.bytes;
        }
    }
}