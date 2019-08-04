// gemtradeDecode.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "gemtradeDecode.h"
#include "ida.h"
#include <stdlib.h>


extern "C" __declspec(dllexport) int char2uint(unsigned __int8 * a2, int a3)
{
	return (a2[a3 + 2] << 16) | (a2[a3 + 3] << 24) | a2[a3] | (a2[a3 + 1] << 8);
}

extern "C" __declspec(dllexport) char* decodeXOR(char* data, int len, size_t* outSize, unsigned int* lessLen )
{
	if (len <= 27 || memcmp(&"gemtradeLcU4xVc1", data, 8u) != 0)
	{
		return 0;
	}
	else
	{
		int v9 = char2uint((unsigned __int8 *)data, 16);
		int v10 = char2uint((unsigned __int8 *)data, 20);
		int v11 = char2uint((unsigned __int8 *)data, 24);
		unsigned int v12 = (unsigned int)(v11 + v10) >> 1;
		unsigned int v13 = (unsigned int)(v10 - v11) >> 1;
		size_t orgSize = len - 28;
		*outSize = orgSize;
		int v15 = v9 - v12 % v13;
		char * decodeData = (char *)malloc(orgSize);
		memcpy(decodeData, data + 28, orgSize);

		unsigned int v8 = 0;

		while (1)
		{
			int v17 = char2uint((unsigned __int8 *)decodeData, v8);
			unsigned int v18 = HIWORD(v17) ^ HIWORD(v15);
			*((char *)decodeData + v8) = v17 ^ v15;
			int v19 = v19 = (int)&decodeData[v8];
			v8 += 4;
			unsigned int v20 = (v17 ^ (unsigned int)v15) >> 8;
			unsigned int v21 = orgSize - v8;
			--v15;
			*(_BYTE *)(v19 + 1) = v20;
			*(_WORD *)(v19 + 2) = __PAIR__(HIBYTE(v18), (unsigned __int8)v18);
			if (orgSize <= v8 || v21 <= 3)
				break;
			if (v12 <= v8 && v12 < v21)
			{
				v8 += v13;
				if (orgSize < v8 || v12 > orgSize - v8)
					v8 = orgSize - v12;
			}
		}
		*lessLen = v8;
		return decodeData;
	}

}
