//Author: Patrik Ahlgren
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    NONE, IDLE, WALK, AIR, DASH, WALLSLIDE, WALLJUMP, HURT, ATTACK, PROJECTILEATTACK, JUMP, DEATH, BUSY
}

public enum PlayerAbility {
    DOUBLEJUMP = 1, WALLCLIMB, DASH, PROJECTILE, VOIDMEND
}

public class Player : StateMachine {
   
    #region publicVariables
    public PlayerState PlayerState { get; set; }
    public List<PlayerAbility> PlayerAbilities { get; set; }

    public Rigidbody2D Rb2D { get; set; }
    
    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public float MaxPlayerVoid { get; set; }
    public float PlayerVoid { get; set; }
    public float DamageReduction { get => damageReduction; set => damageReduction = value; }
    public float MeleeDamage { get; set; }
    public float ProjectileDamage { get; set; }
    public float KnockbackForce { get => knockbackForce; set => knockbackForce = value; }
    public float BounceForce { get => bounceForce; set => bounceForce = value; }
    public float MeleeCooldown { get => meleeCooldown; set => meleeCooldown = value; }
    public float UntilNextMeleeAttack { get; set; }
    public float MeleeLifeLeech { get; set; }
    public float MeleeVoidLeech { get; set; }
    public float ProjectileCooldown { get => projectileCooldown; set => projectileCooldown = value; }
    public float UntilNextProjectileAttack { get; set; }
    public float ProjectileHealthcost { get => projectileHealthcost; set => projectileHealthcost = value; }
    public GameObject VoidMendParticleEffect { get => voidMendParticleEffect; set => voidMendParticleEffect = value; }
    public bool IsTotallyInvulnerable { get; set; }
    public bool IsDead { get; set; }

    public BoxCollider2D BoxCollider2D { get; set; }
    public BoxCollider2D HorizontalMeleeCollider { get => horizontalMeleeCollider; set => horizontalMeleeCollider = value; }
    public BoxCollider2D DownMeleeCollider { get => downMeleeCollider; set => downMeleeCollider = value; }
    public BoxCollider2D UpMeleeCollider { get => upMeleeCollider; set => upMeleeCollider = value; }
    public bool IsAttackingDown { get; set; }
    public bool IsAttackingUp { get; set; }
    public bool IsInvulnerable { get; set; }
    public float UntilInvulnerableEnds { get; set; }

    public float MovementSpeed { get; set; }
    public float JumpForce { get => jumpForce; set => jumpForce = value; }
    public int ExtraJumps { get => extraJumps; set => extraJumps = value; }
    public int ExtraJumpsLeft { get; set; }
    public float VariableJumpHeight { get => variableJumpHeight; set => variableJumpHeight = value; }
    //public float PermanentVariableJumpHeight { get; set; }
    public int NumberOfDashes { get => numberOfDashes; set => numberOfDashes = value; }
    public int DashesLeft { get; set; }
    public float DashCooldown { get; set; }
    public float UntilNextDash { get; set; }
    public float FallSpeed { get => fallSpeed; set => fallSpeed = value; }
    public bool MovementIsStopped { get; set; }
    public GameObject JumpParticle { get => jumpParticle; set => jumpParticle = value; }
    public GameObject DoubleJumpParticle { get => doubleJumpParticle; set => doubleJumpParticle = value; }
    public float OutOfBusyStateJumpCancelTime { get; set; }

    public float XInput { get; set; }
    public float YInput { get; set; }

    public float XScale { get; set; }

