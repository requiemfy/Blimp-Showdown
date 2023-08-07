using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WeaponShooting : MonoBehaviour
{
    public float Power { get; private set; }
    public Vector2 Direction { get; private set; }
    public float Angle { get; private set; }
    public Transform firePoint;

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform fireSparkPS;

    private WeaponType _weapon;
    private Rigidbody2D _parentRB;
    private PlayerEnergy _energy;

    public void Construct(WeaponType weaponType)
    {
        _weapon = weaponType;
        barrel.GetComponent<SpriteRenderer>().sprite = weaponType.barrel;
    }
    private void Start()
    {
        _parentRB = GetComponentInParent<Rigidbody2D>();
        _energy = GetComponentInParent<PlayerEnergy>();
        Power = 1f;
        SetBarrelRotation(new(-1, -1), -135);
        GameManager.Instance.onTurnEnded += () =>
        {
            SetBarrelRotation(new(-1, -1), -135);
        };
    }


    #region AIMING
    private Vector2 baseDirection;
    private Vector2 lastDragVec;

    private const float ROTATESEN = 0.004f;
    private const float POWERSEN = 0.01f;
    public void OnDragBegin()
    {
        Mode1_beginDrag();
    }
    public void Adjust(Vector2 dragVec)
    {
        Mode1_Adjust(dragVec);
    }
    //
    private void Mode1_beginDrag()
    {
        baseDirection = Direction;
        lastDragVec = Vector2.zero;
    }
    private void Mode1_Adjust(Vector2 dragVec)
    {
        //direction
        if ((int)dragVec.x == 0 && (int)dragVec.y == 0) return;
        Direction = (baseDirection + (float)1920 / Screen.width * ROTATESEN * dragVec).normalized;

        //angle
        Angle = GetTanDeg(Direction);
        barrel.eulerAngles = new Vector3(0, 0, Angle);

        //power
        var delta = dragVec - lastDragVec;
        Power += (delta.x * Direction.x + delta.y * Direction.y) * 1920 / Screen.width * POWERSEN;
        Power = Mathf.Clamp(Power, 0.1f, 1f);

        lastDragVec = dragVec;
    }
    private void Mode2_beginDrag()
    {
    }
    private void Mode2_Adjust(Vector2 dragVec)
    {
        //direction
        if ((int)dragVec.x == 0 && (int)dragVec.y == 0) return;
        Direction = dragVec.normalized;

        //angle
        Angle = GetTanDeg(Direction);
        barrel.eulerAngles = new Vector3(0, 0, Angle);

        //power
        Power = dragVec.magnitude * 1920 / Screen.width * 0.001f;
        Power = Mathf.Clamp(Power, 0.1f, 1f);
    }
    #endregion


    #region SHOOTING
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
            StartCoroutine(FireNormal());
        }
    }
    private IEnumerator FireNormal()
    {
        _isFiring = true;
        for (int i = 0; i < _weapon.bulletCount; i++)
        {
            float offset = 1 - i * _weapon.bulletOffset;
            Bullet bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Vector2 LaunchVec;
            //projectile
            if (_weapon.isGravityAffected) LaunchVec = Mathf.Sqrt(_weapon.range * 10) * (offset * Power * Direction + GameManager.Instance.Wind);
            //straight line
            else LaunchVec = 12 * (offset * Direction + GameManager.Instance.Wind);
            bullet.Launch(
                weapon: _weapon,
                layer: gameObject.layer,
                launchVec: LaunchVec
                );

            AudioManager.Instance.PlayAudioGroup("GunShot");
            SpawnSpark();
            Recoil();
            //ShipKnockBack();
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
    private void SpawnSpark()
    {
        var spark = Instantiate(fireSparkPS, firePoint.position, Quaternion.identity);
        spark.up = Direction;
        Destroy(spark.gameObject, 2f);
    }
    #endregion


    #region SHOOTING VISUAL
    [SerializeField]
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
    private void SetBarrelRotation(Vector2 direction, int angle)
    {
        Direction = direction;
        Angle = angle;
        barrel.DORotate(new Vector3(0, 0, angle), duration: 1);
    }
    private float GetTanDeg(Vector2 vec)
    {
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }
    #endregion
}

