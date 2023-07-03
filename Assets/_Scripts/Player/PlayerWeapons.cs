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
            weaponCtrl.weapon = WeaponTypes[i];
            weaponCtrl.Construct(tag);
            i++;
        }
    }


    public WeaponController GetWeaponCtrl(int index)
    {
        return weaponCtrls[index];
    }
}
