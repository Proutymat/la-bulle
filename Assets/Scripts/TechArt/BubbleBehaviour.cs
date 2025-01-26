using UnityEngine;
public class BubbleBehaviour : MonoBehaviour
{
    [SerializeField] private Material _outerBubble = null;
    [SerializeField] private Material _innerBubble = null;
    [SerializeField] private Material _blackAndWhiteMat = null;
    [SerializeField] private AnimationCurve _innerCurve = AnimationCurve.Linear(0,0,1,1);
    [SerializeField] private AnimationCurve _outerCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _bwCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [SerializeField] private float _startDistance = 4;


    private Controller _controller;

    private void Start()
    {
        _controller = GameObject.FindAnyObjectByType<Controller>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(_controller.transform.position, transform.position);
        if (distance <= _startDistance)
        {
            float factor =  distance / _startDistance;
            _outerBubble.SetFloat("_TransitionVisible", Mathf.Lerp(0, 1, _outerCurve.Evaluate(factor)));
            _innerBubble.SetFloat("_TransitionVisible", Mathf.Lerp(1, 0, _innerCurve.Evaluate(factor)));
            _blackAndWhiteMat.SetFloat("_TransitionColor", Mathf.Lerp(1, 0, _bwCurve.Evaluate(factor)));
        }
    }
    private void OnDestroy()
    {
        _innerBubble.SetFloat("_TransitionVisible", 0);
        _outerBubble.SetFloat("_TransitionVisible", 1);
        _blackAndWhiteMat.SetFloat("_TransitionColor",0);
    }
}
