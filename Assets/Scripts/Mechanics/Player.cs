using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe base de cada Player.
/// </summary>
public class Player : MonoBehaviour
{
    public enum NumberPlayer //ID de cada jogador, e também de um objeto que possa herdar a classe.
    {
        player_1,
        player_2,
        movable_agent
    }

    // Events
    public delegate void PlayerDied(int player); //Invoca quando o campo Health do jogador chega a 0.
    public static event PlayerDied OnPlayerDied;

    [Header("Player Status")]
    [SerializeField] private NumberPlayer playerId; //Id de cada jogador em tela.
    [SerializeField] private int health; //Vida do jogador.
    [SerializeField] protected float moveSpeed; //Velocidade de movimentação.
    [SerializeField] protected float angularSpeed; //Velocidade do movimento angular.
    [SerializeField] private float startAngule; //Ângulo do sprite inicial
    [SerializeField] private float moveLimit; //Limite para controle da movimentação.
    [SerializeField] private float angularLimit; //Limite de movimento angular.

    [Header("Player Control")]
    private bool canMoveTurn; //Controla se o jogador pode se movimentar

    [Header("References")]
    [SerializeField] private GameObject spriteObj; //Referência do sprite do jogador
    [SerializeField] private GameObject playerCannon; //Referência do objeto que é a referência e posição do tiro do jogador.
    [SerializeField] private GameObject bulletObj, bulletParent; //Referência da bullet e do GameObject que vai guardar as bullets em c

    [Header("HUD References")]
    [SerializeField] private Text health_txt; //Referência da HUD de vida do jogador.

    public NumberPlayer PlayerId { get => playerId; set => playerId = value; }
    public bool CanMoveTurn { get => canMoveTurn; set => canMoveTurn = value; }
    public GameObject BulletObj { get => bulletObj; set => bulletObj = value; }

    void Start()
    {
        if (spriteObj != null)
            startAngule = spriteObj.transform.rotation.eulerAngles.z;
    }
    void Update()
    {
        if (canMoveTurn)
        {
            Movement();

            if (playerCannon != null)
                Shoot(new Vector2(playerCannon.transform.position.x, playerCannon.transform.position.y), playerCannon.transform.rotation);
        }
    }

    /// <summary>
    /// Controla a movimentação linear e angular do jogador.
    /// </summary>
    protected virtual void Movement()
    {
        if (transform.position.y < Screen.height - moveLimit && transform.position.y > 0 + moveLimit)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate((Vector2.up * moveSpeed) * Time.deltaTime * (Screen.height + Screen.width / 2));
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate((Vector2.down * moveSpeed) * Time.deltaTime * (Screen.height + Screen.width / 2));
            }
        }
        else
        {
            if (transform.position.y >= Screen.height - moveLimit)
                transform.position = new Vector2(transform.position.x, (Screen.height - moveLimit) - 1);
            else if (transform.position.y <= 0 + moveLimit)
                transform.position = new Vector2(transform.position.x, moveLimit + 0.1f);
        }

        if (spriteObj.transform.rotation.eulerAngles.z < startAngule + angularLimit
        && spriteObj.transform.rotation.eulerAngles.z > startAngule - angularLimit)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                spriteObj.transform.Rotate(new Vector3(0, 0, 1 * angularSpeed * Time.deltaTime * (Screen.height + Screen.width / 2)), Space.Self);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                spriteObj.transform.Rotate(new Vector3(0, 0, -1 * angularSpeed * Time.deltaTime * (Screen.height + Screen.width / 2)), Space.Self);
            }
        }
        else
        {
            if (spriteObj.transform.rotation.eulerAngles.z >= startAngule + angularLimit)
            {
                spriteObj.transform.Rotate(new Vector3(0f, 0f, -1f));
            }
            if (spriteObj.transform.rotation.eulerAngles.z <= startAngule - angularLimit)
            {
                spriteObj.transform.Rotate(new Vector3(0f, 0f, 1));
            }
        }
    }

    /// <summary>
    /// Instancia as bullets quando o jogador atirar.
    /// </summary>
    void Shoot(Vector2 pos, Quaternion rotation)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletObj, pos, rotation, bulletParent.transform);
            GameManager.turnMoment = 1;
        }
    }

    /// <summary>
    /// O que acontece quando o jogador é atingido por uma bullet.
    /// </summary>
    void OnReceiveDamage(int damageReceived, string name)
    {
        if (name == this.gameObject.transform.name)
        {
            health -= damageReceived;
            health_txt.text = health.ToString();
            if (health <= 0)
            {
                spriteObj.GetComponent<Image>().color = Color.black;
                health_txt.text = 0.ToString();
                OnPlayerDied?.Invoke((int)playerId);
            }
        }
    }

    /// <summary>
    /// O que acontece quando o jogador pega um powr up.
    /// </summary>
    void OnTakePowerUp(string objName)
    {
        if (GameManager.currentTurn == (int)playerId)
            if (objName.Contains("life"))
            {
                health += 15;
                health_txt.text = health.ToString();
            }
    }

    #region Inscrição e trancamento nos eventos
    void OnEnable()
    {
        Bullet.OnHitPlayer += OnReceiveDamage;
        Bullet.OnHitPowerUp += OnTakePowerUp;
    }

    void OnDisable()
    {
        Bullet.OnHitPlayer -= OnReceiveDamage;
        Bullet.OnHitPowerUp -= OnTakePowerUp;
    }
    #endregion
}