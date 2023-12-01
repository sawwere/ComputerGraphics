#include "Lab12.h"
#include <exception>




GLint unifRotationX;
GLint unifRotationY;
GLint unifRotationZ;
GLuint unifTexture1;
GLuint unifTexture2;
GLuint unifScaler;


void InitUniforms(ShaderProgram shader)
{
    unifRotationX = glGetUniformLocation(shader.ID, "rotationX");
    if (unifRotationX == -1)
    {
        std::cout << "could not bind uniform rotationX" << std::endl;
        return;
    }
    unifRotationY = glGetUniformLocation(shader.ID, "rotationY");
    if (unifRotationY == -1)
    {
        std::cout << "could not bind uniform rotationY" << std::endl;
        return;
    }
    unifRotationZ = glGetUniformLocation(shader.ID, "rotationZ");
    if (unifRotationZ == -1)
    {
        std::cout << "could not bind uniform rotationZ" << std::endl;
        return;
    }
    unifTexture1 = glGetUniformLocation(shader.ID, "texture1");
    if (unifTexture1 == -1)
    {
        std::cout << "could not bind uniform ourTexture1" << std::endl;
        return;
    }
    unifTexture2 = glGetUniformLocation(shader.ID, "texture2");
    if (unifTexture2 == -1)
    {
        std::cout << "could not bind uniform ourTexture2" << std::endl;
        return;
    }
    unifScaler = glGetUniformLocation(shader.ID, "scaler");
    if (unifScaler == -1)
    {
        std::cout << "could not bind uniform scaler" << std::endl;
        return;
    }
}

void task3()
{
    sf::Window window(sf::VideoMode(700, 700), "Lab12", sf::Style::Default, sf::ContextSettings(24));
    //SetIcon(window);
    window.setVerticalSyncEnabled(true);
    window.setActive(true);

    // Инициализация
    glewInit();
    glEnable(GL_DEPTH_TEST);
    ShaderProgram ourShader = ShaderProgram("Shaders//mix_textures.vs", "Shaders//mix_textures.frag");

    
    float vertices[] = {
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
         0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
         0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
         0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
         0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
         0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
    };
    GLuint VBO, VAO;
    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO);
    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

    // position attribute
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);
    // texture coord attribute
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(3 * sizeof(float)));
    glEnableVertexAttribArray(1);

    sf::Texture texture1;
    texture1.loadFromFile("Images//texture1.png");
    sf::Texture texture2;
    texture2.loadFromFile("Images//texture2.png");
    InitUniforms(ourShader);
    glBindVertexArray(0);

    float rotationX = 0.0;
    float rotationY = 0.0;
    float rotationZ = 0.0;

    float scaler = 0.0f;

    bool running = true;
    while (running)
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed)
            {
                running = false;
            }
            else if (event.type == sf::Event::Resized)
            {
                glViewport(0, 0, event.size.width, event.size.height);
            }
            else if (event.type == sf::Event::KeyPressed) 
            {
                switch (event.key.code) {
                case (sf::Keyboard::Q): rotationX += DEGREE_TO_RADIAN; break;
                case (sf::Keyboard::E): rotationX -= DEGREE_TO_RADIAN; break;
                case (sf::Keyboard::A): rotationY += DEGREE_TO_RADIAN; break;
                case (sf::Keyboard::D): rotationY -= DEGREE_TO_RADIAN; break;
                case (sf::Keyboard::Z): rotationZ += DEGREE_TO_RADIAN; break;
                case (sf::Keyboard::C): rotationZ -= DEGREE_TO_RADIAN; break;
                case (sf::Keyboard::T): scaler = MixTexture(scaler, -0.02f); break;
                case (sf::Keyboard::Y): scaler = MixTexture(scaler, +0.02f); break;
                default: break;
                }
            }
        }

        glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        // Activate shader
        ourShader.use();
        glUniform1f(unifRotationX, rotationX);
        glUniform1f(unifRotationY, rotationY);
        glUniform1f(unifRotationZ, rotationZ);

        glUniform1f(unifScaler, scaler);

        glActiveTexture(GL_TEXTURE0);
        sf::Texture::bind(&texture1);
        glUniform1i(unifTexture1, 0);
        glActiveTexture(GL_TEXTURE1);
        sf::Texture::bind(&texture2);
        glUniform1i(unifTexture2, 1);

        //// Draw container
        glBindVertexArray(VAO);
        glDrawArrays(GL_TRIANGLES, 0, sizeof(vertices));
        glBindVertexArray(0);
        sf::Texture::bind(NULL);
        glUseProgram(0);
        window.display();
    }

    //Release();
    window.close();
}