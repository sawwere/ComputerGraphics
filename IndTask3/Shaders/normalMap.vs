#version 330 core
layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 textPosition;
layout (location = 3) in vec3 aTangent;


out Vertex {
    vec3 FragPos;
    vec2 TexCoord;
    mat3 TBN;
} vs_out;

out vec3 FragPos;
out vec2 TexCoord;
//out vec3 Normal;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

void main() 
{
    vec3 position = vertexPosition;
    gl_Position = projection * view * model * vec4(position, 1.0f); 
    TexCoord = textPosition;

    FragPos = vec3(model * vec4(vertexPosition, 1.0));
    mat3 normalMatrix = mat3(model);
    //Normal = normalMatrix * normal;  

    vs_out.FragPos = vec3(model * vec4(vertexPosition, 1.0));
    vs_out.TexCoord = TexCoord;
    
    vec3 T = normalize(normalMatrix * aTangent);
    vec3 N = normalize(normalMatrix * normal);
    vec3 B = cross(N, T);

    vs_out.TBN = mat3(T, B, N);
}