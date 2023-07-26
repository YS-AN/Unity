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
		//서버의 상황보고 싶다는 등 뭐든 서버에 요청이 필요한 작업이 있다면 PhotonNetwork를 사용
		//반대로 서버에서 뭘 받고 싶으면 Callbacks를 사용

		if (state == PhotonNetwork.NetworkClientState) //기존 상태와 동일한 상태면 굳이 로그를 찍을 필요 없음 -> 이전 상황과 달라진 경우에만 로그를 찍도록 함
            return;

        state = PhotonNetwork.NetworkClientState; //현재 네트워크 상태를 보여줌


        AddMessage(state.ToString());
    }

    public void AddMessage(string message)
    {
        TMP_Text newLog = Instantiate(logPrefab, content);
        newLog.text = string.Format("{0} [Photon] : {1}", System.DateTime.Now.ToString("yy-MM-dd HH:mm:ss.ff"), message);
        Debug.Log(string.Format("[Photon] {0}", message));
    }
}
