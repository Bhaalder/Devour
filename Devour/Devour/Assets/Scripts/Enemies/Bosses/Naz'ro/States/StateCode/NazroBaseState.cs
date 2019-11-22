//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Nazro/NazroBaseState")]
public class NazroBaseState : State {

    [SerializeField] protected Color color;

    private float minimumTimeUntilMove = 1.5f;
    private float maximumTimeUntilMove = 3.5f;
    private float timeUntilNextMove;

    private Vector3 currentMovePosition;

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
        if (CanMove()) {
            owner.transform.position = Vector2.MoveTowards(owner.transform.position, currentMovePosition, owner.Speed * Time.deltaTime);
            if (timeUntilNextMove > 0) {
                timeUntilNextMove -= Time.deltaTime;
                return;
            }
            timeUntilNextMove = Random.Range(minimumTimeUntilMove, maximumTimeUntilMove);
            owner.NewLocation = Random.Range(0, 4);
            while (owner.NewLocation == owner.CurrentLocation) {
                owner.NewLocation = Random.Range(0, 4);
            }
            currentMovePosition = owner.MoveLocations[owner.NewLocation].position;
            owner.CurrentLocation = owner.NewLocation;
        }
    }

    protected bool CanMove() {
        switch (owner.State) {
            case BossNazroState.NONE:
                return false;
            case BossNazroState.INTRO:
                return false;
            case BossNazroState.DEATH:
                return false;
            case BossNazroState.WAIT:
                return false;
            case BossNazroState.PHASE_CHANGE:
                return false;
            default:
                break;
        }
        return true;
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
