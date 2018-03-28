// ConsoleApplication2.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "stdint.h"
#include "ida.h"
#include <stdlib.h>
#include <string.h>
#include "gemtradeDecode.h"



int main(int argc, char *argv[])
{
	char* fileUrl = "C:\\Users\\Cirno\\Documents\\apkcreak\\baoshiyan\\base\\assets\\src\\app\\init.lua";
	//获取文件指针
	FILE *pFile = fopen(fileUrl, //打开文件的名称
		"rb");
	char *pBuf;  //定义文件指针
	fseek(pFile, 0, SEEK_END); //把指针移动到文件的结尾 ，获取文件长度
	int len = ftell(pFile); //获取文件长度
	pBuf = new char[len + 1]; //定义数组长度
	rewind(pFile); //把指针移动到文件开头 因为我们一开始把指针移动到结尾，如果不移动回来 会出错
	fread(pBuf, 1, len, pFile); //读文件
	pBuf[len] = 0; //把读到的文件最后一位 写为0 要不然系统会一直寻找到0后才结束
	fclose(pFile); // 关闭文件
	size_t outSize = 0;
	char* decoded = decodeXOR(pBuf, len, &outSize);
	printf(decoded);

	char* outfile = new char[strlen(fileUrl) + 5];
	snprintf(outfile, strlen(fileUrl) + 5, "%s.dec", fileUrl);
	//获取文件指针
	/*FILE *pFileW = fopen(outfile, //打开文件的名称
		"wb"); // 文件打开方式 如果原来有内容也会销毁
			  //向文件写数据
	//
	fwrite(decoded, //要输入的文字
		1,//文字每一项的大小 以为这里是字符型的 就设置为1 如果是汉字就设置为4
		outSize, //单元个数 我们也可以直接写5
		pFileW //我们刚刚获得到的地址
	);


	//fclose(pFile); //告诉系统我们文件写完了数据更新，但是我们要要重新打开才能在写
	fflush(pFileW); //数据刷新 数据立即更新 */
	getchar();
	return 0;
}