#version 330 core
layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 textPosition;
layout (location = 3) in mat4 modelMatrixInstanced;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoord;
out float DistToCenter;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

void main() 
{
    vec3 position = vertexPosition;
    mat4 transformed = model * modelMatrixInstanced;
    gl_Position = projection * view * transformed * vec4(position, 1.0f); 
    TexCoord = textPosition;

    FragPos = vec3(transformed * vec4(vertexPosition, 1.0));
    Normal = mat3(transpose(inverse(transformed))) * normal;  
    DistToCenter = distance(vertexPosition, vec3(0.0, vertexPosition.y, 0.0));
}