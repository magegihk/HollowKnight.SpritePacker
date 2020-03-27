using System.Collections.Generic;
using System.IO;

namespace HollowKnight.SpritePacker
{
    internal class Clip
    {
        public DirectoryInfo info;
        public SpriteInfo spriteInfo;
        public List<Frame> frames;

        public Clip(DirectoryInfo _info, SpriteInfo _spriteInfo)
        {
            info = _info;
            spriteInfo = _spriteInfo;
            frames = FindAllFrames(_info, _spriteInfo);
        }

        private List<Frame> FindAllFrames(DirectoryInfo _info, SpriteInfo _spriteInfo)
        {
            List<Frame> frames = new List<Frame>();
            foreach (var frame in _info.GetFiles())
            {
                if (frame.Name.Contains("-") && frame.Name.Length == 14 && !frame.Name.Contains("position") && !frame.Name.Contains("backup"))
                {
                    try
                    {
                        frames.Add(new Frame(frame, _spriteInfo));
                    }
                    catch (System.Exception)
                    {
                        continue;
                        throw;
                    }
                }
            }
            return frames;
        }
    }
}