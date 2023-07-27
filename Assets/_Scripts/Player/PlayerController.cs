using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int _weaponLeft = 3;
    public int WeaponLeft
    { 
        get { return _weaponLeft; }
        set 
        { 
            _weaponLeft = value;
            if (value <= 0) PlayerDie();
        }
    }
    private void PlayerDie()
    {
        RB.constraints = RigidbodyConstraints2D.None;
        RB.gravityScale = 0.5f;
        shipCollapsedPS.Play();
        SpawnExplosionsOnShipBody();
        CinemachineManager.Instance.PlayCamShake(0.7f, 2f);
        CinemachineManager.Instance.SetFollow(transform);
        HUD.Instance.FindTurnIndicator(_team).sprite = deathIcon;
        GameManager.Instance.DecreasePlayerRemaining(_team);
    }
    private void SpawnExplosionsOnShipBody()
    {
        StartCoroutine(ExplodeSequence());
        IEnumerator ExplodeSequence()
        {
            for (int i =0; i <5; i++)
            {
                var explodePos = transform.position + new Vector3(Random.Range(-3,3), Random.Range(-1.5f, 1.5f), 0);
                var explosion = Instantiate(explosionPS, explodePos, Quaternion.identity);
                Destroy(explosion.gameObject, 2f);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    [HideInInspector] public bool IsInTurn = false;
    [HideInInspector] public PlayerWeapons Weapons;
    [HideInInspector] public Movement Movement;
    [HideInInspector] public PlayerEnergy Energy;
    [HideInInspector] public Health Health;

    [SerializeField] private Bar energyBar;
    [SerializeField] private Bar fuelBar;
    [Header("Visual")]
    [SerializeField] private ParticleSystem shipCollapsedPS;
    [SerializeField] private Transform explosionPS;
    [SerializeField] private Sprite deathIcon;

    private Team _team;
    private Rigidbody2D RB;

    private void Awake()
    {
        Weapons = GetComponent<PlayerWeapons>();
        Movement = GetComponent<Movement>();
        Energy = GetComponent<PlayerEnergy>();
        Health = transform.Find("Ship").GetComponent<Health>();

        RB = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        GameManager.Instance.onTurnEnded += () =>
        {
            Movement.Restore(500);
            Energy.Restore(1);
            PopUpManager.Instance.SpawnText("+1", energyBar.transform.position, CustomColors.Purple);
        };
       
        //energy
        energyBar.SetFill(Energy.currentEnergy, 10);
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
        _team = team;
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
    }
    public void OnPointerUp()
    {
        Health.ShowHealthBar(false);
        fuelBar.gameObject.SetActive(false);
        energyBar.gameObject.SetActive(true);
    }
}
