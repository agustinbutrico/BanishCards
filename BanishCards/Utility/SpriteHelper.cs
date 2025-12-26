using System.Collections.Generic;
using UnityEngine;

namespace BanishCards.Utility
{
    internal class SpriteHelper
    {
        private static readonly Dictionary<string, Sprite> _spriteCache = new Dictionary<string, Sprite>();

        internal static Sprite FindSpriteByName(string spriteName)
        {
            if (_spriteCache.TryGetValue(spriteName, out var sprite))
                return sprite;

            foreach (var s in Resources.FindObjectsOfTypeAll<Sprite>())
            {
                if (s.name == spriteName)
                {
                    _spriteCache[spriteName] = s;
                    return s;
                }
            }

            return null;
        }
    }
}
