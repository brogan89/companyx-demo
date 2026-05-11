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
	
	private List<Lobby> _rooms = new();

	protected override void _OnImGui()
	{
		if (XFrameRoom.CurrentRoom is null)
		{
			// create
			ImGui.InputText("Room Name", ref _roomNameInput, 20);
			if (ImGui.Button("Create Room"))
				CreateRoom();
			
			ImGui.Separator();

			// join by code
			ImGui.InputText("Join Code", ref _joinCodeInput, 10);
			if (ImGui.Button("Join Room"))
				JoinRoom();
			
			// join by id (from list)
			ImGui.Separator();
			if (ImGui.Button("Refresh List"))
				UpdateRoomsList();
			
			foreach (var room in _rooms)
				if (ImGui.Button($"Join Room: {room.Name}"))
					JoinRoomById(room.Id);
			
			if (ImGui.Button("Join Room"))
				JoinRoom();
		}
		else
		{
			DrawLobbyInfo();
		}
	}

	private async void UpdateRoomsList()
	{
		try
		{
			_rooms.Clear();
			_rooms = await XFrameRoom.QueryAsync();
			Debug.Log($"Found {_rooms.Count} rooms");
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
	
	private async void JoinRoomById(string roomId)
	{
		try
		{
			await XFrameRoom.JoinByIdAsync(roomId);
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