using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks //다른 포톤 반응 받아들이기
{
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    public static Launcher instance;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings(); // 설정한 포톤 서버에 따라 마스터 서버에 연결
    }

    private void Awake()
    {
        instance = this; 
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby(); // 마스터 서버 연결 시 로비로 이동
        PhotonNetwork.AutomaticallySyncScene = true; // 자동으로 모든 사람들의 scene을 통일시켜줌.
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("Title");
        Debug.Log("Joined Lobby");

        // 들어온 사람 이름 랜덤으로 숫자붙여서 정해주기
        PhotonNetwork.NickName = "Player " + Random.Range(0, 100).ToString("0000");
    }

    /*
     * 방 만들기
     */
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
            return; // 방 이름이 빈값이면 방 안 만들어짐

        PhotonNetwork.CreateRoom(roomNameInputField.text);

        MenuManager.Instance.OpenMenu("Loading");

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message;
        MenuManager.Instance.OpenMenu("Error");
    }

    /*
     * 방 찾기
     */
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject); // 룸리스트가 업데이트 될 때마다 싹 지우기 
        }

        for(int i=0;i<roomList.Count;i++)
        {
            if (roomList[i].RemovedFromList) // 사라진 방은 목록에 표시하지 않음.
                continue;

            // prefab을 roomListContent 위치에 만들어주고, 그 프리펩은 i번째 룸리스트가 된다.
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    /*
     * 방에 들어왔을 때
     */
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name; // 들어간 방 이름 표시

        Player[] players = PhotonNetwork.PlayerList;
        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject); // 방에 들어가면 전에 있던 이름표들 전부 삭제
        }

        for(int i=0;i<players.Count();i++)
        {
            // prefab을 playerListContent 위치에 만들어주고, 그 프리펩은 i번째 플레이어리스트가 된다.
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient); // 방장만 게임시작 버튼 누르기 가능
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 방에 새로운 플레이어가 들어오면 이름표 만들어주기 
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient) // 방장이 나가서 방장이 바뀌었을 때
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient); // 역시, 방장만 게임시작 버튼 누르기 가능
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1); // 빌드의 scene 번호가 1인 gamescene 시작.
    }

    /*
     * 방 떠나기
     */
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Title");
    }
}
