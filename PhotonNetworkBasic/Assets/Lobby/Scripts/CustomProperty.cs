using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable; //포톤의 Hashtable 이름 자체를 재정의함. (네임스페이스 + 클래스명의 축약형을 선언하는 꼴) 

public static class CustomProperty 
{
	private const string PROPERTYKEY_READY = "Ready";
	private const string PROPERTYKEY_LOAD = "Load";

	public static bool GetReady(this Player player)
	{
		return GetReadyProperty(player, PROPERTYKEY_READY);
	}

	public static void SetReady(this Player player, bool ready)
	{
		SetReadyProperty(player, ready, PROPERTYKEY_READY);
	}

	public static bool GetLoad(this Player player)
	{
		return GetReadyProperty(player, PROPERTYKEY_LOAD);
	}

	public static void SetLoad(this Player player, bool load)
	{
		SetReadyProperty(player, load, PROPERTYKEY_LOAD);
	}

	private static bool GetReadyProperty(Player player, string propertyKey)
	{
		//C#에 있는 Hashtable이 아니라 포톤에서 자체적으로 Hashtable을 따로 만듦.
		PhotonHashtable property = player.CustomProperties;

		if (property.ContainsKey(propertyKey)) //일단 해당 키 값이 있는지를 먼저 확인하기
			return (bool)property[propertyKey];
		else
			return false;
	}

	private static void SetReadyProperty(Player player, bool value, string propertyKey)
	{
		PhotonHashtable property = player.CustomProperties;

		property[propertyKey] = value;
		player.SetCustomProperties(property);
	}
}

