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
		if (PhotonNetwork.InRoom) //��ȿ� �ִٸ�? ��ġ����ŷ�� �� -> ������ ���������� ����
		{
			//Normal game mode
			PhotonNetwork.LocalPlayer.SetLoad(true); //���� �� �Ѿ�Դٴ� �ǹ̷� ������Ƽ�� ����
		}
		else //���� �׽�Ʈ�� ���� ��ġ����ŷ�� ������ -> ����� ���� �Ǵ�
		{
			//Debug game mode
			infoText.text = "Debug Mode";

			//�г��� �ο��ϰ� ���� ��Ʈ��ũ�� ���Ӻ��� ������
			PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public override void OnConnectedToMaster()
	{
		//�׽�Ʈ�� �ϱ� ���� ���� ���̱� ������ ������� ����.
		RoomOptions options = new RoomOptions() { IsVisible = false };
		PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
	}

	public override void OnJoinedRoom()
	{
		StartCoroutine(DebugGameSetupDelay());
	}

	/// <summary>
	/// ������ ���� ���
	/// </summary>
	/// <param name="cause"></param>
	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogError($"Disconnected : {cause}");
		SceneManager.LoadScene("LobbyScene"); //��Ʈ��ũ ������ �������ϱ� ���� ��Ʈ��ũ�� ����� �� ���� -> SceneManager Ȱ��
	}

	/// <summary>
	/// �濡�� ������ ��� 
	/// </summary>
	public override void OnLeftRoom()
	{
		Debug.LogError("Left Room");
		PhotonNetwork.LoadLevel("LobbyScene");
	}

	/// <summary>
	/// ������ �ٲ���� ��
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
				//���� ���� Ÿ�̹��� ���߱� ���� ī��Ʈ �ٿ� ����
				//��� �÷��̾ ������ �ð��� �����ϱ� ���ؼ��� �ð��� ���� ����ȭ�� �ʿ���.
				//(��, ��ΰ� ������ ���� �ð����� �������ָ� ������ ���� �� ������ ������ �ð��� �������� �������ֵ��� ��)
				//-> ���ݺ��� 5�ʵڿ� ������ �ƴ϶� N�ʰ� �Ǹ� ��������!�� �����ؾ���.
				//PhotonNetwork.ServerTimestamp = �����ð� 
				PhotonNetwork.CurrentRoom.SetLoadTime(PhotonNetwork.ServerTimestamp);

				infoText.text = $"All Player Loaded";
				//GameStart();
			}
			else
			{
				//�ٸ� �÷��̾� �ε� �� ������ ��� -> ��� ��Ȳ�� ����
				infoText.text = $"Wait Players ({loadingCnt}/{PhotonNetwork.PlayerList.Length})";
			}
		}
	}

	/// <summary>
	/// Room Property�� ���� �Ǿ��� �� ȣ�� 
	/// -> Room Property���� ������ �ε� �ð� ���� �� -> ��, �ε� �ð��� �������� �� ȣ���
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
		while(countdownTimer > (PhotonNetwork.ServerTimestamp - loadTime) / 1000f) //�����ð� - �ε�ð��� countdownTimer���� ���� ���� �ݺ�
			//�����ð��� ������ �и�������� ������ 1000�� �������
		{
			int reaminTime = (int)(countdownTimer - (PhotonNetwork.ServerTimestamp - loadTime) / 1000f);
			infoText.text = $"All Player Loaded, Start Count donw : {reaminTime + 1}";

			yield return new WaitForEndOfFrame();
		}
		infoText.text = "Game Start!";
		GameStart();

		//1�ʵڿ� �������� ������
		yield return new WaitForSeconds(1f);
		infoText.text = "";
	}

	private IEnumerator DebugGameSetupDelay()
	{
		//�ʹ� �ٷ� �����ϸ� ���� �� ������ ������ 1�� ���� ���� �ð��� �� �� ���� �����ϵ��� ������
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

		//���� �߽����� �÷��̾��� ���� ��ġ�� �ٸ��� ������ �ֱ� ����
		//�� �÷��̾��� ��ȣ�� Ȱ����. 360/8 = 45 -> 0��° �÷��̾�� 0�� / 1��° �÷��̾�� 45�� / 2��° �÷��̾�� 90�� �� ��ȣ�� �´� ��ġ�� �����ص�

		float angularStart = (360.0f / 8f) * PhotonNetwork.LocalPlayer.GetPlayerNumber();
		float x = 20.0f * Mathf.Sin(angularStart * Mathf.Deg2Rad);
		float z = 20.0f * Mathf.Cos(angularStart * Mathf.Deg2Rad);
		Vector3 position = new Vector3(x, 0.0f, z);
		Quaternion rotation = Quaternion.Euler(0.0f, angularStart, 0.0f);

		PhotonNetwork.Instantiate("Player", position, rotation, 0); //�������� ���� ������ٴ� ����� �˷������ -> �ٸ� �÷��̾� ȭ�鿡�� �� �÷��̾ ���������� �׷��� �� ����

		/*���� �� ������ �����ϰ� ����ȭ : Photon View
		* Photon View�� �޾��ָ� �ٸ� ����� ȭ�鿡���� �����ϰ� ������ �� �ֵ��� ����ȭ �����ִ� ������Ʈ��
		* Photon View�� ������, ó������ ��� ����ڿ��� ������ ��ġ�� �ѷ�������, �� ���Ŀ��� �� �������� �� ȭ�鿡���� ���̰� ��
		* Photon Transform View : Transform ����ȭ 
		* Photon Animator View : �ִϸ��̼� ����ȭ 
		* Photon Rigidbody View : Rigidbody ����ȭ 
		* ��� Photon View ���� ������Ʈ�� ����ؼ� � �͵��� ����ȭ �������� ������ �� ����
		*/

		//��� �÷��̾�� ���� ������� �����ߴ� �� x �÷��̾� �� ��ŭ ������.
		//���� ���常 �����ϰ�, �ٸ� �÷��̾���� ������ ������ ���� �޾Ƽ� ���� ���·� ��
		//BUT, ������ ���� ���� ���߿� ������? ������ ������鼭 ���� �� ���� ������� ��.
		//		-> ������ ���� �°迡 ���� ����� �ʿ��� = ���� ���̱׷��̼� �۾� ���� �ʿ�
		//			-> ������ �ٲ�� ������ OnMasterClientSwitched�� ������
		if (PhotonNetwork.IsMasterClient)
			StartCoroutine(SpawnStoneRoutine());

	}

	IEnumerator SpawnStoneRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnStoneTime);

			//������ ������ ���常 �ϴ� �� ���� ����
			//  -> SpawnStone ��ü�� �����͸� �����ϰ� ������, ���⼭ ����� ��ġ�� ���ϸ� ������ random�� ����� �� ���� 
			Vector2 direction = Random.insideUnitCircle.normalized; //���׶�� �ȿ��� ������ ��ġ ����
			Vector3 position = new Vector3(direction.x, 0, direction.y) * 200f;

			Vector3 force = -position.normalized * 30f + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
			Vector3 torque = Random.insideUnitSphere.normalized * Random.Range(10f, 20f); //�� �ȿ��� ������ ��ġ ����

			object[] instaniateData = { force, torque }; //������Ʈ ������ �ʿ��� �߰����� ������

			//Instantiate = �� ������ ������Ʈ�� ������.
			//InstantiateRoomObject = ���� ������ ������Ʈ�� ������
			//	=> ������ ���� �� Instantiate�� ������ ������Ʈ�� ���� ������ ��.
			//		�� ���� ��� �÷��̾ ���� ���� ������Ʈ �� ������ ������ �����־�� ��.
			//		-> ������ �����ٸ� ���ο� ���忡�� �������� �Ѱܼ� ������ ������ ������Ʈ�� �������� �ʵ��� �ϱ� ���ؼ���
			//		   InstantiateRoomObject�� ���� ������Ʈ�� �����ؾ���.
			PhotonNetwork.InstantiateRoomObject("LargeStone", position, Quaternion.identity, 0, instaniateData); //object �迭�� �־ �����ϵ��� ��


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
