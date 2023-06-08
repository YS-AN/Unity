using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CreateAssetMenu : Create 안에 하위 메뉴를 만드는 어트리뷰트임
// fileName : default file Name
// menuName : create 하위 메뉴 위치 지정
[CreateAssetMenu(fileName = "TowerData", menuName = "Data/Tower")] 
public class TowerData : ScriptableObject, ISerializationCallbackReceiver
{
	//파일 형태이기 때문에 데이터 변경이 되면 그 데이터가 그대로 남아 있음. (게임을 꺼도 변경된 값이 유지가 됨)
	// => 스크립터블 데이터를 막 변경하면 안 됨... => 외부에서 변경 못하도록 막는게 좋음 
	//public TowerInfo[] towers;

	[SerializeField]
	private TowerInfo[] towers;
	public TowerInfo[] Towers { get { return towers; } }

	//ISerializationCallbackReceiver를 통해서 복사하면서 넘겨주는 방법도 있음.
	//이 부분은 더 찾아보기!!

	public void OnBeforeSerialize()
	{
	
	}

	public void OnAfterDeserialize()
	{
		
	}

	[Serializable] //Serializable : 클래스 직렬화 하는 방법
	public class TowerInfo
	{
		public Tower Tower;

		/// <summary>
		/// 건설 시간
		/// </summary>
		public float BuildTime;

		/// <summary>
		/// 건설비용
		/// </summary>
		public int BuildCost;

		/// <summary>
		/// 판매비용
		/// </summary>
		public int SellCost;

		/// <summary>
		/// 공격 데미지
		/// </summary>
		public int Damage;

		/// <summary>
		/// 공격 딜레이
		/// </summary>
		public int Delay;

		/// <summary>
		/// 공격 범위
		/// </summary>
		public int Range;

		//메소드도 스크립터블 오브젝트에 넣을 수 있음
		public virtual void Attack()
		{
			Debug.Log("공격");
		}

		// 스크립터블의 활용 방법은 무궁무진함
		// => 파일 형태로 만들다 보니 여러가지 끼워넣기 형태로 활용이 가능함

		//AI행동이라고 해서, 특정 행동을 만들어 놓을 수 있음.
		//	=> Youtube.Pluggable AI With Scriptable Objects

		//유니티에서 제안한 스크립터블 설계 방법 3가지
		//https://unity.com/kr/how-to/architect-game-code-scriptable-objects

	}
}
