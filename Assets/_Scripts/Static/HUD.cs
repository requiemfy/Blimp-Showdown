using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }
    private WeaponController curWeapon;
    private PlayerController curPlayer;

    [SerializeField] Joystick aimJoystick;

    [Header("Weapon")]
    [SerializeField] GameObject weaponHUD;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthTMP;
    [SerializeField] Button fireBtn;
    [SerializeField] Transform rangeIndicator;
    [SerializeField] TextMeshProUGUI powerTMP;
    [SerializeField] TextMeshProUGUI angleTMP;
    [SerializeField] Transform angleIdct;
    [SerializeField] TrajectoryLine trajectory;

    [Header("Player")]
    [SerializeField] GameObject playerHUD;
    [SerializeField] Button endTurnBtn;
    [SerializeField] Bar playerHealthBar;
    [SerializeField] Bar fuelBar;

    [Header("Common")]
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image energyBar;
    [SerializeField] TextMeshProUGUI energyTMP;

    private void Awake()
    {
        Instance = this;

        #region INIT BUTTONS
        fireBtn.onClick.AddListener(() => { curWeapon.shooter.Fire(); });
        endTurnBtn.onClick.AddListener(() => MatchManager.Instance.NextTurn());

        aimJoystick.OnDragStarted = () => { curWeapon.shooter.OnDragBegin(); };
        aimJoystick.WhileDraging = (Vector2 dragVec) => {
            curWeapon.shooter.Adjust(dragVec);
            curWeapon.shooter.AdjustBarrelVisual(dragVec);
            UpdateShotAngleUI();
            UpdatePowerTMP();
        };
        #endregion
    }


    public void StartObserveWeapon(WeaponController newWeapon)
    {
        if (newWeapon == null)
        {
            ShowWeaponHUD(false);
            return;
        }
        // stop observe current weapon
        if (curWeapon)
        {
            curWeapon.health.OnDamageTaken -= UpdateWeaponHealthUI;
        }

        //start subscribe new weapon
        curWeapon = newWeapon;
        curWeapon.health.OnDamageTaken += UpdateWeaponHealthUI;
        SetTrajectoryTarget();
        UpdateWeaponHealthUI();
        UpdateShotAngleUI();
        UpdateRangeIdctUI();
        UpdatePowerTMP();
        ShowWeaponHUD(true);
    }
    public void StartObservePlayer(PlayerController newPlayer)
    {
        //stop observe currentplayer
        if (curPlayer)
        {
            curPlayer.health.OnDamageTaken -= UpdatePlayerHealthUI;
            curPlayer.energy.onEnergyChanged -= UpdateEnergyBarUI;
            curPlayer.movement.whileMoving -= UpdateFuelUI;
        }

        //start subscribe new player
        curPlayer = newPlayer;
        newPlayer.health.OnDamageTaken += UpdatePlayerHealthUI;
        newPlayer.energy.onEnergyChanged += UpdateEnergyBarUI;
        newPlayer.movement.whileMoving += UpdateFuelUI;
        UpdatePlayerHealthUI();
        UpdateEnergyBarUI();
        UpdateFuelUI();
        ShowWeaponHUD(false);
    }







    //_______________________________________________________________
    public void ShowWeaponHUD(bool status)
    {
        trajectory.gameObject.SetActive(status);
        weaponHUD.SetActive(status);
        playerHUD.SetActive(!status);
        if (status)
        {
            title.text = curWeapon.weapon.name;
        }
        else
        {
            title.text = curPlayer?.tag;
        }
    }
    private void SetTrajectoryTarget()
    {
        trajectory.SetShooter(curWeapon);
    }
    private void UpdatePowerTMP()
    {
        powerTMP.text = (curWeapon.shooter.Power * 100).ToString("0");
    }
    private void UpdateRangeIdctUI()
    {
        rangeIndicator.position = (Vector2)curWeapon.transform.position;
        rangeIndicator.localScale = new Vector2(2*curWeapon.weapon.range, rangeIndicator.lossyScale.y);
    }
    private void UpdateWeaponHealthUI()
    {
        healthBar.DOFillAmount((float) curWeapon.health.GetRatio(), duration: 0.5f);
        healthTMP.text = curWeapon.health.CurrentHealth.ToString();
    }
    private void UpdatePlayerHealthUI()
    {
        playerHealthBar.SetFill(curPlayer.health.CurrentHealth, curPlayer.health.MaxHealth);
    }
    private void UpdateEnergyBarUI()
    {
        energyBar.DOFillAmount((float)curPlayer.energy.GetRatio(), duration: 0.5f);
        energyTMP.text = curPlayer.energy.currentEnergy.ToString();
    }
    private void UpdateShotAngleUI()
    {
        float angle = curWeapon.shooter.Angle;
        angleTMP.text = angle.ToString("0");
        angleIdct.eulerAngles = new Vector3(0, 0, angle);
    }
    private void UpdateFuelUI()
    {
        fuelBar.SetFill((int)((float)curPlayer.movement.GetRatio() * 100), 100);
    }


    public void StartMoveRight()
    {
        curPlayer.movement.StartMoveRight();
    }
    public void StartMoveLeft()
    {
        curPlayer.movement.StartMoveLeft();
    }
    public void StopMoveX()
    {
        curPlayer.movement.StopMoveX();
    }

    public void StartMoveUp()
    {
        curPlayer.movement.StartMoveUp();
    }
    public void StartMoveDown()
    {
        curPlayer.movement.StartMoveDown();
    }
    public void StopMoveY()
    {
        curPlayer.movement.StopMoveY();
    }
}
