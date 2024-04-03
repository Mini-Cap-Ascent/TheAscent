using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HostUIManager : MonoBehaviour
{
    public TMP_InputField roomNameInputField; // Reference to the input field where you enter the room name
    public BasicSpawner basicSpawner;
    public GameObject roomEntryPrefab;    // Reference to your room entry prefab
    public Transform roomListContentPanel; // Reference to the content panel where room entries will be instantiated
    public Button createLobbyButton;      // Reference to the create lobby button

    void Start()
    {
        // Add a listener to the create lobby button so that it calls CreateRoom when clicked
        createLobbyButton.onClick.AddListener(CreateRoom);
    }

    private void CreateRoom()
    {

        // Get the room name from the input field
        string roomName = roomNameInputField.text;
        basicSpawner.CreateAndHostRoom(roomName);

        // Instantiate the room entry prefab within the content panel
        GameObject roomEntry = Instantiate(roomEntryPrefab, roomListContentPanel);

        // Find the text components and buttons within the roomEntry to set their properties
        Text roomNameText = roomEntry.transform.Find("RoomNameText").GetComponent<Text>();
        Text playerCountText = roomEntry.transform.Find("PlayerCountText").GetComponent<Text>();
        Button kickButton = roomEntry.transform.Find("KickButton").GetComponent<Button>();

        // Set the room name and initial player count
        roomNameText.text = roomName;
        playerCountText.text = "0/4"; // Initial count, assuming no players have joined yet

        // Add listeners to the kick button (You need to implement KickAllPlayersFromRoom method)
        kickButton.onClick.AddListener(() => KickAllPlayersFromRoom(roomName));

        // Clear the input field
        roomNameInputField.text = "";

        // Here you would also include the code to create the room in your networking system
        // For example: PhotonNetwork.CreateRoom(roomName);
    }

    private void KickAllPlayersFromRoom(string roomName)
    {
        // Implement the functionality to kick all players from the room here
        // This will depend on your networking system, so replace with appropriate code
        Debug.Log("Kick all players from room: " + roomName);

        // For example, if using Photon:
        // PhotonNetwork.CloseRoom();
        // Then clear the UI list, or remove the specific room entry
    }

    // Update is called once per frame
    void Update()
    {
        // Here you could check for updates from the networking system, such as new players joining the room
        // and then update the player count accordingly.
    }
}
