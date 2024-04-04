using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;


public class SessionListEntry : MonoBehaviour
{
    public TextMeshProUGUI roomName, playerCount;
    public Button joinButton;

    public void JoinRoom()
    {
        // Access the NetworkManager instance and call JoinSession with the room name
        FindObjectOfType<NetworkManager>().JoinSession(roomName.text);
    }
}
