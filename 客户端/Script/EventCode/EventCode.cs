using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCode : MonoBehaviour
{
    public enum RequestCode
    {
        None = 0,
        User = 1,
        Room = 2,
        Game = 3
    }
    public enum ActionCode
    {
        None = 0,
        Login = 1,
        Register = 2,
        ListRoom = 3,
        CreateRoom = 4,
        JoinRoom = 5,
        UpdateRoom = 6,
        QuitRoom = 7,
        StartGame = 8,
        ShowTimer = 9,
        StartPlay = 10,
        Move = 11,
        Shoot = 12,
        Attack = 13,
        GameOver = 14,
        UpdateResult = 15,
        QuitBattle = 16,

        DressUp=17,
        Control=18,
        OControl=19,
        SyncPlayer=20,
        Notice=21
    }
    public enum ReturnCode
    {        
        Fail = 0,
        Success = 1,
        NotFound = 2
    }
    public enum ControlEventTypes//按键名称+1表示按下，+0则表示抬起
    {
        MouseL=1,
        MouseR=2,
        W=3,
        S=4,
        A=5,
        D=6,
    }
    public enum InputTypes
    {
        MouseMove=0,
        KeyEven=1
    }
    public enum KeyStateTypes
    {
        Up=0,
        Down=1
    }
    public enum NoticeTypes
    {
        Pick,
        Load,
        Fire,
        LoadMag,
        Discard,
        Create,
        Dead,
    }
    public enum RoleType
    {
        None=0,
        Rocket=1,
        Bomb=2,
        Armor=3,
        Special=4,
        Medic=5
    }
}
