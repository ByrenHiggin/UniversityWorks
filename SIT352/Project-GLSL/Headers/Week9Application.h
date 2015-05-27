#pragma once

// Acknowledgment: models taken from: http://www.myminifactory.com/object/view/396/Minion-Chess

#define SIT354_OPENGL
#define SIT354_WINDOWS
//#define SIT354_LINUX

#define STAGE01
#define STAGE02
#define STAGE03
#define STAGE04
#define STAGE05

#ifdef SIT354_OPENGL

#ifdef SIT354_WINDOWS
#include "resource.h"

#include <GL/glew.h>
#endif

#ifdef SIT354_LINUX
#include <QtGui/QOpenGLFunctions_4_1_Core>
#include <QtOpenGL/QtOpenGL>
#endif

#include <GL/gl.h>
#include <GL/glu.h>

#define GLM_FORCE_RADIANS

#include <glm/glm.hpp>
using glm::mat4;
using glm::mat3;
using glm::vec3;
using glm::vec4;
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/matrix_inverse.hpp>

#endif

#ifdef SIT354_LINUX
#include <QtWidgets/QtWidgets>
#include <QtWidgets/QApplication>
#include <QtWidgets/QWidget>
#include <QtOpenGL/QGLWidget>
#endif
