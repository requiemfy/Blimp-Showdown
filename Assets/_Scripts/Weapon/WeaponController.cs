using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponType WeaponType { get; private set; }
    public bool IsFocusable 
    {
        get { return !_isCollapsed && _parentShip.IsInTurn; }
    }
    [HideInInspector] public Health Health;
    [HideInInspector] public WeaponShooting Shooter;

    [Header("Effects")]
    [SerializeField] private ParticleSystem smokingPS;
    [SerializeField] private ParticleSystem burningPS;

    private bool _isCollapsed = false;
    private PlayerController _parentShip;

    private void Awake()
    {
        _parentShip = GetComponentInParent<PlayerController>();
        Health = GetComponent<Health>();
        Shooter = GetComponent<WeaponShooting>();

        Health.OnDamageTaken += () =>
        {
            float healthRatio = (float)Health.GetRatio();
            if (healthRatio >= 0.5f) return;
            if (healthRatio >= 0.2f)
            {
                smokingPS.Play();
                return;
            }
        };
        Health.OnDeath += () =>
        {
            _isCollapsed = true;
            _parentShip.WeaponLeft -=1;
            burningPS.Play();
            smokingPS.Stop();
        };
    }

    public void Construct(LayerMask layer, WeaponType weaponType)
    {
        gameObject.layer = layer;
        this.WeaponType = weaponType;
        Health.Construct(weaponType.health);
        Shooter.Construct(weaponType);
    }
    public void OnPointerDown()
    {
        HUD.Instance.StartObserveWeapon(this);
        Health.ShowHealthBar(true);
    }
    public void OnPointerUp()
    {
        Health.ShowHealthBar(false);
    }
}

