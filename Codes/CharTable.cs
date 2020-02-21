using System;
using System.IO;
using System.Text;

namespace PokemonCTR
{
    class CharTable
    {
        private readonly char[] CharTableArray = new char[65536];
        private readonly int[] CharTableArrayRev = new int[65536];
        public int maxCharCode;

        /// <summary>
        /// 从码表文件路径读取码表。
        /// </summary>
        /// <param name="path">码表文件路径</param>
        public CharTable(string path)
        {
            if (File.Exists(path))
            {
                string[] CharTableText = File.ReadAllText(path, Encoding.UTF8).Split('\n');
                foreach (string s in CharTableText)
                {
                    string[] ss = s.Split('\t');
                    if (ss.Length > 1)
                    {
                        int i = Convert.ToInt32(ss[0], 16);
                        char c = ss[1][0];
                        CharTableArray[i] = c;
                        CharTableArrayRev[c] = i;
                        maxCharCode = Convert.ToInt32(ss[0], 16);
                    }
                }
            }
        }

        /// <summary>
        /// 将编码转换为字符。
        /// </summary>
        /// <param name="i">编码</param>
        /// <returns></returns>
        public char GetCharacter(int i)
        {
            if (CharTableArray[i] > 0)
                return CharTableArray[i];
            else
                return '\0';
        }

        /// <summary>
        /// 将字符转换为编码。
        /// </summary>
        /// <param name="c">字符</param>
        /// <returns></returns>
        public int WriteCharacter(char c)
        {
            if (CharTableArrayRev[c] > 0)
                return CharTableArrayRev[c];
            else
                return 0;
        }
    }
}
