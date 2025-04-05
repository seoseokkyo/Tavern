using System.Collections.Generic;
using UnityEngine;

public class MemoItemBase : ItemBase
{
    public List<string> orderedFoods;
    public string extraNote;

    public MemoItemBase(ItemData data, List<string> foods, string extra) : base(data)
    {
        this.orderedFoods = new List<string>(foods);
        this.extraNote = extra;
    }
}