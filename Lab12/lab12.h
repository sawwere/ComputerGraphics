#pragma once

#include <GL\glew.h>
#include <SFML/OpenGL.hpp>
#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>

#include <iostream>

#include "ShaderProgram.h"

const float DEGREE_TO_RADIAN = 3.14159265358979323846 / 180.0;

void CheckOpenGLerror();
void ShaderLog(unsigned int shader);
void SetIcon(sf::Window& wnd);
void Init();
void InitShader();
void InitVBO();
void Draw();

void Release();
void ReleaseVBO();
void ReleaseShader();

float MixTexture(float old, float value);
void task2();
void task3();
void task4();

//struct Vertex {
//	GLfloat x;
//	GLfloat y;
//	GLfloat z;
//
//	GLfloat r;
//	GLfloat g;
//	GLfloat b;
//
//	GLfloat texture_x;
//	GLfloat texture_y;
//};



