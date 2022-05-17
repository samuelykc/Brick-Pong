using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BrickPongManagerUI : MonoBehaviour
{
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI stageClearedText;
    public TextMeshProUGUI gameOverText;


    private void Awake()
    {
        BrickPongManager.instance.onLifeChange += OnLifeChange;
        BrickPongManager.instance.onScoreChange += OnScoreChange;
        BrickPongManager.instance.onCurrentStateChange += OnCurrentStateChange;

        OnLifeChange(BrickPongManager.instance.life);
        OnScoreChange(BrickPongManager.instance.score);
        OnCurrentStateChange(BrickPongManager.instance.currentState);
    }
    private void OnDestroy()
    {
        BrickPongManager.instance.onLifeChange -= OnLifeChange;
        BrickPongManager.instance.onScoreChange -= OnScoreChange;
    }


    public void OnLifeChange(int life)
    {
        lifeText.text = "Life: " + life;
    }
    public void OnScoreChange(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void OnCurrentStateChange(BrickPongManager.GameState state)
    {
        stageClearedText.gameObject.SetActive(state == BrickPongManager.GameState.finished && BrickPongManager.instance.life>0);
        gameOverText.gameObject.SetActive(state == BrickPongManager.GameState.finished && BrickPongManager.instance.life<=0);
    }
}
