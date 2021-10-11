using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayScore : MonoBehaviour
{

    TextMeshProUGUI scoreText;
    GameStatus gameStatus;

    // Use this for initialization
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        gameStatus = FindObjectOfType<GameStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gameStatus.GetScore().ToString();
    }
}
