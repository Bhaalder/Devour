using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy2IdleState")]
public class Enemy2IdleState : EnemyBaseState
{

    [SerializeField] private float idleMovementRange = 2f;
    [SerializeField] private float idleSpeed = 2f;
    [SerializeField] private float attackDistance = 15f;

    private Vector2 startingPosition;
    private Vector2 newPosition;
    private Vector2 direction;
    private Vector2 force;

    private float cooldownTime = 2f;
    private float currentCooldown;
    private bool startPositionSet = false;
    private bool isWithinAttackDistance = false;
    private Transform target;

    public override void Enter()
    {
        base.Enter();
        target = FindObjectOfType<Player>().transform;
        owner.GetComponent<Enemy2>().State = Enemy2State.IDLE;
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

        direction = (newPosition - owner.rb.position).normalized;

        force = direction * idleSpeed * Time.deltaTime;
        owner.rb.AddForce(force);

        if (owner.rb.position == newPosition)
        {
            setNewPosition();
        }

        if (!owner.GetComponent<Enemy2>().IdleOnly)
        {
            try
            {
                if (Vector2.Distance(owner.rb.position, target.position) <= attackDistance)
                {
                    owner.Transition<Enemy2MovementState>();
                }
            }
            catch (MissingReferenceException)
            {
                target = FindObjectOfType<Player>().transform;
            }
        }

    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void setNewPosition()
    {
        newPosition = new Vector2(startingPosition.x + Random.Range(0, idleMovementRange), startingPosition.y + Random.Range(0, idleMovementRange));

        if (newPosition.x > owner.rb.position.x +0.5f)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (newPosition.x < owner.rb.position.x + 0.5f)
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
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
