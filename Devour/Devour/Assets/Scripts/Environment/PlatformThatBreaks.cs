using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformThatBreaks : MonoBehaviour
{
    public bool triggerFloor = false;
    public bool timerStart = false;

    [SerializeField] float aTimer = 0.0f;
    public GameObject thePlatform;

    public GameObject particles;

    private void Update()
    {
        if (triggerFloor == true)
        {
            TriggerFloor();
            triggerFloor = false;
        }
        if (timerStart)
        {
            aTimer += Time.deltaTime;
        }
        if(aTimer >= 3.0f)
        {
            thePlatform.SetActive(true);
            timerStart = false;
            aTimer = 0;
        }
    }

    void TriggerFloor()
    {
        GameObject instantiatedParticle = Instantiate(particles, null);
        instantiatedParticle.transform.position = transform.position;
        //Animation här
        StartCoroutine(PlatformBreak());
        //Destroy(this.gameObject);
        Debug.Log("triggered");
        
    }

    //Trigger the object's action when the player enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !triggerFloor)
        {
            triggerFloor = true;
        }
    }
    IEnumerator PlatformBreak()
    {
        Debug.Log("Ienumerator");
        
        yield return new WaitForSeconds(3);
        thePlatform.SetActive(false);
        timerStart = true;

    }

}
