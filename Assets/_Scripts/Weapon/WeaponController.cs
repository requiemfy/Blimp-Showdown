using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponType WeaponType { get; private set; }
    [HideInInspector] public Health Health;
    [HideInInspector] public WeaponShooting Shooter;

    [SerializeField] private ParticleSystem collapsePS;
    private bool _isCollapsed = false;
    private PlayerController _parentShip;

    private void Start()
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

    public void Construct(string tag, WeaponType weaponType)
    {
        this.tag = tag;
        this.WeaponType = weaponType;
        Health.Construct(weaponType.health);
    }
    public void FocusOnMe()
    {
        if (!_parentShip.IsInTurn) return;
        if (_isCollapsed)
        {
            Debug.Log("Weapon collapsed");
            return;
        }

        HUD.Instance.StartObserveWeapon(this);
    }
    public void OnPointerDown()
    {
        FocusOnMe();
        Health.ShowHealthBar(true);
    }
    public void OnPointerUp()
    {
        Health.ShowHealthBar(false);
    }
}
