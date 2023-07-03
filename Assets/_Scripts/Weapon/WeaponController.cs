using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] public Health health;
    [HideInInspector] public WeaponShooting shooter;

    [SerializeField] private ParticleSystem collapsePS;

    public WeaponType weapon;
    private bool _isCollapsed = false;
    private PlayerController _parentShip;

    private void Start()
    {
        _parentShip = GetComponentInParent<PlayerController>();
        health = GetComponent<Health>();
        shooter = GetComponent<WeaponShooting>();

        health.OnDeath += () =>
        {
            _isCollapsed = true;
            _parentShip.weaponLeft -=1;
            collapsePS.Play();
        };
    }

    public void Construct(string tag)
    {
        this.tag = tag;
        health.Construct(weapon.health);
    }
    public void FocusOnMe()
    {
        if (!_parentShip.isInTurn) return;
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
        health.ShowHealthBar(true);
    }
    public void OnPointerUp()
    {
        health.ShowHealthBar(false);
    }
}
