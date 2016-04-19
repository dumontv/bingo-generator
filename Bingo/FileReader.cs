using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bingo
{
    static class FileReader
    {
        //Return a string array if the file could be accessed, null if not
        //Read files from the Data directory, located anywhere you want within the game folder.
        public static List<String> ReadFile(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
                String line = sr.ReadToEnd();
                String[] valuesArray = line.Split(',');
                List<String> values = new List<string>();
                
                for (int i = 0; i < valuesArray.Length; i++)
                    values.Add(valuesArray[i]);

                return values;
            }
            
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
