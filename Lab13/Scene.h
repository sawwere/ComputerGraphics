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
		Camera camera();

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

		

		glm::mat4 projection = glm::perspective(glm::radians(45.0f), (float)800 / (float)800, 0.1f, 1000.0f);

		for (auto& shaderProgram : shaders) 
		{
			shaderProgram->Use();
			shaderProgram->SetUniformMat4("projection", projection);
			shaderProgram->SetUniformMat4("view", camera.GetViewMatrix());
			glUseProgram(0);
		}
		
		for (auto& sceneObject : sceneObjects) 
		{
			sceneObject->rotation.y = rotationAngle;
			sceneObject->Draw();
		}
		skybox.Draw(camera.GetViewMatrix(), projection);
	}

private:
	std::vector<Mesh> meshes;
};


#endif
