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
		joinRoomButton.interactable = info.PlayerCount < info.MaxPlayers; //���� ���� ���� ����
	}

    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby(); //�κ� �ִٰ� �濡 ���ٰ� �ϸ�, �ݵ�� �κ� ���������. �κ� ������ ������ �κ� ��� �ִٰ� �Ǵ��� ��. 
		PhotonNetwork.JoinRoom(roomInfo.Name); 
    }
}
