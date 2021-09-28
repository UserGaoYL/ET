/*

增加一个ResourcesLoaderComponent类，可以非常精细的控制资源的生命周期，用处举例

1.  UI加载的ab包希望UI释放后将ab包释放，那么可以在UI上挂一个ResourcesLoaderComponent，
    该UI的资源都使用这个ResourcesLoaderComponent加载。ui释放的时候ResourcesLoaderComponent也释放了

2.  UI加载的资源希望跟随场景释放，那么加载该UI的资源可以使用场景的ResourcesLoaderComponent去加载资源

3.  UI的资源希望永远不释放，可以在ZoneScene上挂载一个ResourcesLoaderComponent

原则，永远不要直接使用ResourcesComponent去加载资源，而是使用ResourcesLoaderComponent，这样ab包资源将自带生命周期
获取ab包中的Object仍然使用ResourcesComponent来获取，ResourcesLoaderComponent仅仅是用来加载跟释放资源的
 
 */

using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class ResourcesLoaderComponentDestroySystem: DestroySystem<ResourcesLoaderComponent>
    {
        /// <summary>释放资源</summary>
        public override void Destroy(ResourcesLoaderComponent self)
        {
            async ETTask UnLoadAsync()
            {
                using ListComponent<string> list = ListComponent<string>.Create();
            
                list.List.AddRange(self.LoadedResource);
                self.LoadedResource = null;
            
                // 延迟5秒卸载包，因为包卸载是引用技术，5秒之内假如重新有逻辑加载了这个包，那么可以避免一次卸载跟加载
                await TimerComponent.Instance.WaitAsync(5000);
                
                foreach (string abName in list.List)
                {
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.ResourcesLoader, abName.GetHashCode(), 0))
                    {
                        if (ResourcesComponent.Instance == null)
                        {
                            return;
                        }
                        await ResourcesComponent.Instance.UnloadBundleAsync(abName);
                    }
                }
            }
            
            
            UnLoadAsync().Coroutine();
        }
    }

    /// <summary>用来加载和释放资源</summary>
    public class ResourcesLoaderComponent: Entity
    {
        public HashSet<string> LoadedResource = new HashSet<string>();

        public async ETTask LoadAsync(string ab)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.ResourcesLoader, ab.GetHashCode(), 0))
            {
                if (this.IsDisposed)
                {
                    Log.Error($"resourceload already disposed {ab}");
                    return;
                }
                
                if (this.LoadedResource.Contains(ab))
                {
                    return;
                }
                
                LoadedResource.Add(ab);
                await ResourcesComponent.Instance.LoadBundleAsync(ab);
            }
        }
    }
}