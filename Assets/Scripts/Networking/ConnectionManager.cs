using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon using your project's settings.
        PhotonNetwork.NickName = "Player ";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        PhotonNetwork.JoinOrCreateRoom("TestRoom", new RoomOptions { MaxPlayers = 5 }, null); // Join or create a room named "TestRoom".
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        SendMessage();
    }

    void SendMessage()
    {

      

        photonView.RPC("ReceiveMessage", RpcTarget.All, "Hello from PhotonNetwork");
    }

    [PunRPC]
    public void ReceiveMessage(string message)
    {
        Debug.Log("Message received: " + message);
    }
}
