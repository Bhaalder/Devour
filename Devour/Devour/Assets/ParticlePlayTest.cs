using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayTest : MonoBehaviour
{
    public GameObject particleNorm, particleReverse;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        Instantiate(particleNorm, transform.position, Quaternion.identity);
        StartCoroutine(ReversePlay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CalledUpon()
    {
        Instantiate(particleNorm);
        StartCoroutine(ReversePlay());
    }
    IEnumerator ReversePlay()
    {
        yield return new WaitForSeconds(4);
        Instantiate(particleReverse, transform.position, Quaternion.identity);
        
    }
}
