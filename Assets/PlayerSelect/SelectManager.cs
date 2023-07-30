using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

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
    [SerializeField] private TextMeshProUGUI energyTMP;

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
            energyTMP.text = "0";
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



    //total health & damage
    public void UpdateTotalHealthDamage()
    {
        Vector3 current = new (int.Parse(healthTMP.text), int.Parse(damageTMP.text), float.Parse(energyTMP.text));
        Vector3 target = new(GetTotalHealth(), GetTotalDamage(), GetAverageEnergy());

        healthTMP.DOKill();
        damageTMP.DOKill();
        StopAllCoroutines();
        var tween = DOTween.To(() => current, x => current = x, target, 1);
        tween.onPlay = () => StartCoroutine(UpdateString());
        tween.onComplete = () =>
        {
            healthTMP.DOColor(CustomColors.Black, 0.4f);
            damageTMP.DOColor(CustomColors.Black, 0.4f);
            StopAllCoroutines();
        };
        

        IEnumerator UpdateString()
        {
            RecolorText();
            while (tween.IsPlaying())
            {
                healthTMP.text = (MathF.Round(current.x)).ToString();
                damageTMP.text = (MathF.Round(current.y)).ToString();
                energyTMP.text = (MathF.Round(current.z, 1)).ToString();
                yield return null;
            }
        }
        void RecolorText()
        {
            if (target.x < current.x) healthTMP.color = CustomColors.Red;
            else healthTMP.color = CustomColors.Green;

            if (target.y < current.y) damageTMP.color = CustomColors.Red;
            else damageTMP.color = CustomColors.Green;
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
            total += weapon.damage * weapon.bulletCount;
        }
        return total;
    }
    private float GetAverageEnergy()
    {
        int totalEnergyCost = 0;
        int weaponCount = 0;
        foreach (WeaponType weapon in StagedWeapons)
        {
            if (!weapon) continue;
            totalEnergyCost += weapon.energyCost;
            weaponCount++;
        }
        if (weaponCount == 0) return 0;
        return (float) totalEnergyCost / weaponCount;
    }
}
