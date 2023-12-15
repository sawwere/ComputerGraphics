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


float u_random(int hash) {
   return float(uint(hash) % RANDOM_MOD) / float(RANDOM_MOD);
}

float s_random(int hash) {
   return u_random(hash) * 2.0 - 1.0;
}

void main()
{
    vec2 pos = (TexCoord + vec2(0.5)) * 0.01;

    ivec2 ipos = ivec2(floor(pos));
    float color = u_random(hash_2(ipos.x, ipos.y));

    FragColor = vec4(vec3(color), 1.0);

}