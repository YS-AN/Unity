using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] Button startButton;
    [SerializeField] TextMeshProUGUI roomName;

	private void OnEnable()
	{
        roomName.text = PhotonNetwork.CurrentRoom.Name;
		startButton.gameObject.SetActive(false);
	}

    public void EntryPlayer(Player player)
    {
		if (player.IsLocal)
			SetExistingPlayer();
		else
			InstantiatePlayer(player);

		if (PhotonNetwork.IsMasterClient) //�����̴�?
			CheckPlayerReady();
	}

    public void LeavePlayer(Player player)
    {
		int index = GetSiblingIndexNum(player);

		if (index < 0)
			return;

		Destroy(playerContent.GetChild(index).gameObject);
	}

	public void UpdatePlayerState(Player player)
	{
		GetPalyerEntry(player)?.UpdateReadyInfo();

		if (PhotonNetwork.IsMasterClient) //�����̴�?
			CheckPlayerReady();
	}

	/// <summary>
	/// �̹� ������ �ִ� ����� ����
	/// </summary>
	private void SetExistingPlayer()
	{
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			InstantiatePlayer(player);
		}
	}

	private void InstantiatePlayer(Player player)
	{
		PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
		entry.SetPlayer(player);
	}


	/// <summary>
	/// ���̶�Ű ���� �� index ��ȣ�� ��������
	/// </summary>
	/// <param name="chkPlayer"></param>
	/// <returns></returns>
	private int GetSiblingIndexNum(Player chkPlayer)
    {
		var existedPlayer = playerContent.GetComponentsInChildren<PlayerEntry>();

		int index = 0;
		foreach(var player in existedPlayer)
		{
			if(player.player.ActorNumber == chkPlayer.ActorNumber) 
				return index;

			index++;
		}
		return -1;
	}

	private PlayerEntry GetPalyerEntry(Player chkPlayer)
	{
		return playerContent.GetComponentsInChildren<PlayerEntry>().Where(x => x.player.ActorNumber == chkPlayer.ActorNumber).FirstOrDefault();
	}

	public void UpdateActiveStartButton(int actorNumber)
    {
        //startButton.gameObject.SetActive()
    }

	/// <summary>
	/// ���� ������ �̵�
	/// </summary>
	public void StartGame()
    {
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;

		//SceneManager.LoadScene("GameScene"); //LoadScene�� �̿��ؼ� ���� �ѱ�� start�� ���� ������ ���� �̵��ϰ� �� 4
		// -> ��ü ������ ���� �Ѿ���� photonNetwork���� ��û�ؼ� ���� �Ѱܾ� ��. 
		//  -> PhotonNetwork.LoadLevel Ȱ��
		PhotonNetwork.LoadLevel("GameScene");
	}

    /// <summary>
    /// �� ������
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(); //���� ��Ʈ��ũ�� �� �����ٰ� ��û�ϱ�   
    }

	/// <summary>
	/// ��� �÷��̾ ready ���°� �Ǹ� start button Ȱ�� ó��
	/// </summary>
	public void CheckPlayerReady()
	{
		int readyCount = 0;
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			if (player.GetReady())
				readyCount++;
		}

		startButton.gameObject.SetActive(readyCount == PhotonNetwork.PlayerList.Length);
	}
}
