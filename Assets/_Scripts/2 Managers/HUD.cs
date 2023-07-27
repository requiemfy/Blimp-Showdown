using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }
    private WeaponController curWeapon;
    private PlayerController curPlayer;

    [SerializeField] Joystick aimJoystick;
    [SerializeField] Joystick moveJoystick;

    [Header("Weapon")]
    [SerializeField] CanvasGroup weaponHUD;
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI healthTMP;
    [SerializeField] Button fireBtn;
    [SerializeField] Transform rangeIndicator;
    [SerializeField] PowerAnglePad powerAnglePad;
    [SerializeField] Transform angleIdct;
    [SerializeField] TrajectoryLine trajectory;
    [SerializeField] Button weapon0;
    [SerializeField] Button weapon1;
    [SerializeField] Button weapon2;

    [Header("Player")]
    [SerializeField] CanvasGroup playerHUD;
    [SerializeField] Button endTurnBtn;
    [SerializeField] Bar playerHealthBar;
    [SerializeField] Bar fuelBar;

    [Header("Common")]
    [SerializeField] Image energyBar;
    [SerializeField] TextMeshProUGUI energyTMP;
    [SerializeField] Image turnPrefab;
    [SerializeField] Transform turnIndicatorParent;

    [Header("BackToShipBtn")]
    [SerializeField] Button backToShipBtn;
    [SerializeField] Transform backToShipArrow;
    private CanvasGroup _backToShipCanvasGrp;

    private Camera _cam;

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
        InitializeButtons();
        SpawnTurnIndicator();
        weaponHUD.gameObject.SetActive(false);
        playerHUD.gameObject.SetActive(false);
    }
    private void Update()
    {
        CheckPlayerInsideScreen();
    }
    private void InitializeButtons()
    {
        fireBtn.onClick.AddListener(() => { curWeapon.Shooter.Fire(); });
        endTurnBtn.onClick.AddListener(() => GameManager.Instance.NextTurn());
        backToShipBtn.onClick.AddListener(() => CinemachineManager.Instance.SetFollow(curPlayer.transform));
        _backToShipCanvasGrp = backToShipBtn.GetComponent<CanvasGroup>();

        //aim
        aimJoystick.OnDragStarted = () => 
        {
            powerAnglePad.gameObject.SetActive(true);
            curWeapon.Shooter.OnDragBegin(); 
        };
        aimJoystick.WhileDraging = (Vector2 dragVec) => {
            curWeapon.Shooter.Adjust(dragVec);
            UpdatePowerAnglePad();
        };
        aimJoystick.UponPointerUp = () =>
        {
            powerAnglePad.gameObject.SetActive(false);
        };

        //move
        moveJoystick.WhileDraging = (Vector2 dragVec) =>
        {
            curPlayer.Movement.Move(dragVec);
        };
        moveJoystick.UponPointerUp = () =>
        {
            curPlayer.Movement.StopMove();
        };

        //weapons
        weapon0.onClick.AddListener(() =>
        {
            StartObserveWeapon(curPlayer.Weapons.GetWeaponCtrl(0));
        });
        weapon1.onClick.AddListener(() =>
        {
            StartObserveWeapon(curPlayer.Weapons.GetWeaponCtrl(1));
        });
        weapon2.onClick.AddListener(() =>
        {
            StartObserveWeapon(curPlayer.Weapons.GetWeaponCtrl(2));
        });
    }
    public Image FindTurnIndicator(Team target)
    {
        foreach(Transform child in turnIndicatorParent)
        {
            if (child.name == target.ToString())
            {
                return child.GetComponent<Image>();
            }
        }
        throw new System.Exception("No turn indicator found for the given team");
    }
    private void SpawnTurnIndicator()
    {
        foreach (Team team in GameManager.Instance.OpennedTeams)
        {
            var indicator = Instantiate(turnPrefab, turnIndicatorParent);
            Color32 faintColor = team.GetTeamColor(); faintColor.a = 50;
            indicator.color = faintColor;
            indicator.name = team.ToString();
        }
    }


    public void StartObserveWeapon(WeaponController newWeapon)
    {
        // stop observe current weapon
        if (curWeapon)
        {
            curWeapon.transform.localPosition = curWeapon.transform.localPosition.ChangeZ(-1);
            curWeapon.Health.OnDamageTaken -= UpdateWeaponHealthUI;
        }
        if (newWeapon == null)
        {
            TurnOff(weaponHUD);
            trajectory.gameObject.SetActive(false);
            return;
        }
        if (!newWeapon.IsFocusable) return;

        //start subscribe new weapon
        curWeapon = newWeapon;
        curWeapon.transform.localPosition = curWeapon.transform.localPosition.ChangeZ(-2);
        newWeapon.Health.OnDamageTaken += UpdateWeaponHealthUI;
        SetTrajectoryTarget();
        UpdateWeaponHealthUI();
        UpdateRangeIdctUI();
        UpdatePowerAnglePad();
        TurnOn(weaponHUD);
        trajectory.gameObject.SetActive(true);
    }
    public void StartObservePlayer(PlayerController newPlayer)
    {
        //stop observe currentplayer
        if (curPlayer)
        {
            curPlayer.Health.OnDamageTaken -= UpdatePlayerHealthUI;
            curPlayer.Energy.onEnergyChanged -= UpdateEnergyBarUI;
            curPlayer.Movement.whileMoving -= UpdateFuelUI;
            curPlayer.transform.position = curPlayer.transform.position.ChangeZ(0);
        }
        if (newPlayer == null)
        {
            curPlayer = null;
            TurnOff(playerHUD);
            return;
        }
        if (!newPlayer.IsInTurn) return;

        //start subscribe new player
        curPlayer = newPlayer;
        newPlayer.Health.OnDamageTaken += UpdatePlayerHealthUI;
        newPlayer.Energy.onEnergyChanged += UpdateEnergyBarUI;
        newPlayer.Movement.whileMoving += UpdateFuelUI;
        curPlayer.transform.position = curPlayer.transform.position.ChangeZ(-10);
        UpdatePlayerHealthUI();
        UpdateEnergyBarUI();
        UpdateFuelUI();
        Update_weapon_button_thumbnails();
        TurnOn(playerHUD);
    }

    private void TurnOn(CanvasGroup canvasGrp)
    {
        canvasGrp.alpha = 1;
        canvasGrp.gameObject.SetActive(true);
    }
    private void TurnOff(CanvasGroup canvasGrp)
    {
        canvasGrp.DOFade(0, 0.5f)
                .onComplete = () => canvasGrp.gameObject.SetActive(false);
    }







    //_______________________________________________________________
    private bool isCurrentlyInside = false;
    private void CheckPlayerInsideScreen()
    {
        if (curPlayer == null) 
        {
            backToShipBtn.gameObject.SetActive(false);
            return;
        }
        Vector2 pos = _cam.WorldToScreenPoint(curPlayer.transform.position);
        bool insideScreen = Screen.safeArea.Contains(pos);
        RotateArrow();

        if (insideScreen && !isCurrentlyInside)
        {
            isCurrentlyInside = true;
            _backToShipCanvasGrp.DOKill();
            _backToShipCanvasGrp.DOFade(0, duration: 0.4f)
                .onComplete = () => backToShipBtn.gameObject.SetActive(false);
        }
        else if (!insideScreen && isCurrentlyInside)
        {
            isCurrentlyInside = false;
            backToShipBtn.gameObject.SetActive(true);
            _backToShipCanvasGrp.DOKill();
            _backToShipCanvasGrp.DOFade(1, duration: 0.4f);
        }

        void RotateArrow()
        {
            Vector2 playerToCam = curPlayer.transform.position - _cam.transform.position;
            backToShipArrow.right = playerToCam;
        }
    }
    private void SetTrajectoryTarget()
    {
        trajectory.SetShooter(curWeapon);
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
    private void UpdatePowerAnglePad()
    {
        var shooter = curWeapon.Shooter;
        powerAnglePad.UpdateValues((float)shooter.Power,(int)shooter.Angle);
        if (shooter.Angle.InRange(0, 180))
        {
            powerAnglePad.transform.position = curWeapon.transform.position + new Vector3(0,-2,0);
        }
        else
        {
            powerAnglePad.transform.position = curWeapon.transform.position + new Vector3(0,2,0);
        }
        /*
        powerTMP.text = (curWeapon.Shooter.Power * 100).ToString("0");

        float angle = curWeapon.Shooter.Angle;
        angleTMP.text = angle.ToString("0");
        angleIdct.eulerAngles = new Vector3(0, 0, angle);*/
    }
    private void UpdateFuelUI()
    {
        fuelBar.SetFill((int)((float)curPlayer.Movement.GetRatio() * 100), 100);
    }
    private void Update_weapon_button_thumbnails()
    {
        var playerWeapon0 = curPlayer.Weapons.GetWeaponCtrl(0);
        var playerWeapon1 = curPlayer.Weapons.GetWeaponCtrl(1);
        var playerWeapon2 = curPlayer.Weapons.GetWeaponCtrl(2);

        weapon0.interactable = playerWeapon0.IsFocusable;
        weapon0.transform.GetChild(0).GetComponent<Image>().sprite = playerWeapon0.WeaponType.barrel;

        weapon1.interactable = playerWeapon1.IsFocusable;
        weapon1.transform.GetChild(0).GetComponent<Image>().sprite = playerWeapon1.WeaponType.barrel;

        weapon2.interactable = playerWeapon2.IsFocusable;
        weapon2.transform.GetChild(0).GetComponent<Image>().sprite = playerWeapon2.WeaponType.barrel;
    }
}
