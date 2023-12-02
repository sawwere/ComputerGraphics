#include "Lab12.h"
#include <exception>
#define _USE_MATH_DEFINES
#include "math.h"
#include <array>
#include <initializer_list>
#define deg2rad M_PI /180.0

namespace taskname_1
{
    GLuint Program;
    GLint Attrib_vertex;
    GLint Attrib_color;
    GLint Unif_xmove;
    GLint Unif_ymove;
    GLuint VBO_position;
    GLuint VBO_color;
    struct Vertex
    {
        GLfloat x;
        GLfloat y;
        GLfloat z;
    };


    const char* VertexShaderSource = R"(
    #version 330 core
    in vec3 coord;
    in vec4 color;

    uniform float x_move;
    uniform float y_move;
    
    out vec4 vert_color;

    void main() {
        vec3 position = vec3(coord) + vec3(x_move, y_move, 0);
        gl_Position = vec4(position[0], position[1], 0.0, 1.0);
        vert_color = color;
    }
)";

    // Исходный код фрагментного шейдера
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

    float moveX = 0;
    float moveY = 0;

    void moveShape(float moveXinc, float moveYinc) {
        moveX += moveXinc;
        moveY += moveYinc;
    }



    // Функция печати лога шейдера
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

    const int circleVertexCount = 360;

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

        // Вершины тетраэдра
        Vertex triangle[] = {
           { 0.2, 0.45, -0.5 }, { -0.6, 0, -0.5 }, { 0, 0, 0.5 },
           { -0.6, 0, -0.5 }, { 0, 0, 0.5 }, { 0.2, -0.45, -0.5 },
           { 0, 0, 0.5 }, { 0.2, 0.45, -0.5 }, { 0.2, -0.45, -0.5 },

        };
        float colors[9][4] = {
            { 0.0, 0.0, 1.0, 1.0 }, { 1.0, 0.0, 0.0, 1.0 }, { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 0.0, 0.0, 1.0 }, { 1.0, 1.0, 1.0, 1.0 }, { 0.0, 1.0, 0.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 }, { 0.0, 0.0, 1.0, 1.0 }, { 0.0, 1.0, 0.0, 1.0 },
        };

        // Передаем вершины в буфер
        glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
        glBufferData(GL_ARRAY_BUFFER, sizeof(triangle), triangle, GL_STATIC_DRAW);
        glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
        glBufferData(GL_ARRAY_BUFFER, sizeof(colors), colors, GL_STATIC_DRAW);
        //checkOpenGLerror();
    }


    void InitShader() {
        // Создаем вершинный шейдер
        GLuint vShader = glCreateShader(GL_VERTEX_SHADER);
        // Передаем исходный код
        glShaderSource(vShader, 1, &VertexShaderSource, NULL);
        // Компилируем шейдер
        glCompileShader(vShader);
        std::cout << "vertex shader \n";
        ShaderLog(vShader);

        // Создаем фрагментный шейдер
        GLuint fShader = glCreateShader(GL_FRAGMENT_SHADER);
        // Передаем исходный код
        glShaderSource(fShader, 1, &FragShaderSource, NULL);
        // Компилируем шейдер
        glCompileShader(fShader);
        std::cout << "fragment shader \n";
        ShaderLog(fShader);

        // Создаем программу и прикрепляем шейдеры к ней
        Program = glCreateProgram();
        glAttachShader(Program, vShader);
        glAttachShader(Program, fShader);

        // Линкуем шейдерную программу
        glLinkProgram(Program);
        // Проверяем статус сборки
        int link_ok;
        glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
        if (!link_ok)
        {
            std::cout << "error attach shaders \n";
            return;
        }

        // Вытягиваем ID атрибута вершин из собранной программы
        Attrib_vertex = glGetAttribLocation(Program, "coord");
        if (Attrib_vertex == -1)
        {
            std::cout << "could not bind attrib coord" << std::endl;
            return;
        }

        // Вытягиваем ID атрибута цвета
        Attrib_color = glGetAttribLocation(Program, "color");
        if (Attrib_color == -1)
        {
            std::cout << "could not bind attrib color" << std::endl;
            return;
        }

        // Вытягиваем ID юниформ
        const char* unif_name = "x_move";
        Unif_xmove = glGetUniformLocation(Program, unif_name);
        if (Unif_xmove == -1)
        {
            std::cout << "could not bind uniform " << unif_name << std::endl;
            return;
        }

        unif_name = "y_move";
        Unif_ymove = glGetUniformLocation(Program, unif_name);
        if (Unif_ymove == -1)
        {
            std::cout << "could not bind uniform " << unif_name << std::endl;
            return;
        }

        //checkOpenGLerror();
    }

    void Init() {
        InitShader();
        InitVBO();
    }


    void Draw() {
        // Устанавливаем шейдерную программу текущей
        glUseProgram(Program);

        glUniform1f(Unif_xmove, moveX);
        glUniform1f(Unif_ymove, moveY);


        // Включаем массивы атрибутов
        glEnableVertexAttribArray(Attrib_vertex);
        glEnableVertexAttribArray(Attrib_color);

        // Подключаем VBO_position
        glBindBuffer(GL_ARRAY_BUFFER, VBO_position);
        glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);

        // Подключаем VBO_color
        glBindBuffer(GL_ARRAY_BUFFER, VBO_color);
        glVertexAttribPointer(Attrib_color, 4, GL_FLOAT, GL_FALSE, 0, 0);

        // Отключаем VBO
        glBindBuffer(GL_ARRAY_BUFFER, 0);

        // Передаем данные на видеокарту(рисуем)
        glDrawArrays(GL_TRIANGLES, 0, 9);

        // Отключаем массивы атрибутов
        glDisableVertexAttribArray(Attrib_vertex);
        glDisableVertexAttribArray(Attrib_color);

        glUseProgram(0);
        //checkOpenGLerror();
    }


    // Освобождение шейдеров
    void ReleaseShader() {
        // Передавая ноль, мы отключаем шейдрную программу
        glUseProgram(0);
        // Удаляем шейдерную программу
        glDeleteProgram(Program);
    }

    // Освобождение буфера
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

void task1() {
    sf::Window window(sf::VideoMode(600, 600), "My OpenGL window", sf::Style::Default, sf::ContextSettings(24));
    window.setVerticalSyncEnabled(true);

    window.setActive(true);
    glewInit();

    taskname_1::Init();

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
                case (sf::Keyboard::W): taskname_1::moveShape(0, 0.1); break;
                case (sf::Keyboard::S): taskname_1::moveShape(0, -0.1); break;
                case (sf::Keyboard::A): taskname_1::moveShape(-0.1, 0); break;
                case (sf::Keyboard::D): taskname_1::moveShape(0.1, 0); break;
                default: break;
                }
            }
        }
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        taskname_1::Draw();
        window.display();
    }
}