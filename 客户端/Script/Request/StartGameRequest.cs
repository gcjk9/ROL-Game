using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static EventCode;
using UnityEngine.SceneManagement;

public class StartGameRequest : BaseRequest
{
    private RoomPanel roomPanel;
    // Use this for initialization
    private string result = "";
    private JObject JData;
    private bool ToResponeMedthod = false;//异步处理中控制主线程方法执行
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        roomPanel = GetComponent<RoomPanel>();
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
    public void SendRequest()
    {
        string data = "";
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        Debug.Log("Registerata = " + data);
        string[] tmp = data.Split('|');
        int.TryParse(tmp[0], out int state);
        Debug.Log("1 - 0");
        ReturnCode returnCode = (ReturnCode)state;
        if (returnCode == ReturnCode.Success)
        {
            Debug.Log("1 - 1");
            JData = JObject.Parse(tmp[1]);
            ToResponeMedthod = true;
        }
        if (returnCode == ReturnCode.Fail)
        {
            
        }       
    }
    public void ResponeMedthod()
    {
        Debug.Log("1 - 2");
        int id=roomPanel.player.GetComponent<PlayerBase>().ud.Id;
        JData.TryGetValue(id.ToString(), out JToken birthpointId);
        roomPanel.player.GetComponent<PlayerBase>().ud.birthPointId = birthpointId.ToString();
        Debug.Log("1 - 2:"+ roomPanel.player.GetComponent<PlayerBase>().ud.birthPointId);

        foreach (GameObject oplayer in roomPanel.otherPlayers)
        {
            Debug.Log("1 - 3");
            int oid = oplayer.GetComponent<PlayerBase>().ud.Id;
            JData.TryGetValue(oid.ToString(), out JToken obirthpointId);
            oplayer.GetComponent<PlayerBase>().ud.birthPointId = obirthpointId.ToString();            
        }
        Debug.Log("1 - 4");
        DontDestroyOnLoad(GameObject.Find("GameRoot"));
        SceneManager.LoadScene("GameScene"+roomPanel.mapId);//切换游戏场景
    }
}