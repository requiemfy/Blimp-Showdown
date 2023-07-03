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

    private WeaponType weapon;
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
        if ((int)dragVec.x == 0 || (int)dragVec.y == 0) return;
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
            Vector2 LaunchVec = Mathf.Sqrt(weapon.range * 10) * (offset * Power * Direction + GameManager.Instance.wind);
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

    public void AdjustBarrelVisual(Vector2 dragVec)
    {
        if (_isFiring) return;
        //Angle = GetTanDeg(dragVec);
        //DOTween.Kill(barrel);
        //barrel.DORotate(new(0, 0, Angle), duration: 1f);
    }
    private float GetTanDeg(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
    #endregion
}

