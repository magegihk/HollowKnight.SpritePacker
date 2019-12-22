using System.IO;

namespace HollowKnight.SpritePacker
{
    internal class Frame
    {
        public FileInfo info;
        public SpriteInfo spriteInfo;
        public Sprite sprite;
        public Collection collection;

        public Frame(FileInfo _info, SpriteInfo _spriteInfo)
        {
            info = _info;
            spriteInfo = _spriteInfo;
            sprite = GetSprite(_info, _spriteInfo);
            collection = Collection.GetCollectionByName(sprite.collectionname);
            collection.frames.Add(this);
        }
        public Frame(FileInfo _info)
        {
            info = _info;
        }
        private Sprite GetSprite(FileInfo _info, SpriteInfo _spriteInfo)
        {
            if (_spriteInfo == null)
            {
                return null;
            }
            for (int i = 0; i < _spriteInfo.Length; i++)
            {
                if (_info.FullName.EndsWith(_spriteInfo[i].path.Replace("/", "\\")))
                {
                    return _spriteInfo[i];
                }
            }
            return null;
        }
    }
}