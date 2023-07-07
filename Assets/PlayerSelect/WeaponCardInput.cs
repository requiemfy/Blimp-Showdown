using UnityEngine;

public class WeaponCardInput : DropSlot
{
    [SerializeField] int order;
  
    private void Start()
    {
        SelectManager.Instance.onSaved += ResetSlot;
        SelectManager.Instance.onStartedEdit += (team) =>
        {
            TeamData data = DataPersistence.Get(team);
            if (data == null) return;
            if (data.Weapons[order] == null) return;
            var card = Instantiate(WeaponBoard.Instance.WeaponCardPrefab, transform);
            card.SetRepresent(data.Weapons[order]);
        };
    }
    private void ResetSlot()
    {
        ClearDropSlot();
    }

    protected override void OnItemDropped(DragableItem droppedItem)
    {
        var droppedWeapon = droppedItem.GetComponent<WeaponCard>().Represent;
        SelectManager.Instance.StagedWeapons[order] = droppedWeapon;
        SelectManager.Instance.UpdateTotalHealth();
        SelectManager.Instance.UpdateTotalDamage();
    }
}
