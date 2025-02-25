using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatas", menuName = "Scriptable Objects/ItemData")]
public class ItemDatas : ScriptableObject
{
    public List<ItemData> items;
}
