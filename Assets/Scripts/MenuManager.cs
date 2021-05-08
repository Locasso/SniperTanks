using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Obj References")]
    [SerializeField] private Button playGameBtn;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
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

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
