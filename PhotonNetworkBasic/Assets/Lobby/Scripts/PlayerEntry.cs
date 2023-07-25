using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] TMP_Text playerReady;
    [SerializeField] Button playerReadyButton;

	public Player player { get; private set; }

	public void SetPlayer(Player player)
	{
		this.player = player;

		playerName.text = player.NickName;

		UpdateReadyInfo();
	}

	public void UpdateReadyInfo()
	{
		playerReady.text = player.GetReady() ? "Ready" : "";
		//playerReadyButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber);
		//LocalPlayer : 현재 로그인 중인 자기 자신에 대한 정보  
		//ActorNumber : 플레이어마다 방에 들어갔을 때 가지게 되는 고유 아이디
		//				(방안을 기준으로 동일한 번호를가지고 있는 플레이어는 없음. 방에 없을 경우에는 -1임)
		playerReadyButton.gameObject.SetActive(player.IsLocal); //ActorNumber로 비교하지 않고, 내가 나인지 물어보는걸로 간단하게 처리
	}

	public void Ready()
    {
		bool ready = player.GetReady();
		ready = !ready;
		player.SetReady(ready);
	}
}
