using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonCTR
{
    class Text
    {
        public readonly List<List<string>> TextList = new List<List<string>>();

        /// <summary>
        /// 从文本（message）文件根据码表读取文本。
        /// </summary>
        /// <param name="files">文本（message）文件</param>
        /// <param name="charTable">码表</param>
        public Text (Narc files,CharTable charTable)
        {
            foreach (byte[] bytes in files.Files)
            {
                List<string> s = new List<string>();
                BinaryReader br = new BinaryReader(new MemoryStream(bytes));
                int stringCount, originalKey;
                bool flag = false, flag2 = false;
                stringCount = br.ReadUInt16();
                originalKey = br.ReadUInt16();
                int num = ((originalKey * 0x2fd) & 0xffff);
                int[] numArray = new int[stringCount];
                int[] numArray2 = new int[stringCount];
                for (int i = 0; i < stringCount; i++)
                {
                    int num2 = (num * (i + 1)) & 0xffff;
                    int num3 = num2 | (num2 << 16);
                    numArray[i] = br.ReadInt32();
                    numArray[i] = numArray[i] ^ num3;
                    numArray2[i] = br.ReadInt32();
                    numArray2[i] = numArray2[i] ^ num3;
                }
                for (int j = 0; j < stringCount; j++)
                {
                    num = ((0x91bd3 * (j + 1)) & 0xffff);
                    string text = "";
                    for (int k = 0; k < numArray2[j]; k++)
                    {
                        int num4 = br.ReadUInt16();
                        num4 = (num4 ^ num);
                        if (num4 == 57344 || num4 == 9660 || num4 == 9661 || num4 == 61696 || num4 == 65534 || num4 == 65535)
                        {
                            if (num4 == 57344)
                                text += ("\\n");
                            if (num4 == 9660)
                                text += ("\\r");
                            if (num4 == 9661)
                                text += ("\\f");
                            if (num4 == 61696)
                                flag2 = true;
                            if (num4 == 65534)
                            {
                                text += ("\\v");
                                flag = true;
                            }
                        }
                        else
                        {
                            if (flag)
                            {
                                text += Convert.ToString(num4, 16).PadLeft(4, '0');
                                flag = false;
                            }
                            else
                            {
                                if (flag2)
                                {
                                    int num5 = 0;
                                    int num6 = 0;
                                    string str = null;
                                    while (true)
                                    {
                                        if (num5 >= 15)
                                        {
                                            num5 -= 15;
                                            if (num5 > 0)
                                            {
                                                int num8 = (num6 | (num4 << 9 - num5 & 511));
                                                if ((num8 & 255) == 255)
                                                    break;
                                                if (num8 != 0 && num8 != 1)
                                                {
                                                    char str2 = charTable.GetCharacter(num8);
                                                    text += charTable.GetCharacter(num8);
                                                    if (str2 == '\0')
                                                    {
                                                        text += "\\x" + Convert.ToString(num8, 16).PadLeft(4, '0');
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int num8 = (num4 >> num5 & 511);
                                            if ((num8 & 255) == 255)
                                                break;
                                            if (num8 != 0 && num8 != 1)
                                            {
                                                char str3 = charTable.GetCharacter(num8);
                                                text += str3;
                                                if (str3 == '\0')
                                                {
                                                    text += "\\x" + Convert.ToString(num8, 16).PadLeft(4, '0');
                                                }
                                            }
                                            num5 += 9;
                                            if (num5 < 15)
                                            {
                                                num6 = (num4 >> num5 & 511);
                                                num5 += 9;
                                            }
                                            num += 18749;
                                            num &= 65535;
                                            num4 = br.ReadUInt16();
                                            num4 ^= num;
                                            k++;
                                        }
                                    }
                                    text += str;
                                }
                                else
                                {
                                    char str3 = charTable.GetCharacter(num4);
                                    text += str3;
                                    if (str3 == '\0')
                                    {
                                        text += "\\x" + Convert.ToString(num4, 16).PadLeft(4, '0');
                                    }
                                }
                            }
                        }
                        num += 18749;
                        num &= 65535;
                    }
                    s.Add(text);
                }
                TextList.Add(s);
            }
        }
    }
}
