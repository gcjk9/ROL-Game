using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public bool isTest = true;
    public UserData ud;//本地玩家信息；
    public GameObject player;
    public PlayerBase pb;
    public float SyncRate = 1;
    public bool isPlaying = false;
    public bool isUseSync = true;
    public bool isAutoSync = true;
    public int autoSyncTime = 10;
    public List<GameObject> players = new List<GameObject>();
    public List<Transform> birthPoints = new List<Transform>();
    public List<GameObject> arrowrs = new List<GameObject>();
    public BackpackPanel backpackPanel;
    public ScorePanel scorePanel;
    public long lastTimeSync = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!isTest)
            Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAutoSync)
        {
            if (DateTime.Now.Second % autoSyncTime != 0)
            {
                return;
            }
        }
        if (isUseSync && isPlaying)
        {
            if ((DateTime.Now.Ticks - lastTimeSync) < (10000000.0 * SyncRate))
                return;
            pb = player.GetComponent<PlayerBase>();
            lastTimeSync = DateTime.Now.Ticks;
            Vector3 velocity = pb.cc.velocity;
            JObject JData = new JObject();
            JObject JPlayer = new JObject();

            int[] gunData = new int[3];
            gunData[0] = pb.LoadingGun == null ? -1 : pb.LoadingGun.GetComponent<Gun>().id;
            gunData[1] = pb.Guns[0] == null ? -1 : pb.Guns[0].GetComponent<Gun>().id;
            gunData[2] = pb.Guns[1] == null ? -1 : pb.Guns[1].GetComponent<Gun>().id;
            JPlayer.Add("gun", gunData[0] + "&" + gunData[1] + "&" + gunData[2]);
            JPlayer.Add("velocity", velocity.x + "&" + velocity.y + "&" + velocity.z);
            JPlayer.Add("position", player.transform.position.x + "&" + player.transform.position.y + "&" + player.transform.position.z);
            JPlayer.Add("rotation", player.transform.rotation.x + "&" + player.transform.rotation.y + "&" + player.transform.rotation.z + "&" + player.transform.rotation.w);

            JData.Add(ud.Id.ToString(), JPlayer.ToString());
            GetComponent<SyncPlayerRequest>().SendRequest(JData.ToString());
        }
    }
    public void Init()
    {
        ud = GameObject.Find("GameFacade").GetComponent<GameFacade>().playerMng.UserData;
        Transform list = GameObject.Find("GameRoot").transform.Find("PlayersList");
        Transform c = GameObject.Find("GameMode").transform.Find("Control");
        Transform oc = GameObject.Find("GameMode").transform.Find("OControl");
        for (int i = 0; i < list.childCount; i++)
        {
            players.Add(list.GetChild(i).gameObject);
        }
        foreach (GameObject player in players)
        {
            PlayerBase pb = player.GetComponent<PlayerBase>();
            if (pb == null) continue;
            Debug.Log("1-0:" + pb.ud.birthPointId.ToString());
            Transform birthPoint = GameObject.Find("birthPoint:" + pb.ud.birthPointId).transform;
            player.transform.SetPositionAndRotation(birthPoint.position, birthPoint.rotation);


            CPlayer cp = player.GetComponent<CPlayer>();
            Debug.Log("5-7-4");
            if (cp != null)
            {
                Debug.Log("5-7-3");
                this.player = player;
                player.transform.SetParent(c);
                cp.Init();
                cp.playerBase.Init();
                cp.isCanBeControl = true;
                pb = player.GetComponent<PlayerBase>();

                GameObject arrowr = Instantiate(arrowrs[0]);
                arrowr.transform.position = new Vector3(player.transform.position.x, 15, player.transform.position.z);
                //arrowr.transform.rotation = new Quaternion(Quaternion.Euler(new Vector3(90,0,0)).x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w);
                arrowr.GetComponent<MapCamera>().target = player.transform;
                arrowr.GetComponent<MapCamera>().hight = 15f;
                Debug.Log("5-7-2");
                backpackPanel.Init(cp);
            }
            else
            {
                player.transform.SetParent(oc);
                pb.MoveDirection = birthPoint;
                pb.Init();

                GameObject arrowr = Instantiate(arrowrs[1]);
                arrowr.transform.position = new Vector3(player.transform.position.x, 15, player.transform.position.z);
                //arrowr.transform.rotation = new Quaternion(Quaternion.Euler(new Vector3(90,0,0)).x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w);
                arrowr.GetComponent<MapCamera>().target = player.transform;
                arrowr.GetComponent<MapCamera>().hight = 15f;

            }
        }
        GameObject.Find("BackToRoom").GetComponent<BackToRoom>().ud = ud;
        c.GetComponent<ControlRequest>().Init();
        oc.GetComponent<OControlRequest>().Init();
        c.GetComponent<NoticeRequest>().Init();
        StartCoroutine(SyncIEnumerator());
    }
    public void TestInit()
    {

        Transform list = GameObject.Find("GameRoot").transform.Find("PlayersList");
        Transform c = GameObject.Find("GameMode").transform.Find("Control");
        Transform oc = GameObject.Find("GameMode").transform.Find("OControl");
        for (int i = 0; i < list.childCount; i++)
        {
            players.Add(list.GetChild(i).gameObject);
        }
        foreach (GameObject player in players)
        {
            int r = 0;
            if (GameObject.Find("test").GetComponent<NWTest>() != null)
            {
                player.transform.position = birthPoints[0].position;
                player.transform.rotation = birthPoints[0].rotation;
            }
            else
            {
                player.transform.position = birthPoints[1].position;
                player.transform.rotation = birthPoints[1].rotation;
            }

            CPlayer cp = player.GetComponent<CPlayer>();
            if (cp != null)
            {
                this.player = player;
                ud = player.GetComponent<PlayerBase>().ud;
                player.transform.SetParent(c);
                cp.Init();
                cp.playerBase.Init();
                cp.isCanBeControl = true;
                pb = player.GetComponent<PlayerBase>();

                GameObject arrowr = Instantiate(arrowrs[0]);
                arrowr.transform.position = new Vector3(player.transform.position.x, 10, player.transform.position.z);
                //arrowr.transform.rotation = new Quaternion(Quaternion.Euler(new Vector3(90, 0, 0)).x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w);
                arrowr.GetComponent<MapCamera>().target = player.transform;
                arrowr.GetComponent<MapCamera>().hight = 15f;

                backpackPanel.Init(cp);
            }
            else
            {
                player.transform.SetParent(oc);
                PlayerBase pb = player.GetComponent<PlayerBase>();
                pb.MoveDirection = birthPoints[r];
                pb.Init();

                GameObject arrowr = Instantiate(arrowrs[1]);
                arrowr.transform.position = new Vector3(player.transform.position.x, 10, player.transform.position.z);
                //arrowr.transform.rotation = new Quaternion(Quaternion.Euler(new Vector3(90, 0, 0)).x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.w);
                arrowr.GetComponent<MapCamera>().target = player.transform;
                arrowr.GetComponent<MapCamera>().hight = 15f;
            }
        }
        GameObject.Find("BackToRoom").GetComponent<BackToRoom>().ud = ud;
        c.GetComponent<ControlRequest>().Init();
        oc.GetComponent<OControlRequest>().Init();
        c.GetComponent<NoticeRequest>().Init();
        StartCoroutine(SyncIEnumerator());
    }

    IEnumerator SyncIEnumerator()
    {
        if (!isPlaying)
        {
            yield return new WaitForSeconds(0.01f);//等待三秒开始游戏
            isPlaying = true;
            isUseSync = true;
        }
    }
    public void ExitGame()
    {
        DontDestroyOnLoad(GameObject.Find("BackToRoom"));
        SceneManager.LoadScene("LoginScene");
    }
}

