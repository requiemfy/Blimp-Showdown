using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    private WeaponController[] weaponCtrls;
    public void Construct(WeaponType[] weaponTypes)
    {
        int i = 0;
        weaponCtrls = GetComponentsInChildren<WeaponController>();
        foreach (WeaponController weaponCtrl in weaponCtrls)
        {
            WeaponType type = weaponTypes[i];
            weaponCtrl.Construct(gameObject.layer, type);
            i++;
        }
    }
    public WeaponController GetWeaponCtrl(int index)
    {
        return weaponCtrls[index];
    }
}
