using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/// <summary>
/// Chama que controla o comportamento da tela de seleção de bullets e de maps.
/// </summary>
public class PreparationsMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("HUD Objects")]
    [SerializeField] private GameObject explanationMenu; //Popup que abre com as informações de cada bala.
    [SerializeField] private Text explanationMenuName; //Referência do header de nome da popup.
    [SerializeField] private Text explanationMenuDescription; //Referência do campo de texto da popup.

    [Header("Control Objects")]
    [SerializeField] private GameObject[] bullets; //Guarda os prefabs de cada bullets, para controlar a seleção.
    [SerializeField] private GameObject[] maps; //GUarda o GameObject de cada mapa.
    [SerializeField] private RectTransform canvasPreparation; //Referência do rect transforma do canvas para posicionamento da popup.

    /// <summary>
    /// Delega a bullet escolhida a cada player.
    /// </summary>
    public void BulletChoose(GameObject player)
    {
        player.GetComponent<Player>().BulletObj = Array.Find(bullets, item => item.name ==
        EventSystem.current.currentSelectedGameObject.gameObject.name);
    }

    /// <summary>
    /// Ativa o mapa escolhido.
    /// </summary>
    public void MapChoose(GameObject map)
    {
        for (int i = 0; i <= map.transform.parent.transform.childCount - 1; i++)
        {
            map.transform.parent.transform.GetChild(i).gameObject.SetActive(false);
        }

        FindObjectOfType<GameManager>().PowerUpsPlace = map.transform.Find("power_ups").gameObject;
        map.SetActive(true);
    }

    /// <summary>
    /// Controla a seleção dos objetos nessa tela.
    /// </summary>
    public void SelectableBtn(GameObject buttons)
    {
        for (int i = 0; i <= buttons.transform.childCount - 1; i++)
        {
            buttons.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }

        EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// COntrole da posição de spawn e das informações no popup de bullets.
    /// </summary>
    void SpawnExplanation(PointerEventData eventData, string name, string description)
    {
        explanationMenu.SetActive(true);
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasPreparation, eventData.pointerCurrentRaycast.screenPosition,
        canvasPreparation.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out anchoredPos);
        explanationMenu.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        explanationMenuName.text = name;
        explanationMenuDescription.text = description;
    }

    /// <summary>
    /// Informação atribuída a cada botão de bullets.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "default_bullet")
        {
            SpawnExplanation(eventData, "Normal Bullet", "Damage: 30\nResist: 9\nSpeed: Normal\n\nNormal bullet, with great damage.");
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "fast_bullet")
        {
            SpawnExplanation(eventData, "Fast Bullet", "Damage: 14\nResist: 6\nSpeed: Fast\n\nFast bullet that you can shoot 2 times.");
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "resistent_bullet")
        {
            SpawnExplanation(eventData, "Resistent Bullet", "Damage: 26\nResist: 15\nSpeed: Normal\n\nResistent bullet that can ricochet a lot.");
        }
    }

    /// <summary>
    /// Desativa o PopUp
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        explanationMenu.SetActive(false);
    }
}
