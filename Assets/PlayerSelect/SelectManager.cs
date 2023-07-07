using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    public static SelectManager Instance { get; private set; }

    public Action onSaved;
    public Action<Team> onStartedEdit;
    public InfoSection infoSection;

    public WeaponType[] StagedWeapons = new WeaponType[3];
    public Team StagedTeam 
    {
        get { return _stagedTeam; }
        set 
        {
            _stagedTeam = value;
            onStartedEdit(value);
        }
    }
    private Team _stagedTeam;

    [SerializeField] private GameObject readyScreen;
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI damageTMP;

    private void Awake()
    {
        Instance = this;
        onStartedEdit += (team) =>
        {
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
        SceneManager.LoadScene(1);
    }
    public void Save()
    {
        TeamData data = new(StagedWeapons);
        DataPersistence.Push(StagedTeam, data);
        StagedWeapons = new WeaponType[3];
        readyScreen.SetActive(true);
        onSaved();
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
