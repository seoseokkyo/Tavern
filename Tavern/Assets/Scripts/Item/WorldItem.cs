using UnityEngine;
using static IInteractAble;

public class WorldItem : MonoBehaviour, IInteractAble
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public EInteractType InteractType { get; set; } = EInteractType.Loot;
    public string InteractAbleName { get; set; } = "Item";

    public ItemBase item;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {

    }
}
