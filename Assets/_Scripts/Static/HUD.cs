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
    [SerializeField] Button weapon0;
    [SerializeField] Button weapon1;
    [SerializeField] Button weapon2;

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

        fireBtn.onClick.AddListener(() => { curWeapon.Shooter.Fire(); });
        endTurnBtn.onClick.AddListener(() => GameManager.Instance.NextTurn());

        aimJoystick.OnDragStarted = () => { curWeapon.Shooter.OnDragBegin(); };
        aimJoystick.WhileDraging = (Vector2 dragVec) => {
            curWeapon.Shooter.Adjust(dragVec);
            curWeapon.Shooter.AdjustBarrelVisual(dragVec);
            UpdateShotAngleUI();
            UpdatePowerTMP();
        };

        weapon0.onClick.AddListener(() =>
        {
            curPlayer.Weapons.GetWeaponCtrl(0).FocusOnMe();
        });
        weapon1.onClick.AddListener(() =>
        {
            curPlayer.Weapons.GetWeaponCtrl(1).FocusOnMe();
        });
        weapon2.onClick.AddListener(() =>
        {
            curPlayer.Weapons.GetWeaponCtrl(2).FocusOnMe();
        });
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
            curWeapon.Health.OnDamageTaken -= UpdateWeaponHealthUI;
        }

        //start subscribe new weapon
        curWeapon = newWeapon;
        curWeapon.Health.OnDamageTaken += UpdateWeaponHealthUI;
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
            curPlayer.Health.OnDamageTaken -= UpdatePlayerHealthUI;
            curPlayer.Energy.onEnergyChanged -= UpdateEnergyBarUI;
            curPlayer.Movement.whileMoving -= UpdateFuelUI;
        }

        //start subscribe new player
        curPlayer = newPlayer;
        newPlayer.Health.OnDamageTaken += UpdatePlayerHealthUI;
        newPlayer.Energy.onEnergyChanged += UpdateEnergyBarUI;
        newPlayer.Movement.whileMoving += UpdateFuelUI;
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
            title.text = curWeapon.WeaponType.name;
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
        powerTMP.text = (curWeapon.Shooter.Power * 100).ToString("0");
    }
    private void UpdateRangeIdctUI()
    {
        rangeIndicator.position = (Vector2)curWeapon.transform.position;
        rangeIndicator.localScale = new Vector2(2*curWeapon.WeaponType.range, rangeIndicator.lossyScale.y);
    }
    private void UpdateWeaponHealthUI()
    {
        healthBar.DOFillAmount((float) curWeapon.Health.GetRatio(), duration: 0.5f);
        healthTMP.text = curWeapon.Health.CurrentHealth.ToString();
    }
    private void UpdatePlayerHealthUI()
    {
        playerHealthBar.SetFill(curPlayer.Health.CurrentHealth, curPlayer.Health.MaxHealth);
    }
    private void UpdateEnergyBarUI()
    {
        energyBar.DOFillAmount((float)curPlayer.Energy.GetRatio(), duration: 0.5f);
        energyTMP.text = curPlayer.Energy.currentEnergy.ToString();
    }
    private void UpdateShotAngleUI()
    {
        float angle = curWeapon.Shooter.Angle;
        angleTMP.text = angle.ToString("0");
        angleIdct.eulerAngles = new Vector3(0, 0, angle);
    }
    private void UpdateFuelUI()
    {
        fuelBar.SetFill((int)((float)curPlayer.Movement.GetRatio() * 100), 100);
    }


    public void StartMoveRight()
    {
        curPlayer.Movement.StartMoveRight();
    }
    public void StartMoveLeft()
    {
        curPlayer.Movement.StartMoveLeft();
    }
    public void StopMoveX()
    {
        curPlayer.Movement.StopMoveX();
    }

    public void StartMoveUp()
    {
        curPlayer.Movement.StartMoveUp();
    }
    public void StartMoveDown()
    {
        curPlayer.Movement.StartMoveDown();
    }
    public void StopMoveY()
    {
        curPlayer.Movement.StopMoveY();
    }
}
