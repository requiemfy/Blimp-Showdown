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

    public Action onTeamChanged;
    public InfoSection infoSection;

    public WeaponType[] StagedWeapons = new WeaponType[3];
    private Team _currentTeam;

    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;
    private void Awake()
    {
        Instance = this;
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
