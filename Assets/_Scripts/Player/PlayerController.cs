using UnityEngine;

public enum Team
{
    Red = 0,
    Green = 1,
    Blue = 2,
    Yellow = 3
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem shipCollapsedPS;
    [SerializeField] private Bar energyBar;
    [SerializeField] private Bar fuelBar;

    [HideInInspector] public int weaponLeft = 3;
    [HideInInspector] public bool isInTurn = false;
    [HideInInspector] public Movement movement;
    [HideInInspector] public PlayerEnergy energy;
    [HideInInspector] public Health health;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        energy = GetComponent<PlayerEnergy>();
        health = transform.Find("Ship").GetComponent<Health>();
        health.SetMaxHealth(30);
        health.OnDeath += () => shipCollapsedPS.Play();
    }
    private void Start()
    {
        health.tag = tag;
        MatchManager.Instance.onTurnEnded += () =>
        {
            movement.Restore(2);
            energy.Restore(1);
        };
        energy.onEnergyChanged += () =>
        {
            energyBar.SetFill(energy.currentEnergy, 10);
        };

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
