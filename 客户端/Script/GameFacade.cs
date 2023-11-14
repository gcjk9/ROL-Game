using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventCode;

public class GameFacade : MonoBehaviour {

    private static GameFacade _instance;
    public static GameFacade Instance { get {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameFacade").GetComponent<GameFacade>();
            }
            return _instance;
        } }

    public GameMode gameMode;
    public RequestManager requestMng;
    public ClientManager clientMng;
    public PlayerManager playerMng;


    public bool isEnterPlaying = false;

    //private void Awake()
    //{
    //    if (_instance != null)
    //    {
    //        Destroy(this.gameObject);return;
    //    }
    //    _instance = this;
    //}

    // Use this for initialization
    void Awake()
    {
        InitManager();
	}
    // Update is called once per frame
    void Start()
    {
        if (GameObject.Find("GameMode") == null)
            return;

        GameMode gm = GameObject.Find("GameMode").GetComponent<GameMode>();
        if (!gm.isTest)
            gm.Init();
    }
    void Update () {
        UpdateManager();
        if (isEnterPlaying)
        {            
            isEnterPlaying = false;
        }
	}
    
    private void OnDestroy()
    {
        DestroyManager();
    }

    private void InitManager()
    {
        requestMng = new RequestManager(this);
        clientMng = new ClientManager(this);
        playerMng = new PlayerManager(this);

        requestMng.OnInit();
        clientMng.OnInit();
        playerMng.OnInit();
    }
    private void DestroyManager()
    {
        requestMng.OnDestroy();
        clientMng.OnDestroy();
    }
    private void UpdateManager()
    {
        requestMng.Update();
        clientMng.Update();
    }

    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        requestMng.AddRequest(actionCode, request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestMng.RemoveRequest(actionCode);
    }
    public void HandleReponse(ActionCode actionCode, string data)
    {
        requestMng.HandleReponse(actionCode, data);
    }
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientMng.SendRequest(requestCode, actionCode, data);
    }
    public void SetUserData(UserData ud)
    {
        playerMng.UserData = ud;
    }
}
