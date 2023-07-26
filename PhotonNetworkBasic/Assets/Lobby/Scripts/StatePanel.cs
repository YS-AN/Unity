using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class StatePanel : MonoBehaviour
{
    [SerializeField] RectTransform content;
    [SerializeField] TMP_Text logPrefab;

	private ClientState state;

    void Update()
    {
		//������ ��Ȳ���� �ʹٴ� �� ���� ������ ��û�� �ʿ��� �۾��� �ִٸ� PhotonNetwork�� ���
		//�ݴ�� �������� �� �ް� ������ Callbacks�� ���

		if (state == PhotonNetwork.NetworkClientState) //���� ���¿� ������ ���¸� ���� �α׸� ���� �ʿ� ���� -> ���� ��Ȳ�� �޶��� ��쿡�� �α׸� �ﵵ�� ��
            return;

        state = PhotonNetwork.NetworkClientState; //���� ��Ʈ��ũ ���¸� ������


        AddMessage(state.ToString());
    }

    public void AddMessage(string message)
    {
        TMP_Text newLog = Instantiate(logPrefab, content);
        newLog.text = string.Format("{0} [Photon] : {1}", System.DateTime.Now.ToString("yy-MM-dd HH:mm:ss.ff"), message);
        Debug.Log(string.Format("[Photon] {0}", message));
    }
}
