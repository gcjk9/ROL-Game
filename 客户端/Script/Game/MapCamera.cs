using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Transform target;
    public float hight=30f;
    public bool isRotaFollow = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, hight, target.position.z);
            if (isRotaFollow)
            {
                transform.rotation = target.rotation;
            }
        }           
    }
}
