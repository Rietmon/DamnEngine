#version 330
#extension GL_ARB_shading_language_include : require

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

void main()
{
    outputColor = texture(texture0, texCoord);
}