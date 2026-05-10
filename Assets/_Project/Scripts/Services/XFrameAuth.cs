using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public static class XFrameAuth
{
	public static bool IsAuthorized { get; private set; }
	public static bool IsSignedIn { get; private set; }

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