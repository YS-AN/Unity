using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityPlayerMoveController : MonoBehaviour
{
	/// <summary>
	/// �����̴� ���� ����
	/// </summary>
	private Vector3 moveDir;
	private Rigidbody rb;
	private Animator animator;

	[SerializeField]
	private float movePower;

	[SerializeField]
	private float jumpPower;

	[SerializeField]
	private float rotateSpeed;

	[SerializeField]
	private Camera mapCam;

	[Header("Shooter")]
	[SerializeField]
	private GameObject bulletPrefab; //������ �Ѿ��� prefab�ޱ�

	/// <summary>
	/// �Ѿ� �߻� ��ġ
	/// </summary>
	[SerializeField]
	private Transform bulletPoint;

	/// <summary>
	/// �Ѿ� �ݺ� �ð�
	/// </summary>
	[SerializeField]
	private float repeatTime;




	private void Awake()
	{
		//moveDir = this.gameObject.transform.position;
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		Move();
		Rotate();
	}

	private void Move()
	{
		rb.AddForce(moveDir * movePower, ForceMode.Force); //�����̰� ���� ���⿡ ���� �Ǿ���
														   //AddForce : ���������� ���ϴ� ��
														   //Force : ���׽� �б� (�ð��� ������ ���������� ���� ��)

		//this.gameObject.transform.localScale += Vector3.one * 0.0001f; 

		Camera.main.transform.LookAt(this.gameObject.transform);
		mapCam.transform.LookAt(this.gameObject.transform);
	}

	private void Jump()
	{
		Debug.Log("jump");

		rb.AddForce(moveDir * jumpPower, ForceMode.Impulse); //Impulse : ���� �ѹ��� �� -> (1�ʿ� �� ���� �ѹ��� ���ϰ� �ִ� ���)
	}


	private void OnMove(InputValue value)
	{
		//Ű���� ����, ������ / �� �Ʒ�
		//y���� �� �Ʒ���.
		// �ٵ� �츮�� ������ �� �� �Ʒ� �����δٰ� �ؼ� ���ڱ� ���� ���� �߸� �̻���.
		// �׷��ϱ� �������� �������� ������ �� �ֶǷ� y�� z�� �־��� z�� �յڰ�, x�� �¿�ϱ�

		
		moveDir.z = value.Get<Vector2>().y; //���� Z�� �յ���
	}

	private void Rotate()
	{
		transform.Rotate(Vector3.up, moveDir.x * rotateSpeed * Time.deltaTime, Space.Self);
	}

	private void OnRotate(InputValue value)
	{
		moveDir.x = value.Get<Vector2>().x;
	}


	private void OnJump(InputValue value)
	{
		moveDir.y = value.Get<Vector2>().y;

		Jump(); //������ ���� ���� �۵��ϵ��� ��
				//Move�� ������ ���� �������� �ٲ����� jump�� ���� ���� �����ؾ��ؼ�, ȣ�� ��ġ�� �ٸ� ��
	}

	public void MakeBullet()
	{
		Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);

		animator.SetTrigger("Fire");
	}

	/// <summary>
	/// �����̽��� ������ �� �Ѿ� �߻�
	/// </summary>
	/// <param name="value"></param>
	private void OnFire(InputValue value)
	{
		//Debug.Log("OnFire");

		//OnFire �� �۵��ϴ��� Ȯ��
		//Debug.Log("OnFire");

		//Instantiate(bulletPrefab); //Instantiate �������� ���� ����� ���� ������Ű�� ��

		/*
		GameObject obj = Instantiate(bulletPrefab ); //���� ������� ���� �ִ� ���� ������Ʈ ��������

		//��ġ�� ������ ��ũ�� ����
		obj.transform.position = transform.position;
		obj.transform.rotation = transform.rotation;
		//*/

		//��(/* ~ */) �ڵ带 �� �����ϰ� ���� -> ������ ���ÿ� ��ġ�� ȸ�� ���� ��
		//GameObject obj = Instantiate(bulletPrefab, transform.position, transform.rotation);

		//��ũ�� �߹��� ������ -> �ڼ��� ���� �Ѿ��� ��ũ�� �߹ؿ��� ������ ���� ����
		//�׷��� ��ũ �ֵ��� ��ġ���� �Ѿ��� �������� ������
		//  -> ��ũ �ֵ��� ��ġ�� �� ��ġ�� �����, �� ��ġ���� �Ѿ��� ��������, position�� rotation�� ����.
		//		-> ��ũ�� ��ġ�� ������ �����ϰ� �������� �ϱ� ������ ��ũ�� �ڽ����� ������ ������ ��
		//GameObject obj = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);

		MakeBullet();
	}

	/// <summary>
	/// �Ѿ� ���� ��ƾ (�ڷ�ƾ Ȱ���� ��Ÿ�� ����)
	/// </summary>
	private Coroutine bulletRoution;

	IEnumerator BulletMakeRoutine()
	{
		while (true)
		{
			MakeBullet();
			yield return new WaitForSeconds(repeatTime);
		}
	}

	/// <summary>
	/// ���� : �ڷ�ƾ�� Ȱ���� N�ʿ� �ѹ� �Ѿ��� �����ϵ��� ����
	/// </summary>
	/// <param name="value"></param>
	private void OnRepeatFire(InputValue value)
	{
		//Debug.Log("OnRepeatFire");

		if (value.isPressed)
		{
			//Debug.Log("������");
			bulletRoution = StartCoroutine(BulletMakeRoutine());
		}
		else
		{
			//Debug.Log("����");
			StopCoroutine(bulletRoution);
		}
	}

	
}