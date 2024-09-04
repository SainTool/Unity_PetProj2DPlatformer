using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPath : MonoBehaviour
{
    public enum PathTypes // библиотека в которой движение по линии или по циклу 
    {
        linear,
        loop
    }
    public PathTypes PathType; // определение вида пути
    public int MovementDirection = 1; // направление движения : по или прот часовой
    public int moveingTo = 0;         // к какой точке двигаться
    public Transform[] PathElements;  // массив из точек движения
    public void OnDrawGizmos() //рисование линий между точек
    {
        if(PathElements == null || PathElements.Length < 2)
        {
            return;
        }
        for(var i =1; i < PathElements.Length; i++) // отрисовка всех точек между об
        {
            Gizmos.DrawLine(PathElements[i-1].position, PathElements[i].position);
        }
        if(PathType == PathTypes.loop) // если луп
        {
            Gizmos.DrawLine(PathElements[0].position, PathElements[PathElements.Length-1].position);
        }
    }

    public IEnumerator<Transform> GetNextPathPoint()
    {
        if (PathElements == null || PathElements.Length < 1)
        {
            yield break;
        }
        while (true)
        {
            yield return PathElements[moveingTo];

            if(PathElements.Length == 1)
            {
                continue;
            }

            if(PathType == PathTypes.linear)
            {
                if(moveingTo <= 0)
                {
                    MovementDirection = 1;
                }
                else if(moveingTo >= PathElements.Length - 1)
                {
                    MovementDirection = -1;
                }
            }

            moveingTo = moveingTo + MovementDirection;

            if(PathType == PathTypes.loop)
            {
                if (moveingTo >= PathElements.Length)
                {
                    moveingTo = 0;
                }
                if(moveingTo < 0)
                {
                    moveingTo = PathElements.Length - 1;
                }
            }
        }
    }
}
