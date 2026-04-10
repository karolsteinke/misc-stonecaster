using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    public bool dying {get; private set;}
    private SpriteRenderer _sprite;
    private Shader _shaderGUIText;
    private Shader _shaderSpritesDefault;
    private Color _startColor;
    
    void Start() {
        _sprite = GetComponent<SpriteRenderer>();
        _shaderGUIText = Shader.Find("GUI/Text Shader");
        _shaderSpritesDefault = Shader.Find("Sprites/Default");
        _startColor = _sprite.color;
        dying = false;
    }

    public void ReactToHit() {
        if (!dying) {
            dying = true;
            StartCoroutine(BlinkToDie());
        }
    }

    private IEnumerator BlinkToDie() {
        //blink 3x
        WhiteSprite();  yield return new WaitForSeconds(.1f);
        NormalSprite(); yield return new WaitForSeconds(.2f);
        WhiteSprite();  yield return new WaitForSeconds(.1f);
        NormalSprite(); yield return new WaitForSeconds(.2f);
        WhiteSprite();  yield return new WaitForSeconds(.1f);

        Die();
    }

    //public Die() method to be called by itself and EvolutionTrigger
    public void Die() {
        Destroy(this.gameObject);
    }

    private void WhiteSprite() {
        _sprite.material.shader = _shaderGUIText;
        _sprite.color = Color.white;
    }

    private void NormalSprite() {
        _sprite.material.shader = _shaderSpritesDefault;
        _sprite.color = _startColor;
    }
}
