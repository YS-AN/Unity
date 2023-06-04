using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField]
	private float maxDistance; //총의 최대 사거리

	[SerializeField]
	private int damage;

	
	[SerializeField]
	private ParticleSystem muzzleEffect;

	/* //공유 자원으로 변경함 -> 필요할 때마 리소스 매니저에서 가져오도록 함
	[SerializeField]
	private ParticleSystem hitEffect;

	[SerializeField]
	private TrailRenderer bulletTrail; 
	//*/

	[SerializeField]
	private float bulletSpeed;

	public virtual void Fire()
	{
		Debug.Log("[Gun] 빵야!!");

		muzzleEffect.Play(); //위치가 고정이기 때문에 별도로 생성하지 않고, 효과 재생을 함

		RaycastHit hit;

		   //Physics.Raycast(카메라 위치에서, 어느 방향으로? 카메라의 앞 방향으로, hit, 최대 사정거리)
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance)) //레이저에 닿으면?
		{
			//Destroy(hit.transform.gameObject); //닿은 물체 삭제

			//총알에 맞아야 하는 물체와 맞지 말아야 할 물체를 구분하기 -> 인터페이스로 구분
			IHittable hittable = hit.transform.GetComponent<IHittable>(); //인터페이스는 컴포넌트 검색이 가능함 -> 해당 인터페이스를 받은 녀석만 가져올 수 있음 

			ParticleSystem hitEffect = GameManager.Resource.Load<ParticleSystem>("Prefabs/HitEffect");

			//var effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal)); //LookRotation : 백터3를 넣어주면 방향을 보는 방향으로 회전 시켜줌
			//var effect = GameManager.Pool.Get(hitEffect, hit.point, Quaternion.LookRotation(hit.normal)); // ObjectPool적용 -> 대여
			var effect = GameManager.Resource.Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal), true); //생성을 리스소 매니저로 뺌

			//이펙트 효과가 발생한 오브젝트가 이동하고 나면, 그 자리에 효과만 남아 있게 됨
			//효과가 발생하자마자 발생 지점에 있는 오브젝트의 하위 자식으로 넣어주면, 오브젝트가 이동하더라도 효과가 따라가게 됨. 
			effect.transform.parent = hit.transform; 

			Destroy(effect, 3f);

			//DrawTrail(hit.point);
			StartCoroutine(TrailRountine(muzzleEffect.transform.position, hit.point));

			hittable?.Hit(hit, damage); //hittable이 null이 아니면 Hit을 호출함
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
	/// Trail이 조금씩 앞으로 이동하는 코루틴
	/// </summary>
	/// <param name="trail">이동할 Trail</param>
	/// <param name="startPoint">이동 시작 위치</param>
	/// <param name="endPoint">이동 목표 위치</param>
	IEnumerator TrailRountine(Vector3 startPoint, Vector3 endPoint)
	{
		/*
			Resources 폴더에 넣어두고, 경로만 설정해주면 유니티 상에서 드래그앤 드롭과 같은 동작을 함

			BUT 유니티는 Resources 폴더 사용을 권장하지 않음.
			Resources 폴더 안에 있는 것들은 항상 준비 상태로 대기 중임. -> 게임 빌드 시 무조건 포함되어 나오기 때문에 게임 사이즈가 커짐. 

			보통은 프로토타입을 제작할 때 사용함. (6개월 이내 짧은 프로젝트에서 사용)
			장기 프로젝트에서는
			 * 에셋 번들(게임 시작하려고 하면 추가 리스소 다운 받는다고 하는 경우가 에셋 번들 사용하는 예임),
			 * 에셋 어드레서블(2019년부터 나옴. 최근 트렌드임)
			방식을 추천함
		*/
		TrailRenderer bulletTrail = Resources.Load<TrailRenderer>("Prefabs/BulletTail"); //드래그앤 드롭과 같은 동작 -> 미리 만들어두고, 필요에 따라 호출함

		//TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity); //매번 생성하는 방식

		//var trail = GameManager.Pool.Get(bulletTrail, startPoint, Quaternion.identity);  // ObjectPool적용 -> 대여
		var trail = GameManager.Resource.Instantiate(bulletTrail, startPoint, Quaternion.identity, true); //생성을 리스소 매니저로 뺌

		//대여 방식은 원래 있었던 위치가 있다보니, 원래 있던 위치에서 원점으로 이동했다가 다시 원하는 위치로 가려는 성질이 있음 
		//아래와 같이 초기화 절차가 필요함=====================
		trail.Clear(); //일반화해서 Component 자체를 받아옴 -//trail.GetComponent<TrailRenderer>().Clear(); 
		//==================================================

		float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed; // (거리/속력)을 통해 이동 시간을 구함 -> 거속시 공식 이용

		float rate = 0;
		while (rate < 1)
		{
			trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
			rate += Time.deltaTime / totalTime;

			yield return null;

			/* //오브젝트 풀링 주의사항1 (존재 여부 확인 시)
			if(trail == null)
			{
				Debug.Log("트레일 없음");
			}
			else
			{
				Debug.Log("트레일 있음");
			}
			// => "트레일 있음"으로 나옴 -> 왜냐면 오브젝트 pool은 사라지는 것이 아니라 비활성화 처리가 되는 것임. -> 

			//따라서 활성 여부도 같이 확인을 해줘야 함. 
			// -> 꽉 차서 진짜 삭제 되었거나 반납해서 비활성화 되었으면? -> 없다고 판단함
			if (trail == null || trail.gameObject.activeSelf == false)
			{
				Debug.Log("트레일 없음");
			}
			else
			{
				Debug.Log("트레일 있음");
			}
			*/
			//체크할 게 많기 때문에 보통은 확장 메소드로 따로 빼둠 (Utils > Extension > IsValid 참고)
			if(trail.IsValid())
			{
				Debug.Log("트레일 없음");
			}
		}

		//Destroy(trail, 3f); //매번 삭제하는 방식
		//GameManager.Pool.Release(trail.gameObject); // ObjectPool적용 -> 반납
		GameManager.Resource.Destroy(trail.gameObject); //Release를 리소스 매니저로 뺌
	}

	IEnumerator ReleaseRoutine(GameObject effect)
	{
		yield return new WaitForSeconds(3f);
		//GameManager.Pool.Release(effect);
		GameManager.Resource.Destroy(effect.gameObject); //Release를 리소스 매니저로 뺌
	}
}
