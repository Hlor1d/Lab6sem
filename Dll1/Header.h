#pragma once
#include "mkl.h"

extern "C"  _declspec(dllexport) void funccpp(int n, int mytype, double* x, double* rez, double& mytime, int& ret);