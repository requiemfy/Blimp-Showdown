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

    private Weapon weapon;
    private PlayerEnergy energy;

    private void Start()
    {
        energy = GetComponentInParent<PlayerEnergy>();
        weapon = GetComponent<WeaponController>().weapon;
        barrel = transform.Find("Barrel");
        Power = 1f;
        Direction = new Vector2(1, -1).normalized;
        Angle = -45f;
    }

    public void AdjustDirection(Vector2 dragVec)
    {
        Direction = dragVec.normalized;
    }
    public void AdjustPower(float val)
    {
        Power = val;
        if (Power > 1f) Power = 1f;
    }

    private bool _isFiring = false;
    public void Fire()
    {
        if (_isFiring) return;
        if (energy.Drain(weapon.energyCost))
        {
            if (weapon.isRaycast)
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
        for (int i = 0; i < weapon.bulletCount; i++)
        {
            float offset = 1 - i * weapon.bulletOffset;
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector2 LaunchVec = Mathf.Sqrt(weapon.range * 10) * (offset * Power * Direction + MatchManager.Instance.wind);
            bullet.Launch(
                weapon: weapon,
                teamTag: tag,
                launchVec: LaunchVec
                );
            Recoil();
            yield return new WaitForSeconds(weapon.bulletShootTime);
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
            hit.transform?.GetComponent<Health>()?.DecreaseHealth(weapon.damage);
            if (!weapon.isRayMultiple) return;
        }
    }



    #region SHOOTING VISUAL
    private Transform barrel;
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

    public void AdjustBarrel(Vector2 dragVec)
    {
        if (_isFiring) return;
        Angle = GetTanDeg(dragVec);
        DOTween.Kill(barrel);
        /*if (-90 < Angle && Angle < 90)
        {
            barrel.localScale = new(1, 1, 1);
            barrel.eulerAngles = new(0, 0, Angle);
        }
        else
        {
            barrel.localScale = new(-1, 1, 1);
            barrel.eulerAngles = new(0, 0, Angle + 180);
        }*/
        barrel.DORotate(new(0, 0, Angle), duration: 1f);
    }
    private float GetTanDeg(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
    #endregion
}

