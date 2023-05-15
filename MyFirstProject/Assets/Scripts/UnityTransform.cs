using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityTransform : MonoBehaviour
{
	/************************************************************************
	  * Ʈ������ (Transform)
	  * 
	  * ���ӿ�����Ʈ�� ��ġ, ȸ��, ũ�⸦ �����ϴ� ������Ʈ
	  * ���ӿ�����Ʈ�� �θ�-�ڽ� ���¸� �����ϴ� ������Ʈ 
	  * ���ӿ�����Ʈ�� �ݵ�� �ϳ��� Ʈ������ ������Ʈ�� ������ ������ �߰� & ������ �� ����
	  ************************************************************************/

	float moveSpeed = 3;
	float rotateSpeed = 90;

	private Vector3 moveDir;

	private void Start()
	{

	}

	private void Move()
	{
		TranslateMove();
	}

	// <Ʈ������ �̵�>
	// Translate : Ʈ�������� �̵� �Լ�
	private void TranslateMove()
	{
		//transform.Translate //������Ʈ�� ��ġ�� �̵� -> ��ġ �̵��̱� ������ ������ ���� ������
		//transform.localScale : ������Ʈ ũ�⸦ ����

		transform.position += moveDir * Time.deltaTime;
		//���� �� ��ŭ�� �ƴ϶� �ӷ¸�ŭ �̵��ؾ���. deltaTime�� ���� �ð���. (�� ������ �� �ɸ��� �ð�)
		// -> �� �÷��� �� �ɸ��� �ð��� ���ؾ� �ӷ����� �����̴� �� ������ ��
		//  -> A��ǻ�ʹ� 1�ʿ� 100�� ������Ʈ�� �ǰ�, B ��ǻ�ʹ� 1�ʿ� 10�� ������Ʈ�� �ȴٸ�, 
		//	   A��ǻ�ʹ� 1���� 1/100��ŭ�� �̵��ϰ�, B ��ǻ�ʹ� 1���� 1/10��ŭ �̵���.=> �����ϰ� �̵��� ������.
		//		(�Ÿ� = �ð� * �ӵ�) MoveDir = �ӵ� / deltaTiem = �ð�

		//transform ������ ���� �̵�, ���ڸ��� �� ������ (�ٷ� �ٷ� �����ϴ� �������� �߿�)
		//AddForce ������ ����, ���� ���� (������ �߿�)
		//�����ڸ��� �� �����̰�, ���ӹ޾� �����̸� ����� -> ź������ -> �Ѿ��� ���� ���ȴٰ� �ؼ� ������ ������ �ʿ�� ����. ���� ������ŭ �ٷιٷ� ������ �޾� ����������
		//���� ���� �߿��� ������ AddForece�� �߿���. -> �� ������. -> �������� ���� �ް�, �� ���� ������ �ʿ��� ��� ��. 
		//  -> ���� ���� �ƴµ� ���� ������ ���� �ٷ� �ٽ� �����̸� �� �� -> ���������� ���� ��ŭ ���󰬴ٰ� �ٽ� �� ������ �ް� ����������. 

		// ���͸� �̿��� �̵� 
		// position���밡 ���� �Ÿ� �ڴ� �� ���ٴ� transform.Translate�� ����ϴ� ���� �� ��õ��.
		transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime); //���� * �ӷ� * ���� �ð�
																		   //Translate�� ���� �ٷκ��� ������ �������� �����̴� �� ������.
																		   //AddForce�� ���踦 �������� ������. ���� ���� object�� 45�� Ʋ���� �־, ������ ���� �������� ������ ����

		// x,y,z�� �̿��� �̵�
		transform.Translate(0, 0, moveSpeed * Time.deltaTime);
	}

	// <Ʈ������ �̵� ����>
	private void TransformMoveSpace()
	{
		// ���带 �������� �̵�
		transform.Translate(1, 0, 0, Space.World); //������ ���� �������� �����̴� ��. (AdD Force�� ����� ����� ����)

		// ������ �������� �̵�
		transform.Translate(1, 0, 0, Space.Self); //���� ������Ʈ�� ���� �������� ������

		// �ٸ� ����� �������� �̵�
		transform.Translate(1, 0, 0, Camera.main.transform);
	}

	// <Ʈ������ ȸ��>
	// Rotate : Ʈ�������� ȸ�� �Լ�
	private void Rotate()
	{
		//��ȸ��, ��ȸ���� y���� �������� �ؾ���. �� ���� �������� �ؾ���? -> ���� �������� �ð����� �ݽð� �����̶�� �����ϸ� ��.
		// -> ������ �������� �ð�, �ݽð�� �¿� ȸ���� y�� ������ �Ǵ� ��

		// ���� �̿��� ȸ�� (���� �������� �ð�������� ȸ��)
		transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime); //Vector3.up = ���� ������ �������� ȸ���� ����..!

		// ���Ϸ��� �̿��� ȸ��
		transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

		// x,y,z�� �̿��� ȸ��
		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
	}

	// <Ʈ������ ȸ�� ����>
	private void RotateSpace()
	{
		// ���带 �������� ȸ��
		transform.Rotate(1, 0, 0, Space.World);
		// ������ �������� ȸ��
		transform.Rotate(1, 0, 0, Space.Self); //������ ���� ������Ʈ�� �������� ��, �� ȸ���� ��.
											   // ��ġ�� �������� ȸ��
		transform.RotateAround(Camera.main.transform.position, Vector3.up, 1);
	}

	// <Ʈ������ LookAt ȸ��>
	// LookAt : �ٶ󺸴� ������ �������� ȸ�� -> �������� �����ϸ�, �������� �ٶ�
	private void LookAt()
	{
		// ��ġ�� �ٶ󺸴� ȸ��
		transform.LookAt(new Vector3(0, 0, 0));

		// �Ӹ��� ������ �߰��� �ٶ󺸴� ȸ��
		transform.LookAt(new Vector3(0, 0, 0), Vector3.right);
	}

	//�׷��ٸ�... ������ ������Ʈ�� �����̵��� ���� ��... LookAt�� ��ϳ�?
	//rotation�� vector�� �ƴ϶� Quarternion�� ��

	// <Quarternion & Euler>
	// Quarternion	: ����Ƽ�� ���ӿ�����Ʈ�� 3���� ������ �����ϰ� �̸� ���⿡�� �ٸ� ���������� ��� ȸ������ ����
	//				  �������� ȸ������ ������ ������ �߻����� ����
	// EulerAngle	: 3���� �������� ���������� ȸ����Ű�� ���
	//				  ������������ ������ ������ �߻��Ͽ� ȸ���� ��ġ�� ���� ���� �� ����
	// ������(Gimbal Lock) : ���� �������� ������Ʈ�� �� ȸ�� ���� ��ġ�� ���� -> ���� ��ġ�� �ٸ� ���� ȸ���� �Ұ����� ��
	//					     -> ��ģ 2���� ȸ�� ������ ���������� ����

	// Quarternion�� ���� ȸ�������� ����ϴ� ���� ���������� �ʰ� �����ϱ� �����
	//   -> �׳� ���Ϸ� �� ���� ������ ������ �߻��ϱ� ������ ���ʹϿ��� ���� ������ �˸� ��

	// ������ ��� ���ʹϾ� -> ���Ϸ����� -> �������� -> ������Ϸ����� -> ������ʹϾ� �� ���� ������ ��� ���ʹϾ��� �����
	//   -> �ᱹ �츮�� ���Ϸ� ������ ���� �Ǵ� ��.
	private void Rotation()
	{
		// Ʈ�������� ȸ������ Euler���� ǥ���� �ƴ� Quaternion�� �����
		transform.rotation = Quaternion.identity; //identity : ȸ���� �� �� (0, 0, 0)�� 

		// Euler������ Quaternion���� ��ȯ
		transform.rotation = Quaternion.Euler(0, 90, 0); // �Է��� ���Ϸ��� ���ʹϿ����� �ٲ���
														 //transform.rotation.ToEulerAngles(); //���ʹϿ��� ���Ϸ��� �ٲ�
	}

	// <Ʈ������ �θ�-�ڽ� ����>
	// Ʈ�������� �θ� Ʈ�������� ���� �� ����
	// �θ� Ʈ�������� �ִ� ��� �θ� Ʈ�������� ��ġ, ȸ��, ũ�� ������ ���� �����
	// �̸� �̿��Ͽ� ������ ������ �����ϴµ� ������ (ex. ���� �����̸�, �հ����� ���� ������)
	// ���̾��Ű â �󿡼� �巡�� & ����� ���� �θ�-�ڽ� ���¸� ������ �� ����
	private void TransformParent()
	{
		GameObject newGameObject = new GameObject() { name = "NewGameObject" };

		// �θ� ����
		transform.parent = newGameObject.transform;

		// �θ� ���������� Ʈ������
		// transform.localPosition	: �θ�Ʈ�������� �ִ� ��� �θ� �������� �� ��ġ -> �θ� ��ġ�� �������� �ڽ��� ��ġ�� ������ϴ� �� �� ����
		// transform.localRotation	: �θ�Ʈ�������� �ִ� ��� �θ� �������� �� ȸ��
		// transform.localScale		: �θ�Ʈ�������� �ִ� ��� �θ� �������� �� ũ��

		

		// �θ� ����
		transform.parent = null;

		//�θ� ���� ��쿡�� local�̳� world�� ������. -> �ֳĸ� �θ� ���� ��� world�� �θ�� ����
		//�θ� �����ϰ� ���� ���带 �������� ��ġ�� ����.

		// ���带 ���������� Ʈ������
		// transform.localPosition == transform.position	: �θ�Ʈ�������� ���� ��� ���带 �������� �� ��ġ
		// transform.localRotation == transform.rotation	: �θ�Ʈ�������� ���� ��� ���带 �������� �� ȸ��
		// transform.localScale								: �θ�Ʈ�������� ���� ��� ���带 �������� �� ũ��


	}
}