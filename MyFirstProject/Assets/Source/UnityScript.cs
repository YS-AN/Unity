using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityScript : MonoBehaviour
{
    /************************************************************************
	 * ������Ʈ (Component)
	 * 
	 * Ư���� ����� ������ �� �ֵ��� ������ ���� ����� ����
	 * ���ӿ�����Ʈ�� �۵��� ������ ��ǰ
	 * ���ӿ�����Ʈ�� �߰�, �����ϴ� ����� ������ ��ǰ
	 ************************************************************************/

    /************************************************************************
	 * MonoBehaviour
	 * 
	 * ������Ʈ�� �⺻Ŭ������ �ϴ� Ŭ������ ����Ƽ ��ũ��Ʈ�� �Ļ��Ǵ� �⺻ Ŭ����
	 * ���� ������Ʈ�� ��ũ��Ʈ�� ������Ʈ�μ� ������ �� �ִ� ������ ����
	 * ������Ʈ�� ��ũ��Ʈ ����ȭ ���, ����Ƽ�޽��� �̺�Ʈ�� �޴� ���, �ڷ�ƾ ����� �߰��� ������
	 * �ٺ��� ������Ʈ�̱� ������ ������Ʈ�� MonoBehaviour �߰��ϴ� �͵� ������ -> ������Ʈ�μ� ���۽�Ű�� �͵� ������. 
	 * ����, MonoBehaviour ��� �� ������ �װ� ������Ʈ�� �ƴϱ� ������ ������Ʈ�� ���� �� ����. 
	 * cs ���� ���� �ִ� Ŭ���� ���, ���� ���� �����ؾ߸� ������Ʈ�� ���� �� ����!! �̸��� �����ؾ� ��.
	 ***********************************************************************/

    // <��ũ��Ʈ ����ȭ ���>
    // �ν����� â���� ������Ʈ�� �ɹ����� ���� Ȯ���ϰų� �����ϴ� ���
    // ������Ʈ�� ������ �����͸� Ȯ���ϰų� ������ ������. -> �ҽ��� �ʱ�ȭ �� ���� �ִ���, ����Ƽ �ν�����â���� ���� �����ϸ� �ν�����â ������ ���� ��. 
    //    -> �ҽ��󿡼� ���ִ� �� �Ҵ��� �ʱⰪ������ �����ϸ� ��. (������Ʈ �ʱ� ���� �� ����?)
    // ������Ʈ�� ���������� �����͸� �巡�� �� ��� ������� ���ᵵ ������
    

    // <�ν�����â ����ȭ�� ������ �ڷ���>  -> �⺻ �ڷ����� �׳� ����ȭ ����.
    [Header("C# Type")] //Header Attribute : �ν�����â���� ����ȭ�� ������ ���̴� ������.
    public bool boolValue;
    public int intValue; //������ �ν�����â������ �Ҽ����� �Էµ��� ����.
    public float floatValue;
    public string stringValue;
    // �� �� �⺻ �ڷ���

    // �⺻ �ڷ����� �����ڷᱸ�� (�����ڷᱸ���� ����ȭ ������. dictionary�� ����Ʈ�� ���� �� �� ��...!)
    public int[] array;
    public List<int> list;
    public List<A> list2; //list<class>�� ��ü�� ����ȭ �� ��. �ֳĸ� class ��ü�� ����ȭ�� �Ұ�����.
                          //����ȭ�� �Ϸ��� Ŭ������ [Serializable] attribute �ٿ������ -> ���̰� ���� �ν�����â���� �⤱

    [Header("Unity Type")]
    public Vector3 vector3; //��ǥ 3�� �ִ� ���� (x, y, z) -> ��ǥ 2�� �ִ� �� Vector2 �����.
    public Color color;
    public LayerMask layerMask;
    public AnimationCurve curve;
    public Gradient gradient; //���� �׶��̼� ǥ�� ����. ü�¿� ���� ��з��� ������ ���� ǥ����.

    [Header("Unity GameObject")]
    public GameObject obj;

    //������Ʈ ���� ������ ������. -> ���� ���, obj1�� �ִ� Rigidbody�� obj2���� �����ؼ� ����� ������.
    [Header("Unity Component")]
    public new Transform transform;
    public new Rigidbody rigidbody;
    public new Collider collider;

    [Header("Unity Event")]
    public UnityEvent OnEvent; //�̺�Ʈ�� ������Ʈ�� ����� �� ������.
                               ///delegate�� ���� �ٿ��ִ� ���� �ؾ��ϴµ� UnityEvent�� ����Ƽ���� �ڵ����� ���ִ� �κ��� �ֱ� ������ ��κ� �׳� UnityEvent�� �̺�Ʈ�� �����.

    // <��Ʈ����Ʈ>
    // Ŭ����, ������Ƽ �Ǵ� �Լ� ���� ����Ͽ� Ư���� ������ ��Ÿ�� �� �ִ� ��Ŀ

    [Space(30)] //������ 30��ŭ ����Ʈ���� ������.

    // ����ȭ�� �ؾ� �� ��쿡�� public�� ������ �ؾ���. (�ݴ�� �������� ���� �ֵ��� private�� ���� ��. �׷� �ν����� â�� ��Ÿ���� ����)
    // BUT, private���� �ν����� â������ �����ְ� ������ SerializeField 
    //      public������ �ν����� â���� �����ֱ� ���� ���� HideInInspector

    [Header("Unity Attribute")]
    [SerializeField]
    private int privateValue;
    [HideInInspector]
    public int publicValue;

    [Range(0, 10)] //�ν����� â���� �����̵� ���·� ������ �ϴ� ��
    public float rangeValue;

    [TextArea(3, 5)] //�ν����� â���� �����ٷ� ���� ���� ���� TextArea�� ��. (���� ��� �����Ͱ� �� ���� �̰� ��)
    public string textField;
}


[Serializable]
public class A {
    public int a;
    public int b;
}