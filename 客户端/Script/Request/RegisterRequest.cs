using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class RegisterRequest : BaseRequest
{
    private RegistePanel registePanel;
    // Use this for initialization
    private string result="";
    private bool ToResponeMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
        registePanel = GetComponent<RegistePanel>();
        base.Awake();
    }
    private void Update()
    {
        if (ToResponeMedthod)
        {
            ToResponeMedthod = false;
            ResponeMedthod();
        }
    }
    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("Registerata = " + data);
        int.TryParse(data, out int state);

        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            result = "注册成功！";           
        }
        if (returnCode == ReturnCode.Fail)
        {
            result = "该账号名已经存在！";
        }
        ToResponeMedthod = true;
    }
    public void ResponeMedthod()
    {
        registePanel.Result.text=result;
    }
}
