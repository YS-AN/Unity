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
		//LocalPlayer : ���� �α��� ���� �ڱ� �ڽſ� ���� ����  
		//ActorNumber : �÷��̾�� �濡 ���� �� ������ �Ǵ� ���� ���̵�
		//				(����� �������� ������ ��ȣ�������� �ִ� �÷��̾�� ����. �濡 ���� ��쿡�� -1��)
		playerReadyButton.gameObject.SetActive(player.IsLocal); //ActorNumber�� ������ �ʰ�, ���� ������ ����°ɷ� �����ϰ� ó��
	}

	public void Ready()
    {
		bool ready = player.GetReady();
		ready = !ready;
		player.SetReady(ready);
	}
}
