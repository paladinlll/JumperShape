using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MAnim : MonoBehaviour {
    //public string _spriteName;
    //public string _animationName;
    Sprite[] _spritesSequence;

    int m_loop = -1;
    bool m_bPauseAtEnd = false;
    bool m_bIsAnimOver = false;
    public bool IsAnimOver
    {
        get { return m_bIsAnimOver; }
    }

    string m_currentAnim;
    public string SpritesSequence
    {
        get
        {
            return m_currentAnim;
        }
        set
        {
            if(m_currentAnim == value)
            {
                return;
            }
            m_currentAnim = value;
			_spritesSequence = SpriteResourceManager.GetSprites(m_currentAnim);
            _curFrame = 0;
			m_localSpeed = 1.0f;
            m_bIsAnimOver = false;
			if (_spritesSequence.Length > 0)
			{                          
				SetSprite(_spritesSequence[0]);
			}
			else
			{
				SetSprite(null);
			}
        }
    }

	private float m_localSpeed;
	public float LocalSpeed
	{
		
		set
		{
			m_localSpeed = value;
		}
	}

    public void SetLoop(int loop = -1, bool bPauseAtEnd = false)
    {
        m_loop = loop;
        m_bPauseAtEnd = bPauseAtEnd;        
    }
    Image _image;
    public Image GetImage()
    {
        if (_image == null)
        {            
            _image = transform.GetComponent<Image>();
        }
        return _image;
    }

    RectTransform _drawRect;
    RectTransform DrawRect
    {
        get
        {
            if (_drawRect == null)
            {
                _drawRect = transform.GetComponent<RectTransform>();
            }
            return _drawRect;
        }
    }

    int _curFrame;
    float _curTime;


    void SetSprite(Sprite sprite)
    {
        if(sprite == null)
        {
            GetImage().color = new Color32(0, 0, 0, 0);
        }
        else if (GetImage().sprite == null)
        {
            GetImage().color = Color.white;
        }
        

        GetImage().sprite = sprite;
        if (sprite)
        {
            DrawRect.sizeDelta = new Vector2(sprite.rect.size.x / 1.0f, sprite.rect.size.y / 1.0f);
        }
    }

    public Sprite GetCurrentSprite()
    {
        return GetImage().sprite;
    }
	// Update is called once per frame
	void Update ()
    {
        if (_spritesSequence == null || _spritesSequence.Length == 0 || m_loop == 0)
        {
            return;
        }

        float duration = 0.1f;
        while (_curTime >= duration && m_loop != 0)
        {
            _curTime -= duration;
            //
            if(m_loop < 0)
            {
                _curFrame = (_curFrame + 1) % _spritesSequence.Length;
            }
            else
            {
                _curFrame++;
                if(_curFrame == _spritesSequence.Length)
                {
                    m_loop--;
                    if (m_loop == 0)
                    {
                        m_bIsAnimOver = true;
                        if (m_bPauseAtEnd)
                        {
                            _curFrame = _spritesSequence.Length - 1;
                        }
                        else
                        {
                            _curFrame = -1;
                        }                        
                    }
                    else
                    {
                        _curFrame = 0;
                    }
                }
            }
            if (_curFrame == -1)
            {
                SetSprite(null);
            }
            else
            {
                SetSprite(_spritesSequence[_curFrame]);
            }
        }
		_curTime += Time.deltaTime * m_localSpeed;
	}
}
