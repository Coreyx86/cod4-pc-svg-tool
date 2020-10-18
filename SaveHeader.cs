using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace COD4SaveTool
{
    public class BlockObject
    {
        public int sizeOffset = 0;
        public int size = 0;
        public byte[] buffer = null;
        public string bufferString = string.Empty;

        public bool LittleEndian = true;

        public BlockObject()
        {

        }

        public byte[] ToByteArray()
        {
            int len = this.buffer.Length;

            byte[] tmp = new byte[4 + len];

            byte[] sizeBuffer = BitConverter.GetBytes(size);
            if(!LittleEndian)
            {
                Array.Reverse(sizeBuffer);
            }

            Helper.SetByteArrayInArray(tmp, 0, sizeBuffer);
            Helper.SetByteArrayInArray(tmp, 4, this.buffer);

            return tmp;
        }
    }
    public class SaveHeader
    {
        private string savefile;
        private int blockCountOffset = 0;

        public string gamescripts;

        public bool LittleEndian = true;

        public byte[] svg = null;

        public List<byte> svg_tmp = new List<byte>();

        public List<BlockObject> blocks = new List<BlockObject>();


        public SaveHeader(string savefile)
        {
            this.savefile = savefile;
        }

        private List<BlockObject> ConvertTextToBlockObjectArray(string text)
        {
            int maxBlockSize = Int32.MaxValue;
            int blockSize = 250000;

            int offset = blockCountOffset + 4;

            List<BlockObject> buffer = new List<BlockObject>();
            if(text.Length <= blockSize)
            {
                BlockObject bObj = new BlockObject();
                bObj.bufferString = text;//.Replace("\r\n", "\x00");
                bObj.buffer = ASCIIEncoding.UTF8.GetBytes(bObj.bufferString);
                bObj.size = bObj.buffer.Length;
                bObj.sizeOffset = offset;
            }
            else
            {
                //text = text.Replace("\r\n", "\x00");

                string[] textChunks = Helper.ChunksUpto(text, blockSize).ToArray();

                for(int i = 0; i < textChunks.Length; i++)
                {
                    BlockObject bObj = new BlockObject();
                    bObj.bufferString = textChunks[i];
                    bObj.buffer = ASCIIEncoding.UTF8.GetBytes(bObj.bufferString);
                    bObj.size = bObj.buffer.Length;
                    bObj.sizeOffset = offset;

                    offset += 4 + bObj.size;

                    buffer.Add(bObj);
                }
            }

            return buffer;
        }

        public void SaveGameScripts()
        {
            if(svg != null)
            {
                List<byte> tmp = new List<byte>();

                int offset = blockCountOffset;

                string text = File.ReadAllText("modded.gsc");

                var blocks = ConvertTextToBlockObjectArray(text);

                byte[] blockCountBuffer = BitConverter.GetBytes(blocks.Count);

                if(!LittleEndian)
                {
                    Array.Reverse(blockCountBuffer);
                }

                svg_tmp.AddRange(blockCountBuffer);

                foreach (BlockObject bObj in blocks)
                {
                    byte[] bObjArr = bObj.ToByteArray();
                    svg_tmp.AddRange(bObjArr);
                }

                File.WriteAllBytes("modded-killhouse.svg", svg_tmp.ToArray());
            }
        }

        /// <summary>
        /// Parses the save file for the debug gamescripts that can be found when you are working with a savegame file that has the 'developer' DVAR enabled for the save.
        /// </summary>
        public void ParseGameScripts()
        {
            Console.WriteLine(LittleEndian);

            svg = File.ReadAllBytes(savefile);
            svg_tmp.Clear();

            int offset = 0;

            int blocksCount = 0;
            int currentBlockSize = 0;

            string blockstr = string.Empty;

            blocks.Clear();
            gamescripts = "";

            int pos = 0;

            if (svg.Length > 0)
            {
                Dbg.ln($"SVG File Read. Length: {svg.Length} bytes");

                //Read the relative offset for our gamescript's
                int size1 = Helper.ReadIntFromByteArray(svg, 0x450, LittleEndian);
                Dbg.ln($"Parsing size1 : {size1}");
                offset = 0x450 + size1 + 0x4; //Apparently we need to skip the four bytes as they are unrelated
                blockCountOffset = offset;

                

                Application.Current.Dispatcher.InvokeAsync(() => { //Read the number of blocks expected to be read
                    byte[] header = Helper.RangeSubset(svg, 0, offset);
                    this.svg_tmp.AddRange(header);
                    File.WriteAllBytes("svg-header.bin", header); 
                });

                blocksCount = Helper.ReadIntFromByteArray(svg, offset + pos, LittleEndian);
                pos += 0x4;

                Dbg.ln($"blocksCount : {blocksCount}");


                //Loop through all of the blocks and parse them
                for (int i = 0; i < blocksCount; i++)
                {
                    BlockObject bObj = new BlockObject();
                    currentBlockSize = Helper.ReadIntFromByteArray(svg, offset + pos, LittleEndian);
                    bObj.sizeOffset = offset + pos;
                    bObj.size = currentBlockSize;
                    pos += 0x4;

                    byte[] buffer = Helper.RangeSubset(svg, offset + pos, currentBlockSize);

                    string tmpStr = ASCIIEncoding.UTF8.GetString(buffer).Replace("\x00", "");
                    gamescripts += tmpStr;

                    bObj.buffer = buffer;
                    bObj.bufferString = tmpStr;

                    blocks.Add(bObj);

                    pos += currentBlockSize;

                }

                Dbg.ln($"Offset: {(offset + pos).ToString("X2")}");

            }
            else
            {
                Dbg.ln("Failed to read savefile. Make sure a single *compatible* svg file is found in the application directory.");
            }

            
            File.WriteAllText("gamescriptfile.gsc", gamescripts);
        }
    }
}
