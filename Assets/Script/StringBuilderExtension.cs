using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MokomoGames
{
    public static class StringBuilderExtention
    {
        public static void AppendLineWithTab(this StringBuilder self, bool enableTab, string message)
        {
            if(enableTab)
                self.Append("\t");
            self.AppendLine(message);
        }
    }
}
