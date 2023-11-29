#include "lab12.h"
#include <iostream>


int main()
{
    int taskCode = 0;
    while (true)
    {
        std::cout << "Enter task number: ";
        std::cin >> taskCode;
        switch (taskCode)
        {
        case 0:
            break;
        case 2:
            task2();
            break;
        case 3:
            task3();
            break;
        default:
            continue;;
        }
    }
    return 0;
}
/*
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
    texture1.loadFromFile("icon.png");
    std::vector<Texture> textures;
    textures.push_back({ 0, "first", texture1 });

    Mesh mesh = Mesh(vs, indices2, textures);

    //GLfloat vertices[] = {
    ////    // Positions          // Colors         // Texture Coords
    //     -0.5f, -0.5f,  0.5f,  1.0f, 0.0f, 0.0f,  1.0f, 1.0f,   // 0 --+  --+
    //      0.5f, -0.5f,  0.5f,  0.0f, 1.0f, 0.0f,  0.0f, 0.0f, // 1 +-+  +-+
    //     -0.5f,  0.5f,  0.5f,  0.0f, 0.0f, 1.0f,  1.0f, 0.0f,// 2 -++  -++
    //      0.5f,  0.5f,  0.5f,  1.0f, 1.0f, 0.0f,  0.0f, 1.0f,// 3 +++  +++
    //     -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, 1.0f,  1.0f, 1.0f,   // 4 ---  ---
    //      0.5f, -0.5f, -0.5f,  1.0f, 1.0f, 0.0f,  0.0f, 0.0f,  // 5 +--  +--
    //     -0.5f,  0.5f, -0.5f,  1.0f, 0.0f, 0.0f,  1.0f, 0.0f,  // 6 -+-  -+-
    //      0.5f,  0.5f, -0.5f,  0.0f, 1.0f, 0.0f,  0.0f, 1.0f// 7 ++-  ++-
    //};
    //GLuint indices[] = {

    //    2, 6, 7,
    //    2, 3, 7,

    //    //Bottom
    //    0, 4, 5,
    //    0, 1, 5,

    //    //Left
    //    0, 2, 6, //+
    //    0, 4, 6,

    //    //Right
    //    1, 3, 7,
    //    1, 5, 7,

    //    //Front
    //    0, 2, 3,
    //    0, 1, 3,

    //    //Back
    //    4, 6, 7,
    //    4, 5, 7
    //};
    */