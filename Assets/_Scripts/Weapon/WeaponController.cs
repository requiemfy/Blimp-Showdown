using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Action onPointerDown;
    [SerializeField] private ParticleSystem collapsePS;

    [ SerializeField] public Weapon weapon;
    [HideInInspector] public bool isCollapsed = false;
    [HideInInspector] public Health health;
    [HideInInspector] public WeaponShooting shooter;


    private void Start()
    {
        health = GetComponent<Health>();
        shooter = GetComponent<WeaponShooting>();

        health.SetMaxHealth(weapon.health);
        health.OnDeath += () =>
        {
            isCollapsed = true;
            GetComponentInParent<PlayerController>().weaponLeft -=1;
            collapsePS.Play();
        };
    }

    public void OnPointerDown()
    {
        onPointerDown();
        QuickInfo.Instance.StartCheckHold(targetIfHold: this);
    }
    public void OnPointerUp()
    {
        QuickInfo.Instance.StopCheckHold();
    }
}
