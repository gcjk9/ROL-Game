using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float injury = 100;

    public GunTypes fromGun;
    public string fromPlayer;

    public GameObject bulletHole;

    public static Queue<Bullet> AllBullets = new Queue<Bullet>();
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("子弹碰到障碍物:"+ collision.collider.gameObject.name);
        GameObject hitInfo = collision.collider.gameObject;
        string[] result = hitInfo.name.Split(':');
        if (result.Length != 2)
            return;
        bulletHole = Instantiate(Resources.Load<GameObject>("Prefabs/Effect/BulletHoleIn"+ result[1]))as GameObject;
        if (bulletHole == null)
            return;
            bulletHole.transform.LookAt(-1*collision.contacts[0].normal);
            bulletHole.transform.position = transform.position;
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<MeshRenderer>().enabled = false;
        AllBullets.Enqueue(this);
        if (AllBullets.Count > 100)
        {
            Bullet b = AllBullets.Dequeue();
            if (b!= null)
                Destroy(b.gameObject);
        }
            StartCoroutine(DestoryBullet());                        
    }
    IEnumerator DestoryBullet()
    {
        yield return new WaitForSeconds(10f);
        if (bulletHole != null && this.gameObject != null)
        {
            Destroy(bulletHole);
            Destroy(this.gameObject);
        }        
    }
}
