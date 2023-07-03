using UnityEngine;

public class WeaponCard : DragableItem
{
    public WeaponType represent;

    private Vector2 beginScrollPos;

    protected override void OnBeginScroll()
    {
        beginScrollPos = SelectHandler.Instance.scrollContent.anchoredPosition;
    }
    protected override void WhileScrolling(Vector2 scrollVec)
    {
        var targetPos = new Vector2(beginScrollPos.x, beginScrollPos.y + scrollVec.y * SelectHandler.Instance.sensitivity);
        SelectHandler.Instance.TryMoveScrollContentTo(targetPos);
    }

    protected override void OnTap()
    {
        Debug.Log("tapped");
        SelectHandler.Instance.infoSection.ShowInfoOf(represent);
    }

}
