using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicThrustAttackState")]
public class Boss2SonicThrustAttack : Boss2BaseState
{
    //[SerializeField] private float thrustSpeed = 1600f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float rayCastLength;
    [SerializeField] private float dashForce = 40f;
    [SerializeField] private float startDashTime;


    private float dashTime;

    public override void Enter()
    {
        base.Enter();
        dashTime = startDashTime;
        direction = owner.dashStartDirection;
        owner.State = Boss2State.SONIC_THRUST_ATTACK;
    }

    public override void HandleUpdate()
    {
        WallHit();

        owner.rb.velocity = new Vector2((dashForce * direction.x), 0);

        if (dashTime <= 0)
        {
            owner.Transition<Boss2SonicThrustExit>();
        }
        dashTime -= Time.deltaTime;
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void WallHit()
    {
        RaycastHit2D obstructed = Physics2D.Raycast(owner.rb.position, direction, rayCastLength, layerMask);

        if (obstructed.collider == true)
        {
            owner.rb.velocity = new Vector2(0, owner.rb.velocity.y);
            owner.Transition<Boss2SonicThrustExit>();
        }
    }
}
