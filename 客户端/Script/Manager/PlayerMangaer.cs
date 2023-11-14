using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventCode;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFacade facade) : base(facade)
    {
        mainCamera = Camera.main;
    }
    public Camera mainCamera;
    private UserData userData;

    private Transform rolePositions;

    private RoleType currentRoleType;
    private GameObject currentRoleGameObject;
    private GameObject playerSyncRequest;
    private GameObject remoteRoleGameObject;

    //private ShootRequest shootRequest;
    //private AttackRequest attackRequest;
    public void SetCurrentRoleType(RoleType rt)
    {
        currentRoleType = rt;
    }
    public UserData UserData
    {
        set { userData = value; }
        get { return userData; }
    }
    public override void OnInit()
    {
        //rolePositions = GameObject.Find("RolePositions").transform;
    }
    
    public GameObject GetCurrentRoleGameObject()
    {
        return currentRoleGameObject;
    }
    
}

