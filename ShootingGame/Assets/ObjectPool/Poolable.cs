using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesingPattern
{// <summary>
	/// 실제 풀링 될 객체 (빌려질 녀석)
	/// </summary>
	public class Poolable : MonoBehaviour
	{
		[SerializeField]
		private float releaseTime; //몇 초뒤 반납할지 정함 -> 때에 따라 시간이 아닐 수 있음. 

		private ObjectPool _pool; //어디에 반납할 지 알고 있어야 함
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
		/// releaseTime 뒤에 반납하는 코루틴
		/// </summary>
		/// <returns></returns>
		IEnumerator ReleaseTimer()
		{
			yield return new WaitForSeconds(releaseTime);
			_pool.Release(this);
		}
	}
}
