//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroBaseState")]
public class NazroBaseState : State {

    [SerializeField] protected Color color;

    [SerializeField] private Vector2 movementMax;
    [SerializeField] private Vector2 movementMin;

    protected Nazro owner;
    protected bool battleStart;
    protected bool isPhaseTwo;

    public override void Enter() {
        base.Enter();
        owner.GetComponent<SpriteRenderer>().color = color;
    }

    public override void HandleFixedUpdate() {
        if (owner.State == BossNazroState.NONE && PlayerIsInsideBossRoom() && owner.Player.PlayerState != PlayerState.HURT) {
            owner.Transition<NazroIntroState>();
        }
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        base.HandleUpdate();
        Movement();
    }

    protected virtual void Movement() {
        
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
        if (owner.Player.BoxCollider2D.bounds.Intersects(owner.LeftArea.bounds)) {
            return 2;
        }
        return -1;
    }

    public override void Initialize(StateMachine owner) {
        this.owner = (Nazro)owner;
    }
}
