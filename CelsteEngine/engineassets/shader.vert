#version 330 core
layout (location = 0) in vec3 aPosition;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec3 aNormal;

out vec2 texCoord;
out vec3 normal;

void main()
{
    texCoord = aTexCoord;
    normal = aNormal * mat3(transpose(inverse(model)));
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
}