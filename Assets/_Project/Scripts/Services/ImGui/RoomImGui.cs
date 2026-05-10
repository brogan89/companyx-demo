using System;
using ImGuiNET;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class RoomImGui : ImGuiWindow
{
	public override string Name => "Room Manager";
	private Lobby _lobby;

	protected override void _OnImGui()
	{
		if (_lobby is null)
		{
			if (ImGui.Button("Create Room"))
				CreateRoom();
			if (ImGui.Button("Join Room"))
				JoinRoom();
		}
		else
		{
			DrawLobbyInfo();
		}
	}

	private void DrawLobbyInfo()
	{
		if (_lobby is null)
			return;
		
		ImGui.Text($"Lobby ID: {_lobby.Id}");
		ImGui.Text($"Lobby Name: {_lobby.Name}");
		ImGui.Text($"Lobby Max Players: {_lobby.MaxPlayers}");

		foreach (var player in _lobby.Players)
		{
			ImGui.Text($"Player ID: {player.Id}");
			ImGui.Text($"Player Name: {player.Profile?.Name}");	
		}
	}

	private async void CreateRoom()
	{
		try
		{
			_lobby = await XFrameRoom.CreateAsync("Test Room");
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}
	
	private async void JoinRoom()
	{
		try
		{
			_lobby = await XFrameRoom.JoinByIdAsync("");
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}
}