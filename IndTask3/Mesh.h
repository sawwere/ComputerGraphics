#ifndef MESH_H
#define MESH_H

#include <SFML/Graphics.hpp>

#include <initializer_list>
#include <array>
#include <vector>
#include <string>

#include "ShaderProgram.h"
#include "Material.h"

using std::string;

std::vector<string> Split(const string& str, char separator) 
{
	std::stringstream ss(str);
	std::string word;
	std::vector<std::string> elems;
	while (std::getline(ss, word, separator))
	{
		if (word.empty()) 
			continue;
		elems.push_back(word);
	}
	return elems;
}


struct Vertex 
{
    glm::vec3 Position;
    glm::vec3 Normal;
    glm::vec2 TexCoords;
	glm::vec3 Tangent;
};


class Mesh 
{
protected:
    GLuint VAO, VBO, EBO;
	std::vector<Vertex> vertices;
	std::vector<GLuint> indices;
public:
    
	Material* material;
    Texture texture;    
	bool useNormalMap = false;
	sf::Texture normalMap;

    //TODO indexing
    Mesh(const char* filePath, const char* texturePath, const char* normalMap = nullptr)
    {
		try 
		{
			std::ifstream reader(filePath);

			if (!reader.is_open()) 
			{
				throw std::exception("File cannot be opened");
			}

			std::vector<glm::vec3> _vertices;
			std::vector<glm::vec3> _vn;
			std::vector<glm::vec2> _vt;
			

			std::vector <std::string> indexAccordance{};
			std::string line;
			while (std::getline(reader, line))
			{
				std::istringstream iss(line);
				std::string typeOfElement;
				iss >> typeOfElement;
				if (typeOfElement == "v") 
				{
					string word;
					glm::vec3 cv{};
					for (GLuint i = 0; i < 3; i++)
					{
						iss >> word;
						cv[i] = std::stof(word);
					}
					_vertices.push_back(cv);
				}
				else if (typeOfElement == "vn") 
				{
					string word;
					glm::vec3 cv{}; 
					for (GLuint i = 0; i < 3; i++)
					{
						iss >> word;
						cv[i] = std::stof(word);
					}
					_vn.push_back(cv);
				}
				else if (typeOfElement == "vt") 
				{
					string word;
					glm::vec2 cv{};
					for (GLuint i = 0; i < 2; i++)
					{
						iss >> word;
						cv[i] = std::stof(word);
					}
					_vt.push_back(cv);
				}
				else if (typeOfElement == "f") 
				{
					auto splitted = Split(line, ' ');
					auto first = Split(splitted[1], '/');
					for (int i = 2; i < splitted.size() - 1; i++)
					{
						auto cur = Split(splitted[i], '/');
						auto next = Split(splitted[i+1], '/');

						Vertex vertex0;
						int positionIndex = std::stoi(first[0]) - 1;
						vertex0.Position = glm::vec3(_vertices[positionIndex][0], _vertices[positionIndex][1], _vertices[positionIndex][2]);
						if (first.size() > 1)
						{
							int textureIndex = std::stoi(first[1]) - 1;
							vertex0.TexCoords = glm::vec2(_vt[textureIndex][0], _vt[textureIndex][1]);
							if (first.size() > 2)
							{
								int normalIndex = std::stoi(first[2]) - 1;
								vertex0.Normal = glm::vec3(_vn[normalIndex][0], _vn[normalIndex][1], _vn[normalIndex][2]);
							}
						}
						vertices.push_back(vertex0);
						indices.push_back(indices.size());

						Vertex vertex1;
						positionIndex = std::stoi(cur[0]) - 1;
						vertex1.Position = glm::vec3(_vertices[positionIndex][0], _vertices[positionIndex][1], _vertices[positionIndex][2]);
						if (cur.size() > 1)
						{
							int textureIndex = std::stoi(cur[1]) - 1;
							vertex1.TexCoords = glm::vec2(_vt[textureIndex][0], _vt[textureIndex][1]);
							if (cur.size() > 2)
							{
								int normalIndex = std::stoi(cur[2]) - 1;
								vertex1.Normal = glm::vec3(_vn[normalIndex][0], _vn[normalIndex][1], _vn[normalIndex][2]);
							}
						}
						vertices.push_back(vertex1);
						indices.push_back(indices.size());

						Vertex vertex2;
						positionIndex = std::stoi(next[0]) - 1;
						vertex2.Position = glm::vec3(_vertices[positionIndex][0], _vertices[positionIndex][1], _vertices[positionIndex][2]);
						if (next.size() > 1)
						{
							int textureIndex = std::stoi(next[1]) - 1;
							vertex2.TexCoords = glm::vec2(_vt[textureIndex][0], _vt[textureIndex][1]);
							if (next.size() > 2)
							{
								int normalIndex = std::stoi(next[2]) - 1;
								vertex2.Normal = glm::vec3(_vn[normalIndex][0], _vn[normalIndex][1], _vn[normalIndex][2]);
							}
						}
						vertices.push_back(vertex2);
						indices.push_back(indices.size());
					}
				}
				else 
				{
					continue;
				}
			}
			

		}
		catch (const std::exception& e)
		{
			std::cout << e.what() << std::endl;
		}
		std::cout << "Total vertices count: " << vertices.size() << std::endl;
		InitializeTexture(texturePath, normalMap);
		if (normalMap != nullptr)
		{
			useNormalMap = true;
			InitializeTangent(normalMap);
		}
		InitializeBuffers();
    }

