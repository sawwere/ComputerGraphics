#ifndef POINT_LIGHT_H
#define POINT_LIGHT_H

#include "..//ShaderProgram.h"


struct PointLight
{
    glm::vec3 position;

    glm::vec3 attenuation = { 1.0f, 0.007f, 0.0002f };

    glm::vec3 ambient = { 0.2f, 0.2f, 0.2f };
    glm::vec3 diffuse = { 0.7f, 0.7f, 0.7f };
    glm::vec3 specular = { 1.0f, 1.0f, 1.0f };

	PointLight() {};
};

#endif // !DIRECTIONAL_LIGHT_H
