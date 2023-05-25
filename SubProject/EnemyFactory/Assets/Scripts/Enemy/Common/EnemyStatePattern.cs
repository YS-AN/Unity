using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStatePattern<T> : EnemyState<EnemyStateType, T> where T : Enemy
{
	/* 람다식으로 선언한 이유는? 
	*	변수로 생성하면, 한번 데이터 할당해주고 끝이지만, 
	*	람다식으로 선언을 해주면 해당 변수를 호출 되는 시점에 원본 데이터를 매번확인하는 형태가 됨.
	*	
	*	예를 들어 
	*	Rigidbody2D rigidbody = owner.rigidbody;는 한번 할당해주면 끝임.
	*	만약, 할당 시점에 owner에 rigidbody가 없으면 다시 할당받을 때까지 rigidbody는 null임,
	*	하지만, 람다식을 활용해 Rigidbody2D rigidbody => owner.rigidbody; 할당해주면
	*	rigidbody를 사용하려고 호출 할 때마다 owner의 rigidbody를 가져와줌. 
	*	따라서, 처음에는 owner에 값이 없어 null이 할당 되었더라도, 나중에 rigidbody가 추가되면
	*	추가된 후에 rigidbody를 호출시 owner의 rigidbody를 가져오게 됨. 
	*/

	protected GameObject gameObject => owner.gameObject;
	protected Transform transform => owner.transform;
	protected Rigidbody2D rigidbody => owner.Rigidbody;
	protected SpriteRenderer renderer => owner.SpriteRnder;
	protected Animator animator => owner.Animator;
	protected Collider2D collider => owner.Collider;

	/// <summary>
	/// player transform
	/// </summary>
	protected Transform target;

	protected EnemyStatePattern(T owner, StateMachine<EnemyStateType, T> stateMachine)
		: base(owner, stateMachine)
	{
	}
}