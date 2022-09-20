using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum ZombieState
{
    Idle = 0,
    Walk = 1,
    Dead = 2,
    Attack = 3
}
public class ZombieAI : MonoBehaviour
{
    //Idle
    //Walk
    //Attack
    //Dead

    Animator animator;
    NavMeshAgent agent;
    ZombieState zombieState;
    GameObject playerObject;
    PlayerHealth playerHealth;
    ZombieHealth zombieHealth;

    // Start is called before the first frame update
    void Start()
    {
        zombieHealth = GetComponent<ZombieHealth>();
        playerObject = GameObject.FindWithTag("Player");
        playerHealth = playerObject.GetComponent<PlayerHealth>();
        zombieState = ZombieState.Idle;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(zombieHealth.GetHealth() <= 0)
        {
            SetState(ZombieState.Dead);
        }

        switch(zombieState)
        {
            case ZombieState.Dead:
                KillZombie();
                break;
            case ZombieState.Attack:
                Attack();
                break;
            case ZombieState.Walk:
                SearchForTarget();
                break;
            case ZombieState.Idle:
                SearchForTarget();
                break;
            default:
                break;
        }
    }

    
    private void Attack()
    {
        SetState(ZombieState.Attack);
        agent.isStopped = true;
    }

    void MakeAttack()
    {
        Debug.Log("MakeAttack çalıştı");
        playerHealth.DeductHealth(10);
        SearchForTarget();
    }

    private void SearchForTarget()
    {
        float distance = Vector3.Distance(transform.position, playerObject.transform.position);
        if(distance < 1.5f )
        {
            Attack();
        }

        else if (distance < 10)
        {
            MoveToPlayer();
        }

        else
        {
            SetState(ZombieState.Idle);
            agent.isStopped = true;
        }
    }

    private void MoveToPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(playerObject.transform.position);
        SetState(ZombieState.Walk);
    }

    private void SetState(ZombieState state)
    {
        zombieState = state;
        //Animator
        animator.SetInteger("state", (int)state);
    }
    private void KillZombie()
    {
        SetState(ZombieState.Dead);
        agent.isStopped = true;
        Destroy(gameObject, 5);
    }
}
