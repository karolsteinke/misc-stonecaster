using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shootSound;
    private bool reloading = false;
    private SpriteRenderer _sprite;
    private Shader _shaderGUIText;
    private Shader _shaderSpritesDefault;
    private Color _startColor;
    private AudioSource _soundSource;

    void Start() {
        _sprite = GetComponent<SpriteRenderer>();
        _shaderGUIText = Shader.Find("GUI/Text Shader");
        _shaderSpritesDefault = Shader.Find("Sprites/Default");
        _soundSource = GetComponent<AudioSource>();
        _startColor = _sprite.color;
    }

    void FixedUpdate() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 1.92f, LayerMask.GetMask("Player"));

        if (hit.collider && !reloading) {
            StartCoroutine(ShootAndReload());
        }
    }

    private IEnumerator ShootAndReload() {
        reloading = true;
        
        RedSprite();
        yield return new WaitForSeconds(.1f);
        DimmedSprite();

        _soundSource.PlayOneShot(shootSound);
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = new Vector3(
            transform.position.x + transform.localScale.x * 0.08f,
            Mathf.Round(transform.position.y / .32f) * .32f,
            transform.position.z
        );
        bullet.transform.localScale = new Vector3(
            bullet.transform.localScale.x * transform.localScale.x,
            bullet.transform.localScale.y,
            bullet.transform.localScale.z
        );

        yield return new WaitForSeconds(1.8f);
        NormalSprite();
        yield return new WaitForSeconds(0.2f);
        reloading = false;
    }

    private void NormalSprite() {
        _sprite.material.shader = _shaderSpritesDefault;
        _sprite.color = _startColor;
    }

    private void DimmedSprite() {
        _sprite.material.shader = _shaderSpritesDefault;
        _sprite.color = new Color(0,.35f,.55f);
    }

    private void RedSprite() {
        _sprite.material.shader = _shaderGUIText;
        _sprite.color = Color.red;
    }
}
