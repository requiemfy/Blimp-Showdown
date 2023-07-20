using System.Collections;
using System.Collections.Generic;
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
        CinemachineManager.Instance.SetFollow(null);
        Destroy(collision.gameObject, 2f);
    }
}
