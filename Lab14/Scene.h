#ifndef SCENE_H
#define SCENE_H

#include "ShaderProgram.h"
#include "Mesh.h"
#include "SceneObject.h"
#include "Camera.h";
#include "Skybox.h"

#include "Lights.h"

class Scene
{
	DirectionalLight directionalLight;
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

	void SetDirectionalLight(DirectionalLight dirLight)
	{
		this->directionalLight = dirLight;
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
			shaderProgram->SetUniformVec3("viewPos", camera.Position);

			shaderProgram->SetUniformFloat("shadeCount", 5.0f);


			shaderProgram->SetUniformVec3("directionalLight.direction", directionalLight.direction);
			shaderProgram->SetUniformVec3("directionalLight.ambient", directionalLight.ambient);
			shaderProgram->SetUniformVec3("directionalLight.diffuse", directionalLight.diffuse);
			shaderProgram->SetUniformVec3("directionalLight.specular", directionalLight.specular);


			shaderProgram->SetUniformVec3("pointLight.position", 50.0f, 1.0f, -0.3f);
			shaderProgram->SetUniformVec3("pointLight.ambient", 0.05f, 0.05f, 0.05f);
			shaderProgram->SetUniformVec3("pointLight.diffuse", 0.4f, 0.4f, 0.4f);
			shaderProgram->SetUniformVec3("pointLight.specular", 0.5f, 0.5f, 0.5f);
			shaderProgram->SetUniformVec3("pointLight.attenuation", 1.0f, 0.09f, 0.032f);


			shaderProgram->SetUniformVec3("spotLight.position", camera.Position);
			shaderProgram->SetUniformVec3("spotLight.direction", camera.Front);

			shaderProgram->SetUniformVec3("spotLight.ambient", 0.05f, 0.05f, 0.05f);
			shaderProgram->SetUniformVec3("spotLight.diffuse", 0.4f, 0.4f, 0.4f);
			shaderProgram->SetUniformVec3("spotLight.specular", 0.5f, 0.5f, 0.5f);
			shaderProgram->SetUniformVec3("spotLight.attenuation", 1.0f, 0.09f, 0.032f);

			shaderProgram->SetUniformFloat("spotLight.innerAngle", 5.0f);
			shaderProgram->SetUniformFloat("spotLight.outerAngle", 9.0f);

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
