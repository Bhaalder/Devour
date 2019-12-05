using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss2/Boss2SonicSnipeTelegraphState")]
public class Boss2SonicSnipeTelegraph : Boss2BaseState
{
    [SerializeField] private float telegraphTime = 2f;
    [SerializeField] private Vector2 snipeBeamOffset;

    private SpriteRenderer snipeBeamSprite;

    private float currentCooldown;


    public override void Enter()
    {
        base.Enter();
        owner.State = Boss2State.SONIC_SNIPE_TELEGRAPH;
        currentCooldown = telegraphTime;
        TurnedRight();
        //SonicBeam();
    }

    public override void HandleUpdate()
    {
        TelegraphTime();
        TurnedRight();
        owner.rb.velocity = new Vector2(0, 0);
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
            if (owner.SnipeBeamSprite.color == new Color(255, 255, 255))
            {
                owner.SnipeBeamSprite.color = new Color(0, 0, 0);
            }
            else
            {
                owner.SnipeBeamSprite.color = new Color(255, 255, 255);
            }
            return;
        }

        currentCooldown = telegraphTime;
        owner.Transition<Boss2SonicSnipeAttack>();
    }

    private void SonicBeam()
    {
        if(owner.rb.position.x > owner.Player.transform.position.x)
        {
            owner.SonicSnipeBeam.transform.position = owner.rb.position - snipeBeamOffset;
        }
        else
        {
            owner.SonicSnipeBeam.transform.position = owner.rb.position + new Vector2( -snipeBeamOffset.x, snipeBeamOffset.y);
        }

        owner.SnipeBeamSprite.enabled = true;
    }
}
