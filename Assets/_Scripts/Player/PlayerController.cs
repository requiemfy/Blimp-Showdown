using UnityEngine;
using UnityEngine.UI;

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
        health.onDeath += () => shipCollapsedPS.Play();
        MatchManager.Instance.onTurnEnded += () =>
        {
            movement.Restore(2);
            energy.Restore(1);
        };
    }
    private void Start()
    {
        health.tag = tag;
    }


    public void OnPointerDown()
    {
        QuickInfo.Instance.StartCheckHold(targetIfHold: this);
        if (!isInTurn) return;
        HUD.Instance.ShowWeaponHUD(false);
    }

    public void OnPointerUp()
    {
        QuickInfo.Instance.StopCheckHold();
    }
}
