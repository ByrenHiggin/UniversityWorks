// Week9Application.cpp : Defines the entry point for the application.
//

#include "Week9Application.h"

#if defined SIT354_OPENGL && defined STAGE01
#ifdef SIT354_WINDOWS
#include "stdafx.h"
#pragma comment(lib, "opengl32")
#pragma comment(lib, "glu32")
#endif


#include "Model.h"
#include "World.h"

#include "Surface.h"

Surface * surface;
#endif

#ifdef SIT354_WINDOWS
#define MAX_LOADSTRING 100

// Global Variables:
HINSTANCE hInst;								// current instance
TCHAR szTitle[MAX_LOADSTRING];					// The title bar text
TCHAR szWindowClass[MAX_LOADSTRING];			// the main window class name

// Forward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);

int APIENTRY _tWinMain(_In_ HINSTANCE hInstance,
					   _In_opt_ HINSTANCE hPrevInstance,
					   _In_ LPTSTR    lpCmdLine,
					   _In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	// TODO: Place code here.
	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_WEEK9APPLICATION, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_WEEK9APPLICATION));

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_WEEK9APPLICATION));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_WEEK9APPLICATION);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	HWND hWnd;

	hInst = hInstance; // Store instance handle in our global variable

	hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

	if (!hWnd)
	{
		return FALSE;
	}

	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//

float RelativeForceStrafe = 0;
float RelativeForceForward = 0;
float RelativeRotationUpward = 0;

void RelativeForceCalc()
{
	float z = 0;
	float x = 0;
	float y = 0;
	if (RelativeForceStrafe != 0)
	{
		

		if (RelativeForceStrafe < 0)
		{
			for (float i = RelativeForceStrafe; i < 0; i++)
			{
				x += i;
				if (x > 0)
					RelativeForceStrafe = 0;
			}
		}
		else if (RelativeForceStrafe > 0)
		{
			for (float i = RelativeForceStrafe; i > 0; i--)
			{
				x += i;
				if (x < 0)
					RelativeForceStrafe = 0;
			}
		}
	}
	if (RelativeForceForward != 0)
	{
		
		if (RelativeForceForward < 0)
		{
			for (float i = RelativeForceForward; i < 0; i++)
			{
				z += i;

			}
		}
		else if (RelativeForceForward > 0)
		{
			for (float i = RelativeForceForward; i > 0; i--)
			{
				z += i;

			}
		}
		
	}

	if (RelativeRotationUpward != 0)
	{

		if (RelativeRotationUpward < 0)
		{
			for (float i = RelativeRotationUpward; i < 0; i++)
			{
				y += i;
			}
		}
		else if (RelativeRotationUpward > 0)
		{
			for (float i = RelativeRotationUpward; i > 0; i--)
			{
				y += i;
			}
		}

	}
	surface->strafe(x);
	surface->step(z);
	surface->turnup(y);

}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	
	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;
	
	switch (message)
	{
#if defined SIT354_OPENGL && defined STAGE01
	case WM_CREATE:
		{
			surface = new Surface (hInst, hWnd);

			// Force a redraw every 50ms - about 20 times a second.
			SetTimer(hWnd, NULL, 50, NULL);
		}
		break;
	case WM_TIMER:
		RedrawWindow (hWnd, NULL, NULL, RDW_INVALIDATE);
		break;
#endif
	case WM_COMMAND:
		wmId    = LOWORD(wParam);
		wmEvent = HIWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
		break;
#if defined SIT354_OPENGL && defined STAGE02
	case WM_PAINT:
		if (surface != NULL)
		{
			surface->paintGL ();
			RelativeForceCalc();
		}
		break;
	case WM_SIZE:
		if (surface != NULL)
		{
			RECT rect;
			GetClientRect (hWnd, &rect);
			surface->resize (rect.right - rect.left, rect.bottom - rect.top);
		}
		break;
#endif
#if defined SIT354_OPENGL && defined STAGE05
	case WM_KEYDOWN:
		{
			if (surface != NULL)
			{
				switch (wParam)
				{
				
				case 'A':
					if (RelativeForceStrafe == 0.2)
					{
						RelativeForceStrafe += -0.4;
						break;
					}
					else {
						RelativeForceStrafe += -0.2;
						break;
					}
				case 'D':
					if (RelativeForceStrafe == -0.2)
					{
						RelativeForceStrafe += 0.4;
						break;
					}
					else {
						RelativeForceStrafe += 0.2;
						break;
					}
					break;
				case 'W':
					RelativeForceForward += -0.2;
					break;
				case 'S':
					RelativeForceForward += 0.2;
					break;
				case VK_LEFT:
					surface->turnside (-1.0);
					break;
				case VK_RIGHT:
					surface->turnside (1.0);
					break;
				case VK_UP:
					RelativeRotationUpward += -0.02;
					break;
				case VK_DOWN:
					RelativeRotationUpward += 0.02;
					break;
				default:
					break;
				}
			}
		}
		break;
#endif
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
#endif
