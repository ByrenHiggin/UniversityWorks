#pragma once

#include "Week9Application.h"

#if defined SIT354_OPENGL && defined STAGE03
class Model;
#endif
#if defined SIT354_OPENGL && defined STAGE04
class Shader;
#endif

#if defined SIT354_OPENGL && defined STAGE03
class WorldObject
{
private:
	Model * model;
#endif
#if defined SIT354_OPENGL && defined STAGE04
	Shader * shader;
#endif

#if defined SIT354_OPENGL && defined STAGE03
	float modelPosition[3];
	float forward[3];
	float up[3];

public:
	WorldObject(Model * model
#endif
#if defined SIT354_OPENGL && defined STAGE04
		, Shader * shader
#endif
#if defined SIT354_OPENGL && defined STAGE03
		);
	~WorldObject();
	vec3 getPosition();
	void setPosition(float x, float y, float z, float forwardx, float forwardy, float forwardz);
	mat4 getWorldTransform();
	Model * getModel() { return model; }
#endif
#if defined SIT354_OPENGL && defined STAGE04
	Shader * getShader() { return shader; }
#endif
#if defined SIT354_OPENGL && defined STAGE03
};

class World
{
private:

	WorldObject * * models;
	int numModels;

#endif
#if defined SIT354_OPENGL && defined STAGE04
	Shader * NoiseShader;
	Shader * PlanetShader;
#endif
#if defined SIT354_OPENGL && defined STAGE03

	float lx;
	float ly;
	float lz;



	void renderSpecific(WorldObject * object, mat4 view, mat4 proj, mat4 shadow);

public:
	World(void);
	~World(void);

	int textureunit;

	// keep track of available texture units.
	void render(mat4 view, mat4 proj, mat4 shadow);
	//CUSTOM
	void UpdateObjects(int Scalar);
};

#endif
