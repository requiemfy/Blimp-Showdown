using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

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
            shipTail.color = team.GetTeamColor();
            TeamData data = DataPersistence.Get(team);
            StagedWeapons = data == null? new WeaponType[3] : data.Weapons;
            healthTMP.text = "0";
            damageTMP.text = "0";
            UpdateTotalHealthDamage();
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
    public void UpdateTotalHealthDamage()
    {
        Vector2 current = new (int.Parse(healthTMP.text), int.Parse(damageTMP.text));
        var tween = DOTween.To(() => current, x => current = x, new(GetTotalHealth(), GetTotalDamage()), 1);
        tween.onPlay = () => StartCoroutine(UpdateString());
        tween.onComplete = () => StopAllCoroutines();
        

        IEnumerator UpdateString()
        {
            while (tween.IsPlaying())
            {
                healthTMP.text = (MathF.Round(current.x)).ToString();
                damageTMP.text = (MathF.Round(current.y)).ToString();
                yield return null;
            }
        }
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
