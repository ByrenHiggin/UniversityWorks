#include "Week9Application.h"

#ifdef SIT354_WINDOWS
#include "stdafx.h"
#endif

#if defined SIT354_OPENGL && defined STAGE04
#include "Shader.h"
#endif
#if defined SIT354_OPENGL && defined STAGE03
#include "Model.h"
#include "World.h"

WorldObject::WorldObject(Model * model
#endif
#if defined SIT354_OPENGL && defined STAGE04
	, Shader * shader
#endif
#if defined SIT354_OPENGL && defined STAGE03
	) : model(model)
#endif
#if defined SIT354_OPENGL && defined STAGE04
	, shader(shader)
#endif
#if defined SIT354_OPENGL && defined STAGE03
{
	setPosition(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, -1.0f);
	up[0] = 0.0f;
	up[1] = 1.0f;
	up[2] = 0.0f;
}

void WorldObject::setPosition(float x, float y, float z, float forwardx, float forwardy, float forwardz)

{
	modelPosition[0] = x;
	modelPosition[1] = y;
	modelPosition[2] = z;

	forward[0] = forwardx;
	forward[1] = forwardy;
	forward[2] = forwardz;
}
vec3 WorldObject::getPosition()
{
	return vec3(this->modelPosition[0], this->modelPosition[1], this->modelPosition[2]);
}

mat4 WorldObject::getWorldTransform()
{
	vec3 dirforward(forward[0], forward[1], forward[2]);
	vec3 dirup(up[0], up[1], up[2]);
	vec3 dirside = glm::cross(dirup, dirforward);

	// in this format, goes in as the transpose.
	mat4 rot = mat4(dirside[0], dirup[0], dirforward[0], 0.0,
		dirside[1], dirup[1], dirforward[1], 0.0,
		dirside[2], dirup[2], dirforward[2], 0.0,
		0.0, 0.0, 0.0, 1.0);
	mat4 M = glm::translate(mat4(1.0f), vec3(modelPosition[0], modelPosition[1], modelPosition[2])) * rot * glm::scale(mat4(1.0f), vec3(1.0f / 3.0f));
	return M;
}


World::World(void)
{
#ifdef SIT354_LINUX
	glinit = false;
#endif

	
	const int maxModels = 11;
	models = new WorldObject *[maxModels];
	numModels = 0;

	Model * planet = new Model((char *) "sphere.obj");

#endif
#if defined SIT354_OPENGL && defined STAGE04
	NoiseShader = new Shader((char *) "Asteroid.glsl", (char *) "smile.glsl");
	PlanetShader = new Shader((char *) "vertexshader.glsl", (char *) "smile.glsl");
#endif
#if defined SIT354_OPENGL && defined STAGE03
#ifndef STAGE04
	models[numModels] = new WorldObject(pawn);
	models[numModels]->setPosition(0.0f, -0.5f, -3.0f, 0.0, 0.0, -1.0f);
	numModels++;
#endif

#endif
#if defined SIT354_OPENGL && defined STAGE04
	////for (int i = 0; i < 8; i++)
	////{
	////	models[numModels] = new WorldObject(pawn, marbleShader);
	////	models[numModels]->setPosition(-0.5f + (i - 3), 0.0f, 2.5f, 0.0, 0.0, -1.0f);
	////	numModels++;

	////	models[numModels] = new WorldObject(pawn, ebonyShader);
	////	models[numModels]->setPosition(-0.5f + (i - 3), 0.0f, -2.5f, 0.0, 0.0, 1.0f);
	////	numModels++;
	////}

	srand(100);


	models[numModels] = new WorldObject(planet, PlanetShader);
	models[numModels]->setPosition(0.0f, 0.0f, 1.0f, 0.0, 0.0, 1.0f);
	numModels++;

	
	for (int i = 0; i < rand() % 20; i++)
	{
		models[numModels] = new WorldObject(planet, NoiseShader);
		models[numModels]->setPosition(rand() % 20, rand() % 20, rand() % 20, 0.0, 0.0, 1.0f);
		numModels++;

		models[numModels] = new WorldObject(planet, NoiseShader);
		models[numModels]->setPosition(-rand() % 20, rand() % 20, rand() % 20, 0.0, 0.0, 1.0f);
		numModels++;

		models[numModels] = new WorldObject(planet, NoiseShader);
		models[numModels]->setPosition(rand() % 20, rand() % 20, -rand() % 20, 0.0, 0.0, 1.0f);
		numModels++;
		models[numModels] = new WorldObject(planet, NoiseShader);
		models[numModels]->setPosition(rand() % 20, -rand() % 20, rand() % 20, 0.0, 0.0, 1.0f);
		numModels++;

		models[numModels] = new WorldObject(planet, NoiseShader);
		models[numModels]->setPosition(-rand() % 20, -rand() % 20, rand() % 20, 0.0, 0.0, 1.0f);
		numModels++;

		models[numModels] = new WorldObject(planet, NoiseShader);
		models[numModels]->setPosition(rand() % 20, -rand() % 20, -rand() % 20, 0.0, 0.0, 1.0f);
		numModels++;

		models[numModels] = new WorldObject(planet, NoiseShader);
		models[numModels]->setPosition(-rand() % 20, rand() % 20, -rand() % 20, 0.0, 0.0, 1.0f);
		numModels++;

		models[numModels] = new WorldObject(planet, NoiseShader);
		models[numModels]->setPosition(-rand() % 20, -rand() % 20, -rand() % 20, 0.0, 0.0, 1.0f);
		numModels++;
	}

	

#endif
	//assert(numModels == maxModels);
#if defined SIT354_OPENGL && defined STAGE03

	lx = 0.5;
	ly = 3.5;
	lz = 2.0;
}


