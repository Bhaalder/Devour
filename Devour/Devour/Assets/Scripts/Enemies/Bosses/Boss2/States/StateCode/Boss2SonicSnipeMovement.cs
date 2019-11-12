using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicSnipeMovementState")]
public class Boss2SonicSnipeMovement : Boss2BaseState
{
    [SerializeField] private float Count = 1f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float xRightOffset;
    [SerializeField] private float xLeftOffset;
    [SerializeField] [Range(0, 100)] private float playerSidePercentage = 1f;
    [SerializeField] private float middlePointCurve = 10f;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 middlePoint;
    private Vector2 rightSide;
    private Vector2 leftSide;

    private float countUp;


    private bool playerIsRight;

    public override void Enter()
    {
        owner.State = Boss2State.SONIC_SNIPE_MOVEMENT;
        base.Enter();
        countUp = 0f;
        PlayerSideCheck();
        ChooseSide();
    }

    public override void HandleUpdate()
    {
        MoveToSide();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void MoveToSide()
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
            owner.Transition<Boss2SonicSnipeTelegraph>();
        }

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
        int chooseSide = Random.Range(1, 100);

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
        endPoint.y = owner.Player.transform.position.y;
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
}
