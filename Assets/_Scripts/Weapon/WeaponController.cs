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

    [SerializeField] private ParticleSystem collapsePS;
    private bool _isCollapsed = false;
    private PlayerController _parentShip;

    private void Awake()
    {
        _parentShip = GetComponentInParent<PlayerController>();
        Health = GetComponent<Health>();
        Shooter = GetComponent<WeaponShooting>();

        Health.OnDeath += () =>
        {
            _isCollapsed = true;
            _parentShip.WeaponLeft -=1;
            collapsePS.Play();
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

