using UnityEngine;

public class WeaponCardInput : DropSlot
{
    [SerializeField] int order;
  
    private void Start()
    {
        SelectManager.Instance.onStartedEdit += (team) =>
        {
            ClearDropSlot();
            TeamData data = DataPersistence.Get(team);
            if (data == null) return;
            if (data.Weapons[order] == null) return;
            var card = Instantiate(WeaponBoard.Instance.WeaponCardPrefab, transform);
            card.HideFrame();
            card.SetRepresent(data.Weapons[order]);
        };
    }

    protected override void OnItemDropped(DragableItem droppedItem)
    {
        if (droppedItem.TryGetComponent(out WeaponCard card))
        {
            AudioManager.Instance.PlayAudioGroup("InsertWeapon");
            var droppedWeapon = card.Represent;
            SelectManager.Instance.StagedWeapons[order] = droppedWeapon;
            SelectManager.Instance.UpdateTotalHealth();
            SelectManager.Instance.UpdateTotalDamage();
        }
    }
}
