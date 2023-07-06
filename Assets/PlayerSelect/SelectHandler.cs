using System;
using System.Data;
using TMPro;
using UnityEditor;
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

    public WeaponType[] StagedWeapons = new WeaponType[3];
    private Team _currentTeam;

    [SerializeField] private RectTransform weaponBoard;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;
    [SerializeField] private Transform weaponCardPrefab;
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
            var prefab = Instantiate(weaponCardPrefab, weaponBoard.transform);
            var card = prefab.GetChild(0);
            card.GetComponent<WeaponCard>().represent = weapon;
            card.GetComponent<Image>().sprite = weapon.barrel;

            var energyTMP = prefab.GetChild(1).GetComponent<TextMeshProUGUI>();
            energyTMP.text = $"{weapon.energyCost}";

        }
    }

    private const float SCROLLPADDING = 80;
    private void ResizeWeaponBoard()
    {
        var childCount = weaponBoard.childCount;
        weaponBoard.sizeDelta = weaponBoard.sizeDelta.ChangeY(160 * childCount);
        scrollLimit = (float)(weaponBoard.sizeDelta.y - (float)1920 / Screen.width * Screen.height) /2 + SCROLLPADDING;
    }
    public void TryMoveScrollContentTo(Vector2 target)
    {
        bool exceedLim = Mathf.Abs(target.y) > scrollLimit;
        if (exceedLim) return;
        scrollContent.anchoredPosition = target;
    }
    public void NextTeam()
    {
        if (!StagedWeapons.HasNullElement())
        {
            TeamData data = new(StagedWeapons);
            DataPersistence.Push(_currentTeam, data);
        }
        StagedWeapons = new WeaponType[3];
        _currentTeam += 1;
        onTeamChanged();
        if ((int)_currentTeam > 3) SceneManager.LoadScene("Game");
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
        foreach (WeaponType weapon in StagedWeapons)
        {
            if (!weapon) continue;
            total += weapon.health;
        }
        return total;
    }
    private int GetTotalDamage()
    {
        int total = 0;
        foreach (WeaponType weapon in StagedWeapons)
        {
            if (!weapon) continue;
            total += weapon.damage;
        }
        return total;
    }
}
