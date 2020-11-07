using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeStar : Star
{
    [SerializeField] private List<Sprite> type_sprite;
    
    private SpriteRenderer m_renderer;
    // Start is called before the first frame update
    
    public void SetSprite(){        
        m_renderer = GetComponent<SpriteRenderer>();

        int rand = Random.Range(0, type_sprite.Count);
        m_renderer.sprite = type_sprite[rand];
    }
}
