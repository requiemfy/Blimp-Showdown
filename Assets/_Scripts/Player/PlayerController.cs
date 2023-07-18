using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [HideInInspector] public int WeaponLeft = 3;
    [HideInInspector] public bool IsInTurn = false;
    [HideInInspector] public PlayerWeapons Weapons;
    [HideInInspector] public Movement Movement;
    [HideInInspector] public PlayerEnergy Energy;
    [HideInInspector] public Health Health;

    [SerializeField] private ParticleSystem shipCollapsedPS;
    [SerializeField] private Bar energyBar;
    [SerializeField] private Bar fuelBar;

    private void Awake()
    {
        Weapons = GetComponent<PlayerWeapons>();
        Movement = GetComponent<Movement>();
        Energy = GetComponent<PlayerEnergy>();
        Health = transform.Find("Ship").GetComponent<Health>();
    }
    private void Start()
    {
        GameManager.Instance.onTurnEnded += () =>
        {
            Movement.Restore(500);
            Energy.Restore(1);
            PopUpManager.Instance.SpawnText("+1", energyBar.transform.position, CustomColors.Purple);
        };
        //health
        Health.OnDeath += () => shipCollapsedPS.Play();
        //energy
        Energy.onEnergyChanged += () =>
        {
            energyBar.SetFill(Energy.currentEnergy, 10);
        };
        //fuel
        fuelBar.gameObject.SetActive(false);
        Movement.whileMoving += () =>
        {
            fuelBar.gameObject.SetActive(true);
            fuelBar.SetFill((int)((float)Movement.GetRatio() * 100), 100);
        };
        Movement.OnStopped += () =>
        {
            fuelBar.gameObject.SetActive(false);
        };
    }


    public void Construct(Team team)
    {
        DataPersistence.Push(team, this);
        gameObject.layer = LayerMask.NameToLayer(team.ToString());
        Health.Construct(30, tag);
        Weapons.Construct(DataPersistence.Get(team).Weapons);
    }
    public void OnPointerDown()
    {
        Health.ShowHealthBar(true);
        fuelBar.gameObject.SetActive(true);
        energyBar.gameObject.SetActive(false);
        HUD.Instance.StartObservePlayer(this);
    }
    public void OnPointerUp()
    {
        Health.ShowHealthBar(false);
        fuelBar.gameObject.SetActive(false);
        energyBar.gameObject.SetActive(true);
    }
}
