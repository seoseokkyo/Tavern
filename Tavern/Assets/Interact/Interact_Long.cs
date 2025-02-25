using UnityEngine;

public class Interact_Long : Interactable
{
    public Material mat;
    public override string GetInteractingDescription()
    {
        return "";
    }

    public override void Interact()
    {
        mat.color = new Color(Random.value, Random.value, Random.value);
    }

}
