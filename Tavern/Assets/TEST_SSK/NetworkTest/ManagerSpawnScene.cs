using UnityEngine;
using Photon.Pun;

public class ManagerSpawnScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.LoadLevel("MainMenuScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
