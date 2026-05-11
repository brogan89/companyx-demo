using System;
using System.Collections.Generic;
using ImGuiNET;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class RoomImGui : ImGuiWindow
{
	public override string Name => "Room Manager";
	
	private string _roomNameInput = "Test Room";
	private string _joinCodeInput = "";
	
	private float _currentPollTime;
	private List<Lobby> _rooms = new();

	protected override void _OnImGui()
	{
		if (XFrameRoom.CurrentRoom is null)
		{
			var buttonSize = new Vector2(100, 20);
			
			// create
			ImGui.InputText("Room Name", ref _roomNameInput, 20);
			if (ImGui.Button("Create Room", buttonSize))
				CreateRoom();
			
			ImGui.Separator();

			// join
			ImGui.InputText("Join Code", ref _joinCodeInput, 10);
			if (ImGui.Button("Join Room", buttonSize))
				JoinRoom();
		}
		else
		{
			DrawLobbyInfo();
		}
	}

	private void PollRooms()
	{
		if (XFrameRoom.CurrentRoom is not null)
			return;

		_currentPollTime += Time.deltaTime;
		if (_currentPollTime < 2f)
			return;

		_currentPollTime = 0f;
		UpdateRoomsList();
	}

	private async void UpdateRoomsList()
	{
		try
		{
			_rooms = await XFrameRoom.QueryAsync();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	private void DrawLobbyInfo()
	{
		if (XFrameRoom.CurrentRoom is null)
			return;
		
		ImGui.Text($"Lobby ID: {XFrameRoom.CurrentRoom.Id}");
		ImGui.Text($"Lobby Name: {XFrameRoom.CurrentRoom.Name}");
		ImGui.Text($"Join Code: {XFrameRoom.CurrentRoom.LobbyCode}");

		ImGui.Separator();
		ImGui.Text($"Players ({XFrameRoom.CurrentRoom.Players.Count}/{XFrameRoom.CurrentRoom.MaxPlayers}):");
		foreach (var player in XFrameRoom.CurrentRoom.Players)
			ImGui.Text($"  Player ID: {player.Id}");
		
		ImGui.Separator();
		if (ImGui.Button("Leave Lobby"))
			LeaveLobby();
	}

	private void LeaveLobby()
	{
		_ = XFrameRoom.LeaveAsync();
	}

	private async void CreateRoom()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(_roomNameInput))
				_roomNameInput = "Test Room";
			
			await XFrameRoom.CreateAsync(_roomNameInput);
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
			if (string.IsNullOrWhiteSpace(_joinCodeInput))
				return;
			
			await XFrameRoom.JoinByCodeAsync(_joinCodeInput);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}
}