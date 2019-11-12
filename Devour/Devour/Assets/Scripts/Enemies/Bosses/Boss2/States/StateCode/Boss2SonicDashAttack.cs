using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicDashAttackState")]
public class Boss2SonicDashAttack : Boss2BaseState
{
    [SerializeField] private float count = 1f;

    private GameObject[] positions;

    private GameObject position1;
    private GameObject position2;
    private GameObject position3;

    private Vector2 startPosition;

    private float countUp;

    private int currentPosition;

    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_DASH_ATTACK;
        positions = owner.ChosenPattern.GetComponent<SonicDashPositions>().Positions;
        countUp = 0;

        position1 = positions[0];
        position2 = positions[1];
        position3 = positions[2];

        startPosition = owner.rb.position;
        owner.rb.gravityScale = 0;
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        DashingAttack();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void DashingAttack()
    {
        if (countUp < count)
        {
            countUp += count * Time.deltaTime;

            if (currentPosition == 0)
            {
                owner.rb.position = Vector3.Lerp(startPosition, position1.transform.position, countUp);
            }
            else if (currentPosition == 1)
            {
                owner.rb.position = Vector3.Lerp(startPosition, position2.transform.position, countUp);
            }
            else if (currentPosition == 2)
            {
                owner.rb.position = Vector3.Lerp(startPosition, position3.transform.position, countUp);
            }
            return;
        }

        currentPosition++;
        startPosition = owner.rb.position;
        countUp = 0;
        if(currentPosition >= 3)
        {
            currentPosition = 0;
            owner.rb.gravityScale = 6;
            owner.Transition<Boss2SonicDashExit>();
            
        }
    }
}
