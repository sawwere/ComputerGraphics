#include "Lab11.h"
#include <exception>


// ID shader program
GLuint Program;
// ID attribure
GLint Attrib_vertex;
// ID Vertex Buffer Object
GLuint VBO;
// ID uniform
GLint Uniform_color;

const char* VertexShaderSource = R"(
#version 330 core
    layout (location = 0) in vec2 coord;
	layout (location = 1) in vec4 vColor;

	out vec4 color;
    void main() 
    {
        gl_Position = vec4(coord, 0.0, 1.0);
        color = vColor;
    }
)";

const char* FragShaderSource = R"(
#version 330 core
    out vec4 color;
    void main() 
    {
        color = vec4(0.25, 0.5, 0.7, 1);
    }
)";

const char* FragShaderSource_uniform = R"(
#version 330 core
    out vec4 color;
    uniform vec4 uniform_color;
    void main() 
    {
        color = uniform_color;
    }
)";

const char* FragShaderSourceGradient = R"(
#version 330 core
	out vec4 FragColor;

	in vec4 color;

	void main()
	{
		FragColor = vec4(color);
	}
)";

void CheckOpenGLerror()
{
    GLenum err;
    while ((err = glGetError()) != GL_NO_ERROR)
    {
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

void InitShader(int taskCode)
{
    GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vShader, 1, &VertexShaderSource, NULL);
    glCompileShader(vShader);
    std::cout << "Vertex shader compiled\n";
    ShaderLog(vShader);

    GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
    switch (taskCode)
    {
    case 2:
        glShaderSource(fShader, 1, &FragShaderSource, NULL);
        glCompileShader(fShader);
        std::cout << "Fragment shader compiled\n";
        break;
    case 3:
        glShaderSource(fShader, 1, &FragShaderSource_uniform, NULL);
        glCompileShader(fShader);
        std::cout << "Uniform Fragment shader compiled\n";
        break;
    case 4:
        glShaderSource(fShader, 1, &FragShaderSourceGradient, NULL);
        glCompileShader(fShader);
        std::cout << "Uniform Fragment shader compiled\n";
        break;
    default:
        throw std::exception();
    }
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

    if (taskCode == 3)
    {
        //bind uniform color
        //can't set uniform_color here because we need to use program first
        Uniform_color = glGetUniformLocation(Program, "uniform_color");
        
        if (Uniform_color == -1)
        {
            std::cout << "could not bind COLOR " << std::endl;
            return;
        }
        
    }
    CheckOpenGLerror();
}

void InitVBO(int figCode)
{
    glGenBuffers(1, &VBO);
    Vertex square[4] = {
    { 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f },
    { 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f},
    { 0.0f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f },
    { -1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f }
    };

    // TODO
    // Пятиугольник
    /*Vertex pentagon[5] = {
    {0.0f,1.0f},
    {0.9510565f, 0.309017f},
    {0.58778f, -0.809017f},
    {-0.58778f, -0.809017f },
    {-0.9510565f, 0.309017f},
    };*/
    //инвертированный у
    Vertex pentagon[5] = {
    {0.0f,-1.0f, 1.0f, 0.0f, 0.0f, 1.0f},
    {0.9510565f, -0.309017f,0.0f, 1.0f, 0.0f, 1.0f},
    {0.58778f, 0.809017f,0.0f, 0.0f, 1.0f, 1.0f},
    {-0.58778f, 0.809017f,1.0f, 1.0f, 0.0f, 1.0f },
    {-0.9510565f, -0.309017f,1.0f, 0.0f, 1.0f, 1.0f}
    };
    //округленный
    /*Vertex pentagon[5] = {
    {0.0f,1.0f},
    {0.9510565f, 0.31f},
    {0.6f, -0.81f},
    {-0.6f, -0.6f },
    {-0.9510565f, 0.31f},
    };*/
    
     // TODO
     // Веер - набор из треугольников
    Vertex fan[6] = { {0.0f,0.0f,0.0f, 1.0f, 1.0f, 1.0f},
        {0.7f,0.0f, 1.0f, 0.0f, 0.0f, 1.0f},
        {0.536f,0.45f, 0.0f, 1.0f, 0.0f, 1.0f},
        {0.122f,0.689f, 0.0f, 0.0f, 1.0f, 1.0f},
        {-0.35f,0.606f,1.0f, 1.0f, 0.0f, 1.0f},
        {-0.658f,0.239f, 1.0f, 0.0f, 1.0f, 1.0f}
    };

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    switch (figCode)
    {
    case 1:
        glBufferData(GL_ARRAY_BUFFER, sizeof(square), square, GL_STATIC_DRAW);
        break;
    case 2:
        glBufferData(GL_ARRAY_BUFFER, sizeof(pentagon), pentagon, GL_STATIC_DRAW);//TODO
        break;
    case 3:
        glBufferData(GL_ARRAY_BUFFER, sizeof(fan), fan, GL_STATIC_DRAW);//TODO
        break;
    default:
        throw std::exception();;
    }
    //glBufferData(GL_ARRAY_BUFFER, sizeof(square), square, GL_STATIC_DRAW);
    glBindBuffer(GL_ARRAY_BUFFER, NULL);
    CheckOpenGLerror();
}

void Init(int taskCode, int figCode)
{
    InitShader(taskCode);
    InitVBO(figCode);
}



void Draw(int taskCode, int figCode)
{
    glUseProgram(Program);

    //set uniform color in case we are dealing with third task
    if (taskCode == 3)
        glUniform4f(Uniform_color, 0.75, 0.25, 0.75, 1);

    glEnableVertexAttribArray(Attrib_vertex);
    glEnableVertexAttribArray(1); // Для цвета
    glBindBuffer(GL_ARRAY_BUFFER, VBO);

    glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 6 * sizeof(float), 0);
    glVertexAttribPointer(1, 4, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(2 * sizeof(float)));
    glBindBuffer(GL_ARRAY_BUFFER, 0);


    switch (figCode)
    {
    case 1:
        glDrawArrays(GL_QUADS, 0, 4);
        break;
    case 2:
        glDrawArrays(GL_POLYGON, 0, 5);
        break;
    case 3:
        glDrawArrays(GL_TRIANGLE_FAN, 0, 6);
        break;
    default:
        throw std::exception();
    }

    glDisableVertexAttribArray(Attrib_vertex);
    glUseProgram(0);
    CheckOpenGLerror();
}

void Release() 
{
    ReleaseShader();
    ReleaseVBO();
}

void ReleaseVBO() 
{
    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glDeleteBuffers(1, &VBO);
}

void ReleaseShader() 
{
    glUseProgram(0);
    glDeleteProgram(Program);
}



void SetIcon(sf::Window& wnd)
{
    sf::Image image;
    image.loadFromFile("..//Tools//Resources//icon.png");
    wnd.setIcon(image.getSize().x, image.getSize().y, image.getPixelsPtr());
}