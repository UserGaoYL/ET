
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    /// <summary>登录界面Awake</summary>
    public class UILoginComponentAwakeSystem : AwakeSystem<UILoginComponent>
	{
		public override void Awake(UILoginComponent self)
		{
			//	获取相对应的UI控件
			ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			self.loginBtn = rc.Get<GameObject>("LoginBtn");
			//	绑定登录按钮点击事件
			self.loginBtn.GetComponent<Button>().onClick.AddListener(self.OnLogin);
			self.account = rc.Get<GameObject>("Account");
			self.password = rc.Get<GameObject>("Password");
		}
	}

    /// <summary>登录界面逻辑</summary>
    public static class UILoginComponentSystem
	{
        /// <summary>点击登录</summary>
        public static void OnLogin(this UILoginComponent self)
		{
			//	登录传递参数:主域，IP地址，账号，密码
			LoginHelper.Login(
				self.DomainScene(), 
				ConstValue.LoginAddress, 
				self.account.GetComponent<InputField>().text, 
				self.password.GetComponent<InputField>().text).Coroutine();
		}
	}
}
