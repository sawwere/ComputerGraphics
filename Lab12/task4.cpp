#include "Lab12.h"
#include <exception>

#define _USE_MATH_DEFINES
#include "math.h"

#include <array>
#include <initializer_list>

#define deg2rad M_PI /180.0
namespace taskname_4
{

    // ���������� � ����������������� ID
    // ID ��������� ���������
    GLuint Program;
    // ID �������� ������
    GLint Attrib_vertex;
    // ID �������� �����
    GLint Attrib_color;
    // ID ������� ���������� �����
    GLint Unif_xscale;

    GLint Unif_yscale;
    // ID VBO ������
    GLuint VBO_position;
    // ID VBO �����
    GLuint VBO_color;
    // �������
    struct Vertex
    {
        GLfloat x;
        GLfloat y;
    };

    void task4();

    // �������� ��� ���������� �������
    const char* VertexShaderSource = R"(
    #version 330 core
    in vec2 coord;
    in vec4 color;

    uniform float x_scale;
    uniform float y_scale;
    
    out vec4 vert_color;

    void main() {
        vec3 position = vec3(coord, 1.0) * mat3(x_scale,0,0,
                                                0,y_scale,0,
                                                0,0,1);
        gl_Position = vec4(position[0],position[1], 0.0, 1.0);
        vert_color = color;
    }
)";

    // �������� ��� ������������ �������
    const char* FragShaderSource = R"(
    #version 330 core
    in vec4 vert_color;

    out vec4 color;
    void main() {
        color = vert_color;
    }
)";


    void Init();
    void Draw();
    void Release();
    const int circleVertexCount = 360;
    float scaleX = 1.0;
    float scaleY = 1.0;

    void changeScale(float scaleXinc, float scaleYinc) {
        scaleX += scaleXinc;
        scaleY += scaleYinc;
    }



    void ShaderLog(unsigned int shader)
    {
        int infologLen = 0;
        int charsWritten = 0;
        char* infoLog;
        glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
        if (infologLen > 1)
        {
            infoLog = new char[infologLen];
            if (infoLog == NULL)
            {
                std::cout << "ERROR: Could not allocate InfoLog buffer" << std::endl;
                exit(1);
            }
            glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog);
            std::cout << "InfoLog: " << infoLog << "\n\n\n";
            delete[] infoLog;
        }
    }
    float bytify(float color)
    {
        return (1 / 100.0) * color;
    }
    std::array<float, 4> HSVtoRGB(float hue, float saturation = 100.0, float value = 100.0)
    {
        int sw = (int)floor(hue / 60) % 6;
        float vmin = ((100.0f - saturation) * value) / 100.0;
        float a = (value - vmin) * (((int)hue % 60) / 60.0);
        float vinc = vmin + a;
        float vdec = value - a;
        switch (sw)
        {
        case 0: return { bytify(value), bytify(vinc), bytify(vmin), 1.0 };
        case 1: return { bytify(vdec), bytify(value), bytify(vmin), 1.0 };
        case 2: return { bytify(vmin), bytify(value), bytify(vinc), 1.0 };
        case 3: return { bytify(vmin), bytify(vdec), bytify(value), 1.0 };
        case 4: return { bytify(vinc), bytify(vmin), bytify(value), 1.0 };
        case 5: return { bytify(value), bytify(vmin), bytify(vdec), 1.0 };
        }
        return { 0, 0, 0 , 0 };
    }
    void InitVBO()
    {
        glGenBuffers(1, &VBO_position);
        glGenBuffers(1, &VBO_color);
        // ���� ������������
        std::array<std::array<float, 4>, circleVertexCount * 3> colors = {};

        Vertex circle[circleVertexCount * 3] = {};
        for (int i = 0; i < circleVertexCount; i++) {
            circle[i * 3] = { 0.5f * (float)cos(i * (360.0 / circleVertexCount) * deg2rad), 0.5f * (float)sin(i * (360.0 / circleVertexCount) * deg2rad) };
            circle[i * 3 + 1] = { 0.5f * (float)cos((i + 1) * (360.0 / circleVertexCount) * deg2rad), 0.5f * (float)sin((i + 1) * (360.0 / circleVertexCount) * deg2rad) };
            circle[i * 3 + 2] = { 0.0f, 0.0f };
            colors[i * 3] = HSVtoRGB(i % 360);
            colors[i * 3 + 1] = HSVtoRGB((i + 1) % 360);
            colors[i * 3 + 2] = { 1.0, 1.0, 1.0, 1.0 };
        }

        // �������� ������� � �����
        glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
        glBufferData(GL_ARRAY_BUFFER, sizeof(circle), circle, GL_STATIC_DRAW);
        glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
        glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors.data(), GL_STATIC_DRAW);
    }
    void InitShader() {
        // ������� ��������� ������
        GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
        // �������� �������� ���
        glShaderSource(vShader, 1, &VertexShaderSource, NULL);
        // ����������� ������
        glCompileShader(vShader);
        std::cout << "vertex shader \n";
        ShaderLog(vShader);

        // ������� ����������� ������
        GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
        // �������� �������� ���
        glShaderSource(fShader, 1, &FragShaderSource, NULL);
        // ����������� ������
        glCompileShader(fShader);
        std::cout << "fragment shader \n";
        ShaderLog(fShader);

        // ������� ��������� � ����������� ������� � ���
        Program = glCreateProgram();
        glAttachShader(Program, vShader);
        glAttachShader(Program, fShader);

        // ������� ��������� ���������
        glLinkProgram(Program);
        // ��������� ������ ������
        int link_ok;
        glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
        if (!link_ok)
        {
            std::cout << "error attach shaders \n";
            return;
        }

        // ���������� ID �������� ������ �� ��������� ���������
        Attrib_vertex = glGetAttribLocation(Program, "coord");
        if (Attrib_vertex == -1)
        {
            std::cout << "could not bind attrib coord" << std::endl;
            return;
        }

        // ���������� ID �������� �����
        Attrib_color = glGetAttribLocation(Program, "color");
        if (Attrib_color == -1)
        {
            std::cout << "could not bind attrib color" << std::endl;
            return;
        }

        // ���������� ID �������
        const char* unif_name = "x_scale";
        Unif_xscale = glGetUniformLocation(Program, unif_name);
        if (Unif_xscale == -1)
        {
            std::cout << "could not bind uniform " << unif_name << std::endl;
            return;
        }

        unif_name = "y_scale";
        Unif_yscale = glGetUniformLocation(Program, unif_name);
        if (Unif_yscale == -1)
        {
            std::cout << "could not bind uniform " << unif_name << std::endl;
            return;
        }

    }
    void Init() {
        InitShader();
        InitVBO();
    }
    void Draw() {
        // ������������� ��������� ��������� �������
        glUseProgram(Program);

        glUniform1f(Unif_xscale, scaleX);
        glUniform1f(Unif_yscale, scaleY);


        // �������� ������� ���������
        glEnableVertexAttribArray(Attrib_vertex);
        glEnableVertexAttribArray(Attrib_color);

        // ���������� VBO_position
        glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
        glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 0, 0);

        // ���������� VBO_color
        glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
        glVertexAttribPointer(Attrib_color, 4, GL_FLOAT, GL_FALSE, 0, 0);

        // ��������� VBO
        glBindBuffer(GL_ARRAY_BUFFER, 0);

        // �������� ������ �� ����������(������)
        glDrawArrays(GL_TRIANGLES, 0, circleVertexCount * 3);

        // ��������� ������� ���������
        glDisableVertexAttribArray(Attrib_vertex);
        glDisableVertexAttribArray(Attrib_color);

        glUseProgram(0);
    }


    // ������������ ��������
    void ReleaseShader() {
        // ��������� ����, �� ��������� �������� ���������
        glUseProgram(0);
        // ������� ��������� ���������
        glDeleteProgram(Program);
    }

    // ������������ ������
    void ReleaseVBO()
    {
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        glDeleteBuffers(1, &VBO_position);
        glDeleteBuffers(1, &VBO_color);
    }

    void Release() {
        ReleaseShader();
        ReleaseVBO();
    }
}
void task4() {
    sf::Window window(sf::VideoMode(600, 600), "My OpenGL window", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);

    window.setActive(true);
    glewInit();

    taskname_4::Init();

    bool running = true;
    while (running) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) {
                running = false;
            }
            else if (event.type == sf::Event::Resized) {
                glViewport(0, 0, event.size.width, event.size.height);
            }
            else if (event.type == sf::Event::KeyPressed) {
                switch (event.key.code) {
                case (sf::Keyboard::W): taskname_4::changeScale(0, 0.1); break;
                case (sf::Keyboard::S): taskname_4::changeScale(0, -0.1); break;
                case (sf::Keyboard::A): taskname_4::changeScale(-0.1, 0); break;
                case (sf::Keyboard::D): taskname_4::changeScale(0.1, 0); break;
                default: break;
                }
            }
        }

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        taskname_4::Draw();
        window.display();
    }

}