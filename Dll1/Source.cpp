#include "pch.h"
#include < math.h >
#include "mkl.h"
#include <chrono>
#include <thread>
#include <windows.h>

extern "C"  _declspec(dllexport) void funccpp(int n, int mytype, double* x, double* rez, double& mytime, int& ret)
{
	try
	{
        const int sred = 1;// sred arefm of 
        auto start1 = std::chrono::high_resolution_clock::now();
        for (int j = 0; j < sred;j++)
        {
            switch (mytype)
            {
            case 1:
                vmdTan(n, x, rez, VML_EP);
                break;
            case 2:
                vmdTan(n, x, rez, VML_HA);
                break;
            case 3:
                vmdErfInv(n, x, rez, VML_EP);
                break;
            case 4:
                vmdErfInv(n, x, rez, VML_HA);
                break;
            case 5:
                for (int i = 0;i < n;i++)
                {
                    rez[i] = erf(x[i]);
                }
                break;
            }
        }

        auto stop1 = std::chrono::high_resolution_clock::now();
        std::chrono::duration<double> duration = stop1 - start1;
        mytime = (double)duration.count();
        mytime /= sred;
		ret = 0;
	}
	catch (...)
	{
		ret = -1;
	}
}