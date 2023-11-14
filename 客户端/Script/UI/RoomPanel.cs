using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviour
{
    public UserData ud;
    public Camera MainCamera;
    public GameObject playerInstantiate;
    public Transform playersList;//所有玩家游戏对象都归于其子对象
    public GameObject StartGameBut;
    public GameObject SelectRoomBut;
    public GameObject LeaveGameBut;
    public GameObject QuitRoomBut;
    public GameObject dressUpPanel;
    public GameObject selectRoomPanel;
    public GameObject selectMapPanel;
    public Text roomId;
    public Text mapName;
    public GameObject player;
    public Transform points;
    public List<Transform> playerPoints = new List<Transform>();
    public List<GameObject> otherPlayers = new List<GameObject>();
    public int mapId=0;
    public int tmpMapId = 0;

    public void Init(UserData ud)
    {
        this.ud = ud;
        for (int i = 0; i < 6; i++)
        {
            playerPoints.Add(GameObject.Find("PlayerPoint:"+i).transform);
        }
        player = Instantiate(Resources.Load<GameObject>("Prefabs/Role/Player"), playerPoints[0].position, playerPoints[0].rotation);
        GameObject Name = Instantiate(Resources.Load<GameObject>("Prefabs/Role/PlayerName"), playerPoints[0].position, playerPoints[0].rotation);
        Name.transform.SetParent(player.transform);
        player.GetComponent<PlayerBase>().ud = ud;
        player.GetComponent<CPlayer>().isCanBeControl = false;
        Name.transform.Find("NamePanel").transform.Find("Canvas").Find("name").GetComponent<Text>().text = ud.Username;
        Name.transform.Find("NamePanel").Find("Canvas").Find("name").GetComponent<Text>().color = Color.cyan;

        dressUpPanel.SetActive(true);
        dressUpPanel.GetComponent<DressUpPanel>().player = player;
        dressUpPanel.GetComponent<DressUpPanel>().SetDressUp(ud.dressUpData.head + ":head");
        dressUpPanel.GetComponent<DressUpPanel>().SetDressUp(ud.dressUpData.face + ":face");
        dressUpPanel.GetComponent<DressUpPanel>().SetDressUp(ud.dressUpData.body + ":body");

        GameObject gr = GameObject.Find("NewGameRoot");
        Debug.Log("6-2-1");
        if (gr == null)
        {
            gr= GameObject.Find("GameRoot");
            Debug.Log("6-2-2");
        }
        gr.name = "GameRoot";
        Debug.Log("6-2-3");
        playersList = gr.transform.Find("PlayersList");
        player.transform.SetParent(playersList);
        player.name = "Player";
        MainCamera.GetComponent<CUICamera>().MoveTo(3);
        Debug.Log("6-2-4");
        dressUpPanel.SetActive(false);
    }
    public void UpdateRoom(string data)
    {
        JObject JData = JObject.Parse(data);
        int.TryParse(JData.GetValue("menberCount").ToString(), out int menberCount);
        ClearRoom();

        otherPlayers = new List<GameObject>();
        for(int i=0;i< menberCount; i++)
        {
            JObject JMenber = JObject.Parse(JData.GetValue("menber" + i).ToString());
            string id = JMenber.GetValue("id").ToString();
            string oname = JMenber.GetValue("name").ToString();
            int.TryParse(id, out int ID);
            JObject JDressUp = JObject.Parse(JMenber.GetValue("dressUp").ToString());
            if (id.Equals(ud.Id.ToString()))
            {
                continue;
            }
            GameObject oplayer = Instantiate(Resources.Load<GameObject>("Prefabs/Role/oPlayer"), playerPoints[i + 1].position, playerPoints[i + 1].rotation);
            GameObject oName = Instantiate(Resources.Load<GameObject>("Prefabs/Role/PlayerName"), playerPoints[i + 1].position, playerPoints[i + 1].rotation);
            oplayer.GetComponent<PlayerBase>().ud = new UserData(ID, oname);
            //oName.transform.SetParent(oplayer.transform);
            oName.transform.SetParent(oplayer.transform);
            oName.transform.Find("NamePanel").Find("Canvas").Find("name").GetComponent<Text>().text = oname;
            string head = JDressUp.GetValue("head").ToString();
            string face = JDressUp.GetValue("face").ToString();
            string body = JDressUp.GetValue("body").ToString();
            DressUpPanel.SetOtherDressUp(head + ":head", oplayer);
            DressUpPanel.SetOtherDressUp(face + ":face", oplayer);
            DressUpPanel.SetOtherDressUp(body + ":body", oplayer);

            oplayer.transform.SetParent(playersList);
            oplayer.name="oPlayer"+":"+ id;
            roomId.text = "房间ID：" + JData.GetValue("roomId").ToString();
            otherPlayers.Add(oplayer);
        }
    }
    public void ClearRoom()
    {
        foreach (GameObject oplayer in otherPlayers)
        {
            if (oplayer != null)
            {
                Destroy(oplayer);
            }
        }
    }
    public void StartGame()
    {
        GetComponent<StartGameRequest>().SendRequest();
    }
    public void OpenSelectRoomPanel()
    {
        selectRoomPanel.SetActive(true);
    }
    public void OpenDressUpPanel()
    {
        dressUpPanel.SetActive(true);
    }
    public void SelectMap(int id)
    {
        tmpMapId = id;
    }
    public void SureSelectMap()
    {
        
    }
    public void OpenMapPanel(bool b)
    {
        selectMapPanel.SetActive(b);
    }
    public void LeaveGame()
    {
        Application.Quit();
    }
}
public enum MapIdToName
{
    新手训练营=0,
    丧尸测试地图=1
}
