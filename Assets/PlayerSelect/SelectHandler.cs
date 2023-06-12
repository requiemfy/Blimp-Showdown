using System;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectHandler : MonoBehaviour
{
    public static SelectHandler Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    //------------------------------------------
    public Action onTeamChanged;
    public RectTransform scrollContent;
    public float sensitivity;
    private float scrollLimit;
    public InfoSection infoSection;
    [HideInInspector] public Team currentTeam;

    [SerializeField] private RectTransform weaponBoard;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;
    [SerializeField] private Transform weaponCardPrefab;
    [SerializeField] private Weapon[] weapons;

    private void Start()
    {
        SpawnCards();
        ResizeWeaponBoard();
    }
    private void SpawnCards()
    {
        foreach (Weapon weapon in weapons)
        {
            var prefab = Instantiate(weaponCardPrefab, weaponBoard.transform);
            var card = prefab.GetChild(0);
            card.GetComponent<WeaponCard>().represent = weapon;
            card.GetComponent<Image>().sprite = weapon.barrel;

            var energyTMP = prefab.GetChild(1).GetComponent<TextMeshProUGUI>();
            energyTMP.text = $"{weapon.energyCost}";

        }
    }

    private const float scrollPadding = 1.5f;
    private void ResizeWeaponBoard()
    {
        var childCount = weaponBoard.childCount;
        weaponBoard.sizeDelta = weaponBoard.sizeDelta.ChangeY(160 * childCount);
        scrollLimit = (float)(weaponBoard.sizeDelta.y - (float)1920 / Screen.width * Screen.height) /2 + 80;
    }
    public void TryMoveScrollContentTo(Vector2 target)
    {
        bool exceedLim = Mathf.Abs(target.y) > scrollLimit;
        if (exceedLim) return;
        scrollContent.anchoredPosition = target;
    }
    public void NextTeam()
    {
        currentTeam += 1;
        onTeamChanged();
        if ((int)currentTeam > 3) {
            SceneManager.LoadScene("Game");
        }
    }
    public void UpdateTotalHealth()
    {
        healthTMP.text = $"Health: {GetTotalHealth()}";
    }
    public void UpdateTotalDamage()
    {
        damageTMP.text = $"Damage: {GetTotalDamage()}";
    }
    private int GetTotalHealth()
    {
        int total = 0;
        foreach (Weapon weapon in currentTeam.GetWeapons())
        {
            if (!weapon) continue;
            total += weapon.health;
        }
        return total;
    }
    private int GetTotalDamage()
    {
        int total = 0;
        foreach (Weapon weapon in currentTeam.GetWeapons())
        {
            if (!weapon) continue;
            total += weapon.damage;
        }
        return total;
    }
}
