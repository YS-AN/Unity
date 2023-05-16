using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove0516 : MonoBehaviour
{
	/// <summary>
	/// �����̴� ���� ����
	/// </summary>
	private Vector3 moveDir;
	
	private Rigidbody rb;

	/// <summary>
	/// �����̴� �ӵ�
	/// </summary>
	[SerializeField]
	private float movePower;

	/// <summary>
	/// ȸ�� �ӵ�
	/// </summary>
	[SerializeField]
	private float rotateSpeed;

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
		rb = GetComponent<Rigidbody>();
	}

	private void OnMove(InputValue value)
	{
		Debug.Log($"[OnMove] ({value.Get<Vector2>().x}, {value.Get<Vector2>().y})");

		if (value.Get<Vector2>().y > 0)
		{
			transform.Translate(Vector3.forward * movePower * Time.deltaTime, Space.Self);
		}
		else if (value.Get<Vector2>().y < 0)
		{
			transform.Translate(Vector3.back * movePower * Time.deltaTime, Space.Self);
		}
	}

	private void OnRoate(InputValue value)
	{
		//todo. ����Ű �¿쿡 ���� ȸ���ϵ��� 
	}
}
