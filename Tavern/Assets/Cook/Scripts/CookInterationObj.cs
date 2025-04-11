using System;
using System.Collections.Generic;
using UnityEngine;

public enum ECookType
{
    Grill,
    //Slice,    얘는 일단 따로 하는걸로
    Boiling,
    Fry,
    ECookTypeMax
}

public class PairTransformWorldItem
{
    public Transform Item1;
    public WorldItem Item2;
}

public class CookInterationObj : Interactable
{
    private string InteractDescription;

    private WorldItem PlayerHandleWorldItem = null;

    private List<WorldItem> CookingItem = new List<WorldItem>();

    // 초당 증가시킬 Cook Value
    [HideInInspector]
    public float CookingPerSecond = 1;

    public ECookType type;

    public List<Transform> ItemSockets;

    [HideInInspector]
    public List<PairTransformWorldItem> TransformAndUseState = new List<PairTransformWorldItem>();

    public CurrentCookingIngredientsUI UI_Prefab;

    public override string GetInteractingDescription()
    {
        return InteractDescription;
    }

    public override void Interact()
    {
        PlayerHandleWorldItem = interactPlayer.CurrentPlayer.RightHandItem;

        if (null == PlayerHandleWorldItem)
        {
            // 안에 들어있는 애들 상태랑 꺼내기 버튼등이 있는 UI출력
            var temp = Instantiate(UI_Prefab);
            temp.transform.SetParent(interactPlayer.PlayerCanvas.transform, false);

            temp.SetData(type, CookingItem);
        }
        else
        {
            if (ItemSockets.Count <= CookingItem.Count)
            {
                // 최대 동시 조리개수 초과

                return;
            }

            foreach (var Pos in TransformAndUseState)
            {
                if (null == Pos.Item2)
                {
                    Pos.Item2 = PlayerHandleWorldItem;

                    PlayerHandleWorldItem.GetComponent<Collider>().enabled = false;
                    PlayerHandleWorldItem.transform.SetParent(gameObject.transform, false);
                    PlayerHandleWorldItem.transform.localPosition = Pos.Item1.localPosition;
                    PlayerHandleWorldItem.transform.localRotation = Pos.Item1.localRotation;
                    PlayerHandleWorldItem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                    CookingItem.Add(PlayerHandleWorldItem);

                    interactPlayer.CurrentPlayer.RightHandItem = null;

                    break;
                }
            }

            //interactPlayer.CurrentPlayer.ItemDetachFromRightHand();
        }
    }

    public void IngredientTakeOut(WorldItem TargetItem)
    {
        int Count = CookingItem.Count;

        for (int i = 0; i < Count; i++)
        {
            if (CookingItem[i] == TargetItem)
            {
                interactPlayer.CurrentPlayer.ItemAttachToRightHand(TargetItem);

                foreach (var Pos in TransformAndUseState)
                {
                    if(Pos.Item2 == CookingItem[i])
                    {
                        Pos.Item2 = null;
                    }
                }

                CookingItem[i] = null;
            }
        }
    }

    private void Awake()
    {

    }

    public void Start()
    {
        CookingItem.Clear();

        foreach (var Pos in ItemSockets)
        {
            PairTransformWorldItem temp = new PairTransformWorldItem();
            temp.Item1 = Pos;
            temp.Item2 = null;

            TransformAndUseState.Add(temp);
        }
    }

    void FixedUpdate()
    {
        foreach (var item in CookingItem)
        {
            var Ingredient = item.GetComponent<IngredientComp>();

            float Value = CookingPerSecond * Time.fixedDeltaTime;

            switch (type)
            {
                case ECookType.Grill:
                    Ingredient.AccumulateGrilledValue(Value);
                    break;
                case ECookType.Boiling:
                    Ingredient.AccumulateBoiledValue(Value);
                    break;
                case ECookType.Fry:
                    Ingredient.AccumulateFriedValue(Value);
                    break;
            }
        }
    }
}
