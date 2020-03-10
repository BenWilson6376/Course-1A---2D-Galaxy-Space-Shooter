using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float speedBoostSpeed = 20;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject tripleShot;
    [SerializeField]
    private float _laserOffset = 1f;
    [SerializeField]
    private float fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _health = 3;
    private SpawnManager _spawnManager;

    public bool tripleShotActive;
    public float tripleShotCooldown = 0f;
    public bool speedBoostActive;
    public bool shieldActive = false;
    public int shieldHealth = 0;
    public int shieldPower = 3;
    [SerializeField]
    private GameObject shieldVisual;
    public bool battleRamActive;
        public float battleRamCooldown = 0f;
        public float battleRamSpeed = 10f;
        public GameObject battleRamVisual;

    public int score = 0;
    private UIManager _ui;
    [SerializeField]
    private GameObject damageVisualRight, damageVisualLeft;

    [SerializeField]
    private AudioSource fireLaserAudio;
    [SerializeField]
    private AudioSource explosionSound;

    private void Start()
    {
        _ui = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("SpawnManager broke");
    }

    // Update is called once per frame
    void Update()
    {
        CalculatorMovement();

        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
            fireLaser();
    }

    void CalculatorMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //SPEED BOOST STUFF
        if (speedBoostActive)
            transform.Translate(direction * speedBoostSpeed * Time.deltaTime);
        //BATTLE RAM STUFF
        if (battleRamActive)
            transform.Translate(direction * battleRamSpeed * Time.deltaTime);
        //If both powerups are off, do normal speed
        if(!speedBoostActive && !battleRamActive)
            transform.Translate(direction * speed * Time.deltaTime);


        if (transform.position.y >= 6) {
            transform.position = new Vector3(transform.position.x, 6, 0);
        }
        else if (transform.position.y <= -4) {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        if (transform.position.x >= 11.3) {
            //want to wrap back to around screen
            //y coordinate will be the same, z will be the same, x should be left side of the screen
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3) {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void fireLaser()
    {
        _canFire = Time.time + fireRate;

        if (tripleShotActive)
            Instantiate(tripleShot, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
        //IF TRIPLES SHOT FALSE, FIRE NORMAL LASER
        else
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);

        fireLaserAudio.Play();
    }

    ///////////////////
    ////Damage Here////
    ///////////////////
    public void Damage()
    {
        explosionSound.Play();

        if (battleRamActive)
            return;

        //SHIELD CONTROL
        if (shieldActive) {
            if (shieldHealth > 1) {
                shieldHealth -= 1;
                return;
            }
            if (shieldHealth == 1) {
                shieldHealth -= 1;
                shieldActive = false;
                shieldVisual.SetActive(false);
                return;
            }
            if (shieldHealth <= 0) {
                shieldActive = false;
                shieldVisual.SetActive(false);
                return;
            }
        }

        _health -= 1;
        print("Player Took Damage");
        _ui.UpdateLives(_health);
        //DEALING WITH GAMEOVER

        if (_health == 2)
            damageVisualLeft.SetActive(true);
        if (_health == 1)
            damageVisualRight.SetActive(true);

        if (_health < 1) {
            if (_spawnManager != null) {
                explosionSound.Play();
                _spawnManager.OnPlayerDeath();
            }
            Destroy(this.gameObject);
            _ui.GameOver();

        }
    }


    ////////////////
    ////Powerups////
    ////////////////
    public void ActivateTripleShot()
    {
        tripleShotActive = true;
        tripleShotCooldown += 5f;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(tripleShotCooldown);
        tripleShotActive = false;
        tripleShotCooldown = 0f;
    }

    //Speed Boost Found In Movement Method
    public void ActivateSpeedBoost()
    {
        print("Speed Boost Activated");
        speedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(3f);
        speedBoostActive = false;
    }

    //Battle Ram Found In Movement Method AND Damage Method
    public void ActivateBattleRam()
    {
        print("Battle Ram Activated!");
        battleRamActive = true;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        battleRamVisual.SetActive(true);
        StartCoroutine(BattleRamPowerDownRoutine());
    }
    IEnumerator BattleRamPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        battleRamActive = false;
        transform.GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        battleRamVisual.SetActive(false);
    }

    public void ActivateShield()
    {
        print("Shield Activated");
        shieldActive = true;
        shieldHealth += shieldPower;
        shieldVisual.SetActive(true);
    }

    public void addScore(int addedScore)
    {
        score += addedScore;
        _ui.updateScore(score);
    }
}
