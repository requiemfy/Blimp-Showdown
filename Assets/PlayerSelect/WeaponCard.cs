using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCard : DragableItem
{
    //WARNING: Remember to check for raycast before changing prefab
    [field: SerializeField]
    public WeaponType Represent { get; private set; }

    [SerializeField] Image barrelImg;
    [SerializeField] TextMeshProUGUI energyTMP;
    protected override void Awake()
    {
        base.Awake();
    }


    private Vector2 beginScrollPos;
    protected override void OnBeginScroll()
    {
        beginScrollPos = WeaponBoard.Instance.RectTransfrom.anchoredPosition;
    }
    protected override void WhileScrolling(Vector2 scrollVec)
    {
        //var targetPos = new Vector2(beginScrollPos.x, beginScrollPos.y + scrollVec.y * SelectManager.Instance.sensitivity);
        //SelectManager.Instance.TryMoveScrollContentTo(targetPos);
        WeaponBoard.Instance.MoveUp(anchor: beginScrollPos, amount: scrollVec.y);
    }
    protected override void OnTap()
    {
        Debug.Log("tapped");
        SelectManager.Instance.infoSection.ShowInfoOf(Represent);
    }


    public void SetRepresent(WeaponType weapon)
    {
        Represent = weapon;
        barrelImg.sprite = weapon.barrel;
        energyTMP.text = weapon.energyCost.ToString();
    }
}
