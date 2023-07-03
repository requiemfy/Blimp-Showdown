using UnityEngine;

public class WeaponVisuals : MonoBehaviour
{
    private WeaponType weapon;
    private SpriteRenderer barrelRen;
    void Start()
    {
        weapon = GetComponent<WeaponController>().WeaponType;
        barrelRen = transform.Find("Barrel").GetComponent<SpriteRenderer>();

        //Sprite
        barrelRen.sprite = weapon.barrel;
        barrelRen.transform.eulerAngles = new(0, 0, -45);
    }
}
