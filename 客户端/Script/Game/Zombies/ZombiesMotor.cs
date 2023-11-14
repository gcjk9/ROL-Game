using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiesMotor : MonoBehaviour
{
    public bool isDead = false;
    public bool isFall = false;
    public float speed = 0;
    public float speedMax = 1;
    public List<Transform> targetPlayers = new List<Transform>();
    public List<Transform> targetWithinAttackRange = new List<Transform>();
    public List<Transform> patrolPoints = new List<Transform>();

    public Transform target;
    public bool isMoving=false;
    public bool isForage = false;
    public int targetPatrolPoints = 0;
    public NavMeshAgent nav ;
    
    // Start is called before the first frame update
    public void MotorInit()
    {
        Transform ZombiesPoints = GameObject.Find("ZombiesPatrolPoints").transform;
        for (int i = 0; i < 3; i++)
        {
            patrolPoints.Add(ZombiesPoints.GetChild(Random.Range(0, ZombiesPoints.childCount)));
        }

        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(patrolPoints[targetPatrolPoints].position);
    }
    public void MotorUpdate()
    {
        if (speed > speedMax)
        {
            speedMax = speed;
        }
        if (isFall||isDead)
        {           
            nav.speed = 0;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }
            
        if (targetPlayers.Count != 0)
        {
            if (targetWithinAttackRange.Count != 0)
            {
                nav.speed = 0;
            }
            else
            {
                nav.SetDestination(targetPlayers[0].position);
                nav.speed = 3;
            }          
        }
        else
        {
            nav.SetDestination(patrolPoints[targetPatrolPoints].position);
            nav.speed = 0.5f;
            if (Vector3.Distance(transform.position, patrolPoints[targetPatrolPoints].position) < 1f)
            {                
                targetPatrolPoints= (int)Random.Range(0, patrolPoints.Count);
            }
        }
    }
    public void MotorInit(List<Transform> patrolPoints)
    {
        this.patrolPoints = patrolPoints;
        MotorInit();
    }
}
