using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameManager() { }

    public enum E_GameState
    { 
        TutorialToWaits,
        waitingToStart,
        countDownToStart,
        gamePlaying,
        overGame,
    }

    [SerializeField] private TutorialUI tutorialPanel;

    [SerializeField] private float gamePlayingTimerMax = 150f;
    private float waitingToStartTimer = 1f;
    private float countDownToStartTimer = 3f;
    private E_GameState gameState;
    private float gamePlayingTimer = 0f;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGameToggle;

    private bool isGameOver = false;
    private bool isGamePaused = true;
    public bool IsGamePaused => isGamePaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        isGamePaused = true;
        gameState = E_GameState.TutorialToWaits;
        if (DataManager.Instance.GetIsDontFirstPlay())
        {
            isGamePaused = false ;
            gameState = E_GameState.waitingToStart;
            Cursor.visible = false;
        }
        Time.timeScale = isGamePaused ? 0f : 1f;
        GameInput.Instance.OnPause += GameInput_OnPause;
    }

    private void GameInput_OnPause(object sender, EventArgs e)
    {
        ToggleGame();
    }

    private void Update()
    {
        if (isGameOver) return;
        switch (gameState)
        {
            //首次游玩,显示教程，等待玩家操作
            case E_GameState.TutorialToWaits:
                ShowTutorial();
                if (isGamePaused) return;
                Cursor.visible = false;
                Time.timeScale = 1f;
                gameState = E_GameState.waitingToStart;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                break;
            //延迟1秒进入倒计时状态，给玩家准备时间
            case E_GameState.waitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f)
                {
                    waitingToStartTimer = 1f;
                    gameState = E_GameState.countDownToStart;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
            //倒计时3秒进入游戏状态
            case E_GameState.countDownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer <= 0f)
                {
                    countDownToStartTimer = 3f;
                    gameState = E_GameState.gamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            //游戏状态持续180秒，进入游戏结束状态
            case E_GameState.gamePlaying:
                gamePlayingTimer += Time.deltaTime;
                if (gamePlayingTimer >= gamePlayingTimerMax)
                {
                    gamePlayingTimer = 0f;
                    gameState = E_GameState.overGame;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            //游戏结束状态，暂停游戏
            case E_GameState.overGame:
                isGameOver = true;
                Time.timeScale = 0f;
                Cursor.visible = true;
                break;
        }
    }

    private void ShowTutorial()
    {
        if (DataManager.Instance.GetIsDontFirstPlay()) return;
        tutorialPanel.Show();
        DataManager.Instance.SetIsDontFirstPlay(true);
    }

    public bool IsCountDownToStart()
    { 
        return gameState == E_GameState.countDownToStart;
    }

    public bool IsGamePlaying()
    {
        return gameState == E_GameState.gamePlaying;   
    }

    public bool IsGameOver()
    {
        return gameState == E_GameState.overGame;
    }

    public float GetCountDownTime()
    {
        return countDownToStartTimer;
    }

    public float ShowPlayTimeImg()
    {
        return gamePlayingTimer / gamePlayingTimerMax;
    }

    public float ShowPlayTimeTxt()
    {
        return gamePlayingTimerMax - gamePlayingTimer;
    }

    public void ToggleGame()
    {
        if (SettingUI.Instance.IsOpen || gameState == E_GameState.TutorialToWaits) return;
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        Cursor.visible = isGamePaused;
        OnGameToggle?.Invoke(this, EventArgs.Empty);
    }

    public void SetGamePause(bool isPasue)
    { 
        this.isGamePaused = isPasue;
    }

    public E_GameState GetGameState()
    { 
        return gameState;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
