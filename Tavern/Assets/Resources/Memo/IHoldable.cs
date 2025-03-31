using UnityEngine;

public interface IHoldable
{
    void OnPickedUp(Transform handTransform);
    void OnDropped();
}
