using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que herda o jogador para criar um obstáculo com movimentos.
/// </summary>
public class MovableAgent : Player
{
    [Header("Move Params")]
    [SerializeField] private GameObject[] waypoints; //Referências de objetos nos mapas para a movimentação circular do obstáculo.
    [SerializeField] private int currentDestination; //Objeto que é o destino atual.
    [SerializeField] private bool rotate; //Controla se o objeto irá rotacionar no próprio eixo ou não.

    void Update()
    {
        Rotate();
    }

    void FixedUpdate()
    {
        Movement();
    }

    /// <summary>
    /// Movimentação básica do obstáculo em circulos, definida pelos waypoints no mapa.
    /// </summary>
    override protected void Movement()
    {
        if (waypoints.Length > 0)
        {
            Vector3 moveDir = (waypoints[currentDestination].transform.position - transform.position);

            if (moveDir.magnitude <= moveSpeed)
            {
                transform.position = waypoints[currentDestination].transform.position;
                currentDestination++;
                if (currentDestination >= waypoints.Length)
                    currentDestination = 0;
            }
            else
            {
                transform.position += (moveDir * moveSpeed) * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Movimentação angular básica, onde o objeto irá girar em torno do próprio eixo.
    /// </summary>
    void Rotate()
    {
        if (rotate)
            transform.Rotate(new Vector3(0, 0, (1 * angularSpeed) * Time.deltaTime * (Screen.height + Screen.width / 2)), Space.Self);
    }
}
