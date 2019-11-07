using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Boss/B'nath/ClimbDashState")]

public class BnathClimbDash : BnathBaseState
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] [Range(1, 100)] private float voidAssaultPercentage = 1f;
    [SerializeField] [Range(1, 100)] private float playerSidePercentage = 1f;
    [SerializeField] private float Count = 1f;
    [SerializeField] private float hangTime = 1f;
    [SerializeField] private float middlePointCurve = 10f;
    [SerializeField] private float endPointY = 25f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] GameObject particles;
    [SerializeField] private float xRightOffset;
    [SerializeField] private float xLeftOffset;
    [SerializeField] private float targetYOffset;
    [SerializeField] private float dashSpeed = 500f;
    [SerializeField] private float groundMargin = 3f;


    private float countUp;
    private float currentTelegraphCooldown;

    private bool playerIsRight;
    private bool isClimbing;
    private bool dashTelegraph;
    private bool particleInstantiated;
    private bool isChoosingAttack;
    private bool voidAssaultAttack = false;

    private Vector2 rightSide;
    private Vector2 leftSide;
    private Vector2 direction;
    private Vector2 force;



    public override void Enter()
    {
        base.Enter();
        isClimbing = true;
        dashTelegraph = true;
        currentTelegraphCooldown = hangTime;
        countUp = 0f;

        PlayerSideCheck();
        ChooseSide();

    }

    public override void HandleUpdate()
    {
        if (isClimbing)
        {
            owner.State = BossBnathState.CLIMBING;
            SideClimb();
        }
        else if (isChoosingAttack)
        {
            ChooseAttack();
        }
        else if (dashTelegraph)
        {
            owner.State = BossBnathState.DASH_TELEGRAPH;
            DashTelegraph();
        }
        else if (!dashTelegraph)
        {
            owner.State = BossBnathState.DASHING;
            ClimbDash();
        }

    }
    public override void HandleFixedUpdate()
    {

    }

    private void PlayerSideCheck()
    {
        RaycastHit2D RSide = Physics2D.Raycast(owner.rb.position, Vector2.right, 100f, layerMask);
        RaycastHit2D LSide = Physics2D.Raycast(owner.rb.position, Vector2.left, 100f, layerMask);

        rightSide.x = owner.rb.position.x + RSide.distance + xRightOffset;
        leftSide.x = owner.rb.position.x - LSide.distance + xLeftOffset;

        
        if (owner.Player.transform.position.x > owner.rb.position.x)
        {
            playerIsRight = true;
        }
        else
        {
            playerIsRight = false;
        }

    }

    private void ChooseSide()
    {
        int chooseSide = (int)Random.Range(1, 100);

        if (playerIsRight && chooseSide <= playerSidePercentage)
        {
            endPoint.x = rightSide.x;
        }
        else if (chooseSide > playerSidePercentage)
        {
            endPoint.x = leftSide.x;
        }

        if (!playerIsRight && chooseSide <= playerSidePercentage)
        {
            endPoint.x = leftSide.x;
        }
        else if (chooseSide > playerSidePercentage)
        {
            endPoint.x = rightSide.x;
        }


        startPoint = owner.rb.position;
        endPoint.y = endPointY;
        middlePoint = startPoint + (endPoint - startPoint) / 2 + Vector2.up * middlePointCurve;

        owner.rb.gravityScale = 0;
    }

    private void SideClimb()
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
            isClimbing = false;
        }
    }

    private void ClimbDash()
    {
        RaycastHit2D ground = Physics2D.Raycast(owner.rb.position, Vector2.down, groundMargin, layerMask);

        if (ground.collider == true)
        {
            owner.rb.velocity = new Vector2(0, 0);
            owner.rb.gravityScale = 6;
            isChoosingAttack = true;
            voidAssaultAttack = false;
            owner.Transition<BnathIdle>();
        }
        direction = (endPoint - startPoint).normalized;
        force = direction * dashSpeed * Time.deltaTime;

        owner.rb.AddForce(force);

    }

    private void DashTelegraph()
    {
        currentTelegraphCooldown -= Time.deltaTime;

        if (currentTelegraphCooldown > 0)
        {
            if (!particleInstantiated)
            {
                endPoint = new Vector2 (owner.Player.transform.position.x, owner.Player.transform.position.y + targetYOffset);
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

        bossSprite.color = new Color(255, 255, 255);
        currentTelegraphCooldown = hangTime;
        startPoint = owner.rb.position;
        middlePoint = startPoint - endPoint;
        countUp = 0f;
        dashTelegraph = false;
        particleInstantiated = false;
    }

    private void ChooseAttack()
    {
        int chooseAttack = (int)Random.Range(1, 100);

        if (chooseAttack <= voidAssaultPercentage)
        {
            isChoosingAttack = true;
            voidAssaultAttack = true;
            owner.Transition<BnathVoidAssault>();

        }
        else
        {
            isChoosingAttack = false;
            ClimbDash();
        }


    }

}
