#version 330
#extension GL_ARB_shading_language_include : require

out vec4 outputColor;

in vec3 position;
in vec2 texCoord;
in vec3 normal;

uniform sampler2D texture0;
uniform vec4 color = vec4(1,1,1,1);
uniform vec3 lightPosition = vec3(0,5,0);

void main()
{
    vec3 norm = normalize(normal);
    vec3 lightDir = normalize(lightPosition - position);
    float diff = max(dot(norm, lightDir), 0.0) * 2;
    vec4 diffuse = diff * vec4(1,1,1,1);
    vec4 objectColor = texture(texture0, texCoord);
    vec4 result = (0.5 + diffuse) * objectColor;
    outputColor = result * color;
}