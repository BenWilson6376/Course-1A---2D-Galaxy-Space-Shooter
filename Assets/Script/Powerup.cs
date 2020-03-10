using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;

    //ID for Powerups
    //0 = Triple Shot
    //1 = Speed
    //2 = Shields
    //3 = Battle Ram
    [SerializeField]
    private int powerupID;

    private AudioSource powerupSound;


    private void Start()
    {
        powerupSound = GameObject.Find("Powerup_Sound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        if (transform.position.y < -6)
            Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            powerupSound.Play();
            Player player = collision.transform.GetComponent<Player>();
            if(player != null) {
                switch(powerupID) {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShield();
                        break;
                    case 3:
                        player.ActivateBattleRam();
                        break;
                    default:
                        print("Invalid PowerUp!");
                        break;
                }
            }
                
            Destroy(this.gameObject);
        }
    }

}
