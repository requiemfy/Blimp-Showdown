using UnityEngine;
using UnityEngine.UI;

public class WeaponCardInput : DropSlot
{
    [SerializeField] int order;
    private Image image;
    private Color originalCol;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start()
    {
        originalCol = image.color;
        SelectHandler.Instance.onTeamChanged += ResetSlot;
    }
    private void ResetSlot()
    {
        ClearDropSlot();
        image.color = originalCol;
    }

    protected override void OnItemDropped(DragableItem droppedItem)
    {
        image.color = Color.clear;
        var droppedWeapon = droppedItem.GetComponent<WeaponCard>().represent;
        var currentTeam = SelectHandler.Instance.currentTeam;
        currentTeam.Open();
        currentTeam.Set(droppedWeapon, order);
        SelectHandler.Instance.UpdateTotalHealth();
        SelectHandler.Instance.UpdateTotalDamage();
    }
}
