using UnityEngine;

public interface IInteractAble
{
    enum EInteractType
    {
        Loot,
        Interact,
        EInteractTypeMax
    };

    EInteractType InteractType { get; set; }
    public string InteractAbleName { get; set; }

    //public Player UsingPlayer { get; set; }

    void Interact();
}
