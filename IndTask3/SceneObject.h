#ifndef SCENE_OBJECT_H
#define SCENE_OBJECT_H

#include <glm/glm.hpp>

#include "ShaderProgram.h"
#include "Mesh.h"


class SceneObject
{
protected:
	Mesh* mesh;
	ShaderProgram* shaderProgram;

	glm::vec3 forward{ 0.0f, 0.0f, 1.0f };
	glm::vec3 right{ 1.0f, 0.0f, 0.0f };
	glm::vec3 up{ 0.0f, 1.0f, 0.0f };
public:
	glm::vec3 position{ 0.f, 0.f, 0.f };
	glm::vec3 rotation{ 0.f, 0.f, 0.f };
	glm::vec3 scale{ 1.f, 1.f, 1.f };

	SceneObject()
	{
	}

	SceneObject(Mesh* mesh, ShaderProgram* sp)
	{
		this->mesh = mesh;
		this->shaderProgram = sp;
	}
	
	void Draw()
	{
		glm::mat4 model = glm::translate(glm::mat4(1.f), position)
			* glm::rotate(glm::mat4(1.0f), rotation.x, glm::vec3(1.0f, 0.0f, 0.0f))
			* glm::rotate(glm::mat4(1.0f), rotation.y, glm::vec3(0.0f, 1.0f, 0.0f))
			* glm::rotate(glm::mat4(1.0f), rotation.z, glm::vec3(0.0f, 0.0f, 1.0f))

			* glm::scale(glm::mat4(1.f), scale);
		shaderProgram->Use();
		shaderProgram->SetUniformMat4("model", model);
		mesh->Draw(*shaderProgram);
		glUseProgram(0);
	}

};

class Player : public SceneObject
{
public:
	GLfloat MovementSpeed = 35.5f;
	GLfloat RotationSpeed = 15.5f;
	Player(Mesh* mesh, ShaderProgram* sp): SceneObject(mesh, sp)
	{
	}

	void OnKeyPress(sf::Keyboard::Key key, float deltaTime)
	{
		float velocity = MovementSpeed * deltaTime;
		float rotationVelocity = RotationSpeed * deltaTime;
		switch (key) 
		{
			case (sf::Keyboard::W):
				position -= forward * velocity;
				break;
			case (sf::Keyboard::S): position += forward * velocity; break;
			case (sf::Keyboard::A): 
				Yaw -= RotationSpeed * rotationVelocity;
				updateVectors();
				rotation.y += glm::radians(RotationSpeed * rotationVelocity);
				break;
			case (sf::Keyboard::D): 
				Yaw += RotationSpeed * rotationVelocity;
				rotation.y -= glm::radians(RotationSpeed * rotationVelocity);
				updateVectors();
				break;
			case (sf::Keyboard::R): position += up * velocity; break;
			case (sf::Keyboard::F): position -= up * velocity; break;
			case (sf::Keyboard::V): std::cout << "veiw changed" <<std::endl; break;
			default: break;
		}
	}
private:
	float Yaw = 90.0f;
	void updateVectors()
	{
		glm::vec3 front;
		front.x = cos(glm::radians(Yaw));
		front.y = 0.0f;
		front.z = sin(glm::radians(Yaw));
		this->forward = glm::normalize(front);
		right = glm::normalize(glm::cross(forward, glm::vec3(0.0f, 1.0f, 0.0f)));
	}
};

#endif
