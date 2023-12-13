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
// camera
Camera camera(glm::vec3(0.0f, 0.0f, 3.0f));
float lastX = SCREEN_WIDTH / 2.0f;
float lastY = SCREEN_HEIGHT / 2.0f;
bool firstMouse = true;



void SetIcon(sf::Window& wnd)
{
    sf::Image image;
    image.loadFromFile("..//Tools//Resources//icon.png");
    wnd.setIcon(image.getSize().x, image.getSize().y, image.getPixelsPtr());
}

int main()
{
    sf::Window window(sf::VideoMode(SCREEN_WIDTH, SCREEN_HEIGHT), "Lab14", sf::Style::Default, sf::ContextSettings(24));
    SetIcon(window);
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    glEnable(GL_DEPTH_TEST);

    ShaderProgram ourShader = ShaderProgram("Shaders//sun.vs", "Shaders//sun.frag");
    ShaderProgram planetShader = ShaderProgram("Shaders//planet.vs", "Shaders//planet.frag");

    Mesh mesh = Mesh("Meshes//whale.obj", "Meshes//whale.jpg");
    SceneObject sun = SceneObject(&mesh, &ourShader);
    sun.scale = sun.scale * 0.25f;

    InstansedMesh meshPlanet = InstansedMesh("Meshes//bird.obj", "Meshes//bird.jpg", 11);
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
            else if (event.type == sf::Event::KeyPressed)
            {
                switch (event.key.code) {
                case (sf::Keyboard::R): mainScene.camera.ProcessKeyboard(FORWARD, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::F): mainScene.camera.ProcessKeyboard(BACKWARD, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::A): mainScene.camera.ProcessKeyboard(LEFT, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::D): mainScene.camera.ProcessKeyboard(RIGHT, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::W): mainScene.camera.ProcessKeyboard(UP, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::S): mainScene.camera.ProcessKeyboard(DOWN, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::J): mainScene.camera.ProcessKeyboard(LROTATION, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::L): mainScene.camera.ProcessKeyboard(RROTATION, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::I): mainScene.camera.ProcessKeyboard(UPROTATION, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::K): mainScene.camera.ProcessKeyboard(DOWNROTATION, elapsedTime.asSeconds()); break;
                default: break;
                }
                elapsedTime = clock.restart();
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
