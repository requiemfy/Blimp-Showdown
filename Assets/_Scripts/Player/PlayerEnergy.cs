using System;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public Action onEnergyChanged;
    private const int maxEnergy = 10;
    public int currentEnergy = 5;

    public float GetRatio()
    {
        return (float) currentEnergy / maxEnergy;
    }

    public void Restore(int amount)
    {
        if (currentEnergy >= maxEnergy) return;
        currentEnergy += amount;
        onEnergyChanged?.Invoke();
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
    }

    public bool Drain(int amount)
    {
        if (currentEnergy - amount >= 0)
        {
            currentEnergy -= amount;
            onEnergyChanged();
            return true; 
        }
        PopUpManager.Instance.SpawnText("Not enough energy!", Camera.main.transform.position, Color.white);
        return false;
    }
}
