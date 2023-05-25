using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
	

	private Rigidbody2D rigidbd;
	public Rigidbody2D Rigidbody { get { return rigidbd; } }

	private Animator animator;
	public Animator Animator { get { return animator; } }

	private Collider2D colider;
	public Collider2D Collider { get { return colider; } }

	private SpriteRenderer spriteRnder;
	public SpriteRenderer SpriteRnder { get { return spriteRnder; } }

	[SerializeField]
	private EnemyModel dataModel;
	public EnemyModel DataModel { get { return dataModel; } }

	[SerializeField]
	private float enemyHealth;

	private Transform target;
	public Transform Target { get { return target; } }

	public Vector3 ReutnrPosition { protected set; get; }

	protected virtual void Awake()
	{
		rigidbd = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		colider = GetComponent<Collider2D>();
		spriteRnder = GetComponent<SpriteRenderer>();
	}

	protected virtual void Start()
	{
		//todo. 싱글톤으로 만들 경우 변경하기! 
		target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public void GetDamange(float damage)
	{
		enemyHealth -= damage;

		Debug.Log(enemyHealth);
	}
}
