using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEditor;
public class BubbleBehaviour : MonoBehaviour
{
    [SerializeField] private Material _outerBubble = null;
    [SerializeField] private Material _innerBubble = null;
    [SerializeField] private Material _blackAndWhiteMat = null;
    [SerializeField] private AnimationCurve _innerCurve = AnimationCurve.Linear(0,0,1,1);
    [SerializeField] private AnimationCurve _outerCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _bwCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField] private float _startDistance = 4;

    [SerializeField] private AnimationCurve _powerFadeOut = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _transiFadeOut = AnimationCurve.Linear(0, 0, 1, 1);

    private Controller _controller;
    private bool _isWin = false;

    private void Start()
    {
        _controller = GameObject.FindAnyObjectByType<Controller>();
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += RestMat;
#endif
    }

    private void Update()
    {
        if(_isWin)
            return;
        float distance = Vector3.Distance(_controller.transform.position, transform.position);
        if (distance <= _startDistance)
        {
            float factor =  distance / _startDistance;
            _outerBubble.SetFloat("_TransitionVisible", Mathf.Lerp(0, 1, _outerCurve.Evaluate(factor)));
            _innerBubble.SetFloat("_TransitionVisible", Mathf.Lerp(1, 0, _innerCurve.Evaluate(factor)));
            _blackAndWhiteMat.SetFloat("_TransitionColor", Mathf.Lerp(1, 0, _bwCurve.Evaluate(factor)));
        }
    }

    [ContextMenu("WinAnim")]
    public void OnWin()
    {
        _isWin = true;
        //_blackAndWhiteMat.SetFloat("_TransitionColor",1);
        _outerBubble.DOFloat(0, "_TransiFadeOut", 5).SetEase(_transiFadeOut);
        _innerBubble.DOFloat(0, "_TransiFadeOut", 5).SetEase(_transiFadeOut);
        _blackAndWhiteMat.DOFloat(1, "_TransitionColor", 5).SetEase(_transiFadeOut);
        _innerBubble.DOFloat(35, "_PowerNoise", 10).SetEase(_powerFadeOut);
        _outerBubble.DOFloat(35, "_PowerNoise", 10).SetEase(_powerFadeOut).OnComplete(() => 
        {
            Destroy(gameObject);
        });
    }
#if UNITY_EDITOR
    private void RestMat(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredEditMode)
            return;
        _innerBubble.SetFloat("_TransitionVisible", 0);
        _outerBubble.SetFloat("_TransitionVisible", 1);
        _outerBubble.SetFloat("_TransiFadeOut", 50);
        _outerBubble.SetFloat("_PowerNoise", 1);
        _innerBubble.SetFloat("_TransiFadeOut", 50);
        _outerBubble.SetFloat("_PowerNoise", 1);
        _blackAndWhiteMat.SetFloat("_TransitionColor",0);
    }
#endif
}

