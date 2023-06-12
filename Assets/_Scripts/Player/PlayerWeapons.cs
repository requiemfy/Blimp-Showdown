using DG.Tweening;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] Transform indicator;

    private PlayerController playerCtrl;
    private WeaponController[] weaponCtrls;

    private void Start()
    {
        playerCtrl = GetComponent<PlayerController>();
        Team team = tag.GetTeamFromTag();
        SetWeapons(team.GetWeapons());
    }
    public void SetWeapons(Weapon[] weapons)
    {
        int i = 0;
        weaponCtrls = GetComponentsInChildren<WeaponController>();
        foreach (WeaponController weaponCtrl in weaponCtrls)
        {
            weaponCtrl.weapon = weapons[i];
            weaponCtrl.tag = tag;
            int i2 = i;
            weaponCtrl.onPointerDown += () => FocusOn(i2);
            i++;
        }
        indicator.gameObject.SetActive(false);
    }


    public void FocusOn(int index)
    {
        if (!playerCtrl.isInTurn) return;
        var target = weaponCtrls[index];
        if (target.isCollapsed)
        {
            Debug.Log("Weapon collapsed");
            return;
        }

        HUD.Instance.StartObserveWeapon(target);
        indicator.gameObject.SetActive(true);
        indicator.DOMove(target.transform.position + new Vector3(0,1,0), duration: 0.5f);
    }
}
