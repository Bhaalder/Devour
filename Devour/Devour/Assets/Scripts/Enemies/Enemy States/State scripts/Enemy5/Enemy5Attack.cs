using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy5AttackState")]
public class Enemy5Attack : EnemyMovement
{

    [SerializeField] private float Count = 1f;
    [SerializeField] private float telegraphTime = 1f;
    [SerializeField] private float middlePointCurve = 10f;
    [SerializeField] private float targetYOffset = -1.5f;
    [SerializeField] GameObject particles;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 middlePoint;

    private float countUp;
    private float currentTelegraphCooldown;

    private bool startAttack;
    private bool particleInstantiated;

    private bool initializeState;


    public override void Enter()
    {
        base.Enter();

        owner.GetComponent<Enemy5>().State = Enemy5State.ATTACK;

        startAttack = false;
        initializeState = false;
        particleInstantiated = false;

        currentTelegraphCooldown = telegraphTime;
        countUp = 0;
        CheckGround();
    }

    public override void HandleUpdate()
    {
        if (owner.GetComponent<Enemy5>().TooCloseToJump)
        {
            owner.Transition<Enemy5Movement>();
        }
        else
        {
            if (!initializeState)
            {
                RaycastHit2D obstructed = Physics2D.Raycast(owner.Player.transform.position, Vector2.down, layerMask);
                startPoint = owner.rb.position;
                endPoint = new Vector2(owner.Player.transform.position.x, obstructed.point.y + targetYOffset);
                middlePoint = startPoint + (endPoint - startPoint) / 2 + Vector2.up * middlePointCurve;
                RaycastHit2D obstructedMiddleY = Physics2D.CircleCast(owner.rb.position + new Vector2(0, 3), 2f,
                    (middlePoint - owner.rb.position) / (middlePoint - owner.rb.position).magnitude, (middlePoint - owner.rb.position).magnitude, layerMask);

                if (obstructedMiddleY.collider)
                {
                    if (obstructedMiddleY.collider.gameObject.layer == 8)
                    {
                        if (Vector2.Distance(owner.rb.position, middlePoint) > Vector2.Distance(owner.rb.position, obstructedMiddleY.point))
                        {
                            middlePoint = new Vector2(middlePoint.x, obstructedMiddleY.point.y + (owner.rb.position.y - obstructedMiddleY.point.y) / 6f);
                        }
                    }
                }
                initializeState = true;
            }

            base.HandleUpdate();

            if (!startAttack)
            {
                TurnedRight();
                BodySlamTelegraph();
            }
            else
            {
                BodySlam();
            }
        }

    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
    public override void Exit()
    {
        countUp = 0;
        owner.rb.velocity = new Vector2(0, owner.rb.velocity.y);
    }

    private void BodySlam()
    {
        if (countUp < Count)
        {
            countUp += Count * Time.deltaTime;

            Vector2 m1 = Vector2.Lerp(startPoint, middlePoint, countUp);
            Vector2 m2 = Vector2.Lerp(middlePoint, endPoint, countUp);
            owner.gameObject.transform.position = Vector3.Lerp(m1, m2, countUp);
        }
        else
        {
            TurnedRight();
            owner.Transition<Enemy5Idle>();
        }
    }

    private void BodySlamTelegraph()
    {
        currentTelegraphCooldown -= Time.deltaTime;

        if (currentTelegraphCooldown > 0)
        {
            if (!particleInstantiated)
            {
                particleInstantiated = true;
                GameObject instantiatedParticle = Instantiate(particles, null);
                instantiatedParticle.transform.position = endPoint;
            }

            return;
        }

        startAttack = true;
        currentTelegraphCooldown = telegraphTime;
    }

    private void CheckGround()
    {
        RaycastHit2D obstructed = Physics2D.Raycast(owner.rb.position, new Vector2(owner.transform.localScale.x, 0), 5f, layerMask);
        if (obstructed.collider == true)
        {
            owner.GetComponent<Enemy5>().TooCloseToJump = true;
        }
    }

}