    virtual void Draw(ShaderProgram& shader) const
    {
        shader.Use();
		material->Use(&shader);
		auto unifTexture1 = glGetUniformLocation(shader.ID, "material.diffuse");
		glUniform1i(unifTexture1, 0);
		if (useNormalMap)
		{
			glActiveTexture(GL_TEXTURE1);
			sf::Texture::bind(&normalMap);
			auto unifTexture2 = glGetUniformLocation(shader.ID, "normalMap");
			glUniform1i(unifTexture2, 1);
		}
		
        {
			glBindVertexArray(VAO);
			glDrawElements(GL_TRIANGLES, indices.size(), GL_UNSIGNED_INT, 0);
			glBindVertexArray(0);
            sf::Texture::bind(NULL);
            
        }
        glUseProgram(0);
    }

	~Mesh()
	{
		delete material;
		Release();
	}
protected:
	void InitializeTangent(const char* normalMap = nullptr)
	{
		for (unsigned int i = 0; i < indices.size(); i += 3) {
			Vertex& v0 = vertices[indices[i]];
			Vertex& v1 = vertices[indices[i + 1]];
			Vertex& v2 = vertices[indices[i + 2]];

			glm::vec3 Edge1 = v1.Position - v0.Position;
			glm::vec3 Edge2 = v2.Position - v0.Position;

			float DeltaU1 = v1.TexCoords.x - v0.TexCoords.x;
			float DeltaV1 = v1.TexCoords.y - v0.TexCoords.y;
			float DeltaU2 = v2.TexCoords.x - v0.TexCoords.x;
			float DeltaV2 = v2.TexCoords.y - v0.TexCoords.y;

			float f = 1.0f / (DeltaU1 * DeltaV2 - DeltaU2 * DeltaV1);

			glm::vec3 Tangent, Bitangent;

			Tangent.x = f * (DeltaV2 * Edge1.x - DeltaV1 * Edge2.x);
			Tangent.y = f * (DeltaV2 * Edge1.y - DeltaV1 * Edge2.y);
			Tangent.z = f * (DeltaV2 * Edge1.z - DeltaV1 * Edge2.z);

			Bitangent.x = f * (-DeltaU2 * Edge1.x - DeltaU1 * Edge2.x);
			Bitangent.y = f * (-DeltaU2 * Edge1.y - DeltaU1 * Edge2.y);
			Bitangent.z = f * (-DeltaU2 * Edge1.z - DeltaU1 * Edge2.z);

			v0.Tangent += Tangent;
			v1.Tangent += Tangent;
			v2.Tangent += Tangent;
		}

		for (unsigned int i = 0; i < vertices.size(); i++) 
		{
			vertices[i].Tangent = glm::normalize(vertices[i].Tangent);
		}
	}

	void InitializeTexture(const char* texturePath, const char* normalMapPath = nullptr)
	{
		material = new Material(texturePath);
		if (normalMapPath != nullptr)
		{
			normalMap.loadFromFile(normalMapPath);
		}
	}

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
		// Tangent
		glVertexAttribPointer(3, 3, GL_FLOAT, GL_FALSE, sizeof(Vertex), (GLvoid*)(8 * sizeof(GLfloat)));
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