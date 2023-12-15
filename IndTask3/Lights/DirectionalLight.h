#ifndef DIRECTIONAL_LIGHT_H
#define DIRECTIONAL_LIGHT_H

#include "..//ShaderProgram.h"


struct DirectionalLight
{
	glm::vec3 direction = { -0.2f, -1.0f, 0.3f };

	glm::vec3 ambient = {0.2f, 0.2f, 0.2f};
	glm::vec3 diffuse = { 0.7f, 0.7f, 0.7f };
	glm::vec3 specular = { 0.5f, 0.5f, 0.5f };

	DirectionalLight() {};
};

#endif // !DIRECTIONAL_LIGHT_H
