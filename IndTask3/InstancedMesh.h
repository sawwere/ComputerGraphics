#ifndef INSTANCED_MESH_H
#define INSTANCED_MESH_H


#include "Mesh.h"

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>


class InstansedMesh : public Mesh
{
    GLuint buffer;
    GLuint count;
    glm::mat4* modelMatrices;
public:
	

	InstansedMesh(const char* filePath, const char* texturePath, int count, glm::mat4* matrices) : Mesh(filePath, texturePath)
	{
		this->count = count;

        InitializeBuffers(matrices);
	}


	virtual void Draw(ShaderProgram& shader) const override
	{
        shader.Use();
        material->Use(&shader);
        {
            glBindVertexArray(VAO);
            glDrawElementsInstanced(GL_TRIANGLES, static_cast<GLuint>(indices.size()), GL_UNSIGNED_INT, 0, count);
            glBindVertexArray(0);
            sf::Texture::bind(NULL);

        }
        glUseProgram(0);
	}

    ~InstansedMesh()
    {
        delete[] modelMatrices;
        modelMatrices = nullptr;
        Mesh::~Mesh();
    }
private:
    void InitializeBuffers(glm::mat4* matrices)
    {
		Mesh::InitializeBuffers();
        
        modelMatrices = matrices;

		glGenBuffers(1, &buffer);
		glBindBuffer(GL_ARRAY_BUFFER, buffer);
		glBufferData(GL_ARRAY_BUFFER, count * sizeof(glm::mat4), &modelMatrices[0], GL_STATIC_DRAW);

        glBindVertexArray(VAO);
        // set attribute pointers for matrix (4 times vec4)
        glEnableVertexAttribArray(3);
        glVertexAttribPointer(3, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)0);
        glEnableVertexAttribArray(4);
        glVertexAttribPointer(4, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(sizeof(glm::vec4)));
        glEnableVertexAttribArray(5);
        glVertexAttribPointer(5, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(2 * sizeof(glm::vec4)));
        glEnableVertexAttribArray(6);
        glVertexAttribPointer(6, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(3 * sizeof(glm::vec4)));

        glVertexAttribDivisor(3, 1);
        glVertexAttribDivisor(4, 1);
        glVertexAttribDivisor(5, 1);
        glVertexAttribDivisor(6, 1);

        glBindVertexArray(0);
    }
};

#endif
