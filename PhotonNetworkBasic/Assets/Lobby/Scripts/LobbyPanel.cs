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
        //룸 리스트 제거
        while (roomContent.childCount > 0)
        {
            Destroy(roomContent.GetChild(0).gameObject);
        }

		//룸 리스트 세팅
		foreach (RoomInfo info in roomList)
		{
			//방이 사라질 예정이면 + 방이 비공개 되었으면(IsVisible == false) + 방이 닫혔으면(IsOpen == false)
			if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
			{
				if (roomDictionary.ContainsKey(info.Name))
				{
					roomDictionary.Remove(info.Name);
				}
				continue;
			}

			//방이 자료구조에 있었으면 (그냥 무조건 이름이 있었던 방이면 최신으로 변경)
			if (roomDictionary.ContainsKey(info.Name))
			{
				roomDictionary[info.Name] = info;
			}

			//방이 생성됐으면 (지금 막 생긴 방이면)
			else
			{
				roomDictionary.Add(info.Name, info);
			}
		}

		//룸 리스트 생성
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
