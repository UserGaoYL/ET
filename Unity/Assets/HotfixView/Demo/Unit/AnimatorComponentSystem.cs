using System;
using UnityEngine;

namespace ET
{
    /// <summary>动作组件Awake</summary>
    public class AnimatorComponentAwakeSystem : AwakeSystem<AnimatorComponent>
	{
		public override void Awake(AnimatorComponent self)
		{
			self.Awake();
		}
	}

    /// <summary>动作组件Update</summary>
    public class AnimatorComponentUpdateSystem : UpdateSystem<AnimatorComponent>
	{
		public override void Update(AnimatorComponent self)
		{
			self.Update();
		}
	}

    /// <summary>动作组件销毁</summary>
    public class AnimatorComponentDestroySystem : DestroySystem<AnimatorComponent>
	{
		public override void Destroy(AnimatorComponent self)
		{
			self.animationClips = null;
			self.Parameter = null;
			self.Animator = null;
		}
	}

    /// <summary>动作组件逻辑</summary>
    public static class AnimatorComponentSystem
	{
		public static void Awake(this AnimatorComponent self)
		{
			//	获取GameObject身上的Animator脚本
			Animator animator = self.Parent.GetComponent<GameObjectComponent>().GameObject.GetComponent<Animator>();

			if (animator == null)
			{
				return;
			}

			if (animator.runtimeAnimatorController == null)
			{
				return;
			}

			if (animator.runtimeAnimatorController.animationClips == null)
			{
				return;
			}
			//	解析并赋值相对应的动作数据
			self.Animator = animator;
			foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
			{
				self.animationClips[animationClip.name] = animationClip;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				self.Parameter.Add(animatorControllerParameter.name);
			}
		}

		public static void Update(this AnimatorComponent self)
		{
			if (self.isStop)
			{
				return;
			}

			if (self.MotionType == MotionType.None)
			{
				return;
			}

			try
			{
				//	动作速度
				self.Animator.SetFloat("MotionSpeed", self.MontionSpeed);
				//	动作状态
				self.Animator.SetTrigger(self.MotionType.ToString());

				self.MontionSpeed = 1;
				self.MotionType = MotionType.None;
			}
			catch (Exception ex)
			{
				throw new Exception($"动作播放失败: {self.MotionType}", ex);
			}
		}

		/// <summary>
		/// 监测是否包含字段
		/// </summary>
		/// <param name="self">动作组件</param>
		/// <param name="parameter">要获取的字段名</param>
		/// <returns>是否存在</returns>
		public static bool HasParameter(this AnimatorComponent self, string parameter)
		{
			return self.Parameter.Contains(parameter);
		}

		/// <summary>
		/// 播放动作
		/// </summary>
		/// <param name="self">动作组件</param>
		/// <param name="motionType">动作状态</param>
		/// <param name="time">时间</param>
		public static void PlayInTime(this AnimatorComponent self, MotionType motionType, float time)
		{
			//	获取动作片段
			AnimationClip animationClip;
			if (!self.animationClips.TryGetValue(motionType.ToString(), out animationClip))
			{
				throw new Exception($"找不到该动作: {motionType}");
			}

			//	根据动作片段总时长和time，计算播放速度
			float motionSpeed = animationClip.length / time;
			if (motionSpeed < 0.01f || motionSpeed > 1000f)
			{
				Log.Error($"motionSpeed数值异常, {motionSpeed}, 此动作跳过");
				return;
			}
			self.MotionType = motionType;
			self.MontionSpeed = motionSpeed;
		}

		/// <summary>
		/// 播放动作
		/// </summary>
		/// <param name="self">动作组件</param>
		/// <param name="motionType">动作状态</param>
		/// <param name="motionSpeed">动作速度</param>
		public static void Play(this AnimatorComponent self, MotionType motionType, float motionSpeed = 1f)
		{
			if (!self.HasParameter(motionType.ToString()))
			{
				return;
			}
			self.MotionType = motionType;
			self.MontionSpeed = motionSpeed;
		}

		/// <summary>
		/// 获取动画时间
		/// </summary>
		/// <param name="self">动作组件</param>
		/// <param name="motionType">动作类型</param>
		/// <returns></returns>
		public static float AnimationTime(this AnimatorComponent self, MotionType motionType)
		{
			AnimationClip animationClip;
			if (!self.animationClips.TryGetValue(motionType.ToString(), out animationClip))
			{
				throw new Exception($"找不到该动作: {motionType}");
			}
			return animationClip.length;
		}

        /// <summary>暂停动作(设置播放速度为0)</summary>
        public static void PauseAnimator(this AnimatorComponent self)
		{
			if (self.isStop)
			{
				return;
			}
			self.isStop = true;

			if (self.Animator == null)
			{
				return;
			}
			self.stopSpeed = self.Animator.speed;
			self.Animator.speed = 0;
		}

        /// <summary>继续播放动作</summary>
        public static void RunAnimator(this AnimatorComponent self)
		{
			if (!self.isStop)
			{
				return;
			}

			self.isStop = false;

			if (self.Animator == null)
			{
				return;
			}
			//	以停止时的动作继续播放
			self.Animator.speed = self.stopSpeed;
		}

		public static void SetBoolValue(this AnimatorComponent self, string name, bool state)
		{
			if (!self.HasParameter(name))
			{
				return;
			}

			self.Animator.SetBool(name, state);
		}

		public static void SetFloatValue(this AnimatorComponent self, string name, float state)
		{
			if (!self.HasParameter(name))
			{
				return;
			}

			self.Animator.SetFloat(name, state);
		}

		public static void SetIntValue(this AnimatorComponent self, string name, int value)
		{
			if (!self.HasParameter(name))
			{
				return;
			}

			self.Animator.SetInteger(name, value);
		}

		public static void SetTrigger(this AnimatorComponent self, string name)
		{
			if (!self.HasParameter(name))
			{
				return;
			}

			self.Animator.SetTrigger(name);
		}

		public static void SetAnimatorSpeed(this AnimatorComponent self, float speed)
		{
			self.stopSpeed = self.Animator.speed;
			self.Animator.speed = speed;
		}

		public static void ResetAnimatorSpeed(this AnimatorComponent self)
		{
			self.Animator.speed = self.stopSpeed;
		}
	}
}