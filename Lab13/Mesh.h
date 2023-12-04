#ifndef MESH_H
#define MESH_H

#include <SFML/Graphics.hpp>

#include <vector>
#include <string>

#include "ShaderProgram.h"

using std::string;


struct Vertex 
{
    glm::vec3 Position;
    glm::vec3 Normal;
    glm::vec2 TexCoords;
};

struct Texture 
{
    GLuint id;
    string type;
    sf::Texture texture;
};

class Mesh 
{
protected:
    //  render data
    GLuint VAO, VBO, EBO;
public:
    // mesh data
    std::vector<Vertex> vertices;
    std::vector<GLuint> indices;
    std::vector<Texture> textures;
    

    //TODO конструктор из obj файла
    Mesh(const char* filePath)
    {
        //собираем вершиы, нормали и текструры и пихаем во второй конструктор
    }

    Mesh(const std::vector<Vertex>& vertices, const std::vector<GLuint>& indices, const std::vector<Texture>& textures)
    {
        this->vertices = vertices;
        this->indices = indices;
        this->textures = textures;

        InitializeBuffers();
    }

    virtual void Draw(ShaderProgram& shader) const
    {
        shader.Use();
        for (unsigned int i = 0; i < textures.size(); i++)
        {
            glActiveTexture(GL_TEXTURE0 + i);

            shader.SetUniformInt("texture1", i);
            sf::Texture::bind(&textures[i].texture);
            glBindVertexArray(VAO);
            glDrawElements(GL_TRIANGLES, indices.size(), GL_UNSIGNED_INT, 0);
            glBindVertexArray(0);
            sf::Texture::bind(NULL);
            
        }
        glUseProgram(0);
    }
protected:
    virtual void InitializeBuffers()
    {
        glGenVertexArrays(1, &VAO);
        glGenBuffers(1, &VBO);
        glGenBuffers(1, &EBO);

        glBindVertexArray(VAO);

        glBindBuffer(GL_ARRAY_BUFFER, VBO);
        glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(Vertex), &vertices[0], GL_STATIC_DRAW);

        glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
        glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(GLuint) * indices.size(), &indices[0], GL_STATIC_DRAW);

        // Position
        glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (GLvoid*)0);
        glEnableVertexAttribArray(0);
        // Normal
        glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (GLvoid*)(3 * sizeof(GLfloat)));
        glEnableVertexAttribArray(1);
        // Texture
        glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, sizeof(Vertex), (GLvoid*)(6 * sizeof(GLfloat)));
        glEnableVertexAttribArray(2);

        glBindVertexArray(0); // Unbind VAO
    }

    void Release()
    {
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        glDeleteBuffers(1, &VBO);
        glDeleteVertexArrays(1, &VAO);
    }
};

#endif