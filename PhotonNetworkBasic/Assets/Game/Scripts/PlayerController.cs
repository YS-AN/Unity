using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//IPunObservable : 포톤 뷰에게 변경 사항을 감지 당해서 전달할 수 있는 클래스라고 선언함 
// 실시간으로 계속 바뀌는(프레임마다 변한는) 변수에 대해서는 RPC로 동기화 하기에는 무리가 있음. -> 포톤 뷰에 observed component에 등록돼서 사용해야함 = 변수 동기화
// => 변수 동기화를 위해서는 IPunObservable를 상속받아야해
public class PlayerController : MonoBehaviourPun, IPunObservable
{
	[SerializeField] List<Color> playerColor;

	[SerializeField] float accelPower;
	[SerializeField] float maxSpeed;

	[SerializeField] float rotateSpeed;
	
	[SerializeField] Bullet bulletPrefab;

	[SerializeField]
	private float fireCoolTime;
	private float lastFireTime = float.MinValue;

	private PlayerInput PlayerInput;
	private Rigidbody rigidbody;
	private Vector2 inputDir;

	private int bulletCount;
	private int hp;

	[SerializeField]
	private int RPCCnt;

	private void Awake()
	{
		rigidbody =	GetComponent <Rigidbody>();
		PlayerInput = GetComponent<PlayerInput>();

		//내 소유가 아닌 playerInput은 삭제해버림 -> 다른 오브젝트의 PlayerInput은 자연스럽게 제거 -> 내 오브젝트 이동에만 영향 받을 수 있음
		if (!photonView.IsMine)
			Destroy(PlayerInput);
		else
		{
			Camera.main.transform.SetParent(transform);
			Camera.main.transform.localPosition = new Vector3(0, 20, 0);
		}

		SetPlayerColor();
	}

	private void SetPlayerColor()
	{
		int playerNumber = photonView.Owner.GetPlayerNumber();

		if (playerColor == null || playerColor.Count <= playerNumber)
			return;

		Renderer render = GetComponent<Renderer>();
		render.material.color = playerColor[playerNumber];
	}


	private void Update()
	{
		//이동 시 내 오브젝트만 영향을 줄 수 있게 해주야 함
		//if(!photonView.IsMine) //내 소유만 움직이도록 하기 (이 방법은 좀 구식임) -> Awake에서 처리하는 방식을 추천
		Accelate(inputDir.y);
		Rotate(inputDir.x);

		Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
	}

	/// <summary>
	/// 가속
	/// </summary>
	/// <param name="input"></param>
	private void Accelate(float input)
	{
		rigidbody.AddForce(input * accelPower * transform.forward, ForceMode.Force);
		if(rigidbody.velocity.magnitude > maxSpeed) //maxSpeed보다 커지면 
		{
			rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed; //maxSpeed로 변경
		}
	}

	/// <summary>
	/// 회전
	/// </summary>
	/// <param name="input"></param>
	private void Rotate(float input)
	{
		transform.Rotate(Vector3.up, input * rotateSpeed * Time.deltaTime);
	}

	private void OnMove(InputValue value)
	{
		inputDir = value.Get<Vector2>();
	}

	private void OnFire(InputValue value)
	{
		/*
		if (value.isPressed)
			Fire();
		*/

		/*
		* 여기서 쿨타임을 확인 한다면, 어떤 클라이언트가 슬쩍 본인 쿨타임만 짧게 변경한 경우, 변경된 쿨타임이 적용 가능함. 
		 	->클라이언트 변조 가능한 상황
		* 하지만, 쿨타임을 서버에서 확인한다면? 다른 클라이언트가 쿨타임을 변경한다고 해도, 
		  총알 생성횟수만 짧아지는거지, 서버 판단에서 쿨타임이 차지 않으면 반려처리함.
		 	-> 변조를 막을 수 있음 
		* 하지만, 리슨 서버 구조에서 방장이 악의적인 의도를 갖고 있을 때는 방장의 변조에 대해서는 막을 수 있는 방법이 없음. 
		  서버 = 방장이기 때문임
		
		if (Time.time < lastFireTime + fireCoolTime)
			return;
		
		lastFireTime = Time.time; //마지막으로 총을 쏜 타이밍을 현재로 변경
		*/
		//공격 시 바장을 통해 공격할 수 있도록 수정 
		photonView.RPC("CreateBullet", RpcTarget.MasterClient, transform.position, transform.rotation);
	}

