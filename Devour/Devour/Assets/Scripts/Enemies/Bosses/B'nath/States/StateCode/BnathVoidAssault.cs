using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/VoidAssaultState")]

public class BnathVoidAssault : BnathBaseState
{
    [SerializeField] GameObject voidGround;
    [SerializeField] private float hangTime = 3f;

    private int voidGroundLocation;
    private float startCooldown = 0.7f;
    private float betweenCooldown = 0.7f;
    private float currentCooldown;

    public override void Enter()
    {
        owner.State = BossBnathState.VOID_ASSAULT;

        currentCooldown = hangTime;
        startCooldown = 1f;
        owner.rb.velocity = new Vector2(0, 0);

        voidGroundLocation = owner.GetComponent<Bnath>().VoidGroundLocation.Length;
        int safeLocation = (int)Random.Range(0, voidGroundLocation-1);

        foreach (GameObject gameObject in owner.GetComponent<Bnath>().VoidGroundLocation)
        {
            int location = System.Array.IndexOf(owner.GetComponent<Bnath>().VoidGroundLocation, gameObject);
            if (location == safeLocation)
            {

            }
            else
            {
                GameObject voidG = Instantiate(voidGround, null);
                voidG.transform.position = gameObject.transform.position;
                voidG.GetComponent<VoidGround>().StartCooldown = startCooldown;
                startCooldown += betweenCooldown;
            }
        }
        owner.PlayVoice("Chant");
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
        HangTime();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }

    private void HangTime()
    {
        currentCooldown -= Time.deltaTime;

        if (currentCooldown > 0)
        {
            return;
        }

        owner.Transition<BnathClimbDash>();

    }
}
