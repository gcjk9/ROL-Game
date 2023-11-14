using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;

public class LoginRequest : BaseRequest {
    private LoginPanel loginPanel;
    private UserData ud;
    // Use this for initialization
    private string result = "";
    private bool LoginFinishMedthod = false;//异步处理中控制主线程方法执行
    private bool LoginErrorMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }
    private void Update()
    {
        if (LoginFinishMedthod)
        {
            LoginFinishMedthod = false;
            LoginFinish();            
        }
        if (LoginErrorMedthod)
        {
            LoginErrorMedthod = false;
            LoginError();
        }
    }
    public void SendRequest(string username, string password)
    {
        string data = username + "," + password;
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("data=" + data);
        JObject JData = JObject.Parse(data);
        int.TryParse(JData.GetValue("state").ToString(), out int state);       


        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            int.TryParse(JData.GetValue("id").ToString(), out int id);
            string username = JData.GetValue("username").ToString();
            string dressup= JData.GetValue("dressup").ToString();

            UserData ud = new UserData(id,username);
            ud.SetDressUp(new DressUpData(dressup));

            facade.SetUserData(ud);
            this.ud = ud;

            LoginFinishMedthod = true;    
        }
        if (returnCode == ReturnCode.NotFound)
        {
            result = JData.GetValue("return").ToString();
            LoginErrorMedthod = true;
        }
    }
    public void LoginFinish()
    {        
        loginPanel.roomPanel.gameObject.SetActive(true);
        loginPanel.roomPanel.gameObject.GetComponent<RoomPanel>().Init(ud);
    }
    public void LoginError()
    {
        loginPanel.result.text = result;
    }

}