    public float GroundCheckDistance { get => groundCheckDistance; set => groundCheckDistance = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public int FacingDirection { get; set; }

    public bool IsGrounded { get; set; }
    public bool IsTouchingWall { get; set; }
    public bool IsWallSliding { get; set; }
    
    public Transform GroundCheck { get => groundCheck; set => groundCheck = value; }
    public Transform[] GroundChecks { get => groundChecks; set => groundChecks = value; }
    public Transform WallCheck { get => wallCheck; set => wallCheck = value; }
    public Transform Aim { get => aim; set => aim = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
    public RectTransform PlayerCanvas { get => playerCanvas; set => playerCanvas = value; }
    public Animator Animator { get; set; }

    public float TalentMeleeDamage { get; set; }
    public float TalentProjectileDamage { get; set; }
    public float TalentHealth { get; set; }
    public float TalentLifeLeech { get; set; }
    public float TalentMovementSpeed { get; set; }
    public float TalentDashCooldown { get; set; }
    public float TalentPlayerVoid { get; set; }
    public float TalentVoidLeech { get; set; }

    public List<TalentPoint> TalentPoints { get; set; }
    public List<Collectible> Collectibles { get; set; }
    #endregion

    #region privateSerialized Variables
    [Header("Health & Combat")]
    [Tooltip("Player maxHealth")]
    [SerializeField] private float maxHealth;
    [Tooltip("Player current health")]
    [SerializeField] private float health;
    [Tooltip("Player maxHealth")]
    [SerializeField] private float maxPlayerVoid;
    [Tooltip("Player current health")]
    [SerializeField] private float playerVoid;
    [Tooltip("How much less damage the player takes from enemy attacks")]
    [SerializeField] private float damageReduction;
    [Tooltip("Player damage (close combat)")]
    [SerializeField] private float meleeDamage;
    [Tooltip("How much health the close combat attack leeches in health")]
    [SerializeField] private float meleeLifeLeech;
    [Tooltip("How much health the close combat attack leeches void")]
    [SerializeField] private float meleeVoidLeech;
    [Tooltip("Cooldown between attacks (close combat)")]
    [SerializeField] private float meleeCooldown;
    [Tooltip("Player damage (projectile)")]
    [SerializeField] private float projectileDamage;
    [Tooltip("How much health the projectile-attack drains")]
    [SerializeField] private float projectileHealthcost;
    [Tooltip("Cooldown between attacks (projectile)")]
    [SerializeField] private float projectileCooldown;
    [Tooltip("Knockbackvalue applied to enemies from the player")]
    [SerializeField] private float knockbackForce;
    [Tooltip("Bouncevalue applied when player is attacking down on enemies")]
    [SerializeField] private float bounceForce;
    [Tooltip("How long the player is invulnerable to damage after taking damage")]
    [SerializeField] private float invulnerableStateTime;
    [Tooltip("Knockbackvalue applied to player when hurt by enemies")]
    [SerializeField] private Vector2 hurtKnockbackForce;
    [Tooltip("The instantiated particles when player uses voidMend")]
    [SerializeField] private GameObject voidMendParticleEffect;
    private bool lowHealthSoundIsPlaying;
    private bool voidMendIsFull;

    [Header("Camera")]
    [Tooltip("How long the camera will shake when taking damage")]
    [SerializeField] private float cameraShakeDuration;
    [Tooltip("How much the camera will shake when taking damagee")]
    [SerializeField] private float cameraShakeValue;
    
    [Header("Movement")]
    [Tooltip("How fast the player is moving")]
    [SerializeField] private float movementSpeed;   
    [Tooltip("How high the player can jump")]
    [SerializeField] private float jumpForce;
    [Tooltip("How many extra jumps the player has (when not on ground)")]
    [SerializeField] private int extraJumps;
    [Tooltip("How much the jump gets 'cut' if the player releases the jumpbutton")]
    [SerializeField] private float variableJumpHeight;
    [Tooltip("How many dashes the player can do between being on ground/wallslide or bounce on enemies")]
    [SerializeField] private int numberOfDashes;
    [Tooltip("The cooldown between dashes")]
    [SerializeField] private float dashCooldown;
    [Tooltip("The maxvalue the player can fall")]
    [SerializeField] private float fallSpeed;
    [Tooltip("The particle when player walks")]
    [SerializeField] private GameObject walkParticle;
    [Tooltip("The particle when player is on ground and jumps")]
    [SerializeField] private GameObject jumpParticle;
    [Tooltip("The particle when player jumps midair")]
    [SerializeField] private GameObject doubleJumpParticle;
    

    [Header("MovementCheckVariables")]
    [Tooltip("The area of the groundcheck, to see if the player is touching the ground")]
    [SerializeField] private float groundCheckDistance;
    [Tooltip("The length of the wallcheck, to see if the player is touching a wall")]
    [SerializeField] private float wallCheckDistance;
    private float wallCheckDistanceValue;

    [Header("Transforms & Layermask")]
    [SerializeField] private BoxCollider2D horizontalMeleeCollider;
    [SerializeField] private BoxCollider2D upMeleeCollider;
    [SerializeField] private BoxCollider2D downMeleeCollider;
    [SerializeField] private Transform aim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform[] groundChecks;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private RectTransform playerCanvas;
    [SerializeField] private LayerMask whatIsGround;
    #endregion

    [Header("Testing")]//TEST
    [Tooltip("For testing if the player has certain abilities")]//
    [SerializeField] private PlayerAbility[] playerAbilities;//
    public Vector2 PlayerVelocity;//
    private int screenShot = 0;//

    private static bool exists;

    protected override void Awake() {
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            Debug.LogWarning("Destroyed other Singleton with name: " + gameObject.name);
            return;
        }

        GameController.Instance.Player = this;

        PlayerAbilities = new List<PlayerAbility>();

        FacingDirection = 1;

        Rb2D = GetComponent<Rigidbody2D>();
        BoxCollider2D = GetComponent<BoxCollider2D>();

        MaxHealth = maxHealth;
        Health = maxHealth/2;
        MaxPlayerVoid = maxPlayerVoid;
        PlayerVoid = playerVoid;
        MeleeDamage = meleeDamage;
        MeleeLifeLeech = meleeLifeLeech;
        MeleeVoidLeech = meleeVoidLeech;
        ProjectileDamage = projectileDamage;

        MovementSpeed = movementSpeed;
        ExtraJumpsLeft = extraJumps;
        DashCooldown = dashCooldown;
        XScale = transform.localScale.x;

        Collectibles = new List<Collectible>();
        TalentPoints = new List<TalentPoint>();

        Animator = GetComponent<Animator>();

        ChangeHealth(0);//Så att spelet kollar om man har lite HP vid start
        PlayerBounceEvent.RegisterListener(OnBounce);
        PlayerTakeDamageEvent.RegisterListener(OnTakeDamage);
        PlayerHealEvent.RegisterListener(OnHeal);
        PlayerVoidChangeEvent.RegisterListener(OnVoidEvent);
        PlayerTouchKillzoneEvent.RegisterListener(OnTouchKillzone);
        PlayerGetAbilityEvent.RegisterListener(OnGetAbility);
        PlayerCollectibleChangeEvent.RegisterListener(OnChangeCollectible);
        TalentPointGainEvent.RegisterListener(OnGainTalentPoint);
        FadeScreenEvent.RegisterListener(OnFadeScreen);
        PlayerBusyEvent.RegisterListener(OnPlayerBusyEvent);
        MainMenuEvent.RegisterListener(OnMainMenuSwitch);
        base.Awake();

        if (FindObjectOfType<DataStorage>())
        {
            DataStorage.Instance.LoadPlayerData();
            transform.position = GameController.Instance.RestingCheckpoint;
        }

    }

    private void Start() {

    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        CanvasRotation();
    }

    protected override void Update() {
        base.Update();
        TEST();//TEST
        InvulnerableTimeCheck();
        Animator.SetInteger("State", (int)PlayerState);
    }

    public void SpawnWalkDust() {
        GameObject walkDust = Instantiate(walkParticle, GroundCheck.position, Quaternion.identity, GroundCheck);
    }

    private void TEST() {
        if (Input.GetKeyDown(KeyCode.F1)) {//TEST START________________________________________________
            PlayerTakeDamageEvent ptde = new PlayerTakeDamageEvent {//
                damage = 100//
            };//
            ptde.FireEvent();//
        }//TEST
        if (Input.GetKeyDown(KeyCode.F2)) {//
            foreach (PlayerAbility ability in playerAbilities) {//
                PlayerAbilities.Add(ability);//
            }//
            PlayerGetAbilityEvent voidAbility = new PlayerGetAbilityEvent {
                playerAbility = PlayerAbility.VOIDMEND
            };
            voidAbility.FireEvent();
        }//
        if (Input.GetKeyDown(KeyCode.F3)) {//
            Collectible lifeForce = new Collectible(CollectibleType.LIFEFORCE, 1000);//
            Collectible voidEssence = new Collectible(CollectibleType.VOIDESSENCE, 10);//
            PlayerCollectibleChangeEvent gainCollectibleEvent = new PlayerCollectibleChangeEvent {//
                collectible = lifeForce//
            };//
            PlayerCollectibleChangeEvent gainCollectibleEvent2 = new PlayerCollectibleChangeEvent {//
                collectible = voidEssence//
            };//
            gainCollectibleEvent.FireEvent();//
            gainCollectibleEvent2.FireEvent();//
        }//
        if (Input.GetKeyDown(KeyCode.F4)) {//
            VoidTalentScreenEvent e = new VoidTalentScreenEvent { };//
            e.FireEvent();//
        }//
        if (Input.GetKeyDown(KeyCode.F10)){//
            InGameMenuEvent e = new InGameMenuEvent { };//
            e.FireEvent();//
        }//
        if (Input.GetKeyDown(KeyCode.F11)){//
            ScreenCapture.CaptureScreenshot("ScreenShot" + screenShot + ".png");//
            screenShot++;//
        }//
        PlayerVelocity = Rb2D.velocity;//
        health = Health;//TEST END_____________________________________________________________________
    }

    protected void CanvasRotation() {
        if (FacingDirection == -1 && PlayerCanvas.eulerAngles.y == 0) {
            PlayerCanvas.Rotate(new Vector3(0, 180, 0));
        }
        if (FacingDirection == 1 && PlayerCanvas.eulerAngles.y == 180) {
            PlayerCanvas.Rotate(new Vector3(0, -180, 0));
        }
    }

    private void OnFadeScreen(FadeScreenEvent screenEvent) {
        if (screenEvent.isFadeOut) {
            HideTipTextEvent hideTextEvent = new HideTipTextEvent { };
            hideTextEvent.FireEvent();
        }
        if (screenEvent.isFadeIn) {
            MovementIsStopped = false;
        }
        
    }

    private void OnPlayerBusyEvent(PlayerBusyEvent busyEvent) {
        if (busyEvent.playerIsBusy) {
            Transition<PlayerBusyState>();
        } else {
            if (!IsGrounded) {
                Transition<PlayerAirState>();
            } else {
                if(XInput != 0) {
                    Transition<PlayerWalkState>();
                } else {
                    Transition<PlayerIdleState>();
                }
            }
        }
    }


    private void OnTouchKillzone(PlayerTouchKillzoneEvent killzone) {
        if (IsTotallyInvulnerable) {
            return;
        }
        CameraShakeEvent cameraShake = new CameraShakeEvent {
            startDuration = cameraShakeDuration,
            startValue = cameraShakeValue
        };
        cameraShake.FireEvent();
        if (IsInvulnerable) {
            StartRespawn();
            return;
        }
        ChangeHealth(-killzone.damage);
        if (DamageWasDeadly()) {
            Die();
            return;
        }
        StartRespawn();
    }

    private void StartRespawn() {
        IsTotallyInvulnerable = true;
        Transition<PlayerHurtState>();
        UntilInvulnerableEnds = invulnerableStateTime;
        Rb2D.velocity = new Vector2(0, 0);
        Invoke("Respawn", 0.5f);
    }

    private void Respawn() {
        try {
            transform.position = GameController.Instance.SceneCheckpoint;
        } catch (UnassignedReferenceException) {
            Debug.LogError("No 'SceneCheckpoint' assigned in GameController to be able to respawn!");
        }
        IsTotallyInvulnerable = false;
    }

    private void OnTakeDamage(PlayerTakeDamageEvent eventDamage) {
        if (IsTotallyInvulnerable) {
            return;
        }
        if (!eventDamage.isSelfInflicted) {
            if (IsInvulnerable) {
                return;
            }
            eventDamage.damage -= DamageReduction;
            if (eventDamage.damage <= 0) {
                eventDamage.damage = 0;
            }
            CameraShakeEvent cse = new CameraShakeEvent {
                startDuration = cameraShakeDuration,
                startValue = cameraShakeValue
            };
            cse.FireEvent();
            KnockBack(eventDamage.enemyPosition, 2);
            Transition<PlayerHurtState>();
        }       
        ChangeHealth(-eventDamage.damage);
        if (DamageWasDeadly()) {
            Die();
            return;
        }  
    }

    private void OnVoidEvent(PlayerVoidChangeEvent voidEvent) {
        PlayerVoid += voidEvent.amount;
        if (PlayerVoid >= MaxPlayerVoid && voidEvent.amount > 0) {
            voidEvent.amount = 0;
            PlayerVoid = MaxPlayerVoid;
            if (!voidMendIsFull) {
                PlayerVoidIsFullEvent voidFull = new PlayerVoidIsFullEvent { };
                voidFull.FireEvent();
            }
            if (!voidMendIsFull) {
                PlaySound("VoidMendFull");
            }
            voidMendIsFull = true;
            //enable partikel-effekt om den är disabled
        }
        if(PlayerVoid < MaxPlayerVoid) {
            voidMendIsFull = false;
            //disabla partikel-effekt om den är enabled
        }
    }

    private void KnockBack(Vector3 enemyPosition, float amount) {
        int direction = 1;
        if (enemyPosition.x > transform.position.x) {
            direction = -direction;
        }
        Rb2D.velocity = new Vector2(hurtKnockbackForce.x*direction, hurtKnockbackForce.y);
    }

    private void OnHeal(PlayerHealEvent eventHeal) {
        if (eventHeal.isLifeLeech) {
            eventHeal.amount = MeleeLifeLeech;
        }
        ChangeHealth(eventHeal.amount);
        if (Health > MaxHealth) {
            Health = MaxHealth;
        }
    }

    public void ChangeHealth(float amount) {
        Health += amount;
        if(Health <= 25 && Health >= 0) {
            if (!lowHealthSoundIsPlaying) {
                AudioPlaySoundEvent startLowHealthSound = new AudioPlaySoundEvent {
                    name = "LowHealth",
                    soundType = SoundType.SFX
                };
                startLowHealthSound.FireEvent();
                lowHealthSoundIsPlaying = true;
            }
        } else {
            if (lowHealthSoundIsPlaying) {
                StopSound("LowHealth");
                lowHealthSoundIsPlaying = false;
            }
        }
    }

    public bool DamageWasDeadly() {
        if (Health <= 0) {
            Health = 0;
            return true;
        }
        return false;
    }

    private void InvulnerableTimeCheck() {
        if(UntilInvulnerableEnds <= 0) {
            IsInvulnerable = false;
        }
        if (IsInvulnerable) {
            UntilInvulnerableEnds -= Time.deltaTime;
            return;
        }
        UntilInvulnerableEnds = invulnerableStateTime;
    }

    

    private void Die() {
        if (!IsDead) {
            Transition<PlayerDeathState>();
        }   
    }

    public void PlaySound(string sound) {
        string soundName = GetSoundName(sound);
        AudioPlaySoundEvent soundEvent = new AudioPlaySoundEvent {
            name = soundName,
            soundType = SoundType.SFX,
            isRandomPitch = true,
            minPitch = 0.95f,
            maxPitch = 1f
        };
        soundEvent.FireEvent();
    }

    private void StopSound(string sound) {
        string soundName = GetSoundName(sound);
        AudioStopSoundEvent stopSound = new AudioStopSoundEvent {
            name = soundName
        };
        stopSound.FireEvent();
    }

    private string GetSoundName(string sound) {
        switch (sound) {
            case "Walk":
                return "Step1";
            case "VoidMendFull":
                return "VoidMendFull";
            case "LowHealth":
                return "LowHealth";
            default:
                return null;
        }
    }

    private void OnGetAbility(PlayerGetAbilityEvent abilityEvent) {
        if (!HasAbility(abilityEvent.playerAbility)) {
            PlayerAbilities.Add(abilityEvent.playerAbility);
            AudioPlaySoundEvent abilitySound = new AudioPlaySoundEvent {
                name = "AbilityGain",
                isRandomPitch = false,
                soundType = SoundType.MUSIC
            };
            abilitySound.FireEvent();
        } else {
            PlayerLog("Already has " + abilityEvent.playerAbility.ToString());
        }       
    }

    public bool HasAbility(PlayerAbility playerAbility) {
        for(int i = 0; i < PlayerAbilities.Count; i++) {
            if(PlayerAbilities[i] == playerAbility) {
                return true;
            }
        }
        return false;
    }

    private void OnChangeCollectible(PlayerCollectibleChangeEvent collectibleEvent) {
        for(int i = 0; i < Collectibles.Count; i++) {
            if(Collectibles[i].collectibleType == collectibleEvent.collectible.collectibleType) {
                Collectibles[i].amount += collectibleEvent.collectible.amount;
                return;
            }
        }
        Collectibles.Add(collectibleEvent.collectible);
    }

    private void OnGainTalentPoint(TalentPointGainEvent talentPointEvent) {
        TalentPoints.Add(talentPointEvent.talentPoint);
        AddTalentPointEffect(talentPointEvent.talentPoint);
    }

    private void AddTalentPointEffect(TalentPoint talentPoint) {
        switch (talentPoint.talentPointType) {
            case TalentPointType.DAMAGE:
                TalentMeleeDamage += talentPoint.variablesToChange[0].amount;
                TalentProjectileDamage += talentPoint.variablesToChange[1].amount;
                MeleeDamage = meleeDamage + TalentMeleeDamage;
                ProjectileDamage = projectileDamage + TalentProjectileDamage;
                break;
            case TalentPointType.SURVIVAL:
                TalentHealth += talentPoint.variablesToChange[0].amount;
                TalentLifeLeech += talentPoint.variablesToChange[1].amount;
                MaxHealth = maxHealth + TalentHealth;
                MeleeLifeLeech = meleeLifeLeech + TalentLifeLeech;
                break;
            case TalentPointType.SPEED:
                TalentMovementSpeed += talentPoint.variablesToChange[0].amount;
                TalentDashCooldown += talentPoint.variablesToChange[1].amount;
                MovementSpeed = movementSpeed + TalentMovementSpeed;
                DashCooldown = dashCooldown - TalentDashCooldown;
                break;
            case TalentPointType.VOID:
                TalentPlayerVoid += talentPoint.variablesToChange[0].amount;
                TalentVoidLeech += talentPoint.variablesToChange[1].amount;
                MaxPlayerVoid = maxPlayerVoid + TalentPlayerVoid;
                MeleeVoidLeech = meleeVoidLeech + TalentVoidLeech;
                break;
        }
    }

    public void PlayerLog(string message) {
        Debug.Log("PLAYER: " + message);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistanceValue, wallCheck.position.y, wallCheck.position.z));
    }

    private void OnBounce(PlayerBounceEvent bounceEvent) {
        Rb2D.AddForce(bounceEvent.amountOfForce, ForceMode2D.Impulse);
    }

    private void OnMainMenuSwitch(MainMenuEvent menuEvent)
    {
        Transition<PlayerBusyState>();
        exists = false;
        StopSound("LowHealth");
        Destroy(gameObject, 3f);
    }

    private void OnDestroy() {
        PlayerBounceEvent.UnRegisterListener(OnBounce);
        PlayerTakeDamageEvent.UnRegisterListener(OnTakeDamage);
        PlayerHealEvent.UnRegisterListener(OnHeal);
        PlayerVoidChangeEvent.UnRegisterListener(OnVoidEvent);
        PlayerTouchKillzoneEvent.UnRegisterListener(OnTouchKillzone);
        PlayerGetAbilityEvent.UnRegisterListener(OnGetAbility);
        PlayerCollectibleChangeEvent.UnRegisterListener(OnChangeCollectible);
        TalentPointGainEvent.UnRegisterListener(OnGainTalentPoint);
        FadeScreenEvent.UnRegisterListener(OnFadeScreen);
        PlayerBusyEvent.UnRegisterListener(OnPlayerBusyEvent);
        MainMenuEvent.UnRegisterListener(OnMainMenuSwitch);
    }

}
