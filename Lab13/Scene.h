#ifndef SCENE_H
#define SCENE_H

#include "ShaderProgram.h"
#include "Mesh.h"
#include "SceneObject.h"
#include "Camera.h";
#include "Skybox.h"

class Scene
{
public:
	std::vector<ShaderProgram*> shaders;
	std::vector<SceneObject*> sceneObjects;
	Camera camera;
	Skybox skybox;
	Scene() 
	{
		skybox = Skybox();

	}

	void AddShaderProgram(ShaderProgram& sp)
	{
		shaders.push_back(&sp);
	}

	void AddSceneObject(SceneObject& so)
	{
		sceneObjects.push_back(&so);
	}

	void Draw(float rotationAngle)
	{
		//TODO 
		//передача данных в юниформ шейдеров в зависимости от камеры??
		// заменить, когда появится камера
		//TODO camera!!!
		glm::mat4 projection = glm::perspective(glm::radians(45.0f), (float)800 / (float)800, 0.1f, 1000.0f);

		glm::vec3 position = glm::vec3(0.0f, 0.0f, 5.0f);
		glm::vec3 front = glm::vec3(0.0f, 0.0f, -1.0f);
		glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f);
		glm::mat4 view = glm::lookAt(position, position + front, up);

		for (auto& shaderProgram : shaders) 
		{
			shaderProgram->Use();
			shaderProgram->SetUniformMat4("projection", projection);
			shaderProgram->SetUniformMat4("view", view);
			glUseProgram(0);
		}
		
		for (auto& sceneObject : sceneObjects) 
		{
			sceneObject->rotation.y = rotationAngle;
			sceneObject->Draw();
		}
		skybox.Draw(view, projection);
	}

private:
	std::vector<Mesh> meshes;
};


#endif
