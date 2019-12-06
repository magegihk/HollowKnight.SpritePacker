using System.Collections.Generic;
using System.IO;

namespace HollowKnight.SpritePacker
{
    internal class Collection
    {
        public string name;
        public FileInfo info;
        public List<Frame> frames = new List<Frame>();
        public static List<Collection> collections = new List<Collection>();
        public static List<Collection> gencollections = new List<Collection>();

        public Collection(string _name, FileInfo _info)
        {
            name = _name;
            info = _info;
        }

        public void SortFrame()
        {
            frames.Sort(new FrameComparer());
        }

        public static Collection GetCollectionByName(string _name)
        {
            foreach (var collection in collections)
            {
                if (collection.name == _name)
                {
                    return collection;
                }
            }
            return null;
        }

        public static Collection GetGenCollectionByName(string _name)
        {
            foreach (var collection in gencollections)
            {
                if (collection.name == _name)
                {
                    return collection;
                }
            }
            return null;
        }

        private class FrameComparer : IComparer<Frame>
        {
            public int Compare(Frame a, Frame b)
            {
                return a.info.Name.Substring(7, 3).CompareTo(b.info.Name.Substring(7, 3));
            }
        }
    }
}