	private void Fire()
	{
		//내가 함수를 호출할 때 다른 플레이어들도 내가 함수를 호출 했다는 사실을 알아야해. 
		//(ex. 내가 B를 공격한다는 걸 B도 알아야 데미지를 입을 수 있음)
		//=> 원격 함수 호출 방식을 활용 : PunRPC
		photonView.RPC("CreateBullet", RpcTarget.All, 10, false); //포톤 뷰를 통해서 CreateBullet함수가 호출되었다고 모두에게 알림
		/* [RpcTarget]
		 * RpcTarget.Others : 나 빼고 다 알림
		 * RpcTarget.MasterClient : 방장에게만 알림
		 * RpcTarget.AllViaServer : 서버에 있는 애들 전체
		 * RpcTarget.Buffered : Buffered 자체가 축적 방식임. 나중에 들어온 새로운 플레이어에게도 축적했던 내용을 다 전달함
		 */

		/* [네트워크 전달]
		 * 매개변수가 필요한 경우 RpcTarget 다음에 매개변수들을 넘겨주면 돼.
		 * 단, 게임오브젝트는 원격으로 보낼 수 없음 
		 *  -> 동일한 gameobejct라고 해도 플레이어 A와 B가 각각 heap영역에서 생성한 위치가 다름 -> but 넘겨줄 때는 주소를 넘겨줌. 
		 * 		-> A에서 0x01넘겨줄게! 하고 B가 받으면 읭? 난 0x01에 아무것도 없어!! 할 수 있기 때문임. 
		 * 		-> 원격 메소드 호출 시에도 같은 이유로 메소드 자체를 넘기지 못하고 이름으로 호출하는 것. 
		 * 		-> 같은 이유로 네트워크 동기화는 값형식만 전달이 가능함
		 * => 포톤에서 지원하는 타입이 이미 정해져 있음 (Photon의 직렬화 확인) 
		 */
		
		bulletCount++;
	}

	//원격 함수 호출임을 미리 알려줘서 메소드 이름으로 검색하는 양을 줄이도록 함
	//PunRPC attribute 붙이면 RPC List에 추가됨 (Photon Wizard > Locate PhotonServerSettings > PRCs > Rpc List)
	[PunRPC]
	public void CreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo photonInfo, Player player)
	{
		//Debug.Log($"[{photonView.Owner.NickName}.CreateBullet] {value} : {chk}");

		/*
		 * A 플레이어가 1초에 미사일을 쐈는데
		 * B 시점에서는 0.1초가 밀린 시점에 미사일이 그려지고, C시점에서는 0.1초가 더 밀린 0.2초 시점에 미사일이 그려짐.
		 * 이 때, 미사일에 회전이 적용됐다면,
		 * A는 30도 각도에서 미사일이 쏴지지만, B는 30.5도, C는 40도 등 시간이 밀리면서 각도도 같이 밀리게 되는 현상이 발생할 수 있음 
		 * => 지연 보상 개념을 적용할 필요가 있음 -> 틀려지지 않도록 필요한 정보를 추가로 같이 적용함
		 * -> 틀릴 수 없는 정보인 미사일이 발사 시점의 위치와 각도로 같이 전달해주도록함. 
		 *		+ 연속성이 있는 정보라고 한다면 시간 정보를 같이 줘서, 지연된 만큼을 예측해서 그리도록 함. 
		 *		  예를들어, 1초에 (0, 0, 0)의 위치에서 30도 각도로 쐈다면, 
		 *				   0.1초가 밀려서 그려지는 플레이어 입장에서는 밀린 시간만큼 이동한 위치와 각도에서 그려주도록함 = 지연 보상
		 */

		//Transforms과 물리 시스템과 자동으로 동기화되도록 설정해두는게 편함. 
		//설정 방법 : Project Settings > Physics > Auto sync Transforms -> 체크해두기

		//서버에서 쿨타임을 판정하도록 함. (클라이언트 변조 방지를 위함)
		if (Time.time < lastFireTime + fireCoolTime)
			return;

		lastFireTime = Time.time; //마지막으로 총을 쏜 타이밍을 현재로 변경

		Bullet bullet = Instantiate(bulletPrefab, position, rotation);

		//시간을 활용한 지연 보상 개념을 적용
		float lag = (float)(PhotonNetwork.Time - photonInfo.SentServerTime); //현재 서버 시간 - 보냈을 때 서버 시간 = 지연된 시간(lag)
		bullet.ApplyLag(lag);

		RPCCnt++;
	}

	/// <summary>
	/// 방장(마스터 클라이언트) 입장에서 판정을 진행
	/// </summary>
	/// <param name="postion"></param>
	/// <param name="rotation"></param>
	/// <param name="photonInfo"></param>
	[PunRPC]
	public void RequestCreateBullet(Vector3 postion, Quaternion rotation, PhotonMessageInfo photonInfo)
	{
		float sentTime = (float)photonInfo.SentServerTime;
		photonView.RPC("CreateBullet", RpcTarget.AllViaServer, postion, rotation, sentTime, photonInfo.);
		//그냥 All로 하면 방장은 자기가 자기 자신에고 공지를 하는 꼴이라 방장이 유리할 수 밖에 없음
		//	-> AllViaServer로 설정해서 서버를 통해 받도록 하면, 방장도 공지를 서버를 통해 받아야하기 때문에 모든 유저가 가장 비슷한 타이밍에 공지를 받을 수 있음 
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.IsWriting) //포톤뷰를 통해 쓰는 상황
		{
			stream.SendNext(bulletCount); //stream에 동기화 필요한 변수를 같이 보냄
			stream.SendNext(hp);
		}
		else //stream.IsReading
		{
			//그냥 stream에 붙여서 보내는 방식이기 때문에 쓴 순서 그대로 읽어줘야 함 (구분은 순서 뿐)
			bulletCount = (int)stream.ReceiveNext();
			hp = (int)stream.ReceiveNext();
		}
	}

 }
