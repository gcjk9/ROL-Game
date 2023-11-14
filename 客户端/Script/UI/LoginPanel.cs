using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EventCode;

public class LoginPanel : MonoBehaviour
{
    public GameObject roomPanel;
    private LoginRequest loginRequest;
    public InputField account;
    public InputField password;
    public Text result;
    private string msg="";
    private bool isLogining=false;
    // Start is called before the first frame update
    void Start()
    {
        loginRequest = GetComponent<LoginRequest>();
        GameObject backToRoom = GameObject.Find("BackToRoom");
        if (backToRoom != null)
        {
            backToRoom.GetComponent<BackToRoom>().Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnLoginRquest()
    {
        if (string.IsNullOrEmpty(account.text))
        {
            result.text= "用户名不能为空 ";
            return;
        }
        if (string.IsNullOrEmpty(password.text))
        {
            result.text = "密码不能为空 ";
            return;
        }
        if (!isLogining)
        {
            result.text = "正在登陆中... ";
            loginRequest.SendRequest(account.text, password.text);
            isLogining = true;
            StartCoroutine(RetryDelay());
        }        
    }
    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            //uiMng.PushPanelSync(UIPanelType.RoomList);
        }
        else
        {
            result.text="用户名或密码错误，无法登录，请重新输入!!";
        }
    }
    /// <summary>
    /// 点击间隔，防止多次点击触发服务器重复登陆bug
    /// </summary>
    /// <returns></returns>
    IEnumerator RetryDelay()
    {
        yield return new WaitForSeconds(5f);
        isLogining = false;
    }
}
