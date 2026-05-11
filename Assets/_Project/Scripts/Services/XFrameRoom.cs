using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

/// <summary>
/// Provides functionality to manage game lobbies, including creation and deletion.
/// </summary>
public static class XFrameRoom
{
	/// <summary>
	/// Gets the current lobby that the player is connected to.
	/// </summary>
	/// <remarks>
	/// This property holds the most recent lobby instance that the player joined or created.
	/// It is updated automatically when joining, creating, or leaving a lobby.
	/// Returns null if the player is not currently in a lobby.
	/// </remarks>
	public static Lobby CurrentRoom { get; private set; }

	/// <summary>
	/// Creates a new lobby with the specified name and maximum player count.
	/// </summary>
	/// <param name="lobbyName">The name of the lobby to create.</param>
	/// <param name="maxPlayers">The maximum number of players allowed in the lobby. Defaults to 4.</param>
	/// <returns>A task representing the asynchronous operation, with a <see cref="Lobby"/> object for the created lobby.</returns>
	public static async Awaitable<Lobby> CreateAsync(string lobbyName, int maxPlayers = 4)
	{
		var options = new CreateLobbyOptions
		{
			IsPrivate = false,
		};

		CurrentRoom = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
		Debug.Log($"Created lobby {CurrentRoom.Id}");
		HeartbeatLoop();
		PollUpdates();
		return CurrentRoom;
	}

	/// <summary>
	/// Periodically polls for updates to the current lobby's state at regular intervals.
	/// </summary>
	/// <remarks>
	/// This method continuously fetches the latest state of the current lobby
	/// every 2 seconds until the lobby is no longer available.
	/// Handles any exceptions encountered during the polling process.
	/// </remarks>
	private static async void PollUpdates()
	{
		try
		{
			while (CurrentRoom is not null)
			{
				await Awaitable.WaitForSecondsAsync(2);
				CurrentRoom = await LobbyService.Instance.GetLobbyAsync(CurrentRoom.Id);
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	/// <summary>
	/// Continuously sends heartbeat pings to the server to keep the current lobby active.
	/// </summary>
	/// <remarks>
	/// This method runs asynchronously in a loop, sending pings at regular intervals to maintain the connection to the lobby.
	/// It stops when the current lobby is no longer valid or an exception occurs.
	/// </remarks>
	private static async void HeartbeatLoop()
	{
		try
		{
			while (CurrentRoom is not null)
			{
				await Awaitable.WaitForSecondsAsync(15);
				await LobbyService.Instance.SendHeartbeatPingAsync(CurrentRoom.Id);
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	/// <summary>
	/// Deletes the specified lobby.
	/// </summary>
	/// <param name="lobby">The lobby to be deleted.</param>
	public static async Awaitable DeleteAsync(Lobby lobby)
	{
		try
		{
			await LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	/// <summary>
	/// Removes the current player from the specified lobby.
	/// </summary>
	public static async Awaitable LeaveAsync()
	{
		try
		{
			// Ensure you sign-in before calling Authentication Instance
			var playerId = AuthenticationService.Instance.PlayerId;
			await LobbyService.Instance.RemovePlayerAsync(CurrentRoom.Id, playerId);
			CurrentRoom = null;
		}
		catch (LobbyServiceException e)
		{
			// TODO: might want to log something specific here
			Debug.LogException(e);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	/// <summary>
	/// Queries available lobbies based on predefined filters and ordering criteria.
	/// </summary>
	/// <returns>A task representing the asynchronous operation, providing a list of <see cref="Lobby"/> objects that match the query criteria.</returns>
	public static async Awaitable<List<Lobby>> QueryAsync()
	{
		// TODO: placeholder query method to demonstrate paging. This should be replaced with a more specific query in production code.
		// Common query options to use for paging
		// var queryOptions = new QueryLobbiesOptions
		// {
		// 	SampleResults = false, // Paging cannot use randomized results
		// 	Filters = new List<QueryFilter>
		// 	{
		// 		// Only include open lobbies in the pages
		// 		new(
		// 			field: QueryFilter.FieldOptions.AvailableSlots,
		// 			op: QueryFilter.OpOptions.GT,
		// 			value: "0")
		// 	},
		// 	Order = new List<QueryOrder>
		// 	{
		// 		// Show the oldest lobbies first
		// 		new(true, QueryOrder.FieldOptions.Created)
		// 	}
		// };

		var response = await LobbyService.Instance.QueryLobbiesAsync(/*queryOptions*/);
		var lobbies = response.Results;

		// A continuation token will still be returned when the next page is empty,
		// so continue paging until there are no new lobbies in the response
		// while (lobbies.Count > 0)
		// {
		// 	// Do something here with the lobbies in the current page
		//
		// 	// Get the next page. Be careful not to modify the filter or order in the
		// 	// query options, as this will return an error
		// 	queryOptions.ContinuationToken = response.ContinuationToken;
		// 	response = await LobbyService.Instance.QueryLobbiesAsync(queryOptions);
		// 	lobbies = response.Results;
		// }

		return lobbies;
	}

	/// <summary>
	/// Joins an existing lobby using its unique identifier.
	/// </summary>
	/// <param name="lobbyId">The unique identifier of the lobby to join.</param>
	/// <returns>A task representing the asynchronous operation, with a <see cref="Lobby"/> object for the joined lobby.</returns>
	public static async Awaitable<Lobby> JoinByIdAsync(string lobbyId)
	{
		CurrentRoom = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
		Debug.Log($"Joined lobby {CurrentRoom.Id}");
		HeartbeatLoop();
		PollUpdates();
		return CurrentRoom;
	}

	/// <summary>
	/// Joins an existing lobby using a provided join code.
	/// </summary>
	/// <param name="joinCode">The unique code used to join the lobby.</param>
	/// <returns>A task representing the asynchronous operation, with a <see cref="Lobby"/> object for the joined lobby.</returns>
	public static async Awaitable<Lobby> JoinByCodeAsync(string joinCode)
	{
		CurrentRoom = await LobbyService.Instance.JoinLobbyByCodeAsync(joinCode);
		Debug.Log($"Joined lobby {CurrentRoom.Id}");
		HeartbeatLoop();
		PollUpdates();
		return CurrentRoom;
	}
}