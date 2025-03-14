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
        // while 문으로 랜덤 돌리려 했는데 지금 손님 NPC 위에 띄우는 UI 크기 자체가 너무 작아서 기존 ItemUI 같은 프리팹은
        // ScrollView Content 에 제대로 담기지를 않아서 그냥 .. image 3장 띄워놓고 하나면 한개 두개면 두개 이런 식으로 해둠 
        // 그러다 보니 그냥 1개 주문 or 2개 주문 조건으로 돌아가게 만들었음.

        orderItems.Clear();

        List<ItemData> menu = GetMenuFromManager();
        if(menu != null)
        {
            int maxOrderCount = Random.Range(1, 3);
            int random = Random.Range(0, menu.Count);
            ItemData temp = menu[random];
            orderItems.Add(temp);
            // 주문 1개만
            if (maxOrderCount == 1)
            {
                orderUI.SetOrderUI(temp, 0);
            }
            // 주문 2개
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
