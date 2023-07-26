using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//IPunObservable : ���� �信�� ���� ������ ���� ���ؼ� ������ �� �ִ� Ŭ������� ������ 
// �ǽð����� ��� �ٲ��(�����Ӹ��� ���Ѵ�) ������ ���ؼ��� RPC�� ����ȭ �ϱ⿡�� ������ ����. -> ���� �信 observed component�� ��ϵż� ����ؾ��� = ���� ����ȭ
// => ���� ����ȭ�� ���ؼ��� IPunObservable�� ��ӹ޾ƾ���
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

		//�� ������ �ƴ� playerInput�� �����ع��� -> �ٸ� ������Ʈ�� PlayerInput�� �ڿ������� ���� -> �� ������Ʈ �̵����� ���� ���� �� ����
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
		//�̵� �� �� ������Ʈ�� ������ �� �� �ְ� ���־� ��
		//if(!photonView.IsMine) //�� ������ �����̵��� �ϱ� (�� ����� �� ������) -> Awake���� ó���ϴ� ����� ��õ
		Accelate(inputDir.y);
		Rotate(inputDir.x);

		Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
	}

	/// <summary>
	/// ����
	/// </summary>
	/// <param name="input"></param>
	private void Accelate(float input)
	{
		rigidbody.AddForce(input * accelPower * transform.forward, ForceMode.Force);
		if(rigidbody.velocity.magnitude > maxSpeed) //maxSpeed���� Ŀ���� 
		{
			rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed; //maxSpeed�� ����
		}
	}

	/// <summary>
	/// ȸ��
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
		* ���⼭ ��Ÿ���� Ȯ�� �Ѵٸ�, � Ŭ���̾�Ʈ�� ��½ ���� ��Ÿ�Ӹ� ª�� ������ ���, ����� ��Ÿ���� ���� ������. 
		 	->Ŭ���̾�Ʈ ���� ������ ��Ȳ
		* ������, ��Ÿ���� �������� Ȯ���Ѵٸ�? �ٸ� Ŭ���̾�Ʈ�� ��Ÿ���� �����Ѵٰ� �ص�, 
		  �Ѿ� ����Ƚ���� ª�����°���, ���� �Ǵܿ��� ��Ÿ���� ���� ������ �ݷ�ó����.
		 	-> ������ ���� �� ���� 
		* ������, ���� ���� �������� ������ �������� �ǵ��� ���� ���� ���� ������ ������ ���ؼ��� ���� �� �ִ� ����� ����. 
		  ���� = �����̱� ������
		
		if (Time.time < lastFireTime + fireCoolTime)
			return;
		
		lastFireTime = Time.time; //���������� ���� �� Ÿ�̹��� ����� ����
		*/
		//���� �� ������ ���� ������ �� �ֵ��� ���� 
		photonView.RPC("CreateBullet", RpcTarget.MasterClient, transform.position, transform.rotation);
	}

	private void Fire()
	{
		//���� �Լ��� ȣ���� �� �ٸ� �÷��̾�鵵 ���� �Լ��� ȣ�� �ߴٴ� ����� �˾ƾ���. 
		//(ex. ���� B�� �����Ѵٴ� �� B�� �˾ƾ� �������� ���� �� ����)
		//=> ���� �Լ� ȣ�� ����� Ȱ�� : PunRPC
		photonView.RPC("CreateBullet", RpcTarget.All, 10, false); //���� �並 ���ؼ� CreateBullet�Լ��� ȣ��Ǿ��ٰ� ��ο��� �˸�
		/* [RpcTarget]
		 * RpcTarget.Others : �� ���� �� �˸�
		 * RpcTarget.MasterClient : ���忡�Ը� �˸�
		 * RpcTarget.AllViaServer : ������ �ִ� �ֵ� ��ü
		 * RpcTarget.Buffered : Buffered ��ü�� ���� �����. ���߿� ���� ���ο� �÷��̾�Ե� �����ߴ� ������ �� ������
		 */

		/* [��Ʈ��ũ ����]
		 * �Ű������� �ʿ��� ��� RpcTarget ������ �Ű��������� �Ѱ��ָ� ��.
		 * ��, ���ӿ�����Ʈ�� �������� ���� �� ���� 
		 *  -> ������ gameobejct��� �ص� �÷��̾� A�� B�� ���� heap�������� ������ ��ġ�� �ٸ� -> but �Ѱ��� ���� �ּҸ� �Ѱ���. 
		 * 		-> A���� 0x01�Ѱ��ٰ�! �ϰ� B�� ������ ��? �� 0x01�� �ƹ��͵� ����!! �� �� �ֱ� ������. 
		 * 		-> ���� �޼ҵ� ȣ�� �ÿ��� ���� ������ �޼ҵ� ��ü�� �ѱ��� ���ϰ� �̸����� ȣ���ϴ� ��. 
		 * 		-> ���� ������ ��Ʈ��ũ ����ȭ�� �����ĸ� ������ ������
		 * => ���濡�� �����ϴ� Ÿ���� �̹� ������ ���� (Photon�� ����ȭ Ȯ��) 
		 */
		
		bulletCount++;
	}

	//���� �Լ� ȣ������ �̸� �˷��༭ �޼ҵ� �̸����� �˻��ϴ� ���� ���̵��� ��
	//PunRPC attribute ���̸� RPC List�� �߰��� (Photon Wizard > Locate PhotonServerSettings > PRCs > Rpc List)
	[PunRPC]
	public void CreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo photonInfo, Player player)
	{
		//Debug.Log($"[{photonView.Owner.NickName}.CreateBullet] {value} : {chk}");

		/*
		 * A �÷��̾ 1�ʿ� �̻����� ���µ�
		 * B ���������� 0.1�ʰ� �и� ������ �̻����� �׷�����, C���������� 0.1�ʰ� �� �и� 0.2�� ������ �̻����� �׷���.
		 * �� ��, �̻��Ͽ� ȸ���� ����ƴٸ�,
		 * A�� 30�� �������� �̻����� ��������, B�� 30.5��, C�� 40�� �� �ð��� �и��鼭 ������ ���� �и��� �Ǵ� ������ �߻��� �� ���� 
		 * => ���� ���� ������ ������ �ʿ䰡 ���� -> Ʋ������ �ʵ��� �ʿ��� ������ �߰��� ���� ������
		 * -> Ʋ�� �� ���� ������ �̻����� �߻� ������ ��ġ�� ������ ���� �������ֵ�����. 
		 *		+ ���Ӽ��� �ִ� ������� �Ѵٸ� �ð� ������ ���� �༭, ������ ��ŭ�� �����ؼ� �׸����� ��. 
		 *		  �������, 1�ʿ� (0, 0, 0)�� ��ġ���� 30�� ������ ���ٸ�, 
		 *				   0.1�ʰ� �з��� �׷����� �÷��̾� ���忡���� �и� �ð���ŭ �̵��� ��ġ�� �������� �׷��ֵ����� = ���� ����
		 */

		//Transforms�� ���� �ý��۰� �ڵ����� ����ȭ�ǵ��� �����صδ°� ����. 
		//���� ��� : Project Settings > Physics > Auto sync Transforms -> üũ�صα�

		//�������� ��Ÿ���� �����ϵ��� ��. (Ŭ���̾�Ʈ ���� ������ ����)
		if (Time.time < lastFireTime + fireCoolTime)
			return;

		lastFireTime = Time.time; //���������� ���� �� Ÿ�̹��� ����� ����

		Bullet bullet = Instantiate(bulletPrefab, position, rotation);

		//�ð��� Ȱ���� ���� ���� ������ ����
		float lag = (float)(PhotonNetwork.Time - photonInfo.SentServerTime); //���� ���� �ð� - ������ �� ���� �ð� = ������ �ð�(lag)
		bullet.ApplyLag(lag);

		RPCCnt++;
	}

	/// <summary>
	/// ����(������ Ŭ���̾�Ʈ) ���忡�� ������ ����
	/// </summary>
	/// <param name="postion"></param>
	/// <param name="rotation"></param>
	/// <param name="photonInfo"></param>
	[PunRPC]
	public void RequestCreateBullet(Vector3 postion, Quaternion rotation, PhotonMessageInfo photonInfo)
	{
		float sentTime = (float)photonInfo.SentServerTime;
		photonView.RPC("CreateBullet", RpcTarget.AllViaServer, postion, rotation, sentTime, photonInfo.);
		//�׳� All�� �ϸ� ������ �ڱⰡ �ڱ� �ڽſ��� ������ �ϴ� ���̶� ������ ������ �� �ۿ� ����
		//	-> AllViaServer�� �����ؼ� ������ ���� �޵��� �ϸ�, ���嵵 ������ ������ ���� �޾ƾ��ϱ� ������ ��� ������ ���� ����� Ÿ�ֿ̹� ������ ���� �� ���� 
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.IsWriting) //����並 ���� ���� ��Ȳ
		{
			stream.SendNext(bulletCount); //stream�� ����ȭ �ʿ��� ������ ���� ����
			stream.SendNext(hp);
		}
		else //stream.IsReading
		{
			//�׳� stream�� �ٿ��� ������ ����̱� ������ �� ���� �״�� �о���� �� (������ ���� ��)
			bulletCount = (int)stream.ReceiveNext();
			hp = (int)stream.ReceiveNext();
		}
	}

 }
