using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MokomoGames
{
    public static class StringBuilderExtention
    {
        public static void AppendLineWithTab(this StringBuilder self, int tabNum, string message)
        {
            for (int i = 0; i < tabNum; i++)
            {
                self.Append("\t");
            }
            self.AppendLine(message);
        }
    }
}
