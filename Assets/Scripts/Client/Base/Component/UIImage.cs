using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityGameFramework;

namespace Client
{
	public class UIImage : Image
	{
		[SerializeField]
		private SpriteAtlas _atlas;
		public SpriteAtlas Atlas => _atlas;
		[SerializeField]
		private string _currentSpriteName;

		public Sprite GetSprite(string spriteName)
		{
			if(_atlas != null)
			{
				return _atlas.GetSprite(spriteName);
			}
			return null;
		}

		public List<Sprite> GetSprites()
		{
			var sprites = new Sprite[_atlas.spriteCount];
			_atlas.GetSprites(sprites);
			return new List<Sprite>(sprites);
		}

		public void SetSprite(string spriteName)
		{
			var s = GetSprite(spriteName);
			if (s != null)
			{
				_currentSpriteName = spriteName;
				sprite = s;
			}
		}

		protected override void Start()
		{
			if(!string.IsNullOrEmpty(_currentSpriteName))
			{
				SetSprite(_currentSpriteName);
			}
		}
	}
}
