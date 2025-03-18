using UnityEngine;

[System.Serializable]
public class SeatData 
{
    public GameObject chair;
    
    public Transform foodPositionLeft;
    public GameObject foodLeft;

    public Transform foodPositionRight;
    public GameObject foodRight;
    
    public bool isSitting;
}
