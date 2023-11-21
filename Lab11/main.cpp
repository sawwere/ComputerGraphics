#include "lab11.h"

void Loop(int taskCode, int figureCode)
{
    sf::Window window(sf::VideoMode(500, 500), "Lab11", sf::Style::Default, sf::ContextSettings(24));
    SetIcon(window);
    window.setVerticalSyncEnabled(true);
    window.setActive(true);
    glewInit();
    Init(taskCode, figureCode);

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

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

        Draw(taskCode, figureCode);
        window.display();
    }
    Release();
    window.close();
}

int main()
{
    int figureCode = 0;
    int taskCode = 0;
    while (true)
    {
        std::cout << "Enter figure number: ";
        std::cin >> figureCode;
        if (figureCode == 0)
            break;
        std::cout << "Enter task number: ";
        std::cin >> taskCode;
        if (!(taskCode == 2 || taskCode == 3 || taskCode == 4))
            continue;
        Loop(taskCode, figureCode);
    }
    return 0;
}