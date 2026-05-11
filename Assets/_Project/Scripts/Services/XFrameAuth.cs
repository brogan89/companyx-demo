using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

/// <summary>
/// Provides static methods for initializing and managing authentication
/// with Unity Services. This class handles profile-based initialization
/// and anonymous sign-in functionality for Unity Services.
/// </summary>
public static class XFrameAuth
{
	/// <summary>
	/// Indicates whether the Unity Services have been successfully initialized and are authorized for use.
	/// </summary>
	/// <remarks>
	/// This property is set to true after a successful call to the <c>InitAsync</c> method.
	/// It ensures that Unity Services are ready to be used before performing operations like signing in.
	/// </remarks>
	public static bool IsAuthorized { get; private set; }

	/// <summary>
	/// Indicates whether the user is currently signed in to the service.
	/// </summary>
	/// <remarks>
	/// This property reflects the sign-in status of the application and is set to true after a successful call to the <c>SignInAsync</c> method.
	/// It ensures that user-specific operations can be performed when the user is authenticated.
	/// </remarks>
	public static bool IsSignedIn { get; private set; }

	/// <summary>
	/// Initializes Unity Services for the application using the specified environment profile.
	/// </summary>
	/// <param name="profile">The name of the environment profile to use for initialization. Defaults to "production".</param>
	/// <returns>A task representing the asynchronous initialization operation.</returns>
	public static async Awaitable InitAsync(string profile = "production")
	{
		try
		{
			if (IsSignedIn)
				return;

			var options = new InitializationOptions()
				.SetEnvironmentName(profile);

			await UnityServices.InitializeAsync(options);
			Debug.Log("Unity Services Initialized");
			IsAuthorized = true;
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	/// <summary>
	/// Signs in to Unity Services anonymously using the specified profile.
	/// </summary>
	/// <param name="profile">
	/// The optional profile name to use for the sign-in. If not provided, the current profile will be used.
	/// </param>
	/// <returns>A task representing the asynchronous sign-in operation.</returns>
	/// <exception cref="Exception">
	/// Thrown if Unity Services are not initialized before invoking the sign-in.
	/// </exception>
	public static async Awaitable SignInAsync(string profile = null)
	{
		if (!IsAuthorized)
			throw new Exception("Unity Services not initialized");
		
		if (AuthenticationService.Instance.IsSignedIn)
			return;

		if (!string.IsNullOrWhiteSpace(profile))
			AuthenticationService.Instance.SwitchProfile(profile);
		
		await AuthenticationService.Instance.SignInAnonymouslyAsync();
		Debug.Log($"Signed in as: {AuthenticationService.Instance.Profile} - {AuthenticationService.Instance.PlayerId}");
		IsSignedIn = true;
	}
}