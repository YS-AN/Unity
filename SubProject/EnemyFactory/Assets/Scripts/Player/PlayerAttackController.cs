using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		var enemy = collision.gameObject.GetComponent<Enemy>();

		if(enemy != null)
		{
			enemy.GetDamange(2f);
		}
		
	}
}
