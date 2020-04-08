using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float minSpeed = 2f;
    [SerializeField]
    private float maxSpeed = 6f;
    private float _randomSpeed;

    [SerializeField]
    private int killScore = 10;

    private Collider2D coll;

    private Player _playerReference;
    [SerializeField]
    private GameObject explosionAnim;

    [SerializeField]
    private GameObject enemyLaser;
    [SerializeField]
    private float laserFireSpeed = 10;
    [SerializeField]
    private float laserOffset = 1.1f;
    [SerializeField]
    private bool _canFire = true;
    private int fireRate;
    FlickerScript flickerScriptReference;

    private void Start()
    {
        flickerScriptReference = GetComponent<FlickerScript>();
        //        flickerScriptReference
        //figure out how to access flicker variable from flickerScript
        //flickerScriptReference = gameObject.GetComponent<FlickerScript>();
        flickerScriptReference.flicker = false;

        fireRate = Random.Range(0, 1);
        coll = GetComponent<Collider2D>();
        _randomSpeed = Random.Range(minSpeed, maxSpeed);
        _playerReference = GameObject.Find("Player").GetComponent<Player>();
        if (explosionAnim == null)
            print("Enemy Animator Broken");

        if (_canFire == true)
            InvokeRepeating("EnemyFireLaser", 0f, fireRate);

    }

    // Update is called once per frame
    void Update()
    {
        //Movement, gives random speed
        if (!_playerReference.timeStopActive) {
            flickerScriptReference.flicker = false;
            transform.Translate(Vector3.down * _randomSpeed * Time.deltaTime);
        }
        if (_playerReference.timeStopActive) {
            flickerScriptReference.flicker = true;
            transform.Translate(Vector3.down * _randomSpeed * Time.deltaTime * 0);
        }

        //Loop Screen
        if (transform.position.y < -7f) {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 8.5f, 0);
        }

        if (_canFire == false)
            fireRate = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) {
                player.Damage();
            }
            print("Ouchy");
            Instantiate(explosionAnim, transform.position, transform.rotation);
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _randomSpeed = 0f;
            
            Destroy(coll);
            _canFire = false;
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser") {
            Destroy(other.gameObject);
            if (_playerReference != null) {
                _playerReference.addScore(killScore);
                Instantiate(explosionAnim, transform.position, transform.rotation);
                gameObject.GetComponent<Collider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                _randomSpeed = 0;
               
                Destroy(coll);
                _canFire = false;
                Destroy(this.gameObject);
            }

        }
    }


    private void EnemyFireLaser()
    {
        GameObject laserInstance = Instantiate(enemyLaser, transform.position + new Vector3(0f, laserOffset), Quaternion.AngleAxis(180, Vector3.forward));
        laserInstance.GetComponent<EnemyLaser>().speed = _randomSpeed + laserFireSpeed;
    }
}