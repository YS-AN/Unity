using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityScript : MonoBehaviour
{
    /************************************************************************
	 * 컴포넌트 (Component)
	 * 
	 * 특정한 기능을 수행할 수 있도록 구성한 작은 기능적 단위
	 * 게임오브젝트의 작동과 관련한 부품
	 * 게임오브젝트에 추가, 삭제하는 방식의 조립형 부품
	 ************************************************************************/

    /************************************************************************
	 * MonoBehaviour
	 * 
	 * 컴포넌트를 기본클래스로 하는 클래스로 유니티 스크립트가 파생되는 기본 클래스
	 * 게임 오브젝트에 스크립트를 컴포넌트로서 연결할 수 있는 구성을 제공
	 * 컴포넌트에 스크립트 직렬화 기능, 유니티메시지 이벤트를 받는 기능, 코루틴 기능을 추가한 형태임
	 * 근본이 컴포넌트이기 때문에 오브젝트에 MonoBehaviour 추가하는 것도 가능함 -> 컴포넌트로서 동작시키는 것도 가능함. 
	 * 만약, MonoBehaviour 상속 안 받으면 그건 컴포넌트가 아니기 때문에 오브젝트에 붙일 수 없음. 
	 * cs 파일 내에 있는 클래스 명과, 파일 명이 동일해야만 컴포넌트에 붙일 수 있음!! 이름을 주의해야 함.
	 ***********************************************************************/

    // <스크립트 직렬화 기능>
    // 인스펙터 창에서 컴포넌트의 맴버변수 값을 확인하거나 변경하는 기능
    // 컴포넌트의 값형식 데이터를 확인하거나 변경이 가능함. -> 소스에 초기화 된 값이 있더라도, 유니티 인스펙터창에서 값을 변경하면 인스펙터창 값으로 적용 됨. 
    //    -> 소스상에서 해주는 값 할당은 초기값정도를 생각하면 돼. (컴포넌트 초기 세팅 값 느낌?)
    // 컴포넌트의 참조형식은 데이터를 드래그 앤 드랍 방식으로 연결도 가능함
    

    // <인스펙터창 직렬화가 가능한 자료형>  -> 기본 자료형은 그냥 직렬화 가능.
    [Header("C# Type")] //Header Attribute : 인스펙터창에서 직렬화의 제목을 붙이는 느낌임.
    public bool boolValue;
    public int intValue; //정수라 인스펙터창에서도 소수점이 입력되지 않음.
    public float floatValue;
    public string stringValue;
    // 그 외 기본 자료형

    // 기본 자료형의 선형자료구조 (선형자료구조만 직렬화 가능함. dictionary나 이진트리 같은 건 안 돼...!)
    public int[] array;
    public List<int> list;
    public List<A> list2; //list<class>는 자체는 직렬화 안 돼. 왜냐면 class 자체가 직렬화가 불가능함.
                          //직렬화를 하려면 클래스에 [Serializable] attribute 붙여줘야함 -> 붙이고 나면 인스펙터창에도 뜯ㅁ

    [Header("Unity Type")]
    public Vector3 vector3; //좌표 3개 있는 백터 (x, y, z) -> 좌표 2개 있는 건 Vector2 써야함.
    public Color color;
    public LayerMask layerMask;
    public AnimationCurve curve;
    public Gradient gradient; //색상 그라데이션 표시 가능. 체력에 따라서 백분률로 나눠서 색이 표현됨.

    [Header("Unity GameObject")]
    public GameObject obj;

    //컴포넌트 또한 참조가 가능함. -> 예를 들면, obj1에 있는 Rigidbody를 obj2에서 참조해서 사용이 가능함.
    [Header("Unity Component")]
    public new Transform transform;
    public new Rigidbody rigidbody;
    public new Collider collider;

    [Header("Unity Event")]
    public UnityEvent OnEvent; //이벤트도 컴포넌트로 만드는 게 가능함.
                               ///delegate는 직접 붙여주는 일을 해야하는데 UnityEvent는 유니티에서 자동으로 해주는 부분이 있기 때문에 대부분 그냥 UnityEvent로 이벤트를 사용함.

    // <어트리뷰트>
    // 클래스, 프로퍼티 또는 함수 위에 명시하여 특별한 동작을 나타낼 수 있는 마커

    [Space(30)] //공간을 30만큼 떨어트려서 보여줌.

    // 직렬화를 해야 할 경우에는 public로 선언을 해야함. (반대로 변경하지 않을 애들은 private로 쓰면 돼. 그럼 인스펙터 창에 나타나지 않음)
    // BUT, private지만 인스펙터 창에서는 보여주고 싶으면 SerializeField 
    //      public이지만 인스펙터 창에서 보여주기 싫을 떄는 HideInInspector

    [Header("Unity Attribute")]
    [SerializeField]
    private int privateValue;
    [HideInInspector]
    public int publicValue;

    [Range(0, 10)] //인스펙터 창에서 슬라이드 형태로 나오게 하는 것
    public float rangeValue;

    [TextArea(3, 5)] //인스펙터 창에서 여러줄로 쓰고 싶을 때는 TextArea를 씀. (보통 길게 데이터가 들어갈 때는 이거 씀)
    public string textField;
}


[Serializable]
public class A {
    public int a;
    public int b;
}