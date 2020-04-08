using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerScript : MonoBehaviour
{
    //Attach this to a game object and set the flicker intervals in the inspector
    //Can be turned off with public flicker variable, default set to true

    public bool flicker = true;
    [SerializeField]
    private float flickerTime = 0.25f;

    private GameObject objectToFlicker;
    SpriteRenderer spriteToFlicker;

    // Start is called before the first frame update
    void Update()
    {
        if(flicker) {
            for (int i = 0; i < 1; i++) {
                StartCoroutine(flickerCoroutine());
            }
        }

    }

    IEnumerator flickerCoroutine()
    {
        while(flicker) {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(flickerTime);
            gameObject.GetComponent<SpriteRenderer>().enabled = gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(flickerTime);
        }
    }
}