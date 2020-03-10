using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyedAnimation : MonoBehaviour
{
    private AudioSource _explosionSound;
    private bool _audioHasPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        Animator animation = GetComponent<Animator>();
        animation.SetTrigger("OnEnemyDeath");
        _explosionSound = GetComponent<AudioSource>();
        if (_audioHasPlayed == false) {
            _explosionSound.Play();
            _audioHasPlayed = true;
        }
        Destroy(this.gameObject, 2.8f);
    }
}
