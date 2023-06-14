using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    public float moveSpeed;
    public int currentStack;
    public Animator anim;
    public Rigidbody rigid;
    public Joystick joystick;
    public Transform offsetCamera;
    public Transform stackParent;
    private Vector3 rotateDirection;
    private Vector3 limitPosition;
    public List<Stack> listStack = new List<Stack>();
    public StateAnimation stateAnimation;

    public enum StateAnimation
    {
        Idle,
        Run
    }

    public void Active(Vector3 _pos,Vector3 _limitPosition)
    {
        limitPosition = _limitPosition;
        transform.position = _pos;
        transform.rotation = Quaternion.identity;
        currentStack = 0;
        gameObject.SetActive(true); 
        anim.SetTrigger("IsIdle");
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void LateUpdate()
    {
        offsetCamera.transform.position = transform.position;
    }

    private IEnumerator C2_GetStack;
    private IEnumerator C_GetStack(Storage _storage)
    {
        int i = 0;
        int maxStack = (int)(GameManager.Instance.canonDiceGame.pixelArt.listPixel.Count * 0.3f);

        while (true)
        {
            if(_storage.Number > 0 && listStack.Count <= maxStack)
            {
                GetStack();
                int n = _storage.Number;
                n--;
                _storage.UpdateNumber(n);             
            }

            i++;
            if(i % 4 == 0) yield return null;
        }
    }

    public void GetStack()
    {
        Stack _stack = PoolStack.Instance.GetStackInPool();
        _stack.transform.SetParent(stackParent);
        _stack.transform.localPosition = Vector3.up * _stack.transform.localScale.y * listStack.Count;
        _stack.transform.localRotation = Quaternion.identity;

        _stack.gameObject.SetActive(true);
        listStack.Add(_stack);
    }

    private void UpdateMovement()
    {
        Vector3 _dir = new Vector3(joystick.Direction.x, 0.0f, joystick.Direction.y);
        rigid.velocity = _dir * moveSpeed;
        rotateDirection = _dir;
        if(rotateDirection != Vector3.zero) 
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateDirection), Time.deltaTime * 10.0f);


        if (_dir.magnitude > 0.0f)
        {
            if(stateAnimation == StateAnimation.Idle)
            {
                stateAnimation = StateAnimation.Run;
                anim.SetTrigger("IsRun");
            }
        }
        else
        {
            if (stateAnimation == StateAnimation.Run)
            {
                stateAnimation = StateAnimation.Idle;
                anim.SetTrigger("IsIdle");
            }
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -limitPosition.x - 3.0f, limitPosition.x + 3.0f);
        pos.z = Mathf.Clamp(pos.z, -limitPosition.z - 6.0f, limitPosition.z + 3.0f);
        transform.position = pos;
    }

    public void HitStackTrigger(bool _isExit, Storage _Storage)
    {
        if (_isExit)
        {
            if (C2_GetStack != null)
                StopCoroutine(C2_GetStack);
        }
        else
        {
            Storage _s = _Storage;
            C2_GetStack = C_GetStack(_s);
            StartCoroutine(C2_GetStack);
        }
    }

    public void FillStackToPixel(Pixel _pixel)
    {
        Stack _stack = GetStackFromMy();
        if (_stack == null) return;
        _pixel.isFilled = true;
        _stack.transform.parent = GameManager.Instance.canonDiceGame.pixelArt.art.transform;
        Vector3 pos = _pixel.transform.position + Vector3.up * _pixel.transform.localScale.y * 0.5f;
        Vector3 scale = _pixel.transform.localScale;
        Vector3 rot = Vector3.zero;
        _stack.Active(pos, rot, scale, _pixel.mainColor);
    }

    private Stack GetStackFromMy()
    {
        if (listStack.Count == 0) return null;

        Stack _stack  = listStack[listStack.Count - 1];
        listStack.Remove(_stack);
        return _stack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StackTrigger"))
        {
            Storage _s = other.transform.parent.GetComponent<Storage>();
            HitStackTrigger(false, _s);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StackTrigger"))
        {
            HitStackTrigger(true, null);
        }
    }
}
