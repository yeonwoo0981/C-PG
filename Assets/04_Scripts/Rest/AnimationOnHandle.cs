using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationOnHandle : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprite = new List<Sprite>();
    [SerializeField] private float _animationPlaySpeed = 0.1f;

    private void Start()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        for (int i = 0; i < _sprite.Count; i++)
        {
            yield return new WaitForSeconds(_animationPlaySpeed);
            GetComponent<Image>().sprite = _sprite[i];
        }
        
    }
}
