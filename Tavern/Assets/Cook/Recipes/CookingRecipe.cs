using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct IngredientAmount
{
    public string itemName;
    public int amount;
}

[CreateAssetMenu]
public class CookingRecipe : ScriptableObject
{
    public List<IngredientAmount> ingredients;

    public string resultItemName;
    public ItemDatas result;
    public float cookingTime;
}
