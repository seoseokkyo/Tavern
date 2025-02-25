using UnityEngine;
using System.Collections.Generic;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Press,
        Hold,
        UIPop,
    }

    float holdTime;
    public float GetHoldTime() => holdTime;
    public void IncreaseHoldtime() => holdTime += Time.deltaTime;
    public void ResetHoldTime() => holdTime = 0f;

    public InteractionType interactionType;
    public abstract string GetInteractingDescription();
    public abstract void Interact();
}

