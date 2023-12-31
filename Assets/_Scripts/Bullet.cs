using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; private set; }
    private float explRadius;

    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer bulletRen;
    [SerializeField] private CircleCollider2D colldr;
    [SerializeField] private Rigidbody2D rb;

    [Header("Effects")]
    [SerializeField] private float multiplier;
    [SerializeField] private GameObject trailPS;
    [SerializeField] private TrailRenderer trailRen;
    [SerializeField] private ParticleSystem bounceDustPS;

    [Header("Explosion")]
    [SerializeField]
    private SpriteRenderer explosion;
    [SerializeField]
    private Transform explosionPS;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ExplodeImmediate"))
        {
            if (m_lifeRoutine != null) StopCoroutine(m_lifeRoutine);
            Explode();
            return;
        }

        SpawnBounceDust(collision);
        rb.gravityScale = 1f;
        StartLifeTimeCounter(duration: 2);
    }

    public void Launch(WeaponType weapon, LayerMask layer, Vector2 launchVec)
    {
        gameObject.layer = layer;
        Damage = weapon.damage;
        bulletRen.sprite = weapon.bulletSprite;
        colldr.radius = 0.5f * weapon.bulletSprite.rect.width / weapon.bulletSprite.pixelsPerUnit;
        explRadius = weapon.explodeRadius;
        rb.gravityScale = weapon.isGravityAffected? 1 : 0;
        rb.AddForce(launchVec, ForceMode2D.Impulse);
        trailRen.startWidth = colldr.radius * 2;
        CinemachineManager.Instance.SetFollow(transform);
        CinemachineManager.Instance.PlayCamShake(0.5f, 0.25f);
        if (!weapon.isGravityAffected) StartLifeTimeCounter((float)weapon.range/12);
    }
    public void Explode()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        explosion.gameObject.SetActive(true);
        explosion.DOColor(Color.clear, 0.5f);
        explosion.transform.localScale = new Vector2(explRadius, explRadius);
        explosionPS.transform.localScale = 0.3f * new Vector2(explRadius, explRadius);

        bulletRen.enabled = false;
        colldr.enabled = false;
        trailPS.SetActive(false);
        Destroy(gameObject, 2f);
    }


    //_________________________________
    private Coroutine m_lifeRoutine;
    private void StartLifeTimeCounter(float duration)
    {
        if (m_lifeRoutine != null) StopCoroutine(m_lifeRoutine);
        m_lifeRoutine = StartCoroutine(TimeCounter());
        IEnumerator TimeCounter()
        {
            yield return new WaitForSeconds(duration);
            Explode();
        }
    }
    private void SpawnBounceDust(Collision2D collision)
    {
        var vel = collision.relativeVelocity.magnitude;
        if (vel > 5)
        {
            var bounceDustEffect = Instantiate(bounceDustPS, transform.position, Quaternion.identity);
            var main = bounceDustEffect.main;
            var emission = bounceDustEffect.emission;
            //main.startLifetime = vel * multiplier;
            main.startSpeed = vel * multiplier;
            //emission.rateOverTime = vel * multiplier;
            bounceDustEffect.transform.up = collision.relativeVelocity;
            Destroy(bounceDustEffect.gameObject, 2);
        }
    }

    /*
    #region VISUAL
    private void OnLaunch()
    {
        StartCoroutine(FireEffectCO());
    }

    private const float duration = 0.5f;
    private IEnumerator FireEffectCO()
    {
        Color original = bulletRen.color;
        bulletRen.color = Color.red;
        yield return new WaitForSeconds(duration);
        bulletRen.color = original;
    }
    #endregion
    */
}
