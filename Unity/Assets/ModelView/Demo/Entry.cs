using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ET
{
    /// <summary>客户端入口</summary>
    public class Entry : IEntry
	{
		public void Start()
		{
			try
			{
				//	逻辑层和表现层分离
				//	四个程序集Model(Model.dll、MoveView.dll)和热更新Hotfix(Hotfix.dll、HotfixView.dll)
				string[] assemblyNames = { "Unity.Model.dll", "Unity.Hotfix.dll", "Unity.ModelView.dll", "Unity.HotfixView.dll" };
				
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					string assemblyName = $"{assembly.GetName().Name}.dll";
					if (!assemblyNames.Contains(assemblyName))
					{
						continue;
					}
					//	添加解析程序集
					Game.EventSystem.Add(assembly);
				}
				
				ProtobufHelper.Init();
				
				Game.Options = new Options();
				
				Game.EventSystem.Publish(new EventType.AppStart()).Coroutine();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public void Update()
		{
			ThreadSynchronizationContext.Instance.Update();
			Game.EventSystem.Update();
		}

		public void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		public void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}