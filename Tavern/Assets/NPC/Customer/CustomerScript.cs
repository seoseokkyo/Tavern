using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    List<ItemData> orderItems = new List<ItemData>();
    public GameObject menuObj;
    private MenuManager menuManager;

    public GameObject orderUIObject;
    private OrderCanvas orderUI;

    private float time = 0f;
    public bool isOrdering = false;
    public bool isOrdered = false;

    void Start()
    {
        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
    }

    void Update()
    {
        if(isOrdering == false)
        {
            time += Time.deltaTime;
            if (time > 10)
            {
                time = 0;
                isOrdering = true;
            }
        }
        if (isOrdering == true && isOrdered == false)
        {
            time += Time.deltaTime;
            if(time > 5)
            {
                time = 0;
                isOrdered = true;
                Initialize();
                DecideOrder();
            }
        }

    }

    void checkTable()
    {
        
    }

    void Initialize()
    {
        menuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        if (menuManager != null)
        {
            orderItems.Clear();
        }
        
        orderUI = orderUIObject.GetComponent<OrderCanvas>();
        if(orderUI != null)
        {
            orderUI.enabled = true;
        }
    }

    private List<ItemData> GetMenuFromManager()
    {
        if(menuManager != null)
        {
            return menuManager.GetMenuList();
        }

        return null;
    }

    private void DecideOrder()
     {
        // while ������ ���� ������ �ߴµ� ���� �մ� NPC ���� ���� UI ũ�� ��ü�� �ʹ� �۾Ƽ� ���� ItemUI ���� ��������
        // ScrollView Content �� ����� ������� �ʾƼ� �׳� .. image 3�� ������� �ϳ��� �Ѱ� �ΰ��� �ΰ� �̷� ������ �ص� 
        // �׷��� ���� �׳� 1�� �ֹ� or 2�� �ֹ� �������� ���ư��� �������.

        orderItems.Clear();

        List<ItemData> menu = GetMenuFromManager();
        if(menu != null)
        {
            int maxOrderCount = Random.Range(1, 3);
            int random = Random.Range(0, menu.Count);
            ItemData temp = menu[random];
            orderItems.Add(temp);
            // �ֹ� 1����
            if (maxOrderCount == 1)
            {
                orderUI.SetOrderUI(temp, 0);
            }
            // �ֹ� 2��
            else if(maxOrderCount == 2)
            {
                orderUI.SetOrderUI(temp, 1);

                int nextRandom = Random.Range(0, menu.Count);
                ItemData nextTemp = menu[nextRandom];
                orderItems.Add(nextTemp);
                orderUI.SetOrderUI(nextTemp, 2);
            }
        }
    }

    private void SetOrder()
    {

    }

    public void CheckOrder(ItemData servedItem)
    {
        for(int i = 0; i < orderItems.Count; i++)
        {

        }
    }
}
