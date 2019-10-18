using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy2IdleState")]
public class Enemy2IdleState : EnemyBaseState
{

    [SerializeField] private float idleMovementRange = 2f;
    [SerializeField] private float idleSpeed = 2f;
    [SerializeField] private float attackDistance = 25f;

    private Vector2 startingPosition;
    private Vector2 newPosition;

    private float cooldownTime = 2f;
    private float currentCooldown;
    private bool startPositionSet = false;
    private bool isWithinAttackDistance = false;
    private Transform target;





    public override void Enter()
    {
        base.Enter();
        target = FindObjectOfType<Player>().transform;
    }

    public override void HandleUpdate()
    {
        if (!startPositionSet)
        {
            startingPosition = owner.rb.position;
            startPositionSet = true;
        }

        base.HandleUpdate();
        positionUpdateCooldown();
        Debug.Log(newPosition);

        Vector2 position = Vector2.MoveTowards(owner.rb.position, newPosition, idleSpeed * Time.fixedDeltaTime);
        owner.rb.MovePosition(position);

        if(owner.rb.position == newPosition)
        {
            setNewPosition();
        }

        if (Vector2.Distance(owner.rb.position, target.position) <= attackDistance)
        {
            owner.Transition<Enemy2MovementState>();
        }


    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void setNewPosition()
    {
        newPosition = new Vector2(startingPosition.x + Random.Range(0, idleMovementRange), startingPosition.y + Random.Range(0, idleMovementRange));
    }

    private void positionUpdateCooldown()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        setNewPosition();
        currentCooldown = cooldownTime;
    }

}
