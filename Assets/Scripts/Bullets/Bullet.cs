using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// Clase base do comportamento das bullets do jogo.
/// </summary>
public class Bullet : MonoBehaviour
{
    //  Events
    public delegate void HitPlayer(int damage, string objName); //Invoca toda vez que um player é atingido.
    public static event HitPlayer OnHitPlayer;

    public delegate void HitPowerUp(string objName); //Invoca toda vez que um power up é coletado.
    public static event HitPowerUp OnHitPowerUp;

    public delegate void SendSound(string soundName); //Incova toda vez que deve ser tocado um sfx.
    public static event SendSound OnSendSound;

    [Header("Bullet Attributes")]
    [SerializeField] protected float movementSpeed; //Velocidade de movimento da bullet.
    [SerializeField] protected float stepDistance; //Distância de contato entre o centro do objeto e os obstáculos/player.
    [SerializeField] protected float maxReflection; //Quantidade de vezes que a bullet pode ricochetear.
    [SerializeField] protected int damage; //Dano causado.

    [Header("Control parameters")]
    Vector3 direction; //Direção do movimento.
    RaycastHit2D hit; //Referência de raycast para a colisão.
    bool stop; //Controle de quando deve movimentar ou parar.

    private void Start()
    {
        direction = transform.up;
        hit = Physics2D.Raycast(transform.position, direction, stepDistance);
        OnSendSound?.Invoke("shoot");
    }

    void Update()
    {
        Movement();
    }

    /// <summary>
    /// Conrola a movimentação e os contatos da bullet.
    /// </summary>
    protected virtual void Movement()
    {
        if (!stop)
        {
            transform.position += (direction * movementSpeed) * Time.deltaTime * (Screen.height + Screen.width / 2); //Movimentação

            hit = Physics2D.Raycast(transform.position, direction, stepDistance);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag != null && hit.collider.gameObject.tag == "player") //Colidiu com player
                {
                    OnHitPlayer?.Invoke(damage, hit.collider.gameObject.transform.parent.name);
                    Debug.Log(hit.collider.gameObject.transform.parent.name);
                    OnSendSound?.Invoke("hit");
                    TurnControl(2);
                    DestroyBullet(.47f);
                }
                else if (hit.collider.gameObject.tag != null && hit.collider.gameObject.tag == "power_up") //Colidiu com power_up
                {
                    OnHitPowerUp?.Invoke(hit.collider.gameObject.name);
                    OnSendSound?.Invoke("life");
                    GameManager.powerUpControl--;
                    Destroy(hit.collider.gameObject);
                }
                else //Colidiu em paredes ou obstáculos.
                {
                    Vector3 newDirect = Vector3.Reflect(transform.up, hit.normal);
                    direction = newDirect;
                    float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    maxReflection--;
                    OnSendSound?.Invoke("reflect");
                    if (maxReflection <= 0)
                    {
                        OnSendSound?.Invoke("miss");
                        GameManager.turnMoment = 1;
                        TurnControl(2);
                        DestroyBullet(.47f);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Método para setar o momento do turno no GameManager.
    /// </summary>
    protected virtual void TurnControl(short id)
    {
        GameManager.turnMoment = id;
    }

    /// <summary>
    /// Método que controla o destruir do gameobject bullet.
    /// </summary>
    protected virtual void DestroyBullet(float seconds)
    {
        stop = true;
        this.gameObject.GetComponentInChildren<Image>().color = Color.white;
        this.gameObject.GetComponent<Animator>().SetTrigger("destroy");
        Destroy(gameObject, seconds);
    }
}
