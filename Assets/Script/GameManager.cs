using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver == true) {
            SceneManager.LoadScene(1); //Loads current game scene
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }

    }

    public void GameOver()
    {
        isGameOver = true;
    }
}
