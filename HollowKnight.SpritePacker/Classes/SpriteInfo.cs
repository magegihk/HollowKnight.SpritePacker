using System;
using System.Collections.Generic;

namespace HollowKnight.SpritePacker
{
    [Serializable]
    public class SpriteInfo
    {
        public List<int> sid;
        public List<int> sx;
        public List<int> sy;
        public List<int> swidth;
        public List<int> sheight;
        public List<int> satlaswidth;
        public List<int> satlasheight;

        public List<string> scollectionname;
        public List<string> spath;

        public List<bool> sfilpped;

        public Sprite this[int i]
        {
            get { return new Sprite(sid[i], sx[i], sy[i], swidth[i], sheight[i], satlaswidth[i], satlasheight[i], scollectionname[i], spath[i], sfilpped[i]); }
        }

        public int Length { get => sid.Count; }

        public SpriteInfo()
        {
            sid = new List<int>();
            sx = new List<int>();
            sy = new List<int>();
            swidth = new List<int>();
            sheight = new List<int>();
            satlaswidth = new List<int>();
            satlasheight = new List<int>();

            scollectionname = new List<string>();
            spath = new List<string>();

            sfilpped = new List<bool>();
        }

        public void Add(int _sid, int _sx, int _sy, int _swidth, int _height, int _satlaswidth, int _satlasheight, string _scollectionname, string _spath, bool _sfilpped)
        {
            sid.Add(_sid);
            sx.Add(_sx);
            sy.Add(_sy);
            swidth.Add(_swidth);
            sheight.Add(_height);
            satlaswidth.Add(_satlaswidth);
            satlasheight.Add(_satlasheight);
            scollectionname.Add(_scollectionname);
            spath.Add(_spath);
            sfilpped.Add(_sfilpped);
        }
    }
}