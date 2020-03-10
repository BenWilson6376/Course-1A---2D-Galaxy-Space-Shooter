using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private AudioSource _sound;

    // Start is called before the first frame update
    void Start()
    {
        _sound = GetComponent<AudioSource>();
        _sound.Play();
        Destroy(this.gameObject, 5f);
    }
}
