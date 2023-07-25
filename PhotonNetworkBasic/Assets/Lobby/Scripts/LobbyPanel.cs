using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;

    Dictionary<string, RoomInfo> roomDictionary;

	private void Awake()
	{
		roomDictionary = new Dictionary<string, RoomInfo>();
	}

	public void UpdateRoomList(List<RoomInfo> roomList)
    {
        //�� ����Ʈ ����
        while (roomContent.childCount > 0)
        {
            Destroy(roomContent.GetChild(0).gameObject);
        }

		//�� ����Ʈ ����
		foreach (RoomInfo info in roomList)
		{
			//���� ����� �����̸� + ���� ����� �Ǿ�����(IsVisible == false) + ���� ��������(IsOpen == false)
			if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
			{
				if (roomDictionary.ContainsKey(info.Name))
				{
					roomDictionary.Remove(info.Name);
				}
				continue;
			}

			//���� �ڷᱸ���� �־����� (�׳� ������ �̸��� �־��� ���̸� �ֽ����� ����)
			if (roomDictionary.ContainsKey(info.Name))
			{
				roomDictionary[info.Name] = info;
			}

			//���� ���������� (���� �� ���� ���̸�)
			else
			{
				roomDictionary.Add(info.Name, info);
			}
		}

		//�� ����Ʈ ����
		foreach(var info in  roomDictionary.Values)
		{
			RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
			entry.SetRoomInfo(info);
		}

	}

	public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
}
