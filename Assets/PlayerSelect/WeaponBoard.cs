using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class WeaponBoard : MonoBehaviour
{
    public static WeaponBoard Instance { get; private set; }
    public RectTransform RectTransfrom { get; private set; }

    [SerializeField]
    private Transform weaponCardPrefab;

    private float _scrollLimit;

    private const int SENSITIVITY = 1;
    private const int SCROLLPADDING = 80;

    private void Awake()
    {
        Instance = this;
        RectTransfrom = transform as RectTransform;
    }
    private void Start()
    {
        SpawnCards();
        ResizeWeaponBoard();
    }
    private void SpawnCards()
    {
        WeaponType[] allWeapons = Resources.LoadAll<WeaponType>("Weapons");
        Debug.Log(allWeapons.Length);
        foreach (WeaponType weapon in allWeapons)
        {
            var prefab = Instantiate(weaponCardPrefab, transform);
            var card = prefab.GetChild(0);
            card.GetComponent<WeaponCard>().represent = weapon;
            card.GetComponent<Image>().sprite = weapon.barrel;

            var energyTMP = prefab.GetChild(1).GetComponent<TextMeshProUGUI>();
            energyTMP.text = $"{weapon.energyCost}";

        }
    }
    private void ResizeWeaponBoard()
    {
        var childCount = transform.childCount;
        RectTransfrom.sizeDelta = RectTransfrom.sizeDelta.ChangeY(160 * childCount);
        _scrollLimit = (float)(RectTransfrom.sizeDelta.y - (float)1920 / Screen.width * Screen.height) / 2 + SCROLLPADDING;
    }


    //__________METHODS_______________
    public void MoveUp(Vector2 anchor, float amount)
    {
        Vector2 targetPos = anchor + amount * SENSITIVITY * Vector2.up;
        bool exceedLim = Mathf.Abs(targetPos.y) > _scrollLimit;
        if (exceedLim) return;
        RectTransfrom.anchoredPosition = targetPos;
    }
}
