using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public enum NumberPlayer
    {
        player_1,
        player_2,
        movable_agent
    }

    // Events
    public delegate void PlayerDied(int player);
    public static event PlayerDied OnPlayerDied;

    public delegate void ResolutionTurn(int turnMode);
    public static event ResolutionTurn OnResolutionTurn;

    [Header("Player Status")]
    [SerializeField] private NumberPlayer playerId;
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float angularSpeed;
    [SerializeField] private float startAngule;
    [SerializeField] private float moveLimit;
    [SerializeField] private float angularLimit;

    [Header("Player Status")]
    private bool canMoveTurn = true;

    [Header("References")]
    [SerializeField] private GameObject spriteObj;
    [SerializeField] private GameObject playerCannon;
    [SerializeField] private GameObject bulletObj, bulletParent;

    [Header("HUD References")]
    [SerializeField] private Text health_txt;

    public NumberPlayer PlayerId { get => playerId; set => playerId = value; }
    public bool CanMoveTurn { get => canMoveTurn; set => canMoveTurn = value; }

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

    protected virtual void Movement()
    {
        if (transform.position.y < Screen.height - moveLimit && transform.position.y > 0 + moveLimit)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector2.up * moveSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector2.down * moveSpeed);
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
                spriteObj.transform.Rotate(new Vector3(0, 0, 1 * angularSpeed), Space.Self);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                spriteObj.transform.Rotate(new Vector3(0, 0, -1 * angularSpeed), Space.Self);
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

    void Shoot(Vector2 pos, Quaternion rotation)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletObj, pos, rotation, bulletParent.transform);
            GameManager.turnMoment = 1;
        }
    }

    void OnReceiveDamage(int damageReceived, string name)
    {
        if (name == this.gameObject.transform.name)
        {
            health -= damageReceived;
            health_txt.text = health.ToString();
            if (health <= 0)
            {
                health_txt.text = 0.ToString();
                OnPlayerDied?.Invoke((int)playerId);
            }
        }
    }

    #region Inscrição e trancamento nos eventos
    void OnEnable()
    {
        Bullet.OnHitPlayer += OnReceiveDamage;
    }

    void OnDisable()
    {
        Bullet.OnHitPlayer -= OnReceiveDamage;
    }
    #endregion
}