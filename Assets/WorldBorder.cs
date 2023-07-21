using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldBorder : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) return;
        if (CinemachineManager.Instance.VCam.m_Follow == collision.transform)
        {
            CinemachineManager.Instance.SetFollow(null);
            Debug.Log("Is following the thing that going out of border");
        }
        Destroy(collision.gameObject, 2f);
    }
}
