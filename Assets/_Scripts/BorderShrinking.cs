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
            transform.DOScale(new Vector2(transform.localScale.x - 2*unitScale, transform.localScale.y - 2 * unitScale), duration: 1);
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
