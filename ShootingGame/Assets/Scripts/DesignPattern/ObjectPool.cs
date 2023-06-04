using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//  		디자인 패턴  [ObjectPool]	
//========================================
/*
 * 오브젝트의 생성과 삭제를 반복하다보면 메모리 파편화 현상이 발생할 수 있음. -> 생성 오버헤드가 발생할 후 있음 
 *      -> 미리 만들어두고, 생성, 삭제가 아닌 대여, 반냅의 형태로 메모라 파편화 문제를 해결할 수 있음 -> GC의 발생을 줄일 수 있음 
 *      
 *  오브젝트 풀링 패턴 :
 *      프로그램 내에서 빈번하게 재활용되는 인스턴스들을 생성&삭제하지 않고 미리 생성해놓은 인스턴스들을 보관한 객체집합(풀)에서 인스턴스를 대여&반납하는 방식으로 사용하는 기법
 *      
 *  구현 :
 *      1. 인스턴스들을 보관할 객체집합(풀)을 생성
 *      2. 프로그램의 시작시 객체집합(풀)에 인스턴스들을 생성하여 보관
 *      3. 인스턴스가 필요로 하는 상황에서 객체집합(풀)의 인스턴스를 대여하여 사용
 *      4. 인스턴스가 필요로 하지 않는 상황에서 객체집합(풀)에 인스턴스를 반납하여 보관    
 *      
 *  장점 :
 *      1. 빈번하게 사용하는 인스턴스의 생성에 소요되는 오버헤드를 줄임
 *      2. 빈번하게 사용하는 인스턴스의 삭제에 가비지 콜렉터 부담을 줄임
 *  단점 :
 *      1. 미리 생성해놓은 인스턴스에 의해 사용하지 않는 경우에도 메모리를 차지하고 있음
 *      2. 메모리가 넉넉하지 않은 기기에서 너무 많은 오브젝트 풀링을 사용하는 경우, 힙영역의 여유공간이 줄어들어 오히려 프로그램에 부담이 되는 경우가 있음
 *			-> 메모리가 넉넉하지 않은 기기에서 미리 만들어둔 오브젝트가 많아지면, 메모리를 확성화하기 위해 GC가 계속 돌아갈 수 있음 
 *		=> 너무 의존적으로 사용하지 말 것
*/

namespace DesingPattern
{
	public class ObjectPooler
	{
		//Stack 자체를 보관소처럼 사용하기도 함
		//Queue는 원형큐를 쓰기 때문에 rear와 front움직이는 연산이 더 추가됨. stack이 더 효율적임 -> 보통 스택을 쓰는 것
		private Stack<PooledObject> objectPool = new Stack<PooledObject>();

		private int poolSize = 100; //초기 pool 사이즈 -> 성능이 안 좋은 기기면 size를 훨씬 작게 만듦

		public void CreatePool()
		{
			for (int i = 0; i < poolSize; i++)
			{
				objectPool.Push(new PooledObject());
			}
		}

		/// <summary>
		/// 오브젝트 대여
		/// </summary>
		/// <returns></returns>
		public PooledObject GetPool()
		{
			if (objectPool.Count > 0)
				return objectPool.Pop();
			else
				return new PooledObject(); //오브젝트가 다 대여 중이면 새로 만들어 주도록 함
		}

		/// <summary>
		/// 오브젝트 반납
		/// </summary>
		/// <param name="pooled"></param>
		public void ReturnPool(PooledObject pooled)
		{
			//반납 방법은 취향차이임 (게임에 맞게 최적화가 잘 되는 방법으로 선택하면 돼!)
			//i). 생성 시점에 새로 생성된 오브젝트도 pool에 보관함
			//ii). 미리 만들어둔 오브젝트만 pool에 보관하고, 생성 시점에서 필요에 의해 만들어 둔 오브젝트는 반납과 동시에 삭제함

			objectPool.Push(pooled);
		}
	}

	public class PooledObject
	{
		// 생성&삭제가 빈번한 클래스
	}

}