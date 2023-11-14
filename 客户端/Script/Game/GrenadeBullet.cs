using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBullet : MonoBehaviour
{
    public float injury = 10;

    public GoodsTypes bulletType;
    public GoodsTypes viceBulletType;
    public float viceBulletSpeed;
    public string fromPlayer;

    public int divergencyCount = 15;

    public GameObject bulletHole;
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < divergencyCount; i++)
        {
            GameObject b = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/" + viceBulletType.ToString()), transform.position, transform.rotation);
            b.GetComponent<Bullet>().injury = injury;
            b.GetComponent<Rigidbody>().AddForce(-viceBulletSpeed* collision.contacts[0].normal+ transform.transform.up *Random.Range(-25, 25) + transform.transform.right * Random.Range(-25, 25) + transform.transform.forward * Random.Range(-25, 25), ForceMode.Impulse);
        }
        Destroy(this);
    }
}
