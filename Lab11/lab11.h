#pragma once

#include <GL\glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>

#include <iostream>

void CheckOpenGLerror();
void ShaderLog(unsigned int shader);
void SetIcon(sf::Window& wnd);
void Init();
void InitShader();
void InitVBO();
void Draw();
void Close();

struct Vertex {
    GLfloat x;
    GLfloat y;
};



