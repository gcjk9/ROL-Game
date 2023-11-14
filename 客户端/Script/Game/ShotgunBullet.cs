using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    public static int t = 0;
    public GoodsTypes bulletType;
    public GoodsTypes viceBulletType;
    public float injury;
    public float viceBulletSpeed;
    public int divergencyCount = 6;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Divergency()
    {      
        for(int i=0;i<divergencyCount;i++)
        {
            GameObject b = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/" + viceBulletType.ToString()), transform.position, transform.rotation);
            b.GetComponent<Bullet>().injury = injury;
            b.GetComponent<Rigidbody>().AddForce(transform.transform.up * viceBulletSpeed+ transform.transform.right * Random.Range(-15, 15) + transform.transform.forward * Random.Range(-15, 15), ForceMode.Impulse);
            Debug.Log(t++);
        }
        Destroy(gameObject);
    }
}
