using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
    public ChargeHolder playerCharge;
    public CanvasGroup hud;
    public CanvasGroup gameOver;
    public GameObject winMessage;
    public GameObject looseMessage;
    private enum GameStatus { InProgress, Won, Lost };
    private GameStatus gameStatus;

    void OnEnable()
    {
        EnemySupervisor.EnemiesDead += RegisterWin;
        if (playerCharge != null) playerCharge.Overcharged += RegisterLoss;
    }

    void OnDisable()
    {
        EnemySupervisor.EnemiesDead -= RegisterWin;
        if (playerCharge != null) playerCharge.Overcharged -= RegisterLoss;
    }

    private void RegisterWin()
    {
        gameStatus = GameStatus.Won;
        ShowGameoverScreen();
    }

    private void RegisterLoss()
    {
        gameStatus = GameStatus.Lost;
        ShowGameoverScreen();
    }

    private void ShowGameoverScreen() {
        hud.alpha = 0;
        winMessage.SetActive(gameStatus == GameStatus.Won);
        looseMessage.SetActive(gameStatus == GameStatus.Lost);
        gameOver.alpha = 1;
    }

    void Start () {
        gameStatus = GameStatus.InProgress;
        hud.alpha = 1;
        winMessage.SetActive(false);
        looseMessage.SetActive(false);
        gameOver.alpha = 0;
    }

}
