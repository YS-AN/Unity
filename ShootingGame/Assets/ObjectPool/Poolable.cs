using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingPattern
{// <summary>
	/// ���� Ǯ�� �� ��ü (������ �༮)
	/// </summary>
	public class Poolable : MonoBehaviour
	{
		[SerializeField]
		private float releaseTime; //�� �ʵ� �ݳ����� ���� -> ���� ���� �ð��� �ƴ� �� ����. 

		private ObjectPool _pool; //��� �ݳ��� �� �˰� �־�� ��
		public ObjectPool Pool { get { return _pool; } set { _pool = value; } }

		private Coroutine ReleaseTimerRoutine;

		private void OnEnable()
		{
			ReleaseTimerRoutine = StartCoroutine(ReleaseTimer());
		}

		private void OnDisable()
		{
			StopCoroutine(ReleaseTimerRoutine);
		}

		/// <summary>
		/// releaseTime �ڿ� �ݳ��ϴ� �ڷ�ƾ
		/// </summary>
		/// <returns></returns>
		IEnumerator ReleaseTimer()
		{
			yield return new WaitForSeconds(releaseTime);
			_pool.Release(this);
		}
	}
}
