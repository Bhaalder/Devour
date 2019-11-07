using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Boss/B'nath/BodySlamState")]

public class BnathBodySlam : BnathBaseState
{

    [SerializeField] private float Count = 1f;
    [SerializeField] private float telegraphTime = 1f;
    [SerializeField] private float middlePointCurve = 10f;
    [SerializeField] private float minDistance = 10f;
    [SerializeField] private float targetYOffset = -1.5f;
    [SerializeField] GameObject particles;

    private float countUp;
    private float currentTelegraphCooldown;

    private bool startAttack;
    private bool particleInstantiated;

    private bool initializeState;

    public override void Enter()
    {
        base.Enter();
        startAttack = false;
        initializeState = false;
        particleInstantiated = false;

        currentTelegraphCooldown = telegraphTime;
        bossSprite = owner.GetComponentInChildren<SpriteRenderer>(); //TEST

        countUp = 0f;
    }

    public override void HandleUpdate()
    {
        if(DistanceToPlayer() <= minDistance)
        {
            owner.Transition<BnathIdle>();
        }

        if (!initializeState)
        {
            owner.State = BossBnathState.BODYSLAM;
            startPoint = owner.rb.position;
            endPoint = new Vector2(owner.Player.transform.position.x, startPoint.y + targetYOffset);
            middlePoint = startPoint + (endPoint - startPoint) / 2 + Vector2.up * middlePointCurve;
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
    public override void HandleFixedUpdate()
    {

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
            owner.Transition<BnathIdle>();
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

            if (bossSprite.color == new Color(255, 255, 255))
            {
                bossSprite.color = new Color(0, 0, 0);
            }
            else
            {
                bossSprite.color = new Color(255, 255, 255);
            }

            return;
        }

        startAttack = true;
        bossSprite.color = new Color(255, 255, 255);
        currentTelegraphCooldown = telegraphTime;
    }

}
