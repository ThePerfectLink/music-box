shader_type spatial;

void vertex() {
	// Called for every vertex the material is visible on.
}

render_mode blend_mix;

uniform float fade : hint_range(0.0, 1.0) = 0.0;
uniform float crush_amount : hint_range(0.0, 1.0) = 1.0; // 1 = max crush, 0 = clean
uniform sampler2D albedo_texture : source_color;

void fragment() {
    vec4 tex_color = texture(albedo_texture, UV);

    // Bit-depth reduction: fewer steps = more crushed
    float levels = mix(256.0, 4.0, crush_amount); // 4 levels at max crush, 256 = uncrushed
    vec3 crushed = floor(tex_color.rgb * levels) / levels;

    ALBEDO = crushed;
    ALPHA = fade;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
