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

		if (PhotonNetwork.IsMasterClient) //방장이니?
			CheckPlayerReadyState();
	}

	/// <summary>
	/// 이미 접속해 있는 사용자 + 본인 생성
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
	/// 게임 씬으로 이동
	/// </summary>
	public void StartGame()
    {
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;

		//SceneManager.LoadScene("GameScene"); //LoadScene을 이용해서 씬을 넘기면 start를 누른 방장의 씬만 이동하게 됨 4
		// -> 전체 유저가 같이 넘어가려면 photonNetwork에게 요청해서 씬을 넘겨야 함. 
		//  -> PhotonNetwork.LoadLevel 활용
		PhotonNetwork.LoadLevel("GameScene");
	}

    /// <summary>
    /// 방 떠나기
    /// </summary>
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(); //포톤 네트워크에 방 나간다고 신청하기   
    }

	/// <summary>
	/// 모든 플레이어가 ready 상태가 되면 start button 활성 처리
	/// </summary>
	public void CheckPlayerReadyState()
	{
		if (PhotonNetwork.IsMasterClient == false) //방장이 아니면 굳이 확인할 필요는 없음. 
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
