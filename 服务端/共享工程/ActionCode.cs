using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ActionCode
    {
        None,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        UpdateRoom,
        QuitRoom,
        StartGame,
        ShowTimer,
        StartPlay,
        Move,
        Shoot,
        Attack,
        GameOver,
        UpdateResult,
        QuitBattle,

        //===================================
        DressUp,

        Control,
        OControl,        
        SyncPlayer,
        Notice,
        Behavior,
        SyncTransform,
        ChangeGun,
    }
}
