using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    public static SelectManager Instance { get; private set; }

    public Action<Team> onStartedEdit;
    public InfoSection infoSection;
    public GameObject weaponSelectScreen;

    public WeaponType[] StagedWeapons = new WeaponType[3];
    public Team StagedTeam 
    {
        get { return _stagedTeam; }
        set 
        {
            _stagedTeam = value;
            shipTail.color = value.GetTeamColor();
            onStartedEdit(value);
        }
    }
    private Team _stagedTeam;

    [SerializeField] private Image shipTail;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;

    private void Awake()
    {
        Instance = this;
        onStartedEdit += (team) =>
        {
            StagedWeapons = new WeaponType[3];
            TeamData data = DataPersistence.Get(team);
            if (data == null) return;
            StagedWeapons = data.Weapons;
        };
    }

    public void LoadGame()
    {
        if (ReadyCard.NotReadyCount > 0)
        {
            Debug.LogWarning("not all team ready");
            return;
        }
        DOTween.KillAll();
        SceneManager.LoadScene(1);
    }
    public void Save()
    {
        TeamData data = new(StagedWeapons);
        DataPersistence.Push(StagedTeam, data);
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
