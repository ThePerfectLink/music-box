using Godot;

public partial class ClickInputHandler : Node3D
{
	[Export] public Camera3D Cam;
	[Export] public AnimationPlayer[] Player;
	[Export] public Node3D Ticket;
	public Node InHand;

	public async override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mb && mb.Pressed && mb.ButtonIndex == MouseButton.Left)
		{
				var spaceState = GetViewport().World3D.DirectSpaceState;
			var mousePos = mb.Position;

			var from = Cam.ProjectRayOrigin(mousePos);
			var to = from + Cam.ProjectRayNormal(mousePos) * 1000f;

			var query = PhysicsRayQueryParameters3D.Create(from, to);
			var result = spaceState.IntersectRay(query);

			if (result.Count > 0 && result["collider"].As<Node>() is Node collider)
			{
				if (collider.HasMeta("animation_name"))
				{
					if(collider.HasMeta("in_hand")) {
						collider.SetMeta("in_hand", !collider.HasMeta("in_hand"));
						string animName = ((string[])collider.GetMeta("animation_name"))[0];
						Player[(int)collider.GetMeta("player")].Play(animName);
						await ToSignal(Player[(int)collider.GetMeta("player")], AnimationPlayer.SignalName.AnimationFinished);
						InHand = collider;
					} else {
						if(InHand != null) {
							if(collider.Name == "music box") {
								if(InHand.Name == "key") {
									string animName = ((string[])InHand.GetMeta("animation_name"))[1];
									Player[(int)InHand.GetMeta("player")].Play(animName);
									
									await ToSignal(Player[(int)InHand.GetMeta("player")], AnimationPlayer.SignalName.AnimationFinished);
									collider.SetMeta("is_unlocked", true);
									InHand = null;
								}
							}
						} else if(collider.Name == "music box" && !(bool)collider.GetMeta("isPlaying")) {
							if((bool)collider.GetMeta("is_unlocked")) {
								if((bool)collider.GetMeta("is_open")) {
									string animName = ((string[])collider.GetMeta("animation_name"))[1];
									Player[(int)collider.GetMeta("player")].Play(animName);
									Player[2].Play(animName);
									await ToSignal(Player[(int)collider.GetMeta("player")], AnimationPlayer.SignalName.AnimationFinished);
									Player[3].Stop();
									collider.Name = "something else";
									Ticket.Visible = true;
									collider.SetMeta("is_open", false);
								} else {
									string animName = ((string[])collider.GetMeta("animation_name"))[0];
									collider.SetMeta("isPlaying", true);
									Player[(int)collider.GetMeta("player")].Play(animName);
									Player[2].Play(animName);
									Player[3].Play("twirl");
									await ToSignal(Player[(int)collider.GetMeta("player")], AnimationPlayer.SignalName.AnimationFinished);
									collider.SetMeta("is_open", true);
									collider.SetMeta("isPlaying", false);
								}
							}
						}
					}
				}
			}
		}
	}
}
