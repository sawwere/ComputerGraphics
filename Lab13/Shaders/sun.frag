
#version 330 core
out vec4 FragColor;
  
//in vec3 ourColor;
in vec2 TexCoord;

uniform sampler2D texture1;
uniform sampler2D texture2;
uniform float scaler;
void main()
{
    //FragColor = vec4(1.0, 0.0, 0.0, 1.0);
    FragColor = texture(texture1, TexCoord);
//FragColor = texture(ourTexture, texturePosition) * vec4(ourColor, 1.0); 
}
