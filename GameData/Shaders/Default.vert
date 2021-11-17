#version 330 core
#extension GL_ARB_shading_language_include : require

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord0;

uniform mat4 transform;

uniform mat4 view;

uniform mat4 projection;

out vec2 texCoord;

void main()
{
    texCoord = aTexCoord0;

    gl_Position = vec4(aPosition, 1.0) * transform * view * projection;
}