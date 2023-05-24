using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//컴포넌트를 따로 분리하도록 함
// -> 설정 값만 받아서 쓰는 경우도 있기 때문에.
//		-> 특히 UI같은 경우는 공통된 부분이 많아서, UI는 또 다른 모델로 분리하기도 함
public class BeeModel : MonoBehaviour
{
	[SerializeField]
	private TMP_Text stateText;
	public TMP_Text StateText { get { return stateText; } }

	[SerializeField]
	private float detectRange; //해당 영역 안에 있으면 플레이어 쫓아감
	public float DetectRange { get { return detectRange; } }

	[SerializeField]
	private float attackRange; //공격 범위
	public float AttackRange { get { return attackRange; } }

	[SerializeField]
	private float moveSpeed;
	public float MoveSpeed { get { return moveSpeed; } }

	[SerializeField]
	private Transform[] patrolPoints; //순찰 위치를 받아옴
	public Transform[] PatrolPoints { get { return patrolPoints; } }
}