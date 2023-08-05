using DG.Tweening;
using UnityEngine;

public class BorderShrinking : MonoBehaviour
{
    [SerializeField]
    private float unitScale = 2.51f;
    private void Awake()
    {
        GameManager.Instance.onCycleEnded += () =>
        {
            float decrease = 2 * unitScale;
            Vector2 end = new(transform.localScale.x - decrease, transform.localScale.y - decrease);
            if (end.x < 0 || end.y < 0) end = Vector2.zero;
            transform.DOScale(end, duration: 1);
        };
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            if (health.transform.parent.TryGetComponent(out WeaponController weapon))
            {
                weapon.isInDeadZone = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            if (health.transform.parent.TryGetComponent(out WeaponController weapon))
            {
                weapon.isInDeadZone = true;
            }
        }
    }
}
