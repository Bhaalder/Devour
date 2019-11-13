using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy3FollowAttackState")]

public class Enemy3FollowAttack : Enemy3Movement
{

    [SerializeField] private float followSpeed = 300f;
    [SerializeField] private float abortFollow = 25f;

    private Vector2 stuckCheckPosition;

    private float stuckCooldown = 2.5f;
    private float currentStuckCooldown;

    private bool stuckCheckStarted;

    public override void Enter()
    {
        base.Enter();
        currentStuckCooldown = stuckCooldown;
        owner.GetComponent<Enemy3>().State = Enemy3State.MOVEMENT;
    }

    public override void HandleUpdate()
    {
        //base.HandleUpdate();

        FindTargetDirection();
        force = direction.normalized * followSpeed * Time.deltaTime;
        owner.rb.AddForce(force);

        if (Vector2.Distance(owner.rb.position, target.position) >= abortFollow || !CanSeePlayer())
        {
            owner.Transition<Enemy3Movement>();
        }

        StuckCheck();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void FindTargetDirection()
    {
        if (target.position.x > owner.rb.position.x)
        {
            direction = new Vector2(1f, 0f);
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (target.position.x < owner.rb.position.x)
        {
            direction = new Vector2(-1f, 0f);
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
    }

    private void StuckCheck()
    {
        if (stuckCheckStarted)
        {
            StuckCheckCooldown();
        }
        else if (!stuckCheckStarted)
        {
            stuckCheckPosition = new Vector2(Mathf.Round(owner.rb.position.x), Mathf.Round(owner.rb.position.y));
            stuckCheckStarted = true;
        }

    }

    private void StuckCheckCooldown()
    {
        currentStuckCooldown -= Time.deltaTime;

        if (currentStuckCooldown > 0)
        {
            return;
        }
        Debug.Log("current cooldown: " + currentStuckCooldown);

        Vector2 currentPosition = new Vector2(Mathf.Round(owner.rb.position.x), Mathf.Round(owner.rb.position.y));
        Debug.Log("StuckPosition: " + stuckCheckPosition + "currentposition: " + currentPosition);
        if (currentPosition == stuckCheckPosition)
        {
            owner.GetComponent<Enemy3>().IWasStuck = true;
            owner.Transition<Enemy3Movement>();
        }

        stuckCheckStarted = false;
        currentStuckCooldown = stuckCooldown;
    }
}
