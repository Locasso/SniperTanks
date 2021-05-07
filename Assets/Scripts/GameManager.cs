using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References for turn control")]
    [SerializeField] private GameObject[] players;

    [SerializeField] private static short currentTurn;
    public static short turnMoment;

    void Start()
    {
        currentTurn = (short)Random.Range(0, players.Length);
        StartCoroutine(TurnSystem());
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
            currentTurn++;

        turnMoment = 0;
        StartCoroutine(TurnSystem());
    }
    
    void GameOver(int playerId)
    {
        if (playerId == 0)
            Debug.Log("Player 2 Venceu");
        else
            Debug.Log("Player 1 Venceu");
    }

    #region Inscrição e trancamento nos eventos
    void OnEnable()
    {
        Player.OnPlayerDied += GameOver;
        // Player.ResolutionTurn +=
    }

    void OnDisable()
    {
        Player.OnPlayerDied -= GameOver;
    }
    #endregion
}
