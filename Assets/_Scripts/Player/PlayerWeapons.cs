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
            Debug.Log(type.name);
            weaponCtrl.Construct(tag, type);
            i++;
        }
    }
    public WeaponController GetWeaponCtrl(int index)
    {
        return weaponCtrls[index];
    }
}
