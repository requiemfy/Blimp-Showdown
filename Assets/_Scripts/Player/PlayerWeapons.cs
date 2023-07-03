using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    private WeaponController[] weaponCtrls;

    public void Construct(WeaponType[] WeaponTypes)
    {
        int i = 0;
        weaponCtrls = GetComponentsInChildren<WeaponController>();
        foreach (WeaponController weaponCtrl in weaponCtrls)
        {
            weaponCtrl.Construct(tag, WeaponTypes[i]);
            i++;
        }
    }
    public WeaponController GetWeaponCtrl(int index)
    {
        return weaponCtrls[index];
    }
}
