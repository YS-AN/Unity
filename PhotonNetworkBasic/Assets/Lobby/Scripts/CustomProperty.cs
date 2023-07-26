using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable; //포톤의 Hashtable 이름 자체를 재정의함. (네임스페이스 + 클래스명의 축약형을 선언하는 꼴) 

public static class CustomProperty 
{
	public const string PROPERTYKEY_READY = "Ready";
	public const string PROPERTYKEY_LOAD = "Load";
	public const string PROPERTYKEY_LOADTIME = "LoadTime";

	public static bool GetReady(this Player player)
	{
		return GetReadyProperty(player, PROPERTYKEY_READY, false);
	}

	public static void SetReady(this Player player, bool ready)
	{
		SetReadyProperty(player, PROPERTYKEY_READY, ready);
	}

	public static bool GetLoad(this Player player)
	{
		return GetReadyProperty(player, PROPERTYKEY_LOAD, false);
	}

	public static void SetLoad(this Player player, bool load)
	{
		SetReadyProperty(player, PROPERTYKEY_LOAD, load);
	}

	public static int GetLoadTime(this Room room)
	{
		return GetReadyProperty(room, PROPERTYKEY_LOADTIME, -1);
	}

	public static void SetLoadTime(this Room room, int loadTime)
	{
		SetReadyProperty(room, PROPERTYKEY_LOADTIME, loadTime);
	}

	private static T GetReadyProperty<T>(Player player, string propertyKey, T returnValue)
	{
		//C#에 있는 Hashtable이 아니라 포톤에서 자체적으로 Hashtable을 따로 만듦.
		PhotonHashtable property = player.CustomProperties;

		if (property.ContainsKey(propertyKey)) //일단 해당 키 값이 있는지를 먼저 확인하기
			return (T)property[propertyKey];
		else
			return returnValue;
	}

	private static T GetReadyProperty<T>(Room room, string propertyKey, T returnValue)
	{
		PhotonHashtable property = room.CustomProperties;

		if (property.ContainsKey(propertyKey))
			return (T)property[propertyKey];
		else
			return returnValue;
	}

	private static void SetReadyProperty<T>(Player player, string propertyKey, T value)
	{
		PhotonHashtable property = player.CustomProperties;

		property[propertyKey] = value;
		player.SetCustomProperties(property);
	}

	private static void SetReadyProperty<T>(Room room, string propertyKey, T value)
	{
		PhotonHashtable property = room.CustomProperties;

		property[propertyKey] = value;
		room.SetCustomProperties(property);
	}
}

