using System;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace ET
{
    /// <summary>入口生命周期</summary>
    public interface IEntry
	{
        /// <summary>OnStart</summary>
        void Start();
        /// <summary>OnUpdate</summary>
        void Update();
        /// <summary>OnLateUpdate</summary>
        void LateUpdate();
		/// <summary>OnApplicationQuit</summary>
		void OnApplicationQuit();
	}

    /// <summary>项目初始化脚本</summary>
    public class Init: MonoBehaviour
	{
        /// <summary>总入口</summary>
        private IEntry entry;
		
		private void Awake()
		{
			//	同步上下文设置默认主线程
			SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
			
			DontDestroyOnLoad(gameObject);
			
			Assembly modelAssembly = null;

			//	AppDomain所有程序集	
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			//	遍历查找Unity.ModelView.dll
			foreach (Assembly assembly in assemblies)
			{
				string assemblyName = $"{assembly.GetName().Name}.dll";
				if (assemblyName != "Unity.ModelView.dll")
				{
					continue;
				}
				modelAssembly = assembly;
				break;
			}

			//	初始化Unity.ModelView.dll入口Entry
			Type initType = modelAssembly.GetType("ET.Entry");
			this.entry = Activator.CreateInstance(initType) as IEntry;
		}

		private void Start()
		{
			this.entry.Start();
		}

		private void Update()
		{
			this.entry.Update();
		}

		private void LateUpdate()
		{
			this.entry.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			this.entry.OnApplicationQuit();
		}
	}
}