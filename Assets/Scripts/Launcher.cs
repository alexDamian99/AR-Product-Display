using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;
    [Tooltip("The UI Label to inform the user about the connection failure")]
    [SerializeField]
    private TextMeshProUGUI warningText;
    [Tooltip("The UI Input that takes the room ID")]
    [SerializeField]
    private InputField roomId;

    bool isConnecting;
    #endregion


    #region Private Fields
    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";
    #endregion


    #region MonoBehaviour CallBacks
    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    #endregion


    #region Public Methods
    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt to create a new room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.CreateRoom("test", new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    
    public void ConnectWithId()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            if (roomId.text != "")
            {
                PhotonNetwork.JoinRoom(roomId.text);
            }
            else
            {
                this.warningText.text = "Enter room ID";
                progressLabel.SetActive(false);
                controlPanel.SetActive(true);
            }
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    #endregion


    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        if (roomId != null) //connect with id 
        {
            if (roomId.text != "")
            {
                PhotonNetwork.JoinRoom(roomId.text);
                isConnecting = false;
            }
            else
            {
                this.warningText.text = "Enter room ID";
                progressLabel.SetActive(false);
                controlPanel.SetActive(true);
            }
        }
        else //connected by creating a new room
        {
            PhotonNetwork.CreateRoom("test", new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRoomFailed() was called by PUN. No room available");
        // #Critical: we failed to join a room, maybe none exists or they are all full
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        this.warningText.text = "Failed to join room";
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) //TODO: maybe remove this if, if we don't use photon auto sync sense
        {
            Debug.Log("We load the 'Room for 1' ");
            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("Game Room");
        }
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        this.warningText.text = "Failed to create room. Please Try again.";
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    #endregion

}