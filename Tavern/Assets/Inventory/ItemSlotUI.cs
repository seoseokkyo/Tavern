using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IDropHandler
{
    public int SlotIndex;

    public InventoryComp OwnerInventory;

    public bool bNoUseDrop = false;

    public Outline SlotOutline;

    private void Awake()
    {
        SlotOutline.enabled = false;
    }

    public void Start()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(bNoUseDrop)
        {
            return;
        }

        if (null != ItemDrag.beingDraggedIcon)
        {
            ItemUI DragItemUI = ItemDrag.beingDraggedIcon.GetComponent<ItemUI>();

            if (null != DragItemUI)
            {
                OwnerInventory.SwapItemByIndex(ref OwnerInventory, ref DragItemUI.CurrentItemBase.OwnerInventory, SlotIndex, DragItemUI.ItemIndex);

                Destroy(DragItemUI.gameObject);
            }
        }
    }
}
