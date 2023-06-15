using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Enemy
{
    public Player target;
    public BotState state;
    public float patrolSpeed;
    public float chasingSpeed;
    public float minDistance;

    public float patrolRange;
    public bool isEntered = false;

    private Vector3 leftLimit;
    private Vector3 rightLimit;
    private float direction = -1;
    private void Start()
    {
        leftLimit = transform.position + Vector3.left * patrolRange / 2;
        rightLimit = transform.position + Vector3.right * patrolRange / 2;
    }

    private void Update()
    {
        UpdateState();
        transform.localScale = new Vector3(0.5f * -direction, 0.5f, 1);
    }

    private void UpdateState()
    {
        if (state == BotState.IDLE) state = UpdateIdleState();
        if (state == BotState.PATROL) state = UpdatePatrolState();
        if (state == BotState.CHASING) state = UpdateChasingState();
        if (state == BotState.ATTACK) state = UpdateAttackState();

    }
    private BotState UpdateIdleState()
    {
        return BotState.PATROL;
    }

    private BotState UpdatePatrolState()
    {
        if(EventChasing())
        {
            return ChangeState(BotState.CHASING);
        }

        if (transform.position.x < leftLimit.x) {
            direction = 1;
        }

        else if (transform.position.x > rightLimit.x) {
            direction = -1;
        }

        transform.Translate(Vector3.right * Time.deltaTime * patrolSpeed * direction);
        animator.SetFloat("SpeedX", 0.5f);
        return BotState.PATROL;
    }



    private BotState UpdateChasingState()
    {
        direction = Mathf.Sign(target.transform.position.x - transform.position.x);
        if (!NearTarget())
        {
            transform.Translate(Vector3.right * Time.deltaTime * direction * chasingSpeed);
            animator.SetFloat("SpeedX", 1f);
        }
        else
        {
            animator.SetFloat("SpeedX", 0f);
            //return ChangeState(BotState.ATTACK);
        }
        
        return BotState.CHASING;
    }

    private BotState UpdateAttackState()
    {
        return BotState.ATTACK;
    }

    private BotState ChangeState(BotState state)
    {
        isEntered = false;
        return state;
    }

    private bool EventChasing()
    {
        return Vector2.Distance(transform.position, target.transform.position) < 4f;
    }

    private bool NearTarget()
    {
        return Vector2.Distance(transform.position, target.transform.position) < minDistance;
    }
}
