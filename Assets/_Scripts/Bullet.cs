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

    [SerializeField]
    private GameObject explosion;

    public void Launch(WeaponType weapon, LayerMask layer, Vector2 launchVec)
    {
        gameObject.layer = layer;
        Damage = weapon.damage;
        explRadius = weapon.explodeRadius;
        rb.gravityScale = weapon.isGravityAffected? 1 : 0;
        rb.AddForce(launchVec, ForceMode2D.Impulse);
        OnLaunch();
        CinemachineManager.Instance.SetFollow(transform);
        Destroy(gameObject, 20);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tag)) return;
        Explode();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("ExplodeImmediate")) return;
        Explode();
    }

    private void Explode()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        explosion.SetActive(true);
        explosion.transform.localScale = new Vector2(explRadius, explRadius);

        bulletRen.enabled = false;
        colldr.enabled = false;
        Destroy(gameObject, 0.5f);
    }


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
}
