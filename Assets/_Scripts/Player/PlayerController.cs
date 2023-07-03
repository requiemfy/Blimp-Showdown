using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [HideInInspector] public int weaponLeft = 3;
    [HideInInspector] public bool isInTurn = false;
    [HideInInspector] public PlayerWeapons Weapons;
    [HideInInspector] public Movement movement;
    [HideInInspector] public PlayerEnergy energy;
    [HideInInspector] public Health health;

    [SerializeField] private ParticleSystem shipCollapsedPS;
    [SerializeField] private Bar energyBar;
    [SerializeField] private Bar fuelBar;

    private void Awake()
    {
        Weapons = GetComponent<PlayerWeapons>();
        movement = GetComponent<Movement>();
        energy = GetComponent<PlayerEnergy>();
        health = transform.Find("Ship").GetComponent<Health>();
        
    }
    private void Start()
    {
        GameManager.Instance.onTurnEnded += () =>
        {
            movement.Restore(2);
            energy.Restore(1);
        };
        //health
        health.OnDeath += () => shipCollapsedPS.Play();
        //energy
        energy.onEnergyChanged += () =>
        {
            energyBar.SetFill(energy.currentEnergy, 10);
        };
        //fuel
        fuelBar.gameObject.SetActive(false);
        movement.whileMoving += () =>
        {
            fuelBar.gameObject.SetActive(true);
            fuelBar.SetFill((int)((float)movement.GetRatio() * 100), 100);
        };
        movement.OnStopped += () =>
        {
            fuelBar.gameObject.SetActive(false);
        };
    }


    public void Construct(Team team, WeaponType[] WeaponTypes)
    {
        tag = team.ToString();
        health.Construct(30, tag);
        Weapons.Construct(WeaponTypes);
    }

    public void OnPointerDown()
    {
        health.ShowHealthBar(true);
        fuelBar.gameObject.SetActive(true);
        if (!isInTurn) return;
        HUD.Instance.ShowWeaponHUD(false);
    }
    public void OnPointerUp()
    {
        health.ShowHealthBar(false);
        fuelBar.gameObject.SetActive(false);
    }
}
