using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckList : MonoBehaviour
{
    [Header("Player")]
    public string[] atributos_player;
    public bool movimento_liner;
    public bool movimento_angular;
    public bool atirar;
    public bool colisao_projetil;
    public bool perda_vida;

    [Header("Projétil")]
    public string[] atributos_projetil;
    public bool movimento_linear_projetil;
    public bool colisao_cenario;
    public bool colisao_player;
    public bool resultado_colisao;

    [Header("GameLogic")]
    public bool definicao_estrutura_turnos;
    public bool resolucao_turnos;
    public bool passagem_de_turnos;
    public bool interacao_dois_jogadores;
    public bool logica_gameOver;

    [Header("Fases")]
    public string[] design_fases;
    public bool objetos_estaticos;
    public bool transicao_fases;

    [Header("Audios")]
    public string lista_audios;
    public bool construcao_audiomanager;

    [Header("Telas")]
    public bool cena_game;
    public bool cena_menu;
    public bool game_over;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
