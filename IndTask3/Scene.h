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
	std::vector<PointLight*> pointLights;

	sf::Time deltaTime;
	sf::Clock clock;
	sf::Clock unstopClock;
public:
	std::vector<ShaderProgram*> shaders;
	std::vector<SceneObject*> sceneObjects;
	
	Camera camera;
	Skybox skybox;

	SceneObject fir;

	float getDeltaTime()
	{
		return deltaTime.asSeconds();
	}

	Scene() 
	{
		skybox = Skybox();
		camera = Camera({0.0f, 80.0f, 120.0f});
		pointLights = std::vector<PointLight*>();

		clock.restart();
		unstopClock.restart();
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

	void AddPointLight(PointLight* pl)
	{
		pointLights.push_back(pl);
	}

	void RemovePointLight(PointLight* pl)
	{
		std::remove(pointLights.begin(), pointLights.end(), pl);
	}

	void Draw()
	{
		deltaTime = clock.restart();

		glm::mat4 projection = glm::perspective(glm::radians(45.0f), (float)camera.SCREEN_WIDTH / (float)camera.SCREEN_HEIGHT, 0.1f, 1000.0f);

		for (auto& shaderProgram : shaders) 
		{
			shaderProgram->Use();
			shaderProgram->SetUniformFloat("TIME", unstopClock.getElapsedTime().asSeconds());

			shaderProgram->SetUniformMat4("projection", projection);
			shaderProgram->SetUniformMat4("view", camera.GetViewMatrix());
			shaderProgram->SetUniformVec3("viewPos", camera.Position);

			shaderProgram->SetUniformVec3("directionalLight.direction", directionalLight.direction);
			shaderProgram->SetUniformVec3("directionalLight.ambient", directionalLight.ambient);
			shaderProgram->SetUniformVec3("directionalLight.diffuse", directionalLight.diffuse);
			shaderProgram->SetUniformVec3("directionalLight.specular", directionalLight.specular);

			//int s = pointLights.size();
			//glm::vec3 pointLightPositions[5];
			for (int i = 0; i < pointLights.size(); i++)
			{
				//pointLightPositions[i] = pointLights[i]->position;
				shaderProgram->SetUniformVec3((std::string("pointLight[") + std::to_string(i) + std::string("].position")), pointLights[i]->position);
				//std::cout << (std::string("pointLight[") + std::to_string(i) + std::string("].position")) << std::endl;
				shaderProgram->SetUniformVec3(std::string("pointLight[") + std::to_string(i) + "].ambient", 0.05f, 0.05f, 0.05f);
				//std::cout << (std::string("pointLight[") + std::to_string(i) + "].ambient") << std::endl;
				shaderProgram->SetUniformVec3(std::string("pointLight[") + std::to_string(i) + "].diffuse", 0.4f, 0.4f, 0.4f);
				shaderProgram->SetUniformVec3(std::string("pointLight[") + std::to_string(i) + "].specular", 0.5f, 0.5f, 0.5f);
				shaderProgram->SetUniformVec3(std::string("pointLight[") + std::to_string(i) + "].attenuation", 1.0f, 0.09f, 0.032f);
			}
			/*shaderProgram->SetUniformVec3("pointLight[0].position", 50.0f, 1.0f, -0.3f);
			shaderProgram->SetUniformVec3("pointLight[0].ambient", 0.05f, 0.05f, 0.05f);
			shaderProgram->SetUniformVec3("pointLight[0].diffuse", 0.4f, 0.4f, 0.4f);
			shaderProgram->SetUniformVec3("pointLight[0].specular", 0.5f, 0.5f, 0.5f);
			shaderProgram->SetUniformVec3("pointLight[0].attenuation", 1.0f, 0.09f, 0.032f);*/


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
		//std::cout << sceneObjects.size() << std::endl;
		for (auto& sceneObject : sceneObjects) 
		{
			sceneObject->Draw();
		}
		skybox.Draw(camera.GetViewMatrix(), projection);
	}

private:
	std::vector<Mesh> meshes;
};


#endif
