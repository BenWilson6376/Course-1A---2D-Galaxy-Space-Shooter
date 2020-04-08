using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    /*    [SerializeField]
        private Text shieldText; */
    [SerializeField]
    private Text ammoCount;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text restartKeyText;
    private bool _restartGame = false;

    [SerializeField]
    private Image livesImage;
    [SerializeField]
    private Sprite[] livesSprites;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + player.score;
        //shieldText.text = "Shield Strength: " + player.shieldHealth;
        ammoCount.text = "Ammo Count: " + player.ammoCount;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
            print("GameManager Broke");
    }

    public void updateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void updateShield(int shield)
    {
        //shieldText.text = "Shield Strength: " + shield.ToString();
    }

    public void updateAmmo(int ammo)
    {
        ammoCount.text = "Ammo Count: " + ammo.ToString();
    }

    public void UpdateLives(int currentLives)
    { 
        livesImage.sprite = livesSprites[currentLives];
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        restartKeyText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    IEnumerator GameOverFlicker()
    {
        while(true) {
            gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
