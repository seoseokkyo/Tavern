using UnityEngine;

public class Interact_OpenCloseButton : Interactable
{
    public bool isOpend = false;
    public Material material;
    public override string GetInteractingDescription()
    {
        if (!isOpend)
        {
            return "Press [E] to Open";
        }
        else
        {
            return "Press [E] to Close";
        }
    }

    public override void Interact()
    {
        SetStore();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void SetStore()
    {
        if(!isOpend)
        {
            isOpend = true;
            ChangeColor(isOpend);
        }
        else
        {
            isOpend = false;
            ChangeColor(isOpend);
        }
    }

    private void ChangeColor( bool opend)
    {
        if(opend)
        {
            material.color = Color.green;
        }
        else
        {
            material.color = Color.blue;
        }
    }
}
