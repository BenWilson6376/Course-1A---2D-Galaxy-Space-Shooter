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
    //4 = Time Stop
    //5 = Heal Pickup
    //6 = Ammo Pickup
    //7 = Orb Shot
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
                        if(player.shieldHealth <= 3)
                            player.ActivateShield();
                        break;
                    case 3:
                        player.ActivateBattleRam();
                        break;
                    case 4:
                        player.ActivateTimeStop();
                        break;
                    case 5:
                        if(player._health != 3)
                            player.Heal();
                        break;
                    case 6:
                        player.AddAmmo(player.ammoPickupAmmount);
                        print("Ammo Count: " + player.ammoCount);
                        break;
                    case 7:
                        //increases rarity of shot. Has a 1/4 chance of not spawning.
                        int shotRarity = Random.Range(0, 3);
                        if(shotRarity != 0) {
                            player.ActivateOrbShot();
                        }

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
