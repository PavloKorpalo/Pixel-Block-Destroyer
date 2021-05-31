using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    [SerializeField] private int _hitPoints;
    private SpriteRenderer _spriteRenderer;

    public static event Action<Brick> OnBrickDestruction;
    public ParticleSystem DestroyEffect;

    private void Awake()
    {
        this._spriteRenderer = this.GetComponent<SpriteRenderer>();
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        this._hitPoints--;

        if (this._hitPoints <= 0)
        {
            BrickManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this._spriteRenderer.sprite = BrickManager.Instance.Sprites[this._hitPoints - 1];
        }


    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mainModule = effect.GetComponent<ParticleSystem>().main;
        mainModule.startColor = this._spriteRenderer.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

   

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitPoints)
    {
        this.transform.SetParent(containerTransform);
        this._spriteRenderer.sprite = sprite;
        this._spriteRenderer.color = color;
        this._hitPoints = hitPoints;
    }
}
