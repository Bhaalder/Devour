using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/B'nathDeathState")]

public class BnathDeathState : EnemyDeathState
{

    [SerializeField] GameObject abilityEssence;
    [SerializeField] PlayerAbility Ability;

    public override void Enter()
    {
        GameObject ability = Instantiate(abilityEssence, null);
        ability.transform.position = owner.rb.position;
        ability.GetComponent<AbilityEssence>().Ability = Ability;
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
