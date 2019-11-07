using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/B'nath/VoidAssaultState")]

public class BnathVoidAssault : BnathBaseState
{
    [SerializeField] GameObject voidGround;

    private int voidGroundLocation;

    public override void Enter()
    {
        owner.State = BossBnathState.VOID_ASSAULT;

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
            }
        }
        owner.Transition<BnathClimbDash>();
    }

    public override void HandleUpdate()
    {
        base.HandleUpdate();
    }
    public override void HandleFixedUpdate()
    {
        base.HandleFixedUpdate();
    }
}
