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
    [Tooltip("The chance in % that the boss will do spikeAttack right after SonarExpel")]
    [SerializeField] private int spikeFollowUpPercentage;
    [Tooltip("If the boss does spikeFollowUp, after how long does she do it?")]
    [SerializeField] private float spikeFollowUpTime;
    [Tooltip("The ball that grows")]
    [SerializeField] private GameObject sonarExpelPrefab;

    private GameObject sonarGO;
    private float windUpLeft;
    private float attackTimeLeft;
    private float spikeFollowUpTimeLeft;
    private int spikeFollowUp;
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
        spikeFollowUpTimeLeft = spikeFollowUpTime;
        expelHasStarted = false;
        spikeFollowUp = Random.Range(0, 100) + 1;
        base.Enter();
    }

    public override void HandleFixedUpdate() {
        base.HandleFixedUpdate();
    }

    public override void HandleUpdate() {
        windUpLeft -= Time.deltaTime;
        attackTimeLeft -= Time.deltaTime;
        spikeFollowUpTimeLeft -= Time.deltaTime;
        if (windUpLeft <= 0 && !expelHasStarted) {
            StartSonarExpel();
            expelHasStarted = true;
            CameraShakeEvent shakeEvent = new CameraShakeEvent {
                startDuration = growthTime,
                startValue = 0.15f
            };
            shakeEvent.FireEvent();
        }
        if(spikeFollowUpTimeLeft <= 0 && spikeFollowUp <= spikeFollowUpPercentage) {   
            owner.Transition<ZvixaSpikeAttackState>();
            return;
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
