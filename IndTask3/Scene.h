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
	SpotLight spotLight;
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
	ShaderProgram snowProgram = ShaderProgram("Shaders//snow.vs", "Shaders//snow.frag");
	GLuint VBO;
	float getDeltaTime()
	{
		return deltaTime.asSeconds();
	}

	const int numParticles = 10000;
	glm::vec3 triangle[10000];
	Scene() 
	{
		skybox = Skybox();
		camera = Camera({0.0f, 80.0f, 120.0f});
		pointLights = std::vector<PointLight*>();
		snowProgram = ShaderProgram("Shaders//snow.vs", "Shaders//snow.frag");


		for (int i = 0; i < numParticles; i++) 
		{
			triangle[i] = { rand() % 200 - 100, rand() % 130, rand() % 200 - 100 };
		}
		std::cout << sizeof(triangle) << std::endl;
		glGenBuffers(1, &VBO);
		glBindBuffer(GL_ARRAY_BUFFER, VBO);
		glBufferData(GL_ARRAY_BUFFER, sizeof(triangle), triangle, GL_STATIC_DRAW);
		glBindBuffer(GL_ARRAY_BUFFER, NULL);
		shaders.push_back(&snowProgram);
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

	bool RemovePointLight(PointLight* pl)
	{
		return (std::remove(pointLights.begin(), pointLights.end(), pl) == pointLights.end());
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

			shaderProgram->SetUniformVec3("directionalLight.direction", directionalLight.direction );
			shaderProgram->SetUniformVec3("directionalLight.ambient", directionalLight.ambient);
			shaderProgram->SetUniformVec3("directionalLight.diffuse", directionalLight.diffuse);
			shaderProgram->SetUniformVec3("directionalLight.specular", directionalLight.specular);

			for (int i = 0; i < pointLights.size(); i++)
			{
				shaderProgram->SetUniformVec3((std::string("pointLight[") + std::to_string(i) + "].position"), pointLights[i]->position);
				shaderProgram->SetUniformVec3(std::string("pointLight[") + std::to_string(i) + "].ambient", pointLights[i]->ambient);
				shaderProgram->SetUniformVec3(std::string("pointLight[") + std::to_string(i) + "].diffuse", pointLights[i]->diffuse);
				shaderProgram->SetUniformVec3(std::string("pointLight[") + std::to_string(i) + "].specular", pointLights[i]->specular);
				shaderProgram->SetUniformVec3(std::string("pointLight[") + std::to_string(i) + "].attenuation", pointLights[i]->attenuation);
			}

			shaderProgram->SetUniformVec3("spotLight.position", camera.Position);
			shaderProgram->SetUniformVec3("spotLight.direction", camera.Front);

			shaderProgram->SetUniformVec3("spotLight.ambient", spotLight.ambient);
			shaderProgram->SetUniformVec3("spotLight.diffuse", spotLight.diffuse);
			shaderProgram->SetUniformVec3("spotLight.specular", spotLight.specular);
			shaderProgram->SetUniformVec3("spotLight.attenuation", spotLight.attenuation);

			shaderProgram->SetUniformFloat("spotLight.innerAngle", 5.0f);
			shaderProgram->SetUniformFloat("spotLight.outerAngle", 9.0f);

			glUseProgram(0);
		}

		for (auto& sceneObject : sceneObjects) 
		{
			sceneObject->Draw();
		}
		skybox.Draw(camera.GetViewMatrix(), projection);

		for (int i = 0; i < numParticles; i++)
		{
			snowProgram.Use();
			glBindBuffer(GL_ARRAY_BUFFER, VBO);
			glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(glm::vec3), (GLvoid*)0);
			glEnableVertexAttribArray(0);
			glDrawArrays(GL_POINTS, 0, numParticles);
			glDisableVertexAttribArray(0);
			glUseProgram(0);
		}
		
	}


private:
};


#endif
