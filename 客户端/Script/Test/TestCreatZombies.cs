using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreatZombies : MonoBehaviour
{
    public Queue<GameObject> zombiesList = new Queue<GameObject>();
    public bool CreatFinish = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (DateTime.Now.Second % 10 == 0&& !CreatFinish)
        {
            CreatFinish = true;
            Transform p = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount));
            GameObject z = Instantiate(Resources.Load<GameObject>("Prefabs/Role/Test/Zombies-Test"), p.position, p.rotation);
            zombiesList.Enqueue(z);
            if (zombiesList.Count > 50)
            {
                z = zombiesList.Dequeue();
                if (z != null)
                {
                    Destroy(z);
                }
            }
        }
        else
        {
            CreatFinish = false;
        }
        if (DateTime.Now.Second % 2 == 0&& !CreatFinish)
        {
            CreatFinish = true;
            Transform p = transform.GetChild(UnityEngine.Random.Range(0, transform.childCount));
            GameObject e = Instantiate(Resources.Load<GameObject>("Prefabs/Equipment/"+ (GoodsChineseName)UnityEngine.Random.Range(0,5)), p.position, p.rotation);
            e.transform.SetParent(GameObject.Find("AllEquipments").transform);
        }
        else
        {
            CreatFinish = false;
        }
    }
}
