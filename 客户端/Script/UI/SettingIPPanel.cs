using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingIPPanel : MonoBehaviour
{
    public InputField inputIP;
    public Text result;
    // Start is called before the first frame update
    void Start()
    {
        inputIP.text = ConnectSettings.IP + ":" + ConnectSettings.PORT;
        gameObject.SetActive(false);
    }
    public void SubmitIP()
    {
        string[] tmp= inputIP.text.Split(':');
        if (tmp.Length == 2 && tmp[0] != null && tmp[1] != null)
        {
            ConnectSettings.IP = tmp[0];
            int.TryParse(tmp[1], out ConnectSettings.PORT);
            result.text = GameObject.Find("GameFacade").GetComponent<GameFacade>().clientMng.Connect();
        }
        else
        {
            result.text = "IP地址格式错误,请注意格式";
        }        
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

}
