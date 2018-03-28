using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gentradeDecodeGui
{
    class DirListGen
    {
        public List<DirectoryInfo> dAl = new List<DirectoryInfo>();
        public List<FileInfo> fAl = new List<FileInfo>();

        public DirListGen(string path)
        {
            GetAllDirList(path);
        }
        public void GetAllDirList(string strBaseDir)
        {
            GetAllDirListD(strBaseDir);
            DirectoryInfo di = new DirectoryInfo(strBaseDir);
            FileInfo[] fis = di.GetFiles();
            foreach (FileInfo fi in fis)
            {
                fAl.Add(fi);
            }
            dAl.Add(di);
        }

        public void GetAllDirListD(string strBaseDir)
        {
            DirectoryInfo di = new DirectoryInfo(strBaseDir);
            DirectoryInfo[] diA = di.GetDirectories();
            for (int i = 0; i < diA.Length; i++)

            {
                dAl.Add(diA[i]);
                FileInfo[] fis = diA[i].GetFiles();
                foreach (FileInfo fi in fis)
                {
                    fAl.Add(fi);
                }
                //diA[i].FullName是某个子目录的绝对地址，把它记录在ArrayList中
                GetAllDirListD(diA[i].FullName);
                //注意：递归了。逻辑思维正常的人应该能反应过来
            }
        }
    }
}
