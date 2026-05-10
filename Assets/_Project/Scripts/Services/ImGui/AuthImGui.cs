using System;
using ImGuiNET;
using Unity.Services.Authentication;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class AuthImGui : ImGuiWindow
{
	public override string Name => "Auth";

	private string _environment = "production";
	private int _profileIndex;
	private readonly string[] _profiles = {"Profile-1", "Profile-2"};
	private bool _isLoggingIn;

	protected override void _OnPreImGui()
	{
		base._OnPreImGui();
		
		flags |= ImGuiWindowFlags.NoBackground
		         | ImGuiWindowFlags.NoTitleBar;
	}

	protected override void _OnImGui()
	{
		if (XFrameAuth.IsSignedIn)
		{
			ImGui.Text("Logged in!");
			return;
		}
		
		if (_isLoggingIn)
		{
			ImGui.Text("Logging in...");
			return;
		}
		
		ImGui.InputText("Environment", ref _environment, 100);
		ImGui.Combo("Profile", ref _profileIndex, _profiles, _profiles.Length);
		if (ImGui.Button("Login"))
			LogIn();
	}

	private async void LogIn()
	{
		try
		{
			_isLoggingIn = true;
			await XFrameAuth.InitAsync(_environment);
			await XFrameAuth.SignInAsync(_profiles[_profileIndex]);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}

	public static void DrawInfo()
	{
		const ImGuiWindowFlags flags = ImGuiWindowFlags.AlwaysAutoResize
		                               | ImGuiWindowFlags.NoCollapse
		                               | ImGuiWindowFlags.NoBackground
		                               | ImGuiWindowFlags.NoTitleBar
		                               | ImGuiWindowFlags.NoMove
		                               | ImGuiWindowFlags.NoInputs;

		ImGui.SetNextWindowPos(new Vector2(20, 20), ImGuiCond.Always);
		if (ImGui.Begin("Auth Info", flags))
		{
			if (XFrameAuth.IsSignedIn)
			{
				ImGui.Text($"Player ID: {AuthenticationService.Instance.PlayerId}");
				ImGui.Text($"Profile: {AuthenticationService.Instance.Profile}");
			}
		}

		ImGui.End();
	}
}