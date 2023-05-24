using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//������Ʈ�� ���� �и��ϵ��� ��
// -> ���� ���� �޾Ƽ� ���� ��쵵 �ֱ� ������.
//		-> Ư�� UI���� ���� ����� �κ��� ���Ƽ�, UI�� �� �ٸ� �𵨷� �и��ϱ⵵ ��
public class BeeModel : MonoBehaviour
{
	[SerializeField]
	private TMP_Text stateText;
	public TMP_Text StateText { get { return stateText; } }

	[SerializeField]
	private float detectRange; //�ش� ���� �ȿ� ������ �÷��̾� �Ѿư�
	public float DetectRange { get { return detectRange; } }

	[SerializeField]
	private float attackRange; //���� ����
	public float AttackRange { get { return attackRange; } }

	[SerializeField]
	private float moveSpeed;
	public float MoveSpeed { get { return moveSpeed; } }

	[SerializeField]
	private Transform[] patrolPoints; //���� ��ġ�� �޾ƿ�
	public Transform[] PatrolPoints { get { return patrolPoints; } }
}