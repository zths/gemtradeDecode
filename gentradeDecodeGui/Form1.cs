using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using size_t = System.UIntPtr;

namespace gentradeDecodeGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("gemtradeDecode.dll", EntryPoint = "decodeXOR", CallingConvention = CallingConvention.Cdecl)]
        unsafe static extern byte* decodeXOR(byte* data, int len, size_t* outSize, uint* lessLen);


        unsafe private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                Description = "请选择要解密的路径",
                //SelectedPath = @"C:\Users\Cirno\Desktop\baoshiyan\123"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox1.AppendText("已选择路径: " + foldPath + "\r\n");
                DirListGen dlg = new DirListGen(foldPath);

                for (int i = 0; i < dlg.fAl.Count; i++)
                {
                    FileInfo fi = dlg.fAl[i];
                    //textBox1.AppendText((i + 1) + "/" + dlg.fAl.Count + ": ");
                    Stream f = fi.OpenRead();
                    byte[] b = new byte[f.Length];
                    int readLength = f.Read(b, 0, b.Length);
                    if (readLength != b.Length)
                    {
                        textBox1.AppendText((i + 1) + "/" + dlg.fAl.Count + ": ");
                        textBox1.AppendText(fi.FullName + " \t读取错误.\r\n");
                    }
                    f.Close();
                    int outLen = 0;
                    byte* decByte = DecByteArr(b, ref outLen);
                    if (decByte == null)
                    {
                        //textBox1.AppendText((i + 1) + "/" + dlg.fAl.Count + ": ");
                        //textBox1.AppendText(fi.FullName + " \tSkipped;\r\n");
                        continue;
                    }
                    FileStream fs = File.Open(fi.FullName, FileMode.Create, FileAccess.Write);
                    for (int j = 0; j < outLen; j++) {
                        fs.WriteByte(decByte[j]);
                    }
                    //fs.Write(decByte[i], 0, decByte.Length);
                    fs.Flush();
                    fs.Close();
                    //textBox1.AppendText(fi.FullName + " \t处理完成.\r\n");
                }
                textBox1.AppendText("全部处理完成..");
            }
        }

        unsafe public byte* DecByteArr(byte[] inarr, ref int outLength)
        {
            byte[] outdata;

            byte* data = (byte*)Marshal.AllocHGlobal(inarr.Length);//stackalloc byte[inarr.Length]
            for (int i = 0; i < inarr.Length; i++)
            {
                data[i] = inarr[i];
            }

            size_t outLen = (size_t)0;
            size_t* outLenP = &outLen;

            uint lessLen = 0;
            uint* lessLenP = &lessLen;


            byte* os = decodeXOR(data, inarr.Length, outLenP, lessLenP);
            if (os == null)
            {
                return null;
            }
            outLength = Convert.ToInt32(outLen.ToUInt32());
            outdata = new byte[outLength];
            for (int i = 0; i < outLength; i++)
            {
                outdata[i] = os[i];
            }
            if (outLength == inarr.Length)
            {
                textBox1.AppendText("Wtf?\r\n");
            }

            return os;

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
