using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ConfigLoader: IConfigLoader
    {
        /// <summary>获取Unity所有的Config数据</summary>
        public void GetAllConfigBytes(Dictionary<string, byte[]> output)
        {
            //  通过资源组件获取所有的Bundle配置
            Dictionary<string, UnityEngine.Object> keys = ResourcesComponent.Instance.GetBundleAll("config.unity3d");

            //  解析配置文本
            foreach (var kv in keys)
            {
                TextAsset v = kv.Value as TextAsset;
                string key = kv.Key;
                output[key] = v.bytes;
            }
        }

        /// <summary>
        /// 获取指定config配置
        /// </summary>
        /// <param name="configName">config名字</param>
        /// <returns></returns>
        public byte[] GetOneConfigBytes(string configName)
        {
            TextAsset v = ResourcesComponent.Instance.GetAsset("config.unity3d", configName) as TextAsset;
            return v.bytes;
        }
    }
}