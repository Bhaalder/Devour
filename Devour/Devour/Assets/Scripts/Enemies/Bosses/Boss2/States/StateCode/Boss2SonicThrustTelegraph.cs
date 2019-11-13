using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicThrustTelegraphState")]
public class Boss2SonicThrustTelegraph : Boss2BaseState
{
    [SerializeField] private float telegraphTime = 1f;
    [SerializeField] private GameObject telegraph;
    [SerializeField] private float telegraphLength = 10f;

    private float currentCooldown;
    private float telegraphCurrentCooldown;

    private Vector2 telegraphStartPosition;

    private GameObject telegraphObj;

    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_THRUST_TELEGRAPH;

        FindTargetDirection();
        telegraphStartPosition = owner.rb.position;
        currentCooldown = telegraphTime;
        telegraphCurrentCooldown = 0;

        telegraphObj = Instantiate(telegraph, null);
        telegraphObj.transform.position = owner.transform.position;
        telegraphObj.GetComponent<DestroyTimer>().DestructionTime = telegraphTime;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        TelegraphTime();
        Telegraph();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
    private void TelegraphTime()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        currentCooldown = telegraphTime;
        owner.Transition<Boss2SonicThrustAttack>();
    }

    private void Telegraph()
    {
        telegraphCurrentCooldown += Time.deltaTime;

        if (telegraphCurrentCooldown < telegraphTime*0.33f)
        {
            telegraphObj.transform.position = Vector3.Lerp(telegraphStartPosition, telegraphStartPosition + (direction * telegraphLength), telegraphCurrentCooldown);
            return;
        }

        telegraphCurrentCooldown = 0;
    }
}
