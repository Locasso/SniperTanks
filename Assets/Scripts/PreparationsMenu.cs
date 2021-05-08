using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class PreparationsMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("HUD Objects")]
    [SerializeField] private GameObject explanationMenu;
    [SerializeField] private Text explanationMenuName;
    [SerializeField] private Text explanationMenuDescription;

    [Header("Control Objects")]
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private GameObject[] maps;
    [SerializeField] private RectTransform canvasPreparation;

    void Start()
    {

    }

    void Update()
    {

    }

    public void BulletChoose(GameObject player)
    {
        player.GetComponent<Player>().BulletObj = Array.Find(bullets, item => item.name ==
        EventSystem.current.currentSelectedGameObject.gameObject.name);
    }

    public void MapChoose(GameObject map)
    {
        for(int i = 0; i <= map.transform.parent.transform.childCount - 1; i++)
        {
            map.transform.parent.transform.GetChild(i).gameObject.SetActive(false);
        }

        map.SetActive(true);
    }

    public void SelectableBtn(GameObject buttons)
    {
        for(int i = 0; i <= buttons.transform.childCount - 1; i++)
        {
            buttons.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }

        EventSystem.current.currentSelectedGameObject.gameObject.GetComponent<Button>().interactable = false;
    }

    void SpawnExplanation(PointerEventData eventData, string name, string description)
    {
        explanationMenu.SetActive(true);
        Vector2 anchoredPos;
        // explanationMenu.transform.position = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasPreparation, eventData.pointerCurrentRaycast.screenPosition, Camera.main, out anchoredPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasPreparation, eventData.pointerCurrentRaycast.screenPosition, 
        canvasPreparation.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out anchoredPos);
        explanationMenu.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        explanationMenuName.text = name;
        explanationMenuDescription.text = description;      
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "default_bullet")
        {
            SpawnExplanation(eventData, "Normal Bullet", "Damage: 20\nResist: 10\nSpeed: Normal\n\nNormal bullet, for normals.");
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "fast_bullet")
        {
            SpawnExplanation(eventData, "Fast Bullet", "Damage: 9\nResist: 8\nSpeed: Fast\n\nFast bullet that you can shoot 2 times.");
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "resistent_bullet")
        {
            SpawnExplanation(eventData, "Resistent Bullet", "Damage: 16\nResist: 16\nSpeed: Normal\n\nResistent bullet that can ricochet a lot.");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        explanationMenu.SetActive(false);
    }

}
