#version 330
out vec4 FragColor;

uniform vec4 color;

uniform sampler2D texture0;
in vec2 texCoord;

void main()
{
    FragColor = texture(texture0, texCoord) * color;
}