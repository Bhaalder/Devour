using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicDashAttackState")]
public class Boss2SonicDashAttack : Boss2BaseState
{
    [SerializeField] private float count = 1f;
    

    private GameObject[] positions;

    private Vector2 startPosition;

    private float countUp;
    private float dashTime;
    private float currentDashTime;

    private int currentPosition;

    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_DASH_ATTACK;
        positions = owner.ChosenPattern.GetComponent<SonicDashPositions>().Positions;
        dashTime = 1 / count;
        countUp = 0;
        currentDashTime = 0;
        currentPosition = 0;
        owner.HitBoxHorizontal.SetActive(true);
        owner.HitBoxVertical.SetActive(false);

        startPosition = owner.rb.position;
        owner.rb.gravityScale = 0;
    }

    public override void HandleUpdate()
    {
        DashingAttack();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void DashingAttack()
    {
        if (currentDashTime < dashTime)
        {
            countUp += count * Time.deltaTime;
            currentDashTime += Time.deltaTime;
            TurnToTargetPosition();
            owner.rb.position = Vector3.Lerp(startPosition, positions[currentPosition].transform.position, countUp);
            return;
        }
        Destroy(owner.SonicDashParticles[currentPosition]);
        currentPosition++;
        startPosition = owner.rb.position;
        countUp = 0;
        currentDashTime = 0;
        FindTargetDirection();
        if(currentPosition >= positions.Length)
        {
            currentPosition = 0;
            owner.rb.gravityScale = 6;
            owner.HitBoxHorizontal.SetActive(true);
            owner.HitBoxVertical.SetActive(false);
            owner.Transition<Boss2SonicDashExit>();           
        }
    }

    private void TurnToTargetPosition()
    {
        if(positions[currentPosition].transform.position.x >= owner.rb.position.x)
        {
            owner.setGFX(new Vector3(1, 1, 1));
        }
        else
        {
            owner.setGFX(new Vector3(-1, 1, 1));
        }
    }
}
