#ifndef INSTANCED_MESH_H
#define INSTANCED_MESH_H


#include "Mesh.h"

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <chrono>
#include <ctime>  

class InstansedMesh : public Mesh
{
	
public:
	GLuint buffer;
	int count;

	InstansedMesh(const std::string& filePath, int count): Mesh(filePath)
	{
		this->count = count;

        InitializeBuffers();
	}


	void Draw(ShaderProgram& shader)
	{
		shader.Use();

		glBindVertexArray(VAO);
		glDrawElementsInstanced(GL_TRIANGLES, static_cast<unsigned int>(indices.size()), GL_UNSIGNED_INT, 0, count);
		glBindVertexArray(0);

		glUseProgram(0);
	}
private:
    void InitializeBuffers()
    {
        glm::mat4* modelMatrices;
        modelMatrices = new glm::mat4[count];
        srand(std::chrono::system_clock::to_time_t(std::chrono::system_clock::now()));

        float radius = 100.0;
        float offset = 15.0f;
        

        glBindVertexArray(0);
    }
};

#endif
