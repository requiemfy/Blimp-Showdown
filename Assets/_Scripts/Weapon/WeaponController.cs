using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponType WeaponType { get; private set; }
    public bool IsFocusable 
    {
        get { return !_isCollapsed && _parentShip.IsInTurn; }
    }
    public Health Health;
    [HideInInspector] public WeaponShooting Shooter;

    [Header("Effects")]
    [SerializeField] private ParticleSystem smokingPS;
    [SerializeField] private ParticleSystem burningPS;

    private bool _isCollapsed = false;
    private PlayerController _parentShip;

    private void Awake()
    {
        _parentShip = GetComponentInParent<PlayerController>();
        Shooter = GetComponent<WeaponShooting>();

        Health.OnDamageTaken += () =>
        {
            float healthRatio = (float)Health.GetRatio();
            Debug.Log(healthRatio);
            if (healthRatio >= 0.5f) return;
            smokingPS.Play();
        };
        Health.OnDeath += () =>
        {
            _isCollapsed = true;
            _parentShip.WeaponLeft -=1;
            burningPS.Play();
            smokingPS.Stop();
        };
    }
    private void AssignBarrelVisual()
    {
        var barrelRen = transform.Find("Barrel").GetComponent<SpriteRenderer>();

        //Sprite
        barrelRen.sprite = WeaponType.barrel;
        barrelRen.transform.eulerAngles = new(0, 0, 135);
    }
    private void ResetPolygonCollider()
    {
        var colldr = Health.GetComponent<PolygonCollider2D>();
        var spriteRen = Health.GetComponent<SpriteRenderer>();
        List<Vector2> physicsShape = new();
        spriteRen.sprite.GetPhysicsShape(0, physicsShape);
        colldr.SetPath(0, physicsShape);
    }

    public void Construct(LayerMask layer, WeaponType weaponType)
    {
        gameObject.layer = layer;
        this.WeaponType = weaponType;
        Health.gameObject.layer = layer;
        Health.Construct(weaponType.health);
        Shooter.Construct(weaponType);

        AssignBarrelVisual();
        ResetPolygonCollider();
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

