//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SPIKEATTACK är en followup av SonarExpel (%-chans)
[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaBaseState")]
public class ZvixaBaseState : State {

    protected Zvixa owner;
    public Color colorTest; //För tillfället för test
    protected bool battleStart;
    private float minimumTimeUntilMove = 1.5f;
    private float maximumTimeUntilMove = 3.5f;
    private float timeUntilNextMove;
    protected Transform lastTeleport;
    protected Transform teleportLocation;

    public override void Enter() {
        base.Enter();
        owner.GetComponent<SpriteRenderer>().color = colorTest;//TEST       
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
        if (!battleStart) {
            Movement();
        }
        FacingDirection();
    }

    public override void HandleUpdate() {
        if (owner.State == BossZvixaState.NONE && PlayerIsInsideBossRoom() && owner.Player.PlayerState != PlayerState.HURT) {
            owner.Transition<ZvixaIntroState>();
        }
        owner.rb.velocity = Vector2.zero;
        base.HandleUpdate();
    }

    protected virtual void Movement() {
        if(timeUntilNextMove > 0) {
            timeUntilNextMove -= Time.deltaTime;
            return;
        }
        timeUntilNextMove = Random.Range(minimumTimeUntilMove, maximumTimeUntilMove);
        while(lastTeleport == teleportLocation) {
            int position = Random.Range(0, 3) + 1;
            switch (position) {
                case 1:
                    teleportLocation = owner.TeleportAreaLeft;
                    break;
                case 2:
                    teleportLocation = owner.TeleportAreaMiddle;
                    break;
                case 3:
                    teleportLocation = owner.TeleportAreaRight;
                    break;
            }
        }
        if(lastTeleport == null || teleportLocation == null) {
            lastTeleport = owner.TeleportAreaMiddle;
            teleportLocation = owner.TeleportAreaMiddle;
        }
        //owner.Rb2d.MovePosition(teleportLocation.position);
        owner.rb.velocity = new Vector2(0, 0);
        owner.transform.position = teleportLocation.position;
        lastTeleport = teleportLocation;
    }

    private bool PlayerIsInsideBossRoom() {
        if (owner.Player.BoxCollider2D.bounds.Intersects(owner.StartFightArea.bounds)) {
            return true;
        }
        return false;
    }

    protected int CheckPlayerPosition() {
        if (owner.Player.BoxCollider2D.bounds.Intersects(owner.HighArea.bounds)) {
            return 1;
        }
        if (owner.Player.BoxCollider2D.bounds.Intersects(owner.LowArea.bounds)) {
            return 2;
        }
        return -1;
    }

    private void FacingDirection() {
        if (owner.Player.transform.position.x < owner.transform.position.x) {
            Flip(-4);
            owner.FacingDirection = -1;
        } else {
            Flip(4);
            owner.FacingDirection = 1;
        }
    }

    protected void Flip(float direction) {
        Vector3 myScale = owner.transform.localScale;
        myScale.x = direction;
        owner.transform.localScale = myScale;
    }

    public override void Initialize(StateMachine owner) {
        this.owner = (Zvixa)owner;
    }
}
