using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject orbShot;
    [SerializeField]
    private float _laserOffset = 1f;
    [SerializeField]
    private float fireRate = 0.5f;
    private float _canFire = -1f;
    public int _health = 3;
    public int ammoCount = 15;


    //Script References
    private SpawnManager _spawnManager;
    [SerializeField]
    private CameraScript cameraReference;

    //THRUSTERS
    [SerializeField]
    private Image thrusterImage;
    [SerializeField]
    private float thrusterMultiplier = 2;
    [SerializeField]
    private float nextThrusterTime = 2;
    private float thrusterCount = 2;
    private float thrusterBar = 1;


    //  POWERUPS
    public bool tripleShotActive;
        public float tripleShotCooldown = 0f;
    public bool orbShotActive;
    public float orbShotCooldown = 5f;
    public bool speedBoostActive;
    public bool shieldActive = false;
        public int shieldHealth = 0;
        public int shieldPower = 3;
        [SerializeField]
        private GameObject[] shieldVisual = new GameObject[3];
    public bool battleRamActive;
        public float battleRamCooldown = 0f;
        public float battleRamSpeed = 10f;
        public GameObject battleRamVisual;
    public bool timeStopActive;
        [SerializeField]
        private float timeStopDuration = 3f;
    public int ammoPickupAmmount = 10;

    //Score n UI n Stuff
    public int score = 0;
    private UIManager _ui;
    [SerializeField]
    private GameObject damageVisualRight, damageVisualLeft;

    //Audio
    [SerializeField]
    private AudioSource fireLaserAudio;
    [SerializeField]
    private AudioSource noAmmoSound;
    [SerializeField]
    private AudioSource explosionSound;

    private void Start()
    {
        thrusterCount = nextThrusterTime;
        _ui = GameObject.Find("UI_Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("SpawnManager broke");
    }

    // Update is called once per frame
    void Update()
    {
        CalculatorMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            fireLaser();



        if (Input.GetKeyDown(KeyCode.P))
            cameraReference.CallCameraShake();

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
        if(!speedBoostActive && !battleRamActive) {
            Thrusters(direction);
        }


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

    private void Thrusters(Vector3 direction)
    {
        thrusterImage.fillAmount = (thrusterCount / nextThrusterTime);

        if(thrusterCount == nextThrusterTime) {
            thrusterImage.CrossFadeAlpha(0f, 0.4f, false);
        }

        print(thrusterCount);
        //Boosting
        if (Input.GetKey(KeyCode.LeftShift) && thrusterCount > 0) {
            if (thrusterCount != nextThrusterTime) {
                thrusterImage.CrossFadeAlpha(1f, 0.1f, false);
            }
            transform.Translate(direction * speed * thrusterMultiplier * Time.deltaTime);
            thrusterCount = thrusterCount - Mathf.Clamp(Time.deltaTime, 0, nextThrusterTime);
            print(thrusterCount);
        }
        //Letting go of key to recharge
        else if(!Input.GetKey(KeyCode.LeftShift) && thrusterCount < nextThrusterTime) {
            thrusterCount += Time.deltaTime;
            if (thrusterCount > nextThrusterTime)
                thrusterCount = nextThrusterTime;
            transform.Translate(direction * speed * Time.deltaTime);
            if (thrusterCount != nextThrusterTime) {
                thrusterImage.CrossFadeAlpha(1f, 0.1f, false);
            }
        }
        else {
            transform.Translate(direction * speed * Time.deltaTime);

        }
    }

    private void fireLaser()
    {
        _canFire = Time.time + fireRate;

        //tripple shot
        if (tripleShotActive) {
            Instantiate(tripleShot, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
            fireLaserAudio.Play();
        }
        //orbshot
        if (orbShotActive) {
            Instantiate(orbShot, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
            fireLaserAudio.Play();
        }
        //normal laser
        else {
            if (ammoCount > 0) {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserOffset, 0), Quaternion.identity);
                ammoCount -= 1;
                print("Ammo Count: " + ammoCount);
                _ui.updateAmmo(ammoCount);
                fireLaserAudio.Play();
            }
            else
                noAmmoSound.Play();

        }
    }


    ///////////////////
    ////Damage Here////
    ///////////////////
    public void Damage()
    {
        explosionSound.Play();

        cameraReference.CallCameraShake();

        if (battleRamActive)
            return;

        //SHIELD CONTROL
        if (shieldActive) {
            if (shieldHealth == 3) {
                shieldVisual[2].SetActive(false);
                shieldVisual[1].SetActive(true);
                shieldHealth -= 1;
                return;
            }
            if (shieldHealth == 2) {
                shieldVisual[1].SetActive(false);
                shieldVisual[0].SetActive(true);
                shieldHealth -= 1;
                return;
            }
            if (shieldHealth == 1) {
                shieldHealth -= 1;
                _ui.updateShield(shieldHealth);
                shieldActive = false;
                shieldVisual[0].SetActive(false);
                return;
            }
            if (shieldHealth <= 0) {
                shieldActive = false;
                return;
            }
        }

        _health -= 1;
        print("Player Health: " + _health);
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

    ////////////
    ////HEAL////
    ////////////
    public void Heal()
    {
        _health += 1;
        print("Player Health = " + _health);
//        _ui.UpdateLives(_health);

        if (_health == 3) {
            _ui.UpdateLives(3);
            damageVisualLeft.SetActive(false);
        }
            
        if (_health == 2) {
            _ui.UpdateLives(2);
            damageVisualRight.SetActive(false);
        }
    }

   
    ////////////////////
    ////AMMO PICKUP////
    ///////////////////
    public void AddAmmo(int ammo)
    {
        ammoCount += ammo;
        _ui.updateAmmo(ammoCount);
    }


    ////////////////////////////////////
    ////EVERYTHING BELOW IS POWERUPS////
    ////////////////////////////////////
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

    public void ActivateOrbShot()
    {
        orbShotActive = true;
        orbShotCooldown += 5f;
        StartCoroutine(OrbShotPowerDownRoutine());
    }
    IEnumerator OrbShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(orbShotCooldown);
        orbShotActive = false;
        orbShotCooldown = 0f;
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

    //Time Stop Found In
    public void ActivateTimeStop()
    {
        print("Time Stop Activated");
        timeStopActive = true;
        StartCoroutine(TimeStopPowerDownRoutine());
    }
    IEnumerator TimeStopPowerDownRoutine()
    {
        yield return new WaitForSeconds(3f);
        timeStopActive = false;
    }

    public void ActivateShield()
    {
        if(shieldHealth < 3) {

            foreach (var vis in shieldVisual)
                vis.SetActive(false);

            print("Shield Activated");
            shieldActive = true;
            shieldHealth += shieldPower;
            _ui.updateShield(shieldHealth);
            switch(shieldHealth) {
                case 1:
                    shieldVisual[0].SetActive(true);
                    break;
                case 2:
                    shieldVisual[1].SetActive(true);
                    break;
                case 3:
                    shieldVisual[2].SetActive(true);
                    break;
                default: print("Shield Visual Broke. 'Probably' Went Past Limit."); break;
            }
        }
    }

    public void addScore(int addedScore)
    {
        score += addedScore;
        _ui.updateScore(score);
    }
}
