#include <SFML/Window.hpp>
#include <SFML/Graphics.hpp>
#include <glm/gtc/matrix_transform.hpp>

#include <iostream>
#include <chrono>
#include <ctime>  
#include <array>


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

glm::mat4* GenerateModelMatrices(int count, int* board, const glm::vec3& offset)
{
    glm::mat4* modelMatrices = new glm::mat4[count]{ };

    for (GLuint i = 0; i < count; i++)
    {
        int ind = rand() % 100;
        while (board[ind])
        {
            std::cout << board[ind] << " - " << ind << std::endl;
            ind = rand() % 100;

        }
        std::cout << ind << std::endl;
        board[ind] = 1;

        float x = (ind % 10 - 5) * 10;
        float y = 0.0f;
        float z = (ind / 10 - 5) * 10;
        glm::mat4 model = glm::mat4(1.0f);
        float angle = rand() % 360;
        model = glm::translate(model, offset);
        model = glm::translate(model, glm::vec3(x, y, z));
        model = glm::rotate(model, angle, glm::vec3(0.0f, 1.0f, 0.0f));
        modelMatrices[i] = model;
    }
    return modelMatrices;
}

void Fill(Scene* scene, ShaderProgram& defaultShader, ShaderProgram& instancedShader)
{
    int board[100];
    for (int i = 0; i < 100; i++)
    {
        board[i] = 0;
    }
    Mesh* _fir = new Mesh("Meshes//fir.obj", "Meshes//fir.jpg");
    SceneObject* fir = new SceneObject(_fir, &defaultShader);
    board[50] = 1;
    (*scene).AddSceneObject(*fir);

    ShaderProgram* targetShader = new ShaderProgram("Shaders//instanced.vs", "Shaders//target.frag");
    glm::mat4* modelMatrices = GenerateModelMatrices(8, board, glm::vec3(0.0f, 1.0f, 0.0f));
    InstansedMesh* _targets = new InstansedMesh("Meshes//Barn//barn.obj", "Meshes//Barn//barn.jpg", 8, modelMatrices);
    SceneObject* target = new SceneObject(_targets, targetShader);
    (*scene).AddShaderProgram(*targetShader);
    (*scene).AddSceneObject(*target);

    modelMatrices = GenerateModelMatrices(5, board, glm::vec3(0.0f, 1.0f, 0.0f));
    InstansedMesh* _lamps = new InstansedMesh("Meshes//Lamp//lamp.obj", "Meshes//Lamp//lamp.jpg", 5, modelMatrices);
    for (int i = 0; i < 5;i++)
    {
        PointLight* pl = new PointLight();
        glm::mat4 model = modelMatrices[i];
        model = glm::translate(model, glm::vec3(0.0f, 1.0f, 0.0f));
        pl->position =  model * glm::vec4(0.0f, 4.0f, 0.0f, 1.0f);
        (*scene).AddPointLight(pl);
    }
    SceneObject* lamps = new SceneObject(_lamps, &instancedShader);
    (*scene).AddSceneObject(*lamps);

    modelMatrices = GenerateModelMatrices(4, board, glm::vec3(0.0f, 1.0f, 0.0f));
    InstansedMesh* _snowmens = new InstansedMesh("Meshes//Snowman//snowman.obj", "Meshes//Snowman//snowman.png", 4, modelMatrices);
    SceneObject* snowmens = new SceneObject(_snowmens, &instancedShader);
    (*scene).AddShaderProgram(instancedShader);
    (*scene).AddSceneObject(*snowmens);

    modelMatrices = GenerateModelMatrices(5, board, glm::vec3(0.0f, 20.0f, 0.0f));
    InstansedMesh* _pumpkins = new InstansedMesh("Meshes//Pumpkin//pumpkin.obj", "Meshes//Pumpkin//pumpkin.png", 5, modelMatrices);
    SceneObject* pumpkin = new SceneObject(_pumpkins, &instancedShader);
    (*scene).AddSceneObject(*pumpkin);

    modelMatrices = GenerateModelMatrices(4, board, glm::vec3(0.0f, 20.0f, 0.0f));
    ShaderProgram* cloudShader = new ShaderProgram("Shaders//instanced.vs", "Shaders//cloud.frag");
    InstansedMesh* _clouds = new InstansedMesh("Meshes//Cloud//cloud.obj", "Meshes//Pumpkin//pumpkin.png", 4, modelMatrices);
    SceneObject* clouds = new SceneObject(_clouds, cloudShader);
    (*scene).AddShaderProgram(*cloudShader);
    (*scene).AddSceneObject(*clouds);
}

