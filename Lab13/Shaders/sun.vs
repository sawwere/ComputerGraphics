#version 330 core
layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec3 normals;
layout (location = 2) in vec2 textPosition;

out vec2 TexCoord;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;
uniform float rotationY;

void main() 
{
    vec3 position = mat3(
            cos(rotationY),  0, sin(rotationY),
            0,               1, 0,
            -sin(rotationY), 0, cos(rotationY)) 
        * vertexPosition;
    gl_Position = projection * view * model * vec4(position, 1.0f); 
    TexCoord = textPosition;
}