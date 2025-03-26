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
        var renderer = GetComponent<Renderer>();
        material = renderer.material;
    }

    void Update()
    {
        StateChanged(TavernGameManager.Instance.TavernOpen);
    }

    private void SetStore()
    {
        TavernGameManager.Instance.TavernOpen ^= true;
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

    private void StateChanged(bool bState)
    {
        if (bState != isOpend)
        {
            ChangeColor(bState);
            isOpend = bState;
        }
    }
}
