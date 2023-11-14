using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance;
    // Start is called before the first frame update
    void Start()
    {        
            //该单例脚本是避免跳转场景时出现多个该物体
            if (instance != null)
            {
                return;
            }
            else
            {
                instance = this;
                //避免场景加载时该对象销毁
                //DontDestroyOnLoad(gameObject);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
