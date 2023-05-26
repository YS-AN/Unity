using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackController : MonoBehaviour
{
	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();	
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float h = Input.GetAxisRaw("Horizontal");
		rb.AddForce(Vector2.right * h * 3 * Time.deltaTime, ForceMode2D.Impulse);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var enemy = collision.gameObject.GetComponent<Enemy>();

		if(enemy != null)
		{
			enemy.GetDamage(2f);
		}
		
	}

	public void GetDamange(float damage)
	{
		Debug.Log(damage);
	}
}
