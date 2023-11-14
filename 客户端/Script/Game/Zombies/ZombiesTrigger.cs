using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesTrigger : MonoBehaviour
{
    public Zombies z;
    public ZombiesTriggerTypes zombiesTriggerTypes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            z.OnView(zombiesTriggerTypes,other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            z.OutView(zombiesTriggerTypes,other.transform);
        }
    }
}
