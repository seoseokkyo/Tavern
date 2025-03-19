using UnityEngine;

[System.Serializable]
public class SeatData 
{
    public GameObject chair;
    
    public Transform foodPositionLeft;
    public WorldItem foodLeft;

    public Transform foodPositionRight;
    public WorldItem foodRight;
    
    public bool isSitting;
}
