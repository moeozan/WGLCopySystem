using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.InteropServices;
using System.Text;

public class Launcher : MonoBehaviourPunCallbacks, ILobbyCallbacks, IMatchmakingCallbacks
{

    public static Launcher launcher;
    private string roomName;
    [Header("Your Origin URL")]
    [SerializeField] private string origin;

    [Header("All other things")]
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] private int maxPlayers;
    public int MaxPlayers { get { return maxPlayers; } }
    [SerializeField] private Text activeLobbyPlayers;
    private string roomLink;
    public string RoomLink { get { return roomLink; } }

    [DllImport("__Internal")]
    private static extern string GetURLFromPage();
    [DllImport("__Internal")]
    private static extern void SendURL(string x);
    private void Awake()
    {
        if (launcher == null)
        {
            launcher = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("ConnectedToMaster");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("You are in lobby");
        MenuManager.instance.OpenMenu("title");
        GetUniqueName(PhotonNetwork.LocalPlayer);
        FriendsRoom();
    }


    public void CreateRoom()
    {
        roomName = GetUniqueRoom();
        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, IsOpen = true, MaxPlayers = (byte)maxPlayers };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        MenuManager.instance.OpenMenu("loading-lobby");
        Debug.Log(roomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }
    public override void OnJoinedRoom()
    {
        Player[] players = PhotonNetwork.PlayerList;
        MenuManager.instance.OpenMenu("loading-lobby");
        foreach (Transform chield in playerListContent)
        {
            Destroy(chield.gameObject);
        }
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        activeLobbyPlayers.text = players.Length.ToString() + " / " + maxPlayers;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Player[] players = PhotonNetwork.PlayerList;
        MenuManager.instance.OpenMenu("loading-lobby");
        foreach (Transform chield in playerListContent)
        {
            Destroy(chield.gameObject);
        }
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        activeLobbyPlayers.text = players.Length.ToString() + " / " + maxPlayers;
        if (players.Length == maxPlayers)
        {
            //U can start the game
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Waiting for other players");
        Player[] players = PhotonNetwork.PlayerList;
        MenuManager.instance.OpenMenu("loading-lobby");
        foreach (Transform chield in playerListContent)
        {
            Destroy(chield.gameObject);
        }
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        activeLobbyPlayers.text = players.Length.ToString() + " / " + maxPlayers;
    }
    //////////////////////////////////////////////////////////////////////

    public string RandomString(int size, bool lowerCase = false)
    {
        var builder = new StringBuilder(size);
        char offset = lowerCase ? 'a' : 'A';
        const int lettersOffset = 26;

        for (var i = 0; i < size; i++)
        {
            var @char = (char)Random.Range(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        return lowerCase ? builder.ToString().ToLower() : builder.ToString();
    }

    private string GetUniqueRoom()
    {
    one:
        roomName = RandomString(10);
        List<RoomInfo> rooms = new List<RoomInfo>();
        foreach (var room in rooms)
        {
            if (room.Name == roomName)
            {
                goto one;
            }
        }
        return roomName;
    }

    private void GetUniqueName(Player player)
    {
    one:
        player.NickName = "P " + RandomString(13);
        List<Player> players = new List<Player>();
        foreach (var p in players)
        {
            if (p.NickName == player.NickName)
            {
                goto one;
            }
        }
    }

    private void FriendsRoom()
    {
        string url = GetURLFromPage();
        if (url == "")
        {
            return;
        }
        PhotonNetwork.JoinRoom(url);
    }
    public void CopyToClipboard()
    {
        TextEditor textE = new TextEditor();
        textE.text  = origin + "?=" + PhotonNetwork.CurrentRoom.Name;
        textE.SelectAll();
        SendURL(textE.text.ToString());
    }
}





