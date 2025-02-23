using UnityEngine;

public class Interact_Short : Interactable
{
    public Material mat;

    public override string GetInteractingDescription()
    {
        return "Press [E] to Change Color";
    }


    public override void Interact()
    {
        mat.color = new Color(Random.value, Random.value, Random.value);
    }
}
