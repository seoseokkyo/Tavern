using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class OrderCanvasScript_TestSSK : MonoBehaviour
{
    public Transform ContentTransform;

    public ItemUI ItemUI_Prefab;
    private List<ItemUI> ItemUIList = new List<ItemUI>();

    private PlayerController TargetCon;

    private float ScaleValue = 0.1f;

    void Start()
    {
        TargetCon = FindFirstObjectByType<PlayerController>();

        InitRandMenu();
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
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }

    void InitRandMenu()
    {
        // 1 ~ 3 ���� ( Order Num )
        int randNum = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < randNum; i++)
        {
            ItemUI tempUI = Instantiate(ItemUI_Prefab);

            // �ϴ� ��� ������ ����
            int randItem = UnityEngine.Random.Range(0, ItemManager.Instance.items.Count);

            var TempItemBase = ItemBase.ItemBaseCreator.CreateItemBase(ItemManager.Instance.items[randItem]);

            tempUI.InitData(TempItemBase, ContentTransform, i);
            tempUI.transform.localScale = new Vector3(ScaleValue, ScaleValue, ScaleValue);
            tempUI.transform.localPosition = new Vector3(tempUI.transform.localPosition.x, tempUI.transform.localPosition.y, 0);
        }
    }
}
