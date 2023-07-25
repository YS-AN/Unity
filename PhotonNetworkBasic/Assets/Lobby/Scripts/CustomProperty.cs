using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable; //������ Hashtable �̸� ��ü�� ��������. (���ӽ����̽� + Ŭ�������� ������� �����ϴ� ��) 

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
		//C#�� �ִ� Hashtable�� �ƴ϶� ���濡�� ��ü������ Hashtable�� ���� ����.
		PhotonHashtable property = player.CustomProperties;

		if (property.ContainsKey(propertyKey)) //�ϴ� �ش� Ű ���� �ִ����� ���� Ȯ���ϱ�
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

