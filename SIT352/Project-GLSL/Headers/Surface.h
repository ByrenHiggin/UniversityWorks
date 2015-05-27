#include "Week9Application.h"

#if defined SIT354_OPENGL && defined STAGE03
#include "World.h"
#include "Model.h"
#endif

#if defined SIT354_OPENGL && defined STAGE01
#pragma once

// The visualization element. 
class Surface
#ifdef SIT354_LINUX
	: public QGLWidget
#endif
{
#ifdef SIT354_LINUX
	Q_OBJECT
#endif

private:
#ifdef SIT354_WINDOWS
	HWND handle;
	HDC hDC;
#endif
	void init ();

protected:
#endif
#if defined SIT354_OPENGL && defined STAGE03
	// Link to the model; in this case the set
	// of models in the world that we're creating.
	World * world;

#endif 
#if defined SIT354_OPENGL && defined STAGE05
	float cameraPosition [3];
	float cameraForward [3];
	float cameraUp [3];
	float speed;
	float angleSpeed;

	mat3 rotationVectorAngle (float ux, float uy, float uz, float angle);

#endif
#if defined SIT354_OPENGL && defined STAGE01
public:
#ifdef SIT354_WINDOWS
	Surface (HINSTANCE hInst, HWND hWnd);
#endif	
#ifdef SIT354_LINUX
	Surface ();
	void keyPressEvent(QKeyEvent * event);
#endif	
	~Surface(void);
	// resize surface as well as reset the graphics viewport.
	void resize (int width, int height);

#endif
#if defined SIT354_OPENGL && defined STAGE02
	void initializeGL();
	void paintGL();
	void resizeGL(int width, int height);
#endif
#if defined SIT354_OPENGL && defined STAGE05
	// move sideways.
	void strafe (float direction);
	void step (float direction);
	void turnside (float direction);
	void turnup (float direction);
#endif
#if defined SIT354_OPENGL && defined STAGE01

};

#endif
