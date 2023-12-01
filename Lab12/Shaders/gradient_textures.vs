#version 330 core
layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec2 textPosition;

out vec3 ourColor;
out vec2 TexCoord;

uniform float rotationX;
uniform float rotationY;
uniform float rotationZ;

void main() 
{
        vec3 position = vertexPosition * mat3(
            1, 0,              0,
            0, cos(rotationX), -sin(rotationX),
            0, sin(rotationX), cos(rotationX)) 
        * mat3(
            cos(rotationY),  0, sin(rotationY),
            0,               1, 0,
            -sin(rotationY), 0, cos(rotationY))  
        * mat3(
            cos(rotationZ),  sin(rotationZ), 0,
            -sin(rotationZ), cos(rotationZ), 0,
            0,               0,              1);
        gl_Position = vec4(position, 1.0);
        ourColor = aColor;
        TexCoord = textPosition;
}