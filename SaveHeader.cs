using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD4SaveTool
{
    public class SaveHeader
    {
        private string savefile;
        public string gamescripts;
        public SaveHeader(string savefile)
        {
            this.savefile = savefile;
        }

        /// <summary>
        /// Parses the save file for the debug gamescripts that can be found when you are working with a savegame file that has the 'developer' DVAR enabled for the save.
        /// </summary>
        public void ParseGameScripts()
        {
            
            try
            {
                byte[] svg = null;

                svg = File.ReadAllBytes(savefile);

                int offset = 0;

                int blocksCount = 0;
                int currentBlockSize = 0;

                string blockstr = string.Empty;

                List<byte[]> blocks = new List<byte[]>();

                int pos = 0;

                if (svg.Length > 0)
                {
                    Dbg.ln($"SVG File Read. Length: {svg.Length} bytes");

                    //Read the relative offset for our gamescript's
                    int size1 = Helper.ReadIntFromByteArray(svg, 0x450);
                    Dbg.ln($"Parsing size1 : {size1}");
                    offset = 0x450 + size1 + 0x4; //Apparently we need to skip the four bytes as they are unrelated

                    //Read the number of blocks expected to be read
                    blocksCount = Helper.ReadIntFromByteArray(svg, offset + pos);
                    pos += 0x4;

                    Dbg.ln($"blocksCount : {blocksCount}");

                    //Loop through all of the blocks and parse them
                    for (int i = 0; i < blocksCount; i++)
                    {
                        currentBlockSize = Helper.ReadIntFromByteArray(svg, offset + pos);
                        pos += 0x4;

                        byte[] buffer = Helper.RangeSubset(svg, offset + pos, currentBlockSize);

                        blocks.Add(buffer);
                        pos += currentBlockSize;

                        blockstr += ASCIIEncoding.UTF8.GetString(buffer).Replace("\x00", "");
                    }

                    Dbg.ln($"Offset: {(offset + pos).ToString("X2")}");

                }
                else
                {
                    Dbg.ln("Failed to read savefile. Make sure a single *compatible* svg file is found in the application directory.");
                }

                gamescripts = blockstr;
            }
            catch(Exception ee)
            {
                Dbg.ln(ee.Message);
            }
        }
    }
}
