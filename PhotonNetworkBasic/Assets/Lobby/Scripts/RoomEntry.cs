using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text currentPlayer;
    [SerializeField] Button joinRoomButton;

    private RoomInfo roomInfo;

    public void SetRoomInfo(RoomInfo info)
	{
		roomInfo = info;

		roomName.text = info.Name;
		currentPlayer.text = $"{info.PlayerCount} / {info.MaxPlayers}";
		joinRoomButton.interactable = info.PlayerCount < info.MaxPlayers; //입장 가능 여부 설정
	}

    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby(); //로비에 있다가 방에 들어간다고 하면, 반드시 로비를 나가줘야해. 로비를 나가지 않으면 로비에 계속 있다고 판단을 함. 
		PhotonNetwork.JoinRoom(roomInfo.Name); 
    }
}
