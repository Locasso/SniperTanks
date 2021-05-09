using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manager da cena e menu.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public static MenuManager instance; //Instância do objeto para persistência.

    [Header("Obj References")]
    [SerializeField] private Button playGameBtn; //Referência do botão de play.

    void Awake()
    {
        if (instance != null) //Controla a existência de uma única instância da classe.
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (playGameBtn != null)
            playGameBtn.onClick.AddListener(() => ChangeScene("GameScene"));
    }

    /// <summary>
    /// Método de controle de mudança de cena.
    /// </summary>
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
