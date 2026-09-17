shader_type spatial;

uniform bool use_texture = false;
uniform sampler2D albedo_texture : source_color, filter_linear;
uniform vec3 albedo : source_color = vec3(1.0);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	if (use_texture) {
		ALBEDO = texture(albedo_texture, UV).rgb;
	}
	else {
		ALBEDO = albedo;
	}
}

float fresnel(vec3 normal, vec3 view){
	return 1.0 - clamp(dot(normalize(normal), normalize(view)), 0.0, 1.0);
}

void light() {
	float light = max(dot(NORMAL, LIGHT), 0.0) * ATTENUATION;

	// 4 levels of toon shading
	light = round(light * 4.0) / 4.0;

	vec3 diffuse = ALBEDO * light * LIGHT_COLOR / PI;

	DIFFUSE_LIGHT += diffuse;
	
	float rim = 1.0 - clamp(
		dot(normalize(NORMAL), normalize(VIEW)),
		0.0,
		1.0
	);

	rim = step(0.7, rim);
	rim *= light;

	SPECULAR_LIGHT += vec3(rim) * LIGHT_COLOR / PI;
}

/*void light() {
	//Called for every pixel for every light affecting the material.
	float light = max(dot(NORMAL, LIGHT), 0.0) * ATTENUATION;
	light = round(light * 4.0)/4.0;
	
	vec3 diffuse = vec3(light) * LIGHT_COLOR/PI;
	
	DIFFUSE_LIGHT += diffuse;
	
	float rim = fresnel(NORMAL, VIEW);
	rim = step(0.7, rim);
	rim *= light;
	
	SPECULAR_LIGHT += vec3(rim) * LIGHT_COLOR/PI;
	
	float specular = max(dot(NORMAL, LIGHT), 0.0) * ATTENUATION;
	SPECULAR_LIGHT += vec3(specular) * LIGHT_COLOR/PI;
	float NdotL = max(dot(NORMAL, LIGHT),0.0);
	
	float toon_light;
	
	if(NdotL > 0.75){
		toon_light = 1.0;
	}
	else if(NdotL > 0.5){
		toon_light = 0.75;
	}
	else if (NdotL > 0.25){
		toon_light = 0.5;
	}
	else{
		toon_light = 0.25;
	}
	
	DIFFUSE_LIGHT += ALBEDO * LIGHT_COLOR * toon_light * ATTENUATION;
}*/
