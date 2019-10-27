//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Zvixa/ZvixaSonarExpelState")]
public class ZvixaSonarExpelState : ZvixaBaseState {

    [Tooltip("How long time before sonar begins")]
    [SerializeField] private float sonarAttackTimeWindUp;
    [Tooltip("How long time the sonarExpel lasts")]
    [SerializeField] private float sonarAttackTime;
    [Tooltip("How much damage the sonarball does on contact")]
    [SerializeField] private float sonarDamage;
    [Tooltip("How long time the sonarball grows")]
    [SerializeField] private float growthTime;
    [Tooltip("How much the sonarball will grow")]
    [SerializeField] private float growth;
    [Tooltip("The ball that grows")]
    [SerializeField] private GameObject sonarExpelPrefab;

    private GameObject sonarGO;
    private float windUpLeft;
    private float attackTimeLeft;
    private bool expelHasStarted;

    public override void Enter() {
        owner.State = BossZvixaState.SONAR_EXPEL;
        owner.BossLog("SonarExpelState");
        if(teleportLocation != owner.TeleportAreaMiddle) {
            owner.rb.velocity = new Vector2(0, 0);
            owner.transform.position = owner.TeleportAreaMiddle.position;
            lastTeleport = owner.TeleportAreaMiddle;
        }
        windUpLeft = sonarAttackTimeWindUp;
        attackTimeLeft = sonarAttackTime;
        expelHasStarted = false;
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0 && !expelHasStarted) {
            StartSonarExpel();
            expelHasStarted = true;
        }
        if (attackTimeLeft <= 0) {
            owner.Transition<ZvixaIdleState>();
        }
        base.HandleUpdate();
    }

    protected override void Movement() {
        
    }

    public void StartSonarExpel() {
        ZvixaSonarObject zvixaSonar;
        sonarGO = Instantiate(sonarExpelPrefab, owner.transform.position, Quaternion.identity);
        zvixaSonar = sonarGO.GetComponent<ZvixaSonarObject>();
        zvixaSonar.Damage = sonarDamage;
        zvixaSonar.Growth = growth;
        zvixaSonar.LifeSpan = growthTime;
    }

}
