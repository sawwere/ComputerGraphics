#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <glm/gtc/matrix_transform.hpp>

#include <iostream>

#include "ShaderProgram.h"
#include "Mesh.h"
#include "InstancedMesh.h"
#include "SceneObject.h"
#include "Scene.h"



const int SCREEN_WIDTH = 800;
const int SCREEN_HEIGHT = 800;




void SetIcon(sf::Window& wnd)
{
    sf::Image image;
    image.loadFromFile("..//Tools//Resources//icon.png");
    wnd.setIcon(image.getSize().x, image.getSize().y, image.getPixelsPtr());
}

int main()
{
    sf::Window window(sf::VideoMode(SCREEN_WIDTH, SCREEN_HEIGHT), "Lab13", sf::Style::Default, sf::ContextSettings(24));
    SetIcon(window);
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    glEnable(GL_DEPTH_TEST);

    ShaderProgram ourShader = ShaderProgram("Shaders//sun.vs", "Shaders//sun.frag");
    ShaderProgram planetShader = ShaderProgram("Shaders//planet.vs", "Shaders//planet.frag");
    

    //TODO
    //инициализация объектов
    //SceneObject sun = ...
    //SceneObject planet = ...

    //TODO
    std::vector<Vertex> vs;
    vs.push_back({ {-0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 0.0f} });
    vs.push_back({ { 0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    vs.push_back({ { 0.5f,  0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 1.0f} });
    vs.push_back({ { 0.5f,  0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 1.0f} });
    vs.push_back({ {-0.5f,  0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    vs.push_back({ {-0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 0.0f} });
    //
    vs.push_back({ {-0.5f, -0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 0.0f} });
    vs.push_back({ { 0.5f, -0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    vs.push_back({ { 0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 1.0f} });
    vs.push_back({ { 0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 1.0f} });
    vs.push_back({ {-0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    vs.push_back({ {-0.5f, -0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 0.0f} });
    //==============================================
    vs.push_back({ {-0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    vs.push_back({ {-0.5f,  0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 1.0f} });
    vs.push_back({ {-0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    vs.push_back({ {-0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    vs.push_back({ {-0.5f, -0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 0.0f} });
    vs.push_back({ {-0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });

    vs.push_back({ { 0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    vs.push_back({ { 0.5f,  0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 1.0f} });
    vs.push_back({ { 0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    vs.push_back({ { 0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    vs.push_back({ { 0.5f, -0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 0.0f} });
    vs.push_back({ { 0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    //==============================================
    vs.push_back({ {-0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    vs.push_back({ { 0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 1.0f} });
    vs.push_back({ { 0.5f, -0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    vs.push_back({ { 0.5f, -0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    vs.push_back({ {-0.5f, -0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 0.0f} });
    vs.push_back({ {-0.5f, -0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });

    vs.push_back({ {-0.5f,  0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    vs.push_back({ { 0.5f,  0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 1.0f} });
    vs.push_back({ { 0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    vs.push_back({ { 0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {1.0f, 0.0f} });
    vs.push_back({ {-0.5f,  0.5f,  0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 0.0f} });
    vs.push_back({ {-0.5f,  0.5f, -0.5f}, {1.0f, 0.0f, 0.0f }, {0.0f, 1.0f} });
    std::vector<GLuint> indices2;
    for (GLuint i = 0; i < vs.size(); i++)
    {
        indices2.push_back(i);
    }
    sf::Texture texture1;
    texture1.loadFromFile("Images//texture1.png");
    std::vector<Texture> textures1 = std::vector<Texture>();
    textures1.push_back({ 0, "test", texture1 });
    Mesh mesh = Mesh(vs, indices2, textures1);
    SceneObject sun = SceneObject(&mesh, &ourShader);


    sf::Texture texture2;
    texture2.loadFromFile("Images//texture2.png");
    std::vector<Texture> textures2 = std::vector<Texture>();
    textures2.push_back({ 0, "test", texture2 });
    InstansedMesh meshPlanet = InstansedMesh(vs, indices2, textures2, 11);
    SceneObject planet = SceneObject(&meshPlanet, &planetShader);

    Scene mainScene = Scene();
    mainScene.AddSceneObject(sun);
    mainScene.AddSceneObject(planet);
    mainScene.AddShaderProgram(ourShader);
    mainScene.AddShaderProgram(planetShader);

    sf::Time elapsedTime;
    sf::Clock clock;
    GLfloat rotationAngle = 0.0f;
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
                // Изменён размер окна, надо поменять и размер области Opengl отрисовки
                glViewport(0, 0, event.size.width, event.size.height);
            }
        }
        elapsedTime = clock.getElapsedTime();
        if (elapsedTime > sf::milliseconds(5))
        {
            rotationAngle += 0.01f;
            if (rotationAngle > 360)
                rotationAngle = 360.0f;
            elapsedTime = clock.restart();
        }
        //glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        mainScene.Draw(rotationAngle);

        window.display();



        
    }
    window.close();
    return 0;
}