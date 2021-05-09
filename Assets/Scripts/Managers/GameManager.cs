using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private Button playAgainBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Text turnFeedback;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    [Header("References for turn control")]
    [SerializeField] private GameObject[] players;
    public static short currentTurn;
    public static short turnMoment;

    [Header("Special Bullets Control")]
    public static short fastBulletCount;

    [Header("Power Up Spawn")]
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject powerUpsPlace;
    public static short powerUpControl;

    public GameObject PowerUpsPlace { get => powerUpsPlace; set => powerUpsPlace = value; }

    public void StartGame()
    {
        for (int i = 0; i <= players.Length - 1; i++)
        {

            players[i].GetComponent<Player>().CanMoveTurn = true;
        }

        currentTurn = (short)UnityEngine.Random.Range(0, players.Length);
        StartCoroutine(TurnSystem());
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator TurnSystem()
    {
        if (turnMoment == 0)
        {
            for (int i = 0; i <= players.Length - 1; i++)
            {
                //Inicio de turno
                if ((short)players[i].GetComponent<Player>().PlayerId != currentTurn)
                {
                    players[i].GetComponent<Player>().CanMoveTurn = false;
                }
            }
        }

        turnFeedback.text = $"Player {currentTurn + 1} turn";
        turnFeedback.GetComponent<Animator>().SetTrigger("feedback");

        yield return new WaitUntil(() => turnMoment == 1);

        for (int i = 0; i <= players.Length - 1; i++)
        {

            players[i].GetComponent<Player>().CanMoveTurn = false;
        }

        yield return new WaitUntil(() => turnMoment == 2);

        for (int i = 0; i <= players.Length - 1; i++)
        {

            players[i].GetComponent<Player>().CanMoveTurn = true;
        }

        if (currentTurn >= players.Length - 1)
        {
            currentTurn = 0;
        }
        else
        {
            if (fastBulletCount != 1)
            {
                currentTurn++;
            }
        }

        turnMoment = 0;
        StartCoroutine(TurnSystem());
    }

    void AudioPlay(string audioName)
    {
        audioSource.clip = Array.Find(audioClips, item => item.name == audioName);
        audioSource.Play();
    }

    void GameOver(int playerId)
    {
        gameOverCanvas.SetActive(true);
        playAgainBtn.onClick.AddListener(() => FindObjectOfType<MenuManager>().ChangeScene("GameScene"));
        mainMenuBtn.onClick.AddListener(() => FindObjectOfType<MenuManager>().ChangeScene("MenuScene"));

        for (int i = 0; i <= players.Length - 1; i++)
        {
            if ((int)players[i].GetComponent<Player>().PlayerId != playerId)
                gameOverCanvas.transform.Find("winner_txt").GetComponent<Text>().text =
                $"Player {(int)players[i].GetComponent<Player>().PlayerId + 1} wins!";
        }
    }

    IEnumerator SpawnPowerUps()
    {
        powerUpControl++;
        short randomPower = (short)UnityEngine.Random.Range(0, powerUps.Length);
        short randomPlace = (short)UnityEngine.Random.Range(0, powerUpsPlace.transform.childCount);
        GameObject powerUp = Instantiate(powerUps[randomPower], powerUpsPlace.transform.GetChild(randomPlace).transform.position, 
        powerUpsPlace.transform.GetChild(randomPlace).transform.rotation, powerUpsPlace.transform.GetChild(randomPlace).transform);
       
        yield return new WaitUntil(() => powerUpControl == 0);
               
        short randomTimer = (short)UnityEngine.Random.Range(15, 30);
        short count = 0;
        while(count < randomTimer)
        {
            count++;
            yield return new WaitForSeconds(1f);
        }
    
        yield return new WaitUntil(() => count >= randomTimer);
        StartCoroutine(SpawnPowerUps());
    }

    #region Inscrição e trancamento nos eventos
    void OnEnable()
    {
        Player.OnPlayerDied += GameOver;
        Bullet.OnSendSound += AudioPlay;
    }

    void OnDisable()
    {
        Player.OnPlayerDied -= GameOver;
        Bullet.OnSendSound -= AudioPlay;
    }
    #endregion
}
