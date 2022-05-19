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

    [Header("Home")]
    public Canvas homeCanvas;
    public TextMeshProUGUI stageText;

    [Header("Game Playing")]
    public Canvas gamePlayingCanvas;
    public Button ballServeBtn;

    [Header("Game Pausing")]
    public Canvas gamePausingCanvas;

    [Header("Game Finished")]
    public Canvas gameFinishedCanvas;
    public TextMeshProUGUI stageClearedText;
    public TextMeshProUGUI gameOverText;


    private void Awake()
    {
        BrickPongManager.instance.onLevelChange += OnLevelChange;
        BrickPongManager.instance.onLifeChange += OnLifeChange;
        BrickPongManager.instance.onScoreChange += OnScoreChange;
        BrickPongManager.instance.onCurrentStateChange += OnCurrentStateChange;

        OnLevelChange(BrickPongManager.instance.level);
        OnLifeChange(BrickPongManager.instance.life);
        OnScoreChange(BrickPongManager.instance.score);
        OnCurrentStateChange(BrickPongManager.instance.currentState);
    }
    private void OnDestroy()
    {
        BrickPongManager.instance.onLevelChange -= OnLevelChange;
        BrickPongManager.instance.onLifeChange -= OnLifeChange;
        BrickPongManager.instance.onScoreChange -= OnScoreChange;
        BrickPongManager.instance.onCurrentStateChange -= OnCurrentStateChange;
    }


    public void OnLevelChange(int lv)
    {
        stageText.text = "Stage: " + (lv+1);
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
        homeCanvas.enabled = (state == BrickPongManager.GameState.home);
        gamePlayingCanvas.enabled = (state == BrickPongManager.GameState.playing || state == BrickPongManager.GameState.serving);
        gamePausingCanvas.enabled = (state == BrickPongManager.GameState.pausing);
        gameFinishedCanvas.enabled = (state == BrickPongManager.GameState.finished);

        //serving
        ballServeBtn.gameObject.SetActive(state == BrickPongManager.GameState.serving);

        //finished
        stageClearedText.enabled = (BrickPongManager.instance.breakableBrickCount <= 0);
        gameOverText.enabled = (BrickPongManager.instance.breakableBrickCount > 0);
    }
}
