using Godot;

public partial class HoverCursor : Node3D
{
	[Export] public StaticBody3D[] Colliders;
	[Export] public Image GrabImage { get; set; }
	[Export] public Image PointImage { get; set; }

	public override void _Ready()
	{
		PointImage.Resize(32, 32, Image.Interpolation.Lanczos);
		ImageTexture resizedTexture = ImageTexture.CreateFromImage(PointImage);
		Input.SetCustomMouseCursor(
			resizedTexture,
			Input.CursorShape.Arrow,
			new Vector2(16,16)
		);
		foreach (var col in Colliders)
		{
			col.MouseEntered += () => OnMouseEntered(col);
			col.MouseExited += OnMouseExited;
		}
	}

	private void OnMouseEntered(StaticBody3D col)
	{
		GrabImage.Resize(32, 32, Image.Interpolation.Lanczos);
		ImageTexture resizedTexture = ImageTexture.CreateFromImage(GrabImage);
		if (col.HasMeta("animation_name")) // or whatever metadata flags "clickable"
		{
			Input.SetCustomMouseCursor(
				resizedTexture,
				Input.CursorShape.Arrow,
				new Vector2(16,16)
			);
			// or simpler, if you just want a built-in shape instead of a custom texture:
			// Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
		}
	}

	private void OnMouseExited()
	{
		PointImage.Resize(32, 32, Image.Interpolation.Lanczos);
		ImageTexture resizedTexture = ImageTexture.CreateFromImage(PointImage);
		Input.SetCustomMouseCursor(
			resizedTexture,
			Input.CursorShape.Arrow,
			new Vector2(16,16)
		);
	}
}
