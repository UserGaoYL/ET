using System;
using UnityEngine;

namespace ET
{
	public static class GameObjectHelper
	{
		/// <summary>
		/// GameObject扩展方法，根据Key获取泛型T
		/// </summary>
		/// <typeparam name="T">获得对象类型</typeparam>
		/// <param name="gameObject">GameObject</param>
		/// <param name="key">要获取的对象名字</param>
		/// <returns>T</returns>
		public static T Get<T>(this GameObject gameObject, string key) where T : class
		{
			try
			{
				//	借助ReferenceCollector获取T
				return gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
			}
			catch (Exception e)
			{
				throw new Exception($"获取{gameObject.name}的ReferenceCollector key失败, key: {key}", e);
			}
		}
	}
}