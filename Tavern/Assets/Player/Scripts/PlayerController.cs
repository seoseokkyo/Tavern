using System;
using System.Threading;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InventoryComp PlayerInventory;

    void Start()
    {
        PlayerInventory = GetComponent<InventoryComp>();

        // 인벤토리(퀵슬롯) 사이즈
        PlayerInventory.InventoryInitialize(10);
    }

    void Update()
    {
        if (UnityEngine.Input.GetButtonDown("DropItem"))
        {
            // 인벤토리에서 아이템을 빼오는 코드
            ItemBase item = null;
            for (int i = 0; i < PlayerInventory.GetInventorySize(); i++)
            {
                if (PlayerInventory.CheckItem(i) != null)
                {
                    item = PlayerInventory.PopItem(i);
                    break;
                }
            }

            // 빼온 아이템을 월드에 스폰하는 코드
            if (item != null)
            {
                Vector3 Position = gameObject.transform.position;
                Quaternion Rotation = new Quaternion();

                Position += gameObject.transform.forward * 5;

                ItemManager.Instance.ItemSpawn(item, Position, Rotation);
            }
        }

        if (UnityEngine.Input.GetButtonDown("InventoryOpen"))
        {
            var modeCon = gameObject.GetComponent<ModeController>();

            gameObject.GetComponent<ModeController>().SetMode(!modeCon.GetMode());
        }
    }
}
