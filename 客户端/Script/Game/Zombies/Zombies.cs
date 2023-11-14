
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombies : ZombiesAnim
{
    public GameMode gameMode;
    public float HP = 100;
    public float injury = 10;
    public bool isAttacking = false;

    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        MotorInit();        
    }    
    void Init()
    {
        GameObject g = GameObject.Find("GameMode");
        if (g != null)
            gameMode = g.GetComponent<GameMode>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            animator.SetBool("isDead", isDead);           
        }            

        ZombiesUpdate();
        AnimUpdate();
        MotorUpdate();
    }
    void FixedUpdate()
    {
        CalculateSpeed();
    }
    void ZombiesUpdate()
    {
        if (targetWithinAttackRange.Count == 0)
        {
            StopAttackPlayer();
        }
    }
    public void OnView(ZombiesTriggerTypes zombiesTriggerTypes, Transform target)
    {
        if(zombiesTriggerTypes.Equals(ZombiesTriggerTypes.Listen)|| zombiesTriggerTypes.Equals(ZombiesTriggerTypes.View))
            targetPlayers.Add(target);
        if (zombiesTriggerTypes.Equals(ZombiesTriggerTypes.Attack))
        {
            if (isDead)
                return;
            targetWithinAttackRange.Add(target);
            if (!isAttacking)
            {
                StartCoroutine(AttackingPlayer());
                isAttacking = true;
            }
        }           
    }
    public void OutView(ZombiesTriggerTypes zombiesTriggerTypes,Transform target)
    {
        if (zombiesTriggerTypes.Equals(ZombiesTriggerTypes.Listen) || zombiesTriggerTypes.Equals(ZombiesTriggerTypes.View))
            targetPlayers.Remove(target);
        if (zombiesTriggerTypes.Equals(ZombiesTriggerTypes.Attack))
        {
            targetWithinAttackRange.Remove(target);
        }            
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("bullet"))
        {
            float value = collision.gameObject.GetComponent<Bullet>().injury;
            GetComponent<Rigidbody>().AddForce(collision.contacts[0].normal * 100, ForceMode.Impulse);
            Damage(value);
        }
    }
    void StartAttackPlayer()
    {
        AnimAttack();        
    }
    void StopAttackPlayer()
    {
        
    }
    IEnumerator AttackingPlayer()
    {
        AnimAttack();
        yield return new WaitForSeconds(1f);

        foreach(Transform tag in targetWithinAttackRange)
        {
            tag.GetComponent<PlayerBase>().BeAttacked(injury);
        }
        if (targetWithinAttackRange.Count != 0)
        {
            StartCoroutine(AttackingPlayer());
        }
        else
        {
            isAttacking = false;
        }
    }
    public void Damage(float value)
    {
        if (isDead)
        {            
            return;
        }
           
        HP -= value;
        if (HP <= 0)
        {
            isDead = true;
            StartCoroutine(CleanBody());
        }
        gameMode.scorePanel.Add((int)value,transform.position);
        if (Random.Range(0, 10) > 0)
            isFall = true;

        AnimDamage();        
    }
    void CalculateSpeed()
    {
        if (lastPosition == null)
        {
            lastPosition = transform.position;
        }
        float mspeed = (Vector3.Distance(transform.position, lastPosition))*50;
        if (speed < mspeed)
        {
            speed += 0.02f;
        }
        if (speed > mspeed)
        {
            speed -= 0.02f;
        }
        
        lastPosition = transform.position;
    }
    IEnumerator CleanBody()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
}
public enum ZombiesTriggerTypes
{
    View,
    Listen,
    Attack
}
public enum ZombiesState
{
    Idle,
    Track,
    Attack,
    Dead
}
