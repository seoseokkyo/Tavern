using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderCanvasScript_TestSSK : MonoBehaviour
{
    public Transform ContentTransform;
    public ItemDatas itemdatas;
    public ItemUI ItemUI_Prefab;
    private List<ItemUI> ItemUIList = new List<ItemUI>();

    public TextMeshProUGUI stateText;
    public UnityEngine.UI.Slider timerSlider;

    private PlayerController TargetCon;

    private float ScaleValue = 0.1f;

    void Start()
    {
        TargetCon = TavernGameManager.Instance.CurrentLocalPlayer;

       // InitRandMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (TargetCon)
        {
            Vector3 Dir = transform.position - TargetCon.transform.position;
            Vector3 NormalDir = Dir.normalized;
            NormalDir.y = transform.position.y;

            Quaternion targetRotation = Quaternion.LookRotation(NormalDir);
           // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }

    void InitRandMenu()
    {
        // 1 ~ 3 랜덤 ( Order Num )
        int randNum = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < randNum; i++)
        {
            ItemUI tempUI = Instantiate(ItemUI_Prefab);

            // 일단 모든 아이템 랜덤
            int randItem = UnityEngine.Random.Range(0, ItemManager.Instance.items.Count);

            var TempItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.items[randItem]);

            tempUI.InitData(TempItemBase, ContentTransform, i);
            tempUI.transform.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
            tempUI.transform.localPosition = new Vector3(tempUI.transform.localPosition.x, tempUI.transform.localPosition.y, 0);
        }
    }

    public void SetOrderUI(List<ItemData> itemList)
    {
        int idx = 0;
        for (int i = 0; i < itemList.Count; i++)
        {
            for (int j = 0; j < itemdatas.items.Count; j++)
            {
                if (itemList[i].itemID == itemdatas.items[j].itemID)
                {
                    idx = i;
                    break;
                }
            }

            ItemUI tempUI = Instantiate(ItemUI_Prefab);
            var tempItemBase = ItemBase.ItemBaseCreator.CreateItemBase(itemList[i]);

            tempUI.InitData(tempItemBase, ContentTransform, i);
            tempUI.transform.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
            tempUI.transform.localPosition = new Vector3(tempUI.transform.localPosition.x, tempUI.transform.localPosition.y, 0);
        }
    }

    public void RemoveOrderUI(ItemData item)
    {
        int iCount = ContentTransform.childCount;
        for(int i = iCount - 1; i >= 0; i--)
        {
            var temp = ContentTransform.GetChild(i);
            if(temp != null)
            {
                var uiTemp = temp.GetComponent<ItemUI>();
                if (uiTemp != null)
                {
                    if (uiTemp.CurrentItemBase.CurrentItemData.itemID == item.itemID)
                    {
                        Destroy(temp.gameObject);
                        break;
                    }
                }
            }
        }
        
    }
}
