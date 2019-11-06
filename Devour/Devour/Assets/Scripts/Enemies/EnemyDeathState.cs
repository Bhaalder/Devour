using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemy/EnemyDeathState")]

public class EnemyDeathState : EnemyBaseState
{

    [SerializeField] private GameObject particleOnDeath;
    private float deathTimer = 1f;
    private float currentCooldown;
    private GameObject[] childrenToDisable;

    public override void Enter()
    {
        base.Enter();
        currentCooldown = deathTimer;

        childrenToDisable = owner.ChildrenToDisable;

        if (childrenToDisable != null)
        {
            foreach (GameObject gameObject in childrenToDisable)
            {
                gameObject.SetActive(false);
            }
        }

        owner.IsAlive = false;

        GameObject instantiatedParticle = Instantiate(particleOnDeath, null);
        instantiatedParticle.transform.position = owner.rb.position;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        DestroyTimer();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void  DestroyTimer()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        Destroy(owner.gameObject);
    }
}
