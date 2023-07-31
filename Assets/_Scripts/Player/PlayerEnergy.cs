using System;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public Action onEnergyChanged;
    private const int maxEnergy = 10;
    public int currentEnergy;

    public float GetRatio()
    {
        return (float) currentEnergy / maxEnergy;
    }

    public void Restore(int amount)
    {
        currentEnergy += amount;
        PopUpManager.Instance.SpawnText($"+{amount}", transform.position, CustomColors.Energy);
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        onEnergyChanged?.Invoke();
    }

    public bool Drain(int amount)
    {
        if (currentEnergy - amount >= 0)
        {
            PopUpManager.Instance.SpawnText($"-{amount}", transform.position, CustomColors.Energy);
            currentEnergy -= amount;
            onEnergyChanged();
            return true; 
        }
        PopUpManager.Instance.SpawnText("Not enough energy!", Camera.main.transform.position, Color.white);
        return false;
    }
}
