using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public abstract class Interactable : MonoBehaviourPunCallbacks
{
    public enum InteractionType
    {
        Press,
        Hold,
        UIPop,
    }

    public PlayerController interactPlayer;

    float holdTime;
    public float GetHoldTime() => holdTime;
    public void IncreaseHoldtime() => holdTime += Time.deltaTime;
    public void ResetHoldTime() => holdTime = 0f;

    public InteractionType interactionType;
    public abstract string GetInteractingDescription();
    public abstract void Interact();
}

