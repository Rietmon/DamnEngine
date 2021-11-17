#version 330 core
#extension GL_ARB_shading_language_include : require

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord0;
layout(location = 2) in vec3 aNormal;

uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

out vec3 position;
out vec2 texCoord;
out vec3 normal;

void main()
{
    position = vec3(transform * vec4(aPosition, 1.0));
    texCoord = aTexCoord0;
    normal = aNormal * mat3(transpose(inverse(transform)));

    gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
}