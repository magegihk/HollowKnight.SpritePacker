using System.Collections.Generic;
using System.IO;

namespace HollowKnight.SpritePacker
{
    internal class SpritesFolder
    {
        public DirectoryInfo info;
        public List<Animation> animations;

        public SpritesFolder(DirectoryInfo _info)
        {
            info = _info;
            animations = FindAllAnimations();
        }

        private List<Animation> FindAllAnimations()
        {
            List<Animation> animations = new List<Animation>();
            foreach (var anim in info.GetDirectories())
            {
                animations.Add(new Animation(anim));
            }
            return animations;
        }
    }
}