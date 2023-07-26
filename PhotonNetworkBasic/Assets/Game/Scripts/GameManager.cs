using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private TMP_Text infoText;
	
	[SerializeField]
	private float countdownTimer;

	[SerializeField]
	private float spawnStoneTime;

	private void Start()
	{
		if (PhotonNetwork.InRoom) //방안에 있다면? 매치메이킹을 함 -> 게임을 정상적으로 진행
		{
			//Normal game mode
			PhotonNetwork.LocalPlayer.SetLoad(true); //씬에 잘 넘어왔다는 의미로 프로퍼티를 변경
		}
		else //게임 테스트를 위해 매치메이킹을 생략함 -> 디버깅 모드로 판단
		{
			//Debug game mode
			infoText.text = "Debug Mode";

			//닉네임 부여하고 포톤 네트워크에 접속부터 진행함
			PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public override void OnConnectedToMaster()
	{
		//테스트를 하기 위한 게임 방이기 때문에 비공개로 만듦.
		RoomOptions options = new RoomOptions() { IsVisible = false };
		PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
	}

	public override void OnJoinedRoom()
	{
		StartCoroutine(DebugGameSetupDelay());
	}

	/// <summary>
	/// 연결이 끊긴 경우
	/// </summary>
	/// <param name="cause"></param>
	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogError($"Disconnected : {cause}");
		SceneManager.LoadScene("LobbyScene"); //네트워크 접속이 끊겼으니까 포톤 네트워크를 사용할 수 없음 -> SceneManager 활용
	}

	/// <summary>
	/// 방에서 나가진 경우 
	/// </summary>
	public override void OnLeftRoom()
	{
		Debug.LogError("Left Room");
		PhotonNetwork.LoadLevel("LobbyScene");
	}

	/// <summary>
	/// 방장이 바뀌었을 때
	/// </summary>
	/// <param name="newMasterClient"></param>
	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if(PhotonNetwork.IsMasterClient)
			StartCoroutine(SpawnStoneRoutine());
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
	{
		if (changedProps.ContainsKey(CustomProperty.PROPERTYKEY_LOAD))
		{
			int loadingCnt = PlayerLoadCount();
			if (loadingCnt == PhotonNetwork.PlayerList.Length)
			{
				//게임 시작 타이밍을 맞추기 위한 카운트 다운 진행
				//모든 플레이어가 동일한 시간에 시작하기 위해서는 시간에 대한 동기화가 필요함.
				//(단, 모두가 각자의 서버 시간으로 설정해주면 문제가 생길 수 있으니 방장의 시간을 기준으로 설정해주도록 함)
				//-> 지금부터 5초뒤에 시작이 아니라 N초가 되면 시작하자!로 접근해야함.
				//PhotonNetwork.ServerTimestamp = 서버시간 
				PhotonNetwork.CurrentRoom.SetLoadTime(PhotonNetwork.ServerTimestamp);

				infoText.text = $"All Player Loaded";
				//GameStart();
			}
			else
			{
				//다른 플레이어 로딩 될 때까지 대기 -> 대기 현황을 공유
				infoText.text = $"Wait Players ({loadingCnt}/{PhotonNetwork.PlayerList.Length})";
			}
		}
	}

	/// <summary>
	/// Room Property가 변경 되었을 때 호출 
	/// -> Room Property변경 시점은 로딩 시간 결정 뿐 -> 즉, 로딩 시간이 정해졌을 때 호출됨
	/// </summary>
	/// <param name="propertiesThatChanged"></param>
	public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		if(propertiesThatChanged.ContainsKey(CustomProperty.PROPERTYKEY_LOADTIME)) 
		{
			StartCoroutine(GameStartTimer());
		}	
	}

	private IEnumerator GameStartTimer()
	{
		int loadTime = PhotonNetwork.CurrentRoom.GetLoadTime();
		while(countdownTimer > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f) //서버시간 - 로드시간이 countdownTimer보다 작을 동안 반복
			//서버시간은 단위가 밀리세컨드라 나누기 1000을 해줘야함
		{
			int reaminTime = (int)(countdownTimer - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);
			infoText.text = $"All Player Loaded, Start Count donw : {reaminTime + 1}";

			yield return new WaitForEndOfFrame();
		}
		infoText.text = "Game Start!";
		GameStart();

		//1초뒤에 공지사항 제거함
		yield return new WaitForSeconds(1f);
		infoText.text = "";
	}

	private IEnumerator DebugGameSetupDelay()
	{
		//너무 바로 시작하면 꼬일 수 있으니 서버에 1초 정도 여유 시간을 준 후 게임 진행하도록 설정함
		yield return new WaitForSeconds(1f);

		DebugGameStart(); 
	}

	private int PlayerLoadCount()
	{
		return PhotonNetwork.PlayerList.Where(x => x.GetLoad()).Count();
	}

	private void GameStart()
	{
		//TODO. game start

		Debug.Log("Normal Game mode");
	}

	private void DebugGameStart()
	{
		Debug.Log("Debug Game mode");

		//원을 중심으로 플레이어의 시작 위치를 다르게 설정해 주기 위함
		//각 플레이어의 번호를 활용함. 360/8 = 45 -> 0번째 플레이어는 0도 / 1번째 플레이어는 45도 / 2번째 플레이어는 90도 등 번호에 맞는 위치에 설정해둠

		float angularStart = (360.0f / 8f) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
		float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
		float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
		Vector3 position = new Vector3(x, 0.0f, z);
		Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

		PhotonNetwork.Instantiate("Player", position, rotation, 0); //서버한테 내가 만들었다는 사실을 알려줘야해 -> 다른 플레이어 화면에도 내 플레이어를 정상적으로 그려줄 수 있음

		/*변경 된 사항을 감지하고 동기화 : Photon View
		* Photon View를 달아주면 다른 사람의 화면에서도 동일하게 움직일 수 있도록 동기화 시켜주는 컨포넌트임
		* Photon View가 없으면, 처음에는 모든 사용자에게 동일한 위치를 뿌려주지만, 그 이후에는 내 움직임은 내 화면에서만 보이게 됨
		* Photon Transform View : Transform 동기화 
		* Photon Animator View : 애니메이션 동기화 
		* Photon Rigidbody View : Rigidbody 동기화 
		* 등등 Photon View 설정 컨포넌트를 등록해서 어떤 것들을 동기화 시켜줄지 결정할 수 있음
		*/

		//모든 플레이어에서 돌이 날라오면 예상했던 돌 x 플레이어 수 만큼 생성됨.
		//따라서 방장만 생성하고, 다른 플레이어들은 방장이 생성한 돌을 받아서 쓰는 형태로 함
		//BUT, 방장이 만약 게임 도중에 나가면? 방장이 사라지면서 돌도 다 같이 사라지게 됨.
		//		-> 방장의 권한 승계에 대한 고려가 필요함 = 방장 마이그레이션 작업 구현 필요
		//			-> 방장이 바뀌는 시점인 OnMasterClientSwitched에 구현함
		if (PhotonNetwork.IsMasterClient)
			StartCoroutine(SpawnStoneRoutine());

	}

	IEnumerator SpawnStoneRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnStoneTime);

			//랜덤값 결정을 방장만 하는 게 가장 좋음
			//  -> SpawnStone 자체를 마스터만 진행하고 있으니, 여기서 방향과 위치를 정하면 방장의 random을 사용할 수 있음 
			Vector2 direction = Random.insideUnitCircle.normalized; //동그라미 안에서 랜덤한 위치 설정
			Vector3 position = new Vector3(direction.x, 0, direction.y) * 200f;

			Vector3 force = -position.normalized * 30f + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
			Vector3 torque = Random.insideUnitSphere.normalized * Random.Range(10f, 20f); //구 안에서 랜덤한 위치 설정

			object[] instaniateData = { force, torque }; //오브젝트 생성에 필요한 추가적인 데이터

			//Instantiate = 내 소유로 오브젝트를 생성함.
			//InstantiateRoomObject = 방의 소유로 오브젝트를 생성함
			//	=> 방장이 나갈 때 Instantiate로 생성한 오브젝트는 전부 삭제가 됨.
			//		돌 같이 모든 플레이어가 같이 쓰는 오브젝트 는 방장이 나가도 남아있어야 함.
			//		-> 방장이 나간다면 새로운 방장에게 소유권을 넘겨서 방장이 나가도 오브젝트가 삭제되지 않도록 하기 위해서는
			//		   InstantiateRoomObject를 통해 오브젝트를 생성해야함.
			PhotonNetwork.InstantiateRoomObject("LargeStone", position, Quaternion.identity, 0, instaniateData); //object 배열에 넣어서 전달하도록 함


			/*
			Vector2 direction = Random.insideUnitCircle;
			Vector3 position = Vector3.zero;

			
			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				// Make it appear on the left/right side
				position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize);
			}
			else
			{
				// Make it appear on the top/bottom
				position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
			}

			// Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
			position -= position.normalized * 0.1f;


			Vector3 force = -position.normalized * 1000.0f;
			Vector3 torque = Random.insideUnitSphere * Random.Range(100.0f, 300.0f);
			object[] instantiationData = { force, torque };

			if (Random.Range(0, 10) < 5)
			{
				PhotonNetwork.InstantiateRoomObject("LargeStone", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
			}
			else
			{
				PhotonNetwork.InstantiateRoomObject("SmallStone", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
			}
			//*/
		}
	}
		
}
