using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        foreach (Team team in DataPersistence.GetOpenedTeams())
        {
            if (DataPersistence.Get(team).isDestroyed) continue;
            Debug.LogWarning("Winner: " + team);
        }
    }
}
