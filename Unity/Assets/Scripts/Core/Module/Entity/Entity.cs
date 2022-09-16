using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{

    ///<summary>Entity类型</summary>
    [Flags]
    public enum EntityStatus: byte
    {
        None = 0,
        ///<summary>来自对象池</summary>
        IsFromPool = 1,
        ///<summary>需要注册到生命周期管理</summary>
        IsRegister = 1 << 1,
        ///<summary>是否是组件,只有在AddComponent时会内部设置(Component.Id与它的ComponentParent.Id一直)</summary>
        IsComponent = 1 << 2,
        ///<summary>只有在Create或设置Parent或者Domain时会内部设置</summary>
        IsCreated = 1 << 3,
        ///<summary>是否是新的对象，只有在创建Scene或者Entity.Create时会内部设置</summary>
        IsNew = 1 << 4,
    }

    public partial class Entity: DisposeObject
    {
#if ENABLE_VIEW && UNITY_EDITOR
        ///<summary>开启ENABLE_VIEW后,在IsRegister时，会创建GameObject用于显示Entity信息</summary>
        private UnityEngine.GameObject viewGO;
#endif

        #region InstanceId解释
        /*
         *  中国有很多城市（进程），城市中有很多人（Entity对象）居住，每个人（Entity）都有唯一的身份证号（Entity.Id）。
         *  一个人每到一个城市，都需要办理当地的居住证，分配到唯一的居住证号码（Entity.InstanceId），居住证号码的格式是：2字节市编号 + 4字节时间 + 2字节递增
         *  身份证号码是永远不变的，但是居住证号码每到一个城市都是变化的。
         * 
         */
        #endregion
        
        
        /// <summary>
        /// Entity在当前Scene中的实例Id,可能会发生变化,对象的Id是不变的
        /// 比如玩家从线路1换到线路2、或者从场景1进入到场景2，都会造成对象InstanceId变化
        /// </summary>
        [BsonIgnore]
        public long InstanceId
        {
            get;
            protected set;
        }

        protected Entity()
        {
        }

        [BsonIgnore]
        private EntityStatus status = EntityStatus.None;

        [BsonIgnore]
        private bool IsFromPool
        {
            get => (this.status & EntityStatus.IsFromPool) == EntityStatus.IsFromPool;
            set
            {
                if (value)
                {
                    this.status |= EntityStatus.IsFromPool;
                }
                else
                {
                    this.status &= ~EntityStatus.IsFromPool;
                }
            }
        }

        [BsonIgnore]
        protected bool IsRegister
        {
            get => (this.status & EntityStatus.IsRegister) == EntityStatus.IsRegister;
            set
            {
                if (this.IsRegister == value)
                {
                    return;
                }

                if (value)
                {
                    this.status |= EntityStatus.IsRegister;
                }
                else
                {
                    this.status &= ~EntityStatus.IsRegister;
                }

                //  注册到生命周期管理
                EventSystem.Instance.RegisterSystem(this, value);
                
#if ENABLE_VIEW && UNITY_EDITOR
                if (value)
                {
                    this.viewGO = new UnityEngine.GameObject(this.ViewName);
                    this.viewGO.AddComponent<ComponentView>().Component = this;
                    this.viewGO.transform.SetParent(this.Parent == null? 
                            UnityEngine.GameObject.Find("Global").transform : this.Parent.viewGO.transform);
                }
                else
                {
                    UnityEngine.Object.Destroy(this.viewGO);
                }
#endif
            }
        }

        ///<summary>Editor模式下显示名字</summary>
        protected virtual string ViewName
        {
            get
            {
                return this.GetType().Name;    
            }
        }

        [BsonIgnore]
        private bool IsComponent
        {
            get => (this.status & EntityStatus.IsComponent) == EntityStatus.IsComponent;
            set
            {
                if (value)
                {
                    this.status |= EntityStatus.IsComponent;
                }
                else
                {
                    this.status &= ~EntityStatus.IsComponent;
                }
            }
        }

        [BsonIgnore]
        protected bool IsCreated
        {
            get => (this.status & EntityStatus.IsCreated) == EntityStatus.IsCreated;
            set
            {
                if (value)
                {
                    this.status |= EntityStatus.IsCreated;
                }
                else
                {
                    this.status &= ~EntityStatus.IsCreated;
                }
            }
        }
        
        [BsonIgnore]
        protected bool IsNew
        {
            get => (this.status & EntityStatus.IsNew) == EntityStatus.IsNew;
            set
            {
                if (value)
                {
                    this.status |= EntityStatus.IsNew;
                }
                else
                {
                    this.status &= ~EntityStatus.IsNew;
                }
            }
        }

        ///<summary>销毁后实例ID会重置为0</summary>
        [BsonIgnore]
        public bool IsDisposed => this.InstanceId == 0;

        [BsonIgnore]
        protected Entity parent;

        /// <summary>
        /// 设置Entity父节点
        /// </summary>
        /// <exception cref="Exception">不允许设置为空</exception>
        /// <exception cref="Exception">不允许把自己设置为自己的父节点</exception>
        /// <exception cref="Exception">所设置的父节点Domain不允许为空</exception>
        [BsonIgnore]
        public Entity Parent
        {
            get => this.parent;
            private set
            {
                if (value == null)
                {
                    throw new Exception($"cant set parent null: {this.GetType().Name}");
                }
                
                if (value == this)
                {
                    throw new Exception($"cant set parent self: {this.GetType().Name}");
                }

                // 严格限制parent必须要有domain,也就是说parent必须在数据树上面
                if (value.Domain == null)
                {
                    throw new Exception($"cant set parent because parent domain is null: {this.GetType().Name} {value.GetType().Name}");
                }

                if (this.parent != null) // 之前有parent
                {
                    // parent相同，不设置
                    if (this.parent == value)
                    {
                        Log.Error($"重复设置了Parent: {this.GetType().Name} parent: {this.parent.GetType().Name}");
                        return;
                    }
                    this.parent.RemoveFromChildren(this);
                }
                
                this.parent = value;
                //  设置了Parent，作为子节点，就不是组件了
                this.IsComponent = false;
                this.parent.AddToChildren(this);
                this.Domain = this.parent.domain;
            }
        }

        // 该方法只能在AddComponent中调用，其他人不允许调用
        [BsonIgnore]
        private Entity ComponentParent
        {
            set
            {
                if (value == null)
                {
                    throw new Exception($"cant set parent null: {this.GetType().Name}");
                }
                
                if (value == this)
                {
                    throw new Exception($"cant set parent self: {this.GetType().Name}");
                }
                
                // 严格限制parent必须要有domain,也就是说parent必须在数据树上面
                if (value.Domain == null)
                {
                    throw new Exception($"cant set parent because parent domain is null: {this.GetType().Name} {value.GetType().Name}");
                }
                
                if (this.parent != null) // 之前有parent
                {
                    // parent相同，不设置
                    if (this.parent == value)
                    {
                        Log.Error($"重复设置了Parent: {this.GetType().Name} parent: {this.parent.GetType().Name}");
                        return;
                    }
                    this.parent.RemoveFromComponents(this);
                }

                this.parent = value;
                this.IsComponent = true;
                this.parent.AddToComponents(this);
                this.Domain = this.parent.domain;
            }
        }

        public T GetParent<T>() where T : Entity
        {
            return this.Parent as T;
        }

        /// <summary>
        /// 唯一Id，添加组件时，组件Id与当前Entity.Id一致
        /// </summary>
        [BsonIgnoreIfDefault]
        [BsonDefaultValue(0L)]
        [BsonElement]
        [BsonId]
        public long Id
        {
            get;
            set;
        }

        [BsonIgnore]
        protected Entity domain;

        /// <summary>
        /// 设置当前Entity所在domain，即所在Scene
        /// domain还有一个重要作用，就是设置domain时才会执行反序列化System，还有注册EventSystem
        /// 这样在写逻辑时，方便能拿到自己所在Scene上的数据
        /// </summary>
        /// <exception cref="Exception">domain设置不允许为空</exception>
        [BsonIgnore]
        public Entity Domain
        {
            get
            {
                return this.domain;
            }
            private set
            {
                if (value == null)
                {
                    throw new Exception($"domain cant set null: {this.GetType().Name}");
                }
                
                if (this.domain == value)
                {
                    return;
                }
                
                Entity preDomain = this.domain;
                this.domain = value;
                
                if (preDomain == null)
                {
                    this.InstanceId = IdGenerater.Instance.GenerateInstanceId();
                    this.IsRegister = true;
                    
                    // 反序列化出来的需要设置父子关系
                    if (this.componentsDB != null)
                    {
                        foreach (Entity component in this.componentsDB)
                        {
                            component.IsComponent = true;
                            this.Components.Add(component.GetType(), component);
                            component.parent = this;
                        }
                    }

                    if (this.childrenDB != null)
                    {
                        foreach (Entity child in this.childrenDB)
                        {
                            child.IsComponent = false;
                            this.Children.Add(child.Id, child);
                            child.parent = this;
                        }
                    }
                }

                // 递归设置孩子的Domain
                if (this.children != null)
                {
                    foreach (Entity entity in this.children.Values)
                    {
                        entity.Domain = this.domain;
                    }
                }

                if (this.components != null)
                {
                    foreach (Entity component in this.components.Values)
                    {
                        component.Domain = this.domain;
                    }
                }

                if (!this.IsCreated)
                {
                    this.IsCreated = true;
                    EventSystem.Instance.Deserialize(this);
                }
            }
        }

        [BsonElement("Children")]
        [BsonIgnoreIfNull]
        private HashSet<Entity> childrenDB;

        [BsonIgnore]
        private Dictionary<long, Entity> children;

        [BsonIgnore]
        public Dictionary<long, Entity> Children
        {
            get
            {
                if (this.children == null)
                {
                    this.children = ObjectPool.Instance.Fetch<Dictionary<long, Entity>>();
                }
                return this.children;
            }
        }

        private void AddToChildren(Entity entity)
        {
            this.Children.Add(entity.Id, entity);
            this.AddToChildrenDB(entity);
        }

        private void RemoveFromChildren(Entity entity)
        {
            if (this.children == null)
            {
                return;
            }

            this.children.Remove(entity.Id);

            if (this.children.Count == 0)
            {
                ObjectPool.Instance.Recycle(this.children);
                this.children = null;
            }

            this.RemoveFromChildrenDB(entity);
        }

        private void AddToChildrenDB(Entity entity)
        {
            if (!(entity is ISerializeToEntity))
            {
                return;
            }

            this.childrenDB = this.childrenDB ?? ObjectPool.Instance.Fetch<HashSet<Entity>>();

            this.childrenDB.Add(entity);
        }

        private void RemoveFromChildrenDB(Entity entity)
        {
            if (!(entity is ISerializeToEntity))
            {
                return;
            }

            if (this.childrenDB == null)
            {
                return;
            }

            this.childrenDB.Remove(entity);

            if (this.childrenDB.Count == 0 && this.IsNew)
            {
                ObjectPool.Instance.Recycle(this.childrenDB);
                this.childrenDB = null;
            }
        }

        [BsonElement("C")]
        [BsonIgnoreIfNull]
        private HashSet<Entity> componentsDB;

        [BsonIgnore]
        private Dictionary<Type, Entity> components;

        [BsonIgnore]
        public Dictionary<Type, Entity> Components
        {
            get
            {
                if (this.components == null)
                {
                    this.components = ObjectPool.Instance.Fetch<Dictionary<Type, Entity>>();
                }
                return this.components;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.IsRegister = false;
            this.InstanceId = 0;

            // 清理Component
            if (this.components != null)
            {
                foreach (KeyValuePair<Type, Entity> kv in this.components)
                {
                    kv.Value.Dispose();
                }

                this.components.Clear();
                ObjectPool.Instance.Recycle(this.components);
                this.components = null;

                // 创建的才需要回到池中,从db中不需要回收
                if (this.componentsDB != null)
                {
                    this.componentsDB.Clear();
                    if (this.IsNew)
                    {
                        ObjectPool.Instance.Recycle(this.componentsDB);
                        this.componentsDB = null;
                    }
                }
            }

            // 清理Children
            if (this.children != null)
            {
                foreach (Entity child in this.children.Values)
                {
                    child.Dispose();
                }

                this.children.Clear();
                ObjectPool.Instance.Recycle(this.children);
                this.children = null;

                if (this.childrenDB != null)
                {
                    this.childrenDB.Clear();
                    // 创建的才需要回到池中,从db中不需要回收
                    if (this.IsNew)
                    {
                        ObjectPool.Instance.Recycle(this.childrenDB);
                        this.childrenDB = null;
                    }
                }
            }

            // 触发Destroy事件
            if (this is IDestroy)
            {
                EventSystem.Instance.Destroy(this);
            }

            this.domain = null;

            if (this.parent != null && !this.parent.IsDisposed)
            {
                if (this.IsComponent)
                {
                    this.parent.RemoveComponent(this);
                }
                else
                {
                    this.parent.RemoveFromChildren(this);
                }
            }

            this.parent = null;

            base.Dispose();
            
            if (this.IsFromPool)
            {
                ObjectPool.Instance.Recycle(this);
            }
            status = EntityStatus.None;
        }

        private void AddToComponentsDB(Entity component)
        {
            if (!(component is ISerializeToEntity))
            {
                return;
            }
            
            if (this.componentsDB == null)
            {
                this.componentsDB = ObjectPool.Instance.Fetch<HashSet<Entity>>();
            }

            this.componentsDB.Add(component);
        }

        private void RemoveFromComponentsDB(Entity component)
        {
            if (!(component is ISerializeToEntity))
            {
                return;
            }
            
            if (this.componentsDB == null)
            {
                return;
            }

            this.componentsDB.Remove(component);
            if (this.componentsDB.Count == 0 && this.IsNew)
            {
                ObjectPool.Instance.Recycle(this.componentsDB);
                this.componentsDB = null;
            }
        }

        private void AddToComponents(Entity component)
        {
            this.Components.Add(component.GetType(), component);
            this.AddToComponentsDB(component);
        }

        private void RemoveFromComponents(Entity component)
        {
            if (this.components == null)
            {
                return;
            }

            this.components.Remove(component.GetType());

            if (this.components.Count == 0)
            {
                ObjectPool.Instance.Recycle(this.components);
                this.components = null;
            }

            this.RemoveFromComponentsDB(component);
        }

        public K GetChild<K>(long id) where K: Entity
        {
            if (this.children == null)
            {
                return null;
            }
            this.children.TryGetValue(id, out Entity child);
            return child as K;
        }
        
        public void RemoveChild(long id)
        {
            if (this.children == null)
            {
                return;
            }

            if (!this.children.TryGetValue(id, out Entity child))
            {
                return;
            }
            
            this.children.Remove(id);
            child.Dispose();
        }

        public void RemoveComponent<K>() where K : Entity
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.components == null)
            {
                return;
            }

            Type type = typeof (K);
            Entity c = this.GetComponent(type);
            if (c == null)
            {
                return;
            }

            this.RemoveFromComponents(c);
            c.Dispose();
        }

        public void RemoveComponent(Entity component)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.components == null)
            {
                return;
            }

            Entity c = this.GetComponent(component.GetType());
            if (c == null)
            {
                return;
            }

            if (c.InstanceId != component.InstanceId)
            {
                return;
            }

            this.RemoveFromComponents(c);
            c.Dispose();
        }

        public void RemoveComponent(Type type)
        {
            if (this.IsDisposed)
            {
                return;
            }

            Entity c = this.GetComponent(type);
            if (c == null)
            {
                return;
            }

            RemoveFromComponents(c);
            c.Dispose();
        }

        public K GetComponent<K>() where K : Entity
        {
            if (this.components == null)
            {
                return null;
            }

            Entity component;
            if (!this.components.TryGetValue(typeof (K), out component))
            {
                return default;
            }

            // 如果有IGetComponent接口，则触发GetComponentSystem
            if (this is IGetComponent)
            {
                EventSystem.Instance.GetComponent(this, component);
            }

            return (K) component;
        }

        public Entity GetComponent(Type type)
        {
            if (this.components == null)
            {
                return null;
            }

            Entity component;
            if (!this.components.TryGetValue(type, out component))
            {
                return null;
            }
            
            // 如果有IGetComponent接口，则触发GetComponentSystem
            if (this is IGetComponent)
            {
                EventSystem.Instance.GetComponent(this, component);
            }

            return component;
        }
        
        private static Entity Create(Type type, bool isFromPool)
        {
            Entity component;
            if (isFromPool)
            {
                component = (Entity)ObjectPool.Instance.Fetch(type);
            }
            else
            {
                component = Activator.CreateInstance(type) as Entity;
            }
            component.IsFromPool = isFromPool;
            component.IsCreated = true;
            component.IsNew = true;
            component.Id = 0;
            return component;
        }

        #region AddComponent Method

        public Entity AddComponent(Entity component)
        {
            Type type = component.GetType();
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            component.ComponentParent = this;

            if (this is IAddComponent)
            {
                EventSystem.Instance.AddComponent(this, component);
            }
            return component;
        }

        public Entity AddComponent(Type type, bool isFromPool = false)
        {
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            Entity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            EventSystem.Instance.Awake(component);
            
            if (this is IAddComponent)
            {
                EventSystem.Instance.AddComponent(this, component);
            }
            return component;
        }

        public K AddComponent<K>(bool isFromPool = false) where K : Entity, IAwake, new()
        {
            Type type = typeof (K);
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            Entity component = Create(type, isFromPool);
            
            //  这里类似Unity GameObject上挂载Component，实际上还是同一个GameObject，所以Id一致，只不过所添加的组件IsComponent为true
            component.Id = this.Id;
            component.ComponentParent = this;
            EventSystem.Instance.Awake(component);
            
            if (this is IAddComponent)
            {
                EventSystem.Instance.AddComponent(this, component);
            }
            return component as K;
        }

        public K AddComponent<K, P1>(P1 p1, bool isFromPool = false) where K : Entity, IAwake<P1>, new()
        {
            Type type = typeof (K);
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            Entity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            EventSystem.Instance.Awake(component, p1);
            
            if (this is IAddComponent)
            {
                EventSystem.Instance.AddComponent(this, component);
            }
            return component as K;
        }

        public K AddComponent<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = false) where K : Entity, IAwake<P1, P2>, new()
        {
            Type type = typeof (K);
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            Entity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            EventSystem.Instance.Awake(component, p1, p2);
            
            if (this is IAddComponent)
            {
                EventSystem.Instance.AddComponent(this, component);
            }
            return component as K;
        }

        public K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = false) where K : Entity, IAwake<P1, P2, P3>, new()
        {
            Type type = typeof (K);
            if (this.components != null && this.components.ContainsKey(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            Entity component = Create(type, isFromPool);
            component.Id = this.Id;
            component.ComponentParent = this;
            EventSystem.Instance.Awake(component, p1, p2, p3);
            
            if (this is IAddComponent)
            {
                EventSystem.Instance.AddComponent(this, component);
            }
            return component as K;
        }

        #endregion
        
        #region AddChild Method

        public Entity AddChild(Entity entity)
        {
            entity.Parent = this;
            return entity;
        }

        public T AddChild<T>(bool isFromPool = false) where T : Entity, IAwake
        {
            Type type = typeof (T);
            T component = (T) Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            component.Parent = this;

            EventSystem.Instance.Awake(component);
            return component;
        }

        public T AddChild<T, A>(A a, bool isFromPool = false) where T : Entity, IAwake<A>
        {
            Type type = typeof (T);
            T component = (T) Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            component.Parent = this;

            EventSystem.Instance.Awake(component, a);
            return component;
        }

        public T AddChild<T, A, B>(A a, B b, bool isFromPool = false) where T : Entity, IAwake<A, B>
        {
            Type type = typeof (T);
            T component = (T) Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            component.Parent = this;

            EventSystem.Instance.Awake(component, a, b);
            return component;
        }

        public T AddChild<T, A, B, C>(A a, B b, C c, bool isFromPool = false) where T : Entity, IAwake<A, B, C>
        {
            Type type = typeof (T);
            T component = (T) Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            component.Parent = this;

            EventSystem.Instance.Awake(component, a, b, c);
            return component;
        }

        public T AddChild<T, A, B, C, D>(A a, B b, C c, D d, bool isFromPool = false) where T : Entity, IAwake<A, B, C, D>
        {
            Type type = typeof (T);
            T component = (T) Entity.Create(type, isFromPool);
            component.Id = IdGenerater.Instance.GenerateId();
            component.Parent = this;

            EventSystem.Instance.Awake(component, a, b, c, d);
            return component;
        }

        public T AddChildWithId<T>(long id, bool isFromPool = false) where T : Entity, IAwake, new()
        {
            Type type = typeof (T);
            T component = Entity.Create(type, isFromPool) as T;
            component.Id = id;
            component.Parent = this;
            EventSystem.Instance.Awake(component);
            return component;
        }

        public T AddChildWithId<T, A>(long id, A a, bool isFromPool = false) where T : Entity, IAwake<A>
        {
            Type type = typeof (T);
            T component = (T) Entity.Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;

            EventSystem.Instance.Awake(component, a);
            return component;
        }

        public T AddChildWithId<T, A, B>(long id, A a, B b, bool isFromPool = false) where T : Entity, IAwake<A, B>
        {
            Type type = typeof (T);
            T component = (T) Entity.Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;

            EventSystem.Instance.Awake(component, a, b);
            return component;
        }

        public T AddChildWithId<T, A, B, C>(long id, A a, B b, C c, bool isFromPool = false) where T : Entity, IAwake<A, B, C>
        {
            Type type = typeof (T);
            T component = (T) Entity.Create(type, isFromPool);
            component.Id = id;
            component.Parent = this;

            EventSystem.Instance.Awake(component, a, b, c);
            return component;
        }
        
        #endregion
    }
}