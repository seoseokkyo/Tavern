using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviour
{
    public TMP_Text PlayerName;

    public void Init(string PlayerName)
    {
        this.PlayerName.text = PlayerName;
}
}
