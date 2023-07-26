using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
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

	private Dictionary<int, PlayerEntry> playerDictionary;

	private void Awake()
	{
		playerDictionary = new Dictionary<int, PlayerEntry>();
	}

	private void OnEnable()
	{
        roomName.text = PhotonNetwork.CurrentRoom.Name;

		startButton.gameObject.SetActive(false);

		SetInPlayer();
		CheckPlayerReadyState();
	}

	private void OnDisable()
	{
		foreach (int actorNumber in playerDictionary.Keys)
		{
			Destroy(playerDictionary[actorNumber].gameObject);
		}
		playerDictionary.Clear();
	}

	public void EntryPlayer(Player player)
    {
		InstantiatePlayer(player);
		CheckPlayerReadyState();
	}

    public void LeavePlayer(Player leavePlayer)
    {
		Destroy(playerDictionary[leavePlayer.ActorNumber].gameObject);
		playerDictionary.Remove(leavePlayer.ActorNumber);
		CheckPlayerReadyState();
	}

	public void UpdatePlayerState(Player player)
	{
		GetPalyerEntry(player)?.UpdateReadyInfo();

		if (PhotonNetwork.IsMasterClient) //�����̴�?
			CheckPlayerReadyState();
	}

	/// <summary>
	/// �̹� ������ �ִ� ����� + ���� ����
	/// </summary>
	private void SetInPlayer()
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
		playerDictionary.Add(player.ActorNumber, entry);
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
	public void CheckPlayerReadyState()
	{
		if (PhotonNetwork.IsMasterClient == false) //������ �ƴϸ� ���� Ȯ���� �ʿ�� ����. 
			startButton.gameObject.SetActive(false);
		else
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
}
