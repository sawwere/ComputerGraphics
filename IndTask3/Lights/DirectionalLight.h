#ifndef DIRECTIONAL_LIGHT_H
#define DIRECTIONAL_LIGHT_H

#include "..//ShaderProgram.h"


struct DirectionalLight
{
	glm::vec3 direction = { -1.0f, -1.0f, -0.0f };

	glm::vec3 ambient = {0.2f, 0.2f, 0.2f};
	glm::vec3 diffuse = { 1.0f, 1.0f, 1.0f };
	glm::vec3 specular = { 1.0f, 1.0f, 1.0f };

	DirectionalLight() {};
};

#endif // !DIRECTIONAL_LIGHT_H