World::~World(void)
{

}

//CUSTOM
float j = rand() % 50;
void World::UpdateObjects(int Scalar)
{
	//float k = j;
	//	for (int i = 0; i < World::numModels; i++)
	//	{
	//		vec3 temp = models[i]->getPosition();
	//		if (i % 2 == 0 && i != 32)
	//		{
	//			models[i]->setPosition(temp.x, temp.y + (cos(k/30)/60), temp.z, 0.0f, 0.0f, -1.0f);
	//		}

	//		//models[i]->setPosition(temp.x, temp.y*2, temp.z, 0.0f, 0.0f, 1.0f);
	//	}
	//

}
void World::renderSpecific(WorldObject * object, mat4 view, mat4 proj, mat4 shadow)

{
#ifdef SIT354_LINUX
	if (!glinit)
	{
		initializeOpenGLFunctions();
		glinit = true;
	}
#endif

	Model * model = object->getModel();
	mat4 world = object->getWorldTransform();
#endif
#if defined SIT354_OPENGL && defined STAGE04
	Shader * shader = object->getShader();

	shader->activateShader();
	
	glUniformMatrix4fv(glGetUniformLocation(shader->getProgram(), (char *) "WorldMatrix"), 1, GL_FALSE, &world[0][0]);
	glUniformMatrix4fv(glGetUniformLocation(shader->getProgram(), (char *) "ViewMatrix"), 1, GL_FALSE, &view[0][0]);
	glUniformMatrix4fv(glGetUniformLocation(shader->getProgram(), (char *) "ProjectionMatrix"), 1, GL_FALSE, &proj[0][0]);
	glUniformMatrix4fv(glGetUniformLocation(shader->getProgram(), (char *) "ShadowMatrix"), 1, GL_FALSE, &shadow[0][0]);
	glUniform1f(glGetUniformLocation(shader->getProgram(), (char *) "TimeElapsed" ), j);
	glUniform1i(glGetUniformLocation(shader->getProgram(), (char *) "Texture"), textureunit);
	glUniform1i(glGetUniformLocation(shader->getProgram(), (char *) "ShadowMap"), 0);
	glUniform4f(glGetUniformLocation(shader->getProgram(), (char *) "LightPosition"), lx, ly, lz, 1.0f);
#endif
#if defined SIT354_OPENGL && defined STAGE03

	model->render();
}

void World::render(mat4 view, mat4 proj, mat4 shadow)

{
	for (int i = 0; i < numModels; i++)
	{
		renderSpecific(models[i], view, proj, shadow);
	}
}
#endif
