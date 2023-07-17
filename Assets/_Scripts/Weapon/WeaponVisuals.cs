using UnityEngine;

public class WeaponVisuals : MonoBehaviour
{
    void Start()
    {
        var weapon = GetComponent<WeaponController>().WeaponType;
        var barrelRen = transform.Find("Barrel").GetComponent<SpriteRenderer>();

        //Sprite
        barrelRen.sprite = weapon.barrel;
        barrelRen.transform.eulerAngles = new(0, 0, -135);
    }
}
