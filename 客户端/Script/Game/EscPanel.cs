using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscPanel : MonoBehaviour
{
    public GameMode gameMode;
    public Slider syncDelay;
    public Slider syncDelay2;
    public Toggle syncToggle;
    public Toggle syncToggle2;
    public Transform settingPanel;
    public Transform from, to;
    public bool isOpenSettingPanel = false;
    public bool isMovingSettingPanel = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMovingSettingPanel)
        {
            if (isOpenSettingPanel)
            {
                settingPanel.localPosition = Vector3.Lerp(settingPanel.localPosition,from.localPosition,0.5f);
                if(Vector3.Distance(settingPanel.localPosition, from.localPosition) < 0.01f)
                {
                    isOpenSettingPanel = false;
                    isMovingSettingPanel = false;
                }
            }
            else
            {
                settingPanel.localPosition = Vector3.Lerp(settingPanel.localPosition, to.localPosition, 0.5f);
                if (Vector3.Distance(settingPanel.localPosition, to.localPosition) < 0.01f)
                {
                    isOpenSettingPanel = true;
                    isMovingSettingPanel = false;
                }
            }
        }
    }
    public void SettingPanel()
    {
        isMovingSettingPanel = true;
    }
    public void SetSyncDelay()
    {
        gameMode.SyncRate = syncDelay.value;
    }
    public void SetIsSync()
    {
        gameMode.isUseSync = syncToggle.isOn;
    }
    public void SetAutoSyncTime()
    {
        gameMode.autoSyncTime = (int)syncDelay2.value;
    }
    public void SetIsAutoSync()
    {
        gameMode.isAutoSync = syncToggle2.isOn;
    }
    public void ExitGame()
    {       
        Application.Quit();
    }
}
