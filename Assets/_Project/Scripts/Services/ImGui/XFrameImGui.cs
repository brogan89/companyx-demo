using ImGuiNET;
using UImGui;
using UnityEngine;

public class XFrameImGui : MonoBehaviour
{
	private readonly ImGuiWindow[] _windows =
	{
		new RoomImGui()
	};

	private AuthImGui _authImGui;

	private void OnEnable()
	{
		UImGuiUtility.Layout += OnLayout;
	}

	private void OnDisable()
	{
		UImGuiUtility.Layout -= OnLayout;
	}

	private void OnLayout(UImGui.UImGui current)
	{
		DrawMainMenu();
		DrawWindows();
		DrawAuth();
	}

	private void DrawMainMenu()
	{
		if (ImGui.BeginMainMenuBar())
		{
			if (ImGui.BeginMenu("Window", XFrameAuth.IsSignedIn))
			{
				foreach (var window in _windows)
				{
					if (ImGui.MenuItem(window.Name, string.Empty, window.IsShowing))
						window.IsShowing = !window.IsShowing;
				}
				ImGui.EndMenu();
			}

			ImGui.EndMainMenuBar();
		}
	}

	private void DrawWindows()
	{
		foreach (var window in _windows)
			if (window.IsShowing)
				window.OnImGui();
	}

	private void DrawAuth()
	{
		if (XFrameAuth.IsSignedIn)
		{
			AuthImGui.DrawInfo();
			_authImGui = null;
			return;
		}

		_authImGui ??= new AuthImGui();
		_authImGui.OnImGui();
	}
}