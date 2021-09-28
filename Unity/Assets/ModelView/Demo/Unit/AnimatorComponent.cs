using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    /// <summary>动画状态</summary>
    public enum MotionType
	{
        /// <summary>初始</summary>
        None,
        /// <summary>待机</summary>
        Idle,
        /// <summary>移动</summary>
        Run,
	}

    /// <summary>动作实体数据结构</summary>
    public class AnimatorComponent : Entity
	{
        /// <summary>动作列表</summary>
        public Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();
        /// <summary>动作参数列表</summary>
		public HashSet<string> Parameter = new HashSet<string>();

        /// <summary>动作状态</summary>
		public MotionType MotionType;
        /// <summary>速度</summary>
		public float MontionSpeed;
        /// <summary>是否停止</summary>
		public bool isStop;
        /// <summary>停止时的速度</summary>
		public float stopSpeed;
        /// <summary>动画</summary>
		public Animator Animator;
	}
}