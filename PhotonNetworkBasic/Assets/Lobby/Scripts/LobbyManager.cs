using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

//MonoBehaviourPunCallbacks : 포톤 서버에서 보내는 반응을 받을 수 있는 클래스 
// MonoBehaviourPunCallbacks = MonoBehaviour + Pun + Callbacks -> MonoBehaviour를 상속받은 클래스이기 때문에 MonoBehaviour의 내용도 모두 사용이 가능함

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Lobby, Room }

    [SerializeField] StatePanel statePanel;

    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MenuPanel menuPanel;
    [SerializeField] RoomPanel roomPanel;
    [SerializeField] LobbyPanel lobbyPanel;

	private void Start()
	{
        SetActivePanel(Panel.Login); //무조건 시작 시 Login화면 나오도록 설정
	}

	/// <summary>
	/// 접속됐을 때 행동
	/// </summary>
	public override void OnConnectedToMaster()
	{
        SetActivePanel(Panel.Menu);
	}

    /// <summary>
    /// 접속 끊겼을 때 행동
    /// </summary>
    /// <param name="cause"></param>
	public override void OnDisconnected(DisconnectCause cause)
	{
		SetActivePanel(Panel.Login);
	}

	/// <summary>
	/// 방 만들기 실패
	/// </summary>
	/// <param name="returnCode"></param>
	/// <param name="message"></param>
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		BackToTheMenuPanel(returnCode, message);
	}

	/// <summary>
	/// 플레이어가 방에 입장
	/// </summary>
	public override void OnJoinedRoom()
	{
		//플레이어가 방 만들면 당연히 만들어진 방에 들어감. -> "플레이어가 방에 들어갔다? = 방 만들기에 성공했다"로 방만들기 성공여부 판단이 가능.

		SetActivePanel(Panel.Room); //방 만들기 성공했으니 방 panel을 활성화 시킴

		//플레이어의 상태를 초기화해줘야 함 -> 다른 방에 들어갔을 때 이전 상태가 유지될 수 있음
		PhotonNetwork.LocalPlayer.SetReady(false);
		PhotonNetwork.LocalPlayer.SetLoad(false);

		PhotonNetwork.AutomaticallySyncScene = true; // 마스터 클라이언트와 일반 클라이언트들이 레벨을 동기화함.
													 // => 씬을 전환할 때 방장이 있는 씬으로 모두 같이 이동함

		roomPanel.EntryPlayer(PhotonNetwork.LocalPlayer); 
	}

	/// <summary>
	/// 플레이어가 방을 나감
	/// </summary>
	public override void OnLeftRoom()
	{
		PhotonNetwork.AutomaticallySyncScene = false; //false로 안 돌려 두면 Local Player가 방을 나갈 때 모든 유저가 같이 따라서 나가게 됨.

		roomPanel.LeavePlayer(PhotonNetwork.LocalPlayer);

		SetActivePanel(Panel.Menu);
	}

	/// <summary>
	/// 이미 내가 방안에 있는 상태에서 새로운 플레이어가 들어 왔을 때 호출
	/// </summary>
	/// <param name="newPlayer"></param>
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		roomPanel.EntryPlayer(newPlayer);
	}

	/// <summary>
	/// 이미 내가 방안에 있는 상태에서 다른 플레이어가 방을 나갈 때 호출
	/// </summary>
	/// <param name="otherPlayer"></param>
	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		roomPanel.LeavePlayer(otherPlayer);
	}

	/// <summary>
	/// 방장이 바뀌었을 때
	/// </summary>
	/// <param name="newMasterClient"></param>
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if(newMasterClient.IsMasterClient)
			roomPanel.CheckPlayerReady();
	}

	/// <summary>
	/// 플레이어의 레디 상황이 바뀌었을 때 호출함 -> 바뀐 결과를 통보하는 역할
	/// </summary>
	/// <param name="targetPlayer"></param>
	/// <param name="changedProps"></param>
	public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
	{
		roomPanel.UpdatePlayerState(targetPlayer);
	}

	/// <summary>
	/// 방 입장 실패
	/// </summary>
	/// <param name="returnCode"></param>
	/// <param name="message"></param>
	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		BackToTheMenuPanel(returnCode, message);
	}

	/// <summary>
	/// 방 랜덤 입장 실패
	/// </summary>
	/// <param name="returnCode"></param>
	/// <param name="message"></param>
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		BackToTheMenuPanel(returnCode, message);
	}

	/// <summary>
	/// 로비에 입장
	/// </summary>
	public override void OnJoinedLobby()
	{
		SetActivePanel(Panel.Lobby);
	}

	/// <summary>
	/// 로비 나가기
	/// </summary>
	public override void OnLeftLobby()
	{
		SetActivePanel(Panel.Menu);
	}

	/// <summary>
	/// 방 목록 갱신 될 때마다 호출
	/// </summary>
	/// <param name="roomList"></param>
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		lobbyPanel.UpdateRoomList(roomList);
	}

	/// <summary>
	/// 지정된 특정 패널만 활성화, 나머지는 비활성화 하도록 설정
	/// </summary>
	/// <param name="panel"></param>
	private void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject?.SetActive(panel == Panel.Login);
        menuPanel.gameObject?.SetActive(panel == Panel.Menu);
        roomPanel.gameObject?.SetActive(panel == Panel.Room);
        lobbyPanel.gameObject?.SetActive(panel == Panel.Lobby);
    }

	private void BackToTheMenuPanel(short returnCode, string message)
	{
		SetActivePanel(Panel.Menu); //실패했으니까 다시 메뉴 창으로 복귀

		string errorMsg = $"Create room faild with error ({returnCode}) : {message}";
		Debug.Log(errorMsg); //실패 사유 로그로 기록
		statePanel.AddMessage(errorMsg);
	}
}
