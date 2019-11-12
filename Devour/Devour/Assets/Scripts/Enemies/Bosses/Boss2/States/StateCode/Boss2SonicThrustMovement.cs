using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicThrustMovementState")]
public class Boss2SonicThrustMovement : Boss2BaseState
{
    [SerializeField] private float thrustDistance = 10f;
    [SerializeField] private float followSpeed = 500f;

    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_THRUST_MOVEMENT;
    }

    public override void HandleUpdate()
    {
        FindTargetDirection();
        force = direction.normalized * followSpeed * Time.deltaTime;
        owner.rb.AddForce(force);

        if (DistanceToPlayer() <= thrustDistance)
        {
            owner.Transition<Boss2SonicThrustAttack>();
        }

    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

}
