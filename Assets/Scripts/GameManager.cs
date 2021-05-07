using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

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
    }

    void OnDisable()
    {
        Player.OnPlayerDied -= GameOver;
    }
    #endregion
}
