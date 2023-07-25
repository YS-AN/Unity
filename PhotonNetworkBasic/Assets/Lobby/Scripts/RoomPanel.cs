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

		if (PhotonNetwork.IsMasterClient) //방장이니?
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

		if (PhotonNetwork.IsMasterClient) //방장이니?
			CheckPlayerReady();
	}

	/// <summary>
	/// 이미 접속해 있는 사용자 세팅
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
	/// 하이라키 구조 상 index 번호를 가져오기
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