int main()
{
    srand(std::chrono::system_clock::to_time_t(std::chrono::system_clock::now()));

    sf::Window window(sf::VideoMode(800, 800), "IndTask3", sf::Style::Default, sf::ContextSettings(24));
    SetIcon(window);
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    glEnable(GL_DEPTH_TEST);
    glEnable(GL_PROGRAM_POINT_SIZE);
    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

    ShaderProgram defaultShader = ShaderProgram("Shaders//defaultPhong.vs", "Shaders//defaultPhong.frag");
    ShaderProgram normalMapShader = ShaderProgram("Shaders//normalMap.vs", "Shaders//normalMap.frag");
    ShaderProgram instancedShader = ShaderProgram("Shaders//instanced.vs", "Shaders//defaultPhong.frag");

    Mesh plane = Mesh("Meshes//Ground//cube.obj", "Meshes//Ground//grass.png", "Meshes//Ground//normal.png");
    SceneObject ground = SceneObject(&plane, &normalMapShader);
    ground.scale = { 200.0f, 1.0f, 200.0f };

    Mesh mesh = Mesh("Meshes//Zeppelin//zeppelin.obj", "Meshes//Zeppelin//zeppelin.png", "Meshes//Zeppelin//NormalMap.png");
    Player player = Player(&mesh, &normalMapShader);
    player.position.y += 30.0f;
    player.scale = player.scale * 0.06f;
    player.rotation.y = glm::radians(-90.0f);

    Scene mainScene = Scene();
    mainScene.AddSceneObject(player);
    mainScene.AddSceneObject(ground);
    mainScene.AddShaderProgram(defaultShader);
    mainScene.AddShaderProgram(normalMapShader);
    mainScene.AddShaderProgram(instancedShader);


    Fill(&mainScene, defaultShader, instancedShader);

    bool running = true;
    bool isCamActive = false;
    glm::vec2 mousePos;
    glm::vec2 mouseDelta;
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
            else if (event.type == sf::Event::MouseWheelMoved)
            {
                mainScene.camera.ProcessMouseScroll(event.mouseWheel.delta);
            }
            else if (event.type == sf::Event::MouseButtonPressed) {

                switch (event.mouseButton.button)
                {
                case sf::Mouse::Right:
                    isCamActive = true;
                    break;
                default:
                    break;
                }
            }
            else if (event.type == sf::Event::MouseMoved)
            {
                auto newMousePos = glm::vec2(event.mouseMove.x, event.mouseMove.y);
                mouseDelta = newMousePos - mousePos;
                mousePos = newMousePos;
                if (isCamActive)
                    mainScene.camera.OnMouseMove(mouseDelta);
            }
            else if (event.type == sf::Event::MouseButtonReleased) {
                switch (event.mouseButton.button)
                {
                case sf::Mouse::Right:
                    isCamActive = false;
                    break;
                default:
                    break;
                }
            }
            else if (event.type == sf::Event::KeyPressed)
            {
                player.OnKeyPress(event.key.code, mainScene.getDeltaTime());
                switch (event.key.code) 
                {
                case (sf::Keyboard::J): mainScene.camera.ProcessKeyboard(LROTATION, mainScene.getDeltaTime()); break;
                case (sf::Keyboard::L): mainScene.camera.ProcessKeyboard(RROTATION, mainScene.getDeltaTime()); break;
                case (sf::Keyboard::I): mainScene.camera.ProcessKeyboard(UPROTATION, mainScene.getDeltaTime()); break;
                case (sf::Keyboard::K): mainScene.camera.ProcessKeyboard(DOWNROTATION, mainScene.getDeltaTime()); break;
                default: break;
                }
            }
        }
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        mainScene.Draw();

        window.display();
    }
    window.close();
    return 0;
}
