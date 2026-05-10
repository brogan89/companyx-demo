using ImGuiNET;

public abstract class ImGuiWindow
{
	public abstract string Name { get; }
	public bool IsShowing;

	protected ImGuiWindowFlags flags = ImGuiWindowFlags.AlwaysAutoResize
	                                       | ImGuiWindowFlags.NoCollapse;
	
		
	public void OnImGui()
	{
		_OnPreImGui();
		
		if (ImGui.Begin(Name, ref IsShowing, flags))
		{
			_OnImGui();
			
			// center the window to the screen
			var windowSizeHalf = ImGui.GetWindowSize() / 2f;
			ImGui.SetWindowPos(ImGui.GetMainViewport().GetCenter() - windowSizeHalf);
		}
		ImGui.End();
	}
	protected virtual void _OnPreImGui() { }
	protected abstract void _OnImGui();
}