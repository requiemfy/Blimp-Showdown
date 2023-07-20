using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldBorder : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            PopUpManager.Instance.SpawnText("Do not fly out of border", player.transform.position, Color.white);
            return;
        }
        if (CinemachineManager.Instance.VCam.m_Follow == collision.transform)
        {
            CinemachineManager.Instance.SetFollow(null);
            Debug.Log("Is following the thing that going out of border");
        }
        Destroy(collision.gameObject, 2f);
    }
}
