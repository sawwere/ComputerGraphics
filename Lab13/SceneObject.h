#ifndef SCENE_OBJECT_H
#define SCENE_OBJECT_H

#include <glm/glm.hpp>

#include "ShaderProgram.h"
#include "Mesh.h"


class SceneObject
{
private:
	Mesh* mesh;
	ShaderProgram* shaderProgram;
public:
	glm::vec3 position{ 0.f, 0.f, 0.f };
	glm::vec3 rotation{ 0.f, 0.f, 0.f };
	glm::vec3 scale{ 1.f, 1.f, 1.f };

	SceneObject(Mesh* mesh, ShaderProgram* sp)
	{
		this->mesh = mesh;
		this->shaderProgram = sp;
	}

	~SceneObject();
	
	void Draw(ShaderProgram& shaderProgram)
	{
		mesh->Draw(shaderProgram);
	}


};

#endif
