#include "Lab10.h"

// ID шейдерной программы
GLuint Program;
// ID атрибута
GLint Attrib_vertex;
// ID Vertex Buffer Object
GLuint VBO;

// Исходный код вершинного шейдера
const char* VertexShaderSource = R"(
#version 330 core
in vec2 coord;
void main() {
gl_Position = vec4(coord, 0.0, 1.0);
}
)";

// Исходный код фрагментного шейдера
const char* FragShaderSource = R"(
#version 330 core
out vec4 color;
void main() {
color = vec4(0, 1, 0, 1);
}
)";

void CheckOpenGLerror()
{
    GLenum err;
    while ((err = glGetError()) != GL_NO_ERROR)
    {
        // Process/log the error.
        std::cout << err << std::endl;
    }
}

void ShaderLog(unsigned int shader)
{
    int infologLen = 0;
    glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
    if (infologLen > 1)
    {
        int charsWritten = 0;
        std::vector<char> infoLog(infologLen);
        glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog.data());
        std::cout << "InfoLog: " << infoLog.data() << std::endl;
    }
}

void InitShader()
{
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &VertexShaderSource, NULL);
    glCompileShader(vShader);
    std::cout << "Vertex shader compiled\n";
    ShaderLog(vShader);

    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fShader, 1, &FragShaderSource, NULL);
    glCompileShader(fShader);
    std::cout << "Fragment shader compiled\n";
    ShaderLog(fShader);

    Program = glCreateProgram();
    glAttachShader(Program, vShader);
    glAttachShader(Program, fShader);
    glLinkProgram(Program);
    int link_ok;
    glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
    if (!link_ok) {
        std::cout << "error attach shaders \n";
        return;
    }

    const char* attr_name = "coord";
    Attrib_vertex = glGetAttribLocation(Program, attr_name);
    if (Attrib_vertex == -1)
    {
        std::cout << "could not bind attrib " << attr_name << std::endl;
        return;
    }
    CheckOpenGLerror();
}

void InitVBO()
{
    glGenBuffers(1, &VBO);
    Vertex triangle[3] = {
        { -1.0f, -1.0f },
        { 0.0f,  1.0f },
        { 1.0f, -1.0f }
    };
    // Передаем вершины в буфер
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(triangle), triangle, GL_STATIC_DRAW);
    glBindBuffer(GL_ARRAY_BUFFER, NULL);
    CheckOpenGLerror();
}

void Init()
{
    InitShader();
    InitVBO();
}



void Draw()
{
    glUseProgram(Program);
    glEnableVertexAttribArray(Attrib_vertex);
    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 0, 0);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glDrawArrays(GL_TRIANGLES, 0, 3);
    glDisableVertexAttribArray(Attrib_vertex);
    glUseProgram(0);
    CheckOpenGLerror();
}

void Close()
{
    glDeleteProgram(Program);
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glDeleteBuffers(1, &VBO);
    CheckOpenGLerror();
}

void SetIcon(sf::Window& wnd)
{
    sf::Image image;
    image.loadFromFile("icon.png");
    wnd.setIcon(image.getSize().x, image.getSize().y, image.getPixelsPtr());
}