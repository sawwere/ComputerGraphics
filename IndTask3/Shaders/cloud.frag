#version 330 core
out vec4 FragColor;

#define RANDOM_MOD (1u << 10u)

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;
in float DistToCenter;


uniform vec3 viewPos;

uniform float TIME;
uniform float Stripes = 1.0;

int hash_1(int v) {
    v ^= v * 32063 >> 5;
    v ^= v * 51577 >> 2;
    v ^= v * 43573 >> 2;
   return v;
}

int hash_2(int x, int y) {
   return hash_1(x ^ hash_1(y));
}

int hash_3(int x, int y, int z) {
    return hash_1(z ^ hash_2(x, y));
}

int hash_4(int x, int y, int z, int w) {
    return hash_1(w ^ hash_3(x, y, z));
}


float u_random(int hash) {
   return float(uint(hash) % RANDOM_MOD) / float(RANDOM_MOD);
}

float s_random(int hash) {
   return u_random(hash) * 2.0 - 1.0;
}

float simple_noise_2(vec2 pos) {
   ivec2 ipos = ivec2(floor(pos));
   ivec2 inxt = ivec2(floor(pos)) + ivec2(1);
  
   float p00 = u_random(hash_2(ipos.x, ipos.y));
   float p01 = u_random(hash_2(ipos.x, inxt.y));
   float p10 = u_random(hash_2(inxt.x, ipos.y));
   float p11 = u_random(hash_2(inxt.x, inxt.y));

   float p0 = mix(p00, p10, pos.x - float(ipos.x));
   float p1 = mix(p01, p11, pos.x - float(ipos.x));
  
   float p = mix(p0, p1, pos.y - float(ipos.y));

   return p;
}

float simple_fractal_2(vec2 pos, int iters) {
   float res = 0.0;
   float scale = 1.0;
   float sum = 0.0;

   for (int i = 0; i < iters; i++) {
       res += simple_noise_2(pos * scale) / scale;
       sum += 1.0 / scale;
       scale *= 2.0;
   }
  
   return clamp(res / sum, 0, 1);
}


float smooth_step(float v0, float v1, float f) {
	f = clamp(f, 0.0, 1.0);
	return v0 + (3.0 - f * 2.0) * f * f * (v1 - v0);
}

float simple_noise_3(vec3 pos) {
    ivec3 ipos = ivec3(floor(pos));
    ivec3 inxt = ivec3(floor(pos)) + ivec3(1);

    float p000 = u_random(hash_3(ipos.x, ipos.y, ipos.z));
    float p001 = u_random(hash_3(ipos.x, ipos.y, inxt.z));
    float p010 = u_random(hash_3(ipos.x, inxt.y, ipos.z));
    float p011 = u_random(hash_3(ipos.x, inxt.y, inxt.z));
    float p100 = u_random(hash_3(inxt.x, ipos.y, ipos.z));
    float p101 = u_random(hash_3(inxt.x, ipos.y, inxt.z));
    float p110 = u_random(hash_3(inxt.x, inxt.y, ipos.z));
    float p111 = u_random(hash_3(inxt.x, inxt.y, inxt.z));

    float r00 = mix(p000, p100, pos.x - float(ipos.x));
    float r10 = mix(p010, p110, pos.x - float(ipos.x));
    float r0 = mix(r00, r10, pos.y - float(ipos.y));

    float r01 = mix(p001, p101, pos.x - float(ipos.x));
    float r11 = mix(p011, p111, pos.x - float(ipos.x));
    float r1 = mix(r01, r11, pos.y - float(ipos.y));
	
	float r = mix(r0, r1, pos.z - float(ipos.z));

    return r;
}

float simple_fractal_3(vec3 pos, int iters) {
	float res = 0.0;
	float scale = 1.0;
	float sum = 0.0;

	for (int i = 0; i < iters; i++) {
		res += simple_noise_3(pos * scale) / scale;
		sum += 1.0 / scale;
		scale *= 2.0;
	}
	
	return smoothstep(0.5, 1, clamp(res / sum, 0.5, 1.0));
}

vec3 perlin_gradient_3(ivec3 pos) {
	float x = s_random(hash_4(pos.x, pos.y, pos.z, 1));
	float y = s_random(hash_4(pos.x, pos.y, pos.z, 2));
	float z = s_random(hash_4(pos.x, pos.y, pos.z, 3));
	return normalize(vec3(x, y, z));
}

float perlin_noise_3(vec3 pos) {
    ivec3 ipos = ivec3(floor(pos));
    ivec3 inxt = ivec3(floor(pos)) + ivec3(1);
	
	ivec3 p000 = ipos;
	ivec3 p001 = ivec3(ipos.x, ipos.y, inxt.z);
	ivec3 p010 = ivec3(ipos.x, inxt.y, ipos.z);
	ivec3 p011 = ivec3(ipos.x, inxt.y, inxt.z);
	ivec3 p100 = ivec3(inxt.x, ipos.y, ipos.z);
	ivec3 p101 = ivec3(inxt.x, ipos.y, inxt.z);
	ivec3 p110 = ivec3(inxt.x, inxt.y, ipos.z);
	ivec3 p111 = inxt;
	
	float v000 = dot(perlin_gradient_3(p000), pos - vec3(p000));
	float v001 = dot(perlin_gradient_3(p001), pos - vec3(p001));
	float v010 = dot(perlin_gradient_3(p010), pos - vec3(p010));
	float v011 = dot(perlin_gradient_3(p011), pos - vec3(p011));
	float v100 = dot(perlin_gradient_3(p100), pos - vec3(p100));
	float v101 = dot(perlin_gradient_3(p101), pos - vec3(p101));
	float v110 = dot(perlin_gradient_3(p110), pos - vec3(p110));
	float v111 = dot(perlin_gradient_3(p111), pos - vec3(p111));
	
    float r00 = smooth_step(v000, v100, pos.x - float(ipos.x));
    float r01 = smooth_step(v001, v101, pos.x - float(ipos.x));
    float r10 = smooth_step(v010, v110, pos.x - float(ipos.x));
    float r11 = smooth_step(v011, v111, pos.x - float(ipos.x));
	
    float r0 = smooth_step(r00, r10, pos.y - float(ipos.y));
    float r1 = smooth_step(r01, r11, pos.y - float(ipos.y));
	
	float r = smooth_step(r0, r1, pos.z - float(ipos.z));
	
	return r * 0.5 + 0.5;
}

float perlin_fractal_3(vec3 pos, int iters) {
	float res = 0.0;
	float scale = 1.0;
	float sum = 0.0;

	for (int i = 0; i < iters; i++) {
		res += perlin_noise_3(pos * scale) / scale;
		sum += 1.0 / scale;
		scale *= 2.0;
	}
	
	return smooth_step(0, 1, res / sum);
}

uniform vec4 Color1 = vec4(0.30);
uniform vec4 Color2 = vec4(0.55);
void main()
{
    vec2 pos = (TexCoord + vec2(0.5)) * 10;

    ivec2 ipos = ivec2(floor(pos));
    float scaler = perlin_fractal_3(vec3(pos, TIME / 2.0), 4);
    vec3 color = vec3(mix(Color1, Color2, scaler));
    //FragColor = vec4(1.0, 0.0, 0.0, 1.0);
    FragColor = vec4(color, 1.0);
}