using Godot;

public partial class FadeInBitcrush : Node3D
{
	[Export] public MeshInstance3D Target;
	[Export] public float Duration = 1.2f;

	public void PlayFadeInCrush()
	{
		var mat = (ShaderMaterial)Target.MaterialOverride;

		mat.SetShaderParameter("fade", 0.0f);
		mat.SetShaderParameter("crush_amount", 1.0f);

		var tween = CreateTween();
		tween.SetParallel(true); // animate both uniforms simultaneously
		tween.TweenMethod(
			Callable.From((float v) => mat.SetShaderParameter("fade", v)),
			0.0f, 1.0f, Duration
		).SetEase(Tween.EaseType.Out);

		tween.TweenMethod(
			Callable.From((float v) => mat.SetShaderParameter("crush_amount", v)),
			1.0f, 0.0f, Duration
		).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo); // resolves late, snaps clean
	}
	
	public void PlayFadeOutCrush()
	{
		var mat = (ShaderMaterial)Target.MaterialOverride;

		mat.SetShaderParameter("fade", 1.0f);
		mat.SetShaderParameter("crush_amount", 0.0f);

		var tween = CreateTween();
		tween.SetParallel(true); // animate both uniforms simultaneously
		tween.TweenMethod(
			Callable.From((float v) => mat.SetShaderParameter("fade", v)),
			1.0f, 0.0f, Duration
		).SetEase(Tween.EaseType.Out);

		tween.TweenMethod(
			Callable.From((float v) => mat.SetShaderParameter("crush_amount", v)),
			0.0f, 1.0f, Duration
		).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Expo); // resolves late, snaps clean
	}
}
