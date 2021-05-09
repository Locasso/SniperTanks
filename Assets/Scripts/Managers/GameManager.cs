using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// Classe manager do projeto, que realiza diversos métodos relacionados à mecânica ou utilitários.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject gameOverCanvas; //Referência do gameObject da tela de game over.
    [SerializeField] private Button playAgainBtn; //Referência do botão de jogar novamente.
    [SerializeField] private Button mainMenuBtn; //Referência do botão de retornar ao menu.
    [SerializeField] private Text turnFeedback; //Referência do texto de feedback do turno atual na tela e jogo.
    [SerializeField] private AudioSource audioSource; //Referência do audioSource da cena GameScene.
    [SerializeField] private AudioClip[] audioClips; //Referência de todos os SFX do jogo.

    [Header("References for turn control")]
    [SerializeField] private GameObject[] players; //Array que guarda todos os jogadores instanciados.
    public static short currentTurn; //COntrola de qual jogador é o turno atual.
    public static short turnMoment; //Controla qual é o momento do turno: 0 - Início, 1 - resolução ou 2 - fim.

    [Header("Special Bullets Control")]
    public static short fastBulletCount; //Controla o comportamento da fast bullet na lógica do jogo.

    [Header("Power Up Spawn")]
    [SerializeField] private GameObject[] powerUps; //Guarda todos os gameobjects de power ups diponíveis.
    [SerializeField] private GameObject powerUpsPlace; //Guarda os waypoints de spawn de powerups do mapa atual.
    public static short powerUpControl; //Controla quantos powerups podem haver na tela de jogo.

    public GameObject PowerUpsPlace { get => powerUpsPlace; set => powerUpsPlace = value; }

    public void StartGame()
    {
        ControlPlayers(true);
        currentTurn = (short)UnityEngine.Random.Range(0, players.Length); //Sorteio de quem começa jogando
        StartCoroutine(TurnSystem());
        StartCoroutine(SpawnPowerUps());
    }

    /// <summary>
    /// Método que controla os turnos do jogo.
    /// </summary>
    IEnumerator TurnSystem()
    {
        if (gameOverCanvas.activeInHierarchy == false)
        {
            if (turnMoment == 0) //Inicio de turno
            {
                for (int i = 0; i <= players.Length - 1; i++)
                {
                    if ((short)players[i].GetComponent<Player>().PlayerId != currentTurn)
                    {
                        players[i].GetComponent<Player>().CanMoveTurn = false;
                    }
                }
            }

            turnFeedback.text = $"Player {currentTurn + 1} turn";
            turnFeedback.GetComponent<Animator>().SetTrigger("feedback");

            yield return new WaitUntil(() => turnMoment == 1); //Resolução do turno

            for (int i = 0; i <= players.Length - 1; i++)
            {

                players[i].GetComponent<Player>().CanMoveTurn = false;
            }

            yield return new WaitUntil(() => turnMoment == 2); //Fim do turno

            for (int i = 0; i <= players.Length - 1; i++)
            {

                players[i].GetComponent<Player>().CanMoveTurn = true;
            }

            if (fastBulletCount != 1) //Controle especial da fast bullet
            {
                currentTurn++;
            }

            if (currentTurn > players.Length - 1)
            {
                currentTurn = 0;
            }

            turnMoment = 0;

            StartCoroutine(TurnSystem()); //Chama o próximo turno
        }
        else
        {
            ControlPlayers(false);
        }
    }

    /// <summary>
    /// Procura e toca o som que é passado pela string.
    /// </summary>
    void AudioPlay(string audioName)
    {
        audioSource.clip = Array.Find(audioClips, item => item.name == audioName);
        audioSource.Play();
    }

    /// <summary>
    /// Controles do jogo para os jogadores.
    /// </summary>
    void ControlPlayers(bool control)
    {
        for (int i = 0; i <= players.Length - 1; i++)
        {
            players[i].GetComponent<Player>().CanMoveTurn = control;
        }
    }

    /// <summary>
    /// Chama a tela de GameOver, e procura o jogador que está vivo para feedback em tela.
    /// </summary>
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

    /// <summary>
    /// Spawne power ups em um dos 10 pontos pré definidos randomicamente em cada mapa.
    /// </summary>
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
        while (count < randomTimer)
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
        Player.OnPlayerDied += GameOver; //Escuta quando um player perde.
        Bullet.OnSendSound += AudioPlay; //Escuta quando um som é invocado.
    }

    void OnDisable()
    {
        Player.OnPlayerDied -= GameOver;
        Bullet.OnSendSound -= AudioPlay;
    }
    #endregion
}
