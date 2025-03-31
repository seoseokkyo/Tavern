using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MemoData
{
    public List<FoodSelect> foods = new List<FoodSelect>();
    public string extraNote = "";
}
