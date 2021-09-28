using UnityEngine.SceneManagement;

namespace ET
{
    /// <summary>场景切换Update事件</summary>
    public class SceneChangeComponentUpdateSystem: UpdateSystem<SceneChangeComponent>
    {
        public override void Update(SceneChangeComponent self)
        {
            //  操作还未完成
            if (!self.loadMapOperation.isDone)
            {
                return;
            }

            //  还没有启动Task
            if (self.tcs == null)
            {
                return;
            }
            
            //  操作完成
            ETTask tcs = self.tcs;
            self.tcs = null;
            tcs.SetResult();
        }
    }

    /// <summary>场景切换组件销毁</summary>
    public class SceneChangeComponentDestroySystem: DestroySystem<SceneChangeComponent>
    {
        public override void Destroy(SceneChangeComponent self)
        {
            self.loadMapOperation = null;
            self.tcs = null;
        }
    }

    /// <summary>场景切换逻辑</summary>
    public static class SceneChangeComponentSystem
    {
        /// <summary>
        /// 异步切换场景
        /// </summary>
        /// <param name="self">场景切换管理自身</param>
        /// <param name="sceneName">要切换的场景名字</param>
        /// <returns></returns>
        public static async ETTask ChangeSceneAsync(this SceneChangeComponent self, string sceneName)
        {
            self.tcs = ETTask.Create(true);
            //  异步加载map
            self.loadMapOperation = SceneManager.LoadSceneAsync(sceneName);
            //this.loadMapOperation.allowSceneActivation = false;
            await self.tcs;
        }

        /// <summary>获取加载进度</summary>
        public static int Process(this SceneChangeComponent self)
        {
            if (self.loadMapOperation == null)
            {
                return 0;
            }
            return (int)(self.loadMapOperation.progress * 100);
        }
    }
}