using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARController : MonoBehaviour
{
    //рисовать ли луч дебага и какой длинны
    public bool DrawRay = true;
    public int RayLength = 50;

    //время срабатывания в секундах
    public float TriggerTime = 1f;


    void Update()
    {
        GameObject objWeLookOn;

        //здесь делаются всякие реакции на взгляд на обьект
        // то есть вызывать метод проигрывания звука нужно будет отсюда
        if (CheckIfWeLookOnObject(out objWeLookOn))
        {
            Debug.Log("I'm looking on" + objWeLookOn.name);
        }
    }

    //эти 2 переменные я разместил именно здесь, а не выше потому, что они
    //используются только в этом методе. Если бы использовались еще где-то
    //я бы разместил их над всеми методами
    private GameObject _lastObjWeLookedOn;
    private float _timeOfLook = 0;
    //метод возвращает тру если мы смотрим на обьект triggerTime времени и в аут запихивает сам обьект на который мы смотрим
    public bool CheckIfWeLookOnObject(out GameObject go)
    {
        RaycastHit hit;

        //рисуем луч в эдиторе если нужно для дебага
        //в рантайме видно не будет. 
        //Что бы было видно в рантайме используй lineRenderer компонет
        if (DrawRay)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward) * RayLength;
            Debug.DrawRay(transform.position, forward, Color.green);
        }

        //если ни на что не смотрим -- выходим из метода
        //и обнуляем сохраненные данные
        if (!Physics.Raycast(transform.position, transform.forward, out hit))
        {
            _timeOfLook = 0;
            _lastObjWeLookedOn = null;
            go = null;
            return false;
        }

        //Проверяем на какой обьект мы смотрим
        var currObj = hit.transform.gameObject;

        //если это новый обьект, обнуляемся
        if (_lastObjWeLookedOn != currObj)
        {
            _timeOfLook = 0;
            _lastObjWeLookedOn = currObj;
        }

        //если прошел период времени -- возвращаем тру и геймобджект на который смотрим
        if (_timeOfLook > TriggerTime)
        {
            go = hit.collider.gameObject;
            _timeOfLook = 0;
            return true;
        }

        //иначе - возвращаем ничего, но считаем время т.к. мы продолжаем смотреть на этот же обьект
        go = null;
        _timeOfLook += Time.deltaTime;
        return false;
    }
}