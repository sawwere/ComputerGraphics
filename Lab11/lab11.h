#pragma once

#include <GL\glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>

#include <iostream>

void CheckOpenGLerror();
void ShaderLog(unsigned int shader);
void SetIcon(sf::Window& wnd);
void Init(int taskCode, int figCode);
void InitShader(int taskCode);
void InitVBO(int figCode);
void Draw(int taskCode, int figCode);

void Release();
void ReleaseVBO();
void ReleaseShader();

struct Vertex {
	GLfloat x;
	GLfloat y;
	GLfloat r;
	GLfloat g;
	GLfloat b;
	GLfloat a;
};



