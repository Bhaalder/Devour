using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    [Tooltip("Time in Seconds between Damage.")]
    [SerializeField] private float timeBetweenDamage;

    private bool canDamage;

    void Start()
    {
        canDamage = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && GetComponentInParent<Enemy>().CurrentAnimatorState("MeleeAttack") == true && canDamage == true)
        {
            GetComponentInParent<Enemy>().DoMeleeDamage();
            StartCoroutine(TimeBetweeenDamage());
        }
    }

    private IEnumerator TimeBetweeenDamage()
    {
        canDamage = false;
        yield return new WaitForSeconds(timeBetweenDamage);
        canDamage = true;
    }
}
