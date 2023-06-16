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
    [SerializeField] private Sprite explosion;
    [SerializeField] private SpriteRenderer explosionRen;

    public void Launch(Weapon weapon, string teamTag, Vector2 launchVec)
    {
        tag = teamTag;
        Damage = weapon.damage;
        explRadius = weapon.explodeRadius;
        rb.gravityScale = weapon.isGravityAffected? 1 : 0;
        rb.AddForce(launchVec, ForceMode2D.Impulse);
        OnLaunch();
        CameraManager.Instance.SetFollow(transform, camSpeed: rb.velocity.magnitude / 4);
        Destroy(gameObject, 20);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tag)) return;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        explosionRen.gameObject.SetActive(true);
        explosionRen.sprite = explosion;
        explosionRen.transform.localScale = new Vector2(explRadius, explRadius);

        bulletRen.enabled = false;
        colldr.radius = explRadius;
        Destroy(gameObject, 0.5f);
    }


    #region VISUAL
    private void OnLaunch()
    {
        StartCoroutine(FireEffectCO());
    }

    private const int durationFrames = 5;
    private IEnumerator FireEffectCO()
    {
        int i = 0;
        Sprite original = bulletRen.sprite;
        bulletRen.sprite = explosion;
        while (i < durationFrames)
        {
            yield return null;
            i++;
        }
        bulletRen.sprite = original;
    }
    #endregion
}
