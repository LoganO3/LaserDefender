using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 1f;

    public void LoadGameOver()
    {
        StartCoroutine(DelayForSeconds());
    }
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScreen");
        FindObjectOfType<GameStatus>().ResetScore();
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScreen");
        FindObjectOfType<GameStatus>().ResetScore();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
IEnumerator DelayForSeconds()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("GameOver");
    }
}
