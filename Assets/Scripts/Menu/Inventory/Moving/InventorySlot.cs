using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public int numberCell => int.Parse(transform.name);
    public ItemInfo itemInfo;
    public bool isEquipmentSlot;
    public ItemType allowedType;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag?.GetComponent<InventoryItem>();
        if (draggedItem == null) return;

        InventorySlot sourceSlot = draggedItem.parentAfterDrag?.GetComponent<InventorySlot>();
        if (sourceSlot == null || sourceSlot == this) return;

        ItemInfo draggedInfo = sourceSlot.itemInfo;
        ItemInfo targetInfo = this.itemInfo;

        // Проверка: можно ли положить предмет в текущий слот
        if (this.isEquipmentSlot && draggedInfo.itemType != this.allowedType)
        {
            Debug.Log("Нельзя положить этот предмет в данный слот экипировки.");
            return;
        }

        // Проверка: можно ли положить предмет из этого слота в исходный
        if (sourceSlot.isEquipmentSlot && targetInfo != null && targetInfo.itemType != sourceSlot.allowedType)
        {
            Debug.Log("Нельзя переместить предмет отсюда, потому что целевой слот не поддерживает его тип.");
            return;
        }

        // Меняем местами предметы
        this.itemInfo = draggedInfo;
        sourceSlot.itemInfo = targetInfo;

        // Обновляем inventorySlotID
        (GameManager.InventoryData.slotItemIDs[this.numberCell], GameManager.InventoryData.slotItemIDs[sourceSlot.numberCell]) =
            (GameManager.InventoryData.slotItemIDs[sourceSlot.numberCell], GameManager.InventoryData.slotItemIDs[this.numberCell]);
    }

}

