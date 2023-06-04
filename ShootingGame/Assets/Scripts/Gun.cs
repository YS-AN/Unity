using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField]
	private float maxDistance; //���� �ִ� ��Ÿ�

	[SerializeField]
	private int damage;

	
	[SerializeField]
	private ParticleSystem muzzleEffect;

	/* //���� �ڿ����� ������ -> �ʿ��� ���� ���ҽ� �Ŵ������� ���������� ��
	[SerializeField]
	private ParticleSystem hitEffect;

	[SerializeField]
	private TrailRenderer bulletTrail; 
	//*/

	[SerializeField]
	private float bulletSpeed;

	public virtual void Fire()
	{
		Debug.Log("[Gun] ����!!");

		muzzleEffect.Play(); //��ġ�� �����̱� ������ ������ �������� �ʰ�, ȿ�� ����� ��

		RaycastHit hit;

		   //Physics.Raycast(ī�޶� ��ġ����, ��� ��������? ī�޶��� �� ��������, hit, �ִ� �����Ÿ�)
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance)) //�������� ������?
		{
			//Destroy(hit.transform.gameObject); //���� ��ü ����

			//�Ѿ˿� �¾ƾ� �ϴ� ��ü�� ���� ���ƾ� �� ��ü�� �����ϱ� -> �������̽��� ����
			IHittable hittable = hit.transform.GetComponent<IHittable>(); //�������̽��� ������Ʈ �˻��� ������ -> �ش� �������̽��� ���� �༮�� ������ �� ���� 

			ParticleSystem hitEffect = GameManager.Resource.Load<ParticleSystem>("Prefabs/HitEffect");

			//var effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal)); //LookRotation : ����3�� �־��ָ� ������ ���� �������� ȸ�� ������
			//var effect = GameManager.Pool.Get(hitEffect, hit.point, Quaternion.LookRotation(hit.normal)); // ObjectPool���� -> �뿩
			var effect = GameManager.Resource.Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal), true); //������ ������ �Ŵ����� ��

			//����Ʈ ȿ���� �߻��� ������Ʈ�� �̵��ϰ� ����, �� �ڸ��� ȿ���� ���� �ְ� ��
			//ȿ���� �߻����ڸ��� �߻� ������ �ִ� ������Ʈ�� ���� �ڽ����� �־��ָ�, ������Ʈ�� �̵��ϴ��� ȿ���� ���󰡰� ��. 
			effect.transform.parent = hit.transform; 

			Destroy(effect, 3f);

			//DrawTrail(hit.point);
			StartCoroutine(TrailRountine(muzzleEffect.transform.position, hit.point));

			hittable?.Hit(hit, damage); //hittable�� null�� �ƴϸ� Hit�� ȣ����
		}
		else
		{
			//DrawTrail(Camera.main.transform.forward * maxDistance);
			StartCoroutine(TrailRountine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
		}
	}

	private void DrawTrail(Vector3 endPoint)
	{
		//TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
		StartCoroutine(TrailRountine(muzzleEffect.transform.position, endPoint));
		//Destroy(trail, 3f);
	}

	/// <summary>
	/// Trail�� ���ݾ� ������ �̵��ϴ� �ڷ�ƾ
	/// </summary>
	/// <param name="trail">�̵��� Trail</param>
	/// <param name="startPoint">�̵� ���� ��ġ</param>
	/// <param name="endPoint">�̵� ��ǥ ��ġ</param>
	IEnumerator TrailRountine(Vector3 startPoint, Vector3 endPoint)
	{
		/*
			Resources ������ �־�ΰ�, ��θ� �������ָ� ����Ƽ �󿡼� �巡�׾� ��Ӱ� ���� ������ ��

			BUT ����Ƽ�� Resources ���� ����� �������� ����.
			Resources ���� �ȿ� �ִ� �͵��� �׻� �غ� ���·� ��� ����. -> ���� ���� �� ������ ���ԵǾ� ������ ������ ���� ����� Ŀ��. 

			������ ������Ÿ���� ������ �� �����. (6���� �̳� ª�� ������Ʈ���� ���)
			��� ������Ʈ������
			 * ���� ����(���� �����Ϸ��� �ϸ� �߰� ������ �ٿ� �޴´ٰ� �ϴ� ��찡 ���� ���� ����ϴ� ����),
			 * ���� ��巹����(2019����� ����. �ֱ� Ʈ������)
			����� ��õ��
		*/
		TrailRenderer bulletTrail = Resources.Load<TrailRenderer>("Prefabs/BulletTail"); //�巡�׾� ��Ӱ� ���� ���� -> �̸� �����ΰ�, �ʿ信 ���� ȣ����

		//TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity); //�Ź� �����ϴ� ���

		//var trail = GameManager.Pool.Get(bulletTrail, startPoint, Quaternion.identity);  // ObjectPool���� -> �뿩
		var trail = GameManager.Resource.Instantiate(bulletTrail, startPoint, Quaternion.identity, true); //������ ������ �Ŵ����� ��

		//�뿩 ����� ���� �־��� ��ġ�� �ִٺ���, ���� �ִ� ��ġ���� �������� �̵��ߴٰ� �ٽ� ���ϴ� ��ġ�� ������ ������ ���� 
		//�Ʒ��� ���� �ʱ�ȭ ������ �ʿ���=====================
		trail.Clear(); //�Ϲ�ȭ�ؼ� Component ��ü�� �޾ƿ� -//trail.GetComponent<TrailRenderer>().Clear(); 
		//==================================================

		float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed; // (�Ÿ�/�ӷ�)�� ���� �̵� �ð��� ���� -> �żӽ� ���� �̿�

		float rate = 0;
		while (rate < 1)
		{
			trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
			rate += Time.deltaTime / totalTime;

			yield return null;

			/* //������Ʈ Ǯ�� ���ǻ���1 (���� ���� Ȯ�� ��)
			if(trail == null)
			{
				Debug.Log("Ʈ���� ����");
			}
			else
			{
				Debug.Log("Ʈ���� ����");
			}
			// => "Ʈ���� ����"���� ���� -> �ֳĸ� ������Ʈ pool�� ������� ���� �ƴ϶� ��Ȱ��ȭ ó���� �Ǵ� ����. -> 

			//���� Ȱ�� ���ε� ���� Ȯ���� ����� ��. 
			// -> �� ���� ��¥ ���� �Ǿ��ų� �ݳ��ؼ� ��Ȱ��ȭ �Ǿ�����? -> ���ٰ� �Ǵ���
			if (trail == null || trail.gameObject.activeSelf == false)
			{
				Debug.Log("Ʈ���� ����");
			}
			else
			{
				Debug.Log("Ʈ���� ����");
			}
			*/
			//üũ�� �� ���� ������ ������ Ȯ�� �޼ҵ�� ���� ���� (Utils > Extension > IsValid ����)
			if(trail.IsValid())
			{
				Debug.Log("Ʈ���� ����");
			}
		}

		//Destroy(trail, 3f); //�Ź� �����ϴ� ���
		//GameManager.Pool.Release(trail.gameObject); // ObjectPool���� -> �ݳ�
		GameManager.Resource.Destroy(trail.gameObject); //Release�� ���ҽ� �Ŵ����� ��
	}

	IEnumerator ReleaseRoutine(GameObject effect)
	{
		yield return new WaitForSeconds(3f);
		//GameManager.Pool.Release(effect);
		GameManager.Resource.Destroy(effect.gameObject); //Release�� ���ҽ� �Ŵ����� ��
	}
}
