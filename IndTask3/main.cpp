#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <glm/gtc/matrix_transform.hpp>

#include <iostream>

#include "ShaderProgram.h"
#include "Mesh.h"
#include "InstancedMesh.h"
#include "SceneObject.h"
#include "Scene.h"



void SetIcon(sf::Window& wnd)
{
    sf::Image image;
    image.loadFromFile("..//Tools//Resources//icon.png");
    wnd.setIcon(image.getSize().x, image.getSize().y, image.getPixelsPtr());
}


void Fill(Scene* scene, ShaderProgram& defaultShader, ShaderProgram& instancedShader)
{
    int board[100];
    for (int i = 0; i < 100; i++)
    {
        board[i] == 0;
    }
    Mesh* _fir = new Mesh("Meshes//bird.obj", "Meshes//bird.jpg");
    SceneObject* fir = new SceneObject(_fir, &defaultShader);
    (*scene).AddSceneObject(*fir);

    ShaderProgram* targetShader = new ShaderProgram("Shaders//instanced.vs", "Shaders//target.frag");
    InstansedMesh* _targets = new InstansedMesh("Meshes//cube.obj", "Meshes//bird.jpg", 5, board);
    SceneObject* target = new SceneObject(_targets, targetShader);
    (*scene).AddShaderProgram(*targetShader);
    (*scene).AddSceneObject(*target);
}

int main()
{
    sf::Window window(sf::VideoMode(800, 800), "IndTask3", sf::Style::Default, sf::ContextSettings(24));
    SetIcon(window);
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    glEnable(GL_DEPTH_TEST);

    ShaderProgram defaultShader = ShaderProgram("Shaders//player.vs", "Shaders//sun.frag");
    ShaderProgram procShader = ShaderProgram("Shaders//player.vs", "Shaders//proc.frag");
    ShaderProgram planetShader = ShaderProgram("Shaders//instanced.vs", "Shaders//proc.frag");

    Mesh plane = Mesh("Meshes//cube.obj", "Meshes//grass.jpg");
    SceneObject ground = SceneObject(&plane, &procShader);
    ground.scale = { 200.0f, 1.0f, 200.0f };

    Mesh mesh = Mesh("Meshes//zeppelin.obj", "Meshes//zeppelin.png");
    Player player = Player(&mesh, &defaultShader);
    player.position.y += 20.0f;
    player.scale = player.scale * 0.025f;
    player.rotation.y = glm::radians(-90.0f);

    Scene mainScene = Scene();
    mainScene.AddSceneObject(player);
    mainScene.AddSceneObject(ground);
    mainScene.AddShaderProgram(defaultShader);
    mainScene.AddShaderProgram(procShader);
    mainScene.AddShaderProgram(planetShader);


    Fill(&mainScene, defaultShader, planetShader);

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
                glViewport(0, 0, event.size.width, event.size.height);
                mainScene.camera.SCREEN_WIDTH = event.size.width;
                mainScene.camera.SCREEN_HEIGHT = event.size.height;
            }
            else if (event.type == sf::Event::KeyPressed)
            {
                player.OnKeyPress(event.key.code, elapsedTime.asSeconds());
                switch (event.key.code) 
                {
                case (sf::Keyboard::J): mainScene.camera.ProcessKeyboard(LROTATION, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::L): mainScene.camera.ProcessKeyboard(RROTATION, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::I): mainScene.camera.ProcessKeyboard(UPROTATION, elapsedTime.asSeconds()); break;
                case (sf::Keyboard::K): mainScene.camera.ProcessKeyboard(DOWNROTATION, elapsedTime.asSeconds()); break;
                default: break;
                }
            }
        }
        elapsedTime = clock.getElapsedTime();
        if (elapsedTime > sf::milliseconds(4))
        {
            rotationAngle += 0.01f;
            if (rotationAngle > 360)
                rotationAngle = 360.0f;
            //elapsedTime = clock.restart();
        }
        //glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        mainScene.Draw(clock.getElapsedTime().asSeconds());

        window.display();




    }
    window.close();
    return 0;
}
