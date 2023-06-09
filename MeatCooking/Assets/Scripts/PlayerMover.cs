using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
	[SerializeField]
	private float MoveSpeed;

	private NavMeshAgent meshAgent;
	private Animator animator;
	private CharacterController characterController;

	private Vector3 moveDir;

	private Coroutine moveRoutine;

	

	private void Awake()
	{
		characterController = GetComponent<CharacterController>();
		meshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		//Cursor.lockState = CursorLockMode.Confined; //마우스가 게임창을 벗어나지 못하게 막음

	}

	private void OnDisable()
	{
	
		//mouseClickActions.performed -= OnMove;
	}

	private void nDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	private void Update()
	{
		
	}

	private void OnMove(InputValue value)
	{
		var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		if (Physics.Raycast(ray: ray, hitInfo: out var hit) && hit.collider)
		{
			if (moveRoutine != null)
				StopCoroutine(moveRoutine);

			moveRoutine = StartCoroutine(MoveRoutine(hit.point));
		}
	}



	private IEnumerator MoveRoutine(Vector3 target)
	{
		animator.SetTrigger("Move");

		meshAgent.destination = target;

		float playerDistancetoFloor = transform.position.y - target.y;
		target.y += playerDistancetoFloor;

		while (Vector3.Distance(transform.position, target) > 0.1f)
		{
			yield return null;
		}

		/*
		while (Vector3.Distance(transform.position, target) > 0.1f)
		{
			//Debug.Log($"transform : ({transform.position.x}, {transform.position.y}, {transform.position.z}) / target : ({target.x}, {target.y}, {target.z})");

			//Turn(target);

			
			Vector3 destination = Vector3.MoveTowards(transform.position, target, MoveSpeed * Time.deltaTime);
			transform.position = destination;

			Vector3 direction = target - transform.position;	
			Vector3 movement = direction.normalized * MoveSpeed * Time.deltaTime;
			characterController.Move(movement);

			//Vector2 cameraPos = Camera.main.ScreenToWorldPoint(target);
			//cameraPos.x - transform.position.z;




			yield return null; 
		}
		//*/


	}


	/*
	private void Turn(Vector3 target)
	{
		

		Vector3 dir = target - transform.position;
		Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z);

		Debug.Log($"[Turn] dirXZ : ({dirXZ.x}, {dirXZ.y}, {dirXZ.z})");

		Quaternion targetRot = Quaternion.LookRotation(dirXZ);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, RotationSpeed * Time.deltaTime);
	}
	//*/
}
