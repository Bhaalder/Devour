using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2DeathState")]
public class Boss2DeathState : EnemyDeathState
{

    [SerializeField] float deathDelay = 2f;
    public override void Enter()
    {
        if (owner.GetComponent<BoxCollider2D>() != null)
        {
            owner.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (owner.GetComponent<CircleCollider2D>() != null)
        {
            owner.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
