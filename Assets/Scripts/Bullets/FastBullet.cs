using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Herda a classe de Bullet e cria uma bullet que gera 2 turnos ao jogador.
/// </summary>
public class FastBullet : Bullet
{
    /// <summary>
    /// Modifica o TUrnControl para atingir o propósito da classe.
    /// </summary>
    protected override void TurnControl(short id)
    {
        GameManager.fastBulletCount++;
        GameManager.turnMoment = id;
        Debug.Log("FastBullet" + GameManager.fastBulletCount);
        if (GameManager.fastBulletCount == 2)
        {
            GameManager.fastBulletCount = 0;
        }
    }
}
