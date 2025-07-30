using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public RectTransform imageTransform;
    public Image image;
    public Transform parentAfterDrag;
    public bool isPlaceholder = false;

    [ContextMenu("load")]
    public void Load()
    {
        imageTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        if (isPlaceholder) return; // Нельзя кликать по заглушке
        EventManager.Trigger(GameEvents.ItemClicked, transform.GetComponentInParent<InventorySlot>().itemInfo);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaceholder || imageTransform.sizeDelta.x == 0) return;

        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.parent.parent);
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaceholder || imageTransform.sizeDelta.x == 0) return;

        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlaceholder) return;

        image.raycastTarget = true;
        Debug.Log("parentAfterDrag: " + parentAfterDrag?.name);
        transform.SetParent(parentAfterDrag.transform);
        transform.localPosition = Vector3.zero;

        EventManager.Trigger(GameEvents.ItemDragEnded);
        parentAfterDrag = null;
    }
}
