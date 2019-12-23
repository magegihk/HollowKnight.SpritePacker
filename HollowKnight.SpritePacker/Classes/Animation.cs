using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace HollowKnight.SpritePacker
{
    internal class Animation
    {
        public DirectoryInfo info;
        public SpriteInfo spriteInfo;
        public List<Clip> clips;

        public Animation(DirectoryInfo _info)
        {
            info = _info;

            string Jpath = _info.FullName + "\\0.Atlases\\SpriteInfo.json";
            if (File.Exists(Jpath))
            {
                spriteInfo = JsonConvert.DeserializeObject<SpriteInfo>(File.ReadAllText(Jpath));
            }

            clips = FindAllClips(_info, spriteInfo);
        }

        private List<Clip> FindAllClips(DirectoryInfo _info, SpriteInfo spriteInfo)
        {
            List<Clip> clips = new List<Clip>();
            foreach (var clip in _info.GetDirectories())
            {
                if (!clip.Name.StartsWith("0."))
                    clips.Add(new Clip(clip, spriteInfo));
                else
                {
                    foreach (var atlas in clip.GetFiles())
                    {
                        if (atlas.Name.EndsWith(".png") && !atlas.Name.StartsWith("Gen-"))
                        {
                            if (Collection.GetCollectionByName(atlas.Name) == null)
                            {
                                Collection.collections.Add(new Collection(atlas.Name, atlas));
                            }
                        }
                        if (atlas.Name.EndsWith(".png") && atlas.Name.StartsWith("Gen-"))
                        {
                            if (Collection.GetGenCollectionByName(atlas.Name) == null)
                            {
                                Collection.gencollections.Add(new Collection(atlas.Name, atlas));
                            }
                        }
                    }
                }
            }
            return clips;
        }
    }
}