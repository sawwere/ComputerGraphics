#version 330 core
layout (location = 0) in vec3 vertexPosition;

uniform float radius = 2.5f;
uniform float yLimit = 100;
uniform float TIME;

uniform mat4 projection;
uniform mat4 view;

void main() 
{
    vec3 position = vertexPosition;
    position.x += cos((TIME + vertexPosition.z) * 0.5f) * radius;
    position.y = mod(position.y - TIME, yLimit);
    position.z += sin((TIME + vertexPosition.x) * 0.5f) * radius;

    vec4 viewPos = view * vec4(position, 1.0f);
    gl_PointSize = 200 / length( viewPos.xyz );

    gl_Position = projection * viewPos; 
}