#include "lab11.h"
#include <iostream>

int main()
{
    sf::Window window(sf::VideoMode(500, 500), "Lab11", sf::Style::Default, sf::ContextSettings(24));
    SetIcon(window);
    window.setVerticalSyncEnabled(true);
    window.setActive(true);

    // Инициализация
    glewInit();
    Init();

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

        Draw();
        window.display();
    }
    Close();
    window.close();


    return 0;
}