using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistePanel : MonoBehaviour
{
    public InputField account;
    public InputField password;
    public Text Result;
    private bool isRegistering = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Register()
    {
        if (account.text.Equals("") || password.text.Equals(""))
        {
            Result.text = "账号密码不得为空！";
            return;
        }
        if (!isRegistering)
        {
            Result.text = "正在进行注册中...";
            GetComponent<RegisterRequest>().SendRequest(account.text, password.text);
            isRegistering = true;
            StartCoroutine(RetryDelay());
        }        
    }
    IEnumerator RetryDelay()
    {
        yield return new WaitForSeconds(5f);
        isRegistering = false;
    }
}
