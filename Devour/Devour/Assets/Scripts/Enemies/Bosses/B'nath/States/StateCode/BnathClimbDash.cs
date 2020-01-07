//Author: Marcus Söderberg
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
    private bool isDashing;

    private Vector2 rightSide;
    private Vector2 leftSide;
    private Vector2 direction;
    private Vector2 force;



    public override void Enter()
    {
        base.Enter();
        isClimbing = true;
        dashTelegraph = true;
        isDashing = false;
        currentTelegraphCooldown = hangTime;
        countUp = 0f;
        Debug.Log("voidAssault: " + voidAssaultAttack);
        PlayerSideCheck();
        if (!voidAssaultAttack)
        {
            ChooseSide();
        }
        owner.rb.velocity = new Vector2(0, 0);

    }

    public override void HandleUpdate()
    {
        if (isClimbing && !voidAssaultAttack)
        {
            owner.State = BossBnathState.CLIMBING;
            SideClimb();
            owner.rb.velocity = new Vector2(0, 0);
        }
        else if (isChoosingAttack)
        {
            ChooseAttack();
            owner.rb.velocity = new Vector2(0, 0);
        }
        else if (dashTelegraph)
        {
            owner.State = BossBnathState.DASH_TELEGRAPH;
            DashTelegraph();
            owner.rb.velocity = new Vector2(0, 0);
        }
        else if (!dashTelegraph)
        {
            owner.State = BossBnathState.DASHING;
            ClimbDash();
            isClimbing = false;
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

        if (endPoint.x < owner.rb.position.x)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
        else
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
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

        if(startPoint.x < endPoint.x)
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if(startPoint.x > endPoint.x)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }
        owner.rb.velocity = new Vector2(0, 0);
    }

    private void ClimbDash()
    {
        if (!isDashing)
        {
            isDashing = true;
        }

        RaycastHit2D ground = Physics2D.Raycast(owner.rb.position, Vector2.down, groundMargin, layerMask);

        if (ground.collider == true)
        {
            owner.rb.velocity = new Vector2(0, 0);
            owner.rb.gravityScale = 6;
            isChoosingAttack = true;
            voidAssaultAttack = false;
            isDashing = false;
            TurnedRight();
            CameraShakeEvent shakeEvent = new CameraShakeEvent {
                startDuration = 0.4f,
                startValue = 0.35f
            };
            shakeEvent.FireEvent();
            AudioPlaySoundAtLocationEvent landSound = new AudioPlaySoundAtLocationEvent {
                name = "BnathLand",
                isRandomPitch = true,
                minPitch = 0.95f,
                maxPitch = 1f,
                soundType = SoundType.SFX,
                gameObject = owner.AudioGO
            };
            landSound.FireEvent();
            owner.Transition<BnathIdle>();
            Debug.Log("STOPPED DASHING");
        }
        direction = (endPoint - startPoint).normalized;
        force = direction * dashSpeed * Time.deltaTime;
        owner.rb.AddForce(force);
        if (startPoint.x < endPoint.x)
        {
            Vector3 v = new Vector3(1f, 1f, 1f);
            owner.setGFX(v);
        }
        else if (startPoint.x > endPoint.x)
        {
            Vector3 v = new Vector3(-1f, 1f, 1f);
            owner.setGFX(v);
        }

    }

    private void DashTelegraph()
    {
        currentTelegraphCooldown -= Time.deltaTime;

        if (currentTelegraphCooldown > 0)
        {
            if (!particleInstantiated)
            {
                endPoint = new Vector2(owner.Player.transform.position.x, owner.YPoint.transform.position.y + targetYOffset);
                particleInstantiated = true;
                GameObject instantiatedParticle = Instantiate(particles, null);
                instantiatedParticle.transform.position = endPoint;
            }

            if (owner.Player.transform.position.x > owner.rb.position.x)
            {
                Vector3 v = new Vector3(-1f, 1f, 1f);
                owner.setGFX(v);
            }
            else if (owner.Player.transform.position.x < owner.rb.position.x)
            {
                Vector3 v = new Vector3(1f, 1f, 1f);
                owner.setGFX(v);
            }

            return;
        }

        currentTelegraphCooldown = hangTime;
        startPoint = owner.rb.position;
        middlePoint = startPoint - endPoint;
        countUp = 0f;
        dashTelegraph = false;
        particleInstantiated = false;
        owner.PlayVoice("JumpFromWall");
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
    public override void Exit()
    {
        owner.rb.velocity = new Vector2(0, 0);
        base.Exit();

    }

}
