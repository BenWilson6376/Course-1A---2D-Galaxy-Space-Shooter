using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbLaser : Laser
{
    //on start, run ienumerator that spawn Orb_Shots
    //Need reference to orb shots prefab 

    [SerializeField]
    private GameObject orbShots;
    private float waitForShotTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireOrbShots());
    }

    IEnumerator FireOrbShots()
    {
        while(true) {
            Instantiate(orbShots, this.transform);

            yield return new WaitForSeconds(1f);
        }
    }

}
