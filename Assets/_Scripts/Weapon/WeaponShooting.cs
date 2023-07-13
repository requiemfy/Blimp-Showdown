using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WeaponShooting : MonoBehaviour
{
    public float Power { get; private set; }
    public Vector2 Direction { get; private set; }
    public float Angle { get; private set; }
    public Transform firePoint;
    [SerializeField] Bullet bulletPrefab;

    private WeaponType _weapon;
    private Rigidbody2D _parentRB;
    private PlayerEnergy _energy;

    public void Construct(WeaponType weaponType)
    {
        _weapon = weaponType;
    }
    private void Start()
    {
        _parentRB = GetComponentInParent<Rigidbody2D>();
        _energy = GetComponentInParent<PlayerEnergy>();
        barrel = transform.Find("Barrel");
        Power = 1f;
        Direction = new Vector2(1, -1).normalized;
        Angle = -45f;
    }


    private Vector2 baseDirection;
    private Vector2 lastDragVec;

    private const float ROTATESEN = 0.004f;
    private const float POWERSEN = 0.01f;
    public void OnDragBegin()
    {
        baseDirection = Direction;
        lastDragVec = Vector2.zero;
    }
    public void Adjust(Vector2 dragVec)
    {
        //direction
        if ((int)dragVec.x == 0 && (int)dragVec.y == 0) return;
        Direction = (baseDirection + (float)1920/Screen.width * ROTATESEN * dragVec ).normalized;
        Debug.Log(Screen.width);

        //angle
        Angle = GetTanDeg(Direction);
        barrel.eulerAngles = new Vector3(0,0,Angle);

        //power
        var delta = dragVec - lastDragVec;
        Power += (delta.x * Direction.x + delta.y * Direction.y) * 1920 / Screen.width*POWERSEN;
        Power = Mathf.Clamp(Power, 0.1f, 1f);

        lastDragVec = dragVec;
    }

    private bool _isFiring = false;
    public void Fire()
    {
        if (_isFiring) return;
        if (_energy.Drain(_weapon.energyCost))
        {
            if (_weapon.isRaycast)
            {
                FireRayCast();
                return;
            }
            StartCoroutine(FireCO());
        }
    }
    private IEnumerator FireCO()
    {
        _isFiring = true;
        for (int i = 0; i < _weapon.bulletCount; i++)
        {
            float offset = 1 - i * _weapon.bulletOffset;
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector2 LaunchVec = Mathf.Sqrt(_weapon.range * 10) * (offset * Power * Direction + GameManager.Instance.wind);
            bullet.Launch(
                weapon: _weapon,
                teamTag: tag,
                launchVec: LaunchVec
                );
            Recoil();
            ShipKnockBack();
            yield return new WaitForSeconds(_weapon.bulletShootTime);
        }
        _isFiring = false;
    }

    private void FireRayCast()
    {
        Recoil();
        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, Direction);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.transform?.tag == tag) continue;
            hit.transform?.GetComponent<Health>()?.DecreaseHealth(_weapon.damage);
            if (!_weapon.isRayMultiple) return;
        }
    }



    #region SHOOTING VISUAL
    private Transform barrel;
    private void ShipKnockBack()
    {
        Debug.Log("knockback");
        Vector2 knockBack = 0.2f * Mathf.Sqrt(_weapon.range * 10) * Power * -Direction;
        _parentRB.AddForce(knockBack, ForceMode2D.Impulse);
    }
    private void Recoil()
    {
        DOTween.Kill(barrel, true);
        firePoint.parent = barrel;
        var originalPos = firePoint.localPosition;
        firePoint.parent = transform;
        barrel.DOScale(new Vector2(0.66f, 1.5f), duration: 0.1f)
            .onComplete = () =>
            {
                barrel.DOScale(Vector2.one, duration: 0.2f)
                .onComplete = () =>
                {
                    firePoint.parent = barrel;
                    firePoint.localPosition = originalPos;
                };
            };
    }
    private float GetTanDeg(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
    #endregion
}

