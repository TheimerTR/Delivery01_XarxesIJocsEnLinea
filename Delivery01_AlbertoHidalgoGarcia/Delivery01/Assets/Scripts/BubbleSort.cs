using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class BubbleSort : MonoBehaviour
{
    public enum threadPipeline
    {
        swapChange,
        noChange
    }

    float[] array;
    List<GameObject> mainObjects;
    threadPipeline statusChange = threadPipeline.noChange;
    public GameObject prefab;

    float[] arrayInsert;
    List<GameObject> mainObjectsInsert;
    threadPipeline statusChangeInsert = threadPipeline.noChange;
    public GameObject prefabInsert;

    Thread gmInstanceBubbleHeight;
    Thread gmInstanceInsertHeight;

    void Start()
    {
        mainObjects = new List<GameObject>();
        array = new float[30000];
        for (int i = 0; i < 30000; i++)
        {
            array[i] = (float)Random.Range(0, 1000)/100;
        }

        mainObjectsInsert = new List<GameObject>();
        arrayInsert = new float[30000];
        for (int i = 0; i < 30000; i++)
        {
            arrayInsert[i] = (float)Random.Range(0, 1000) / 100;
        }

        //TO DO 4
        //Call the three previous functions in order to set up the exercise
        logArray(array);
        logArray(arrayInsert);

        spawnObjs(mainObjects, prefab);
        spawnObjs(mainObjectsInsert, prefabInsert, 10);

        statusChange = updateHeights(array, mainObjects);
        statusChangeInsert = updateHeights(arrayInsert, mainObjectsInsert);

        //TO DO 5
        //Create a new thread using the function "bubbleSort" and start it.
        gmInstanceBubbleHeight = new Thread(bubbleSort);
        gmInstanceBubbleHeight.Start();

        gmInstanceInsertHeight = new Thread(InsertSort);
        gmInstanceInsertHeight.Start();
    }

    void Update()
    {
        //TO DO 6
        //Call ChangeHeights() in order to update our object list.
        //Since we'll be calling UnityEngine functions to retrieve and change some data,
        //we can't call this function inside a Thread

        //if (Input.GetKeyDown(KeyCode.Space) && !gmInstanceBubbleHeight.IsAlive)
        //{
        //    gmInstanceBubbleHeight.Abort();
        //    Shuffle();
        //    gmInstanceBubbleHeight.Start();
        //    Debug.Log("Shuffle!");
        //}

        if (statusChange != threadPipeline.noChange)
        {
            statusChange = updateHeights(array, mainObjects);
            Debug.Log("Changing");
        }
        
        if (statusChangeInsert != threadPipeline.noChange)
        {
            statusChangeInsert = updateHeights(arrayInsert, mainObjectsInsert);
            Debug.Log("Changing");
        }
    }

    //private void Shuffle()
    //{
    //    for (int i = 0; i < 50; i++)
    //    {
    //        int rNumb1 = Random.RandomRange(0, 30000);
    //        int rNumb2 = Random.RandomRange(0, 30000);

    //        float tmpNumb = array[rNumb1];
    //        array[rNumb1] = array[rNumb2];
    //        array[rNumb2] = tmpNumb;
    //    }
    //}

    //TO DO 5
    //Create a new thread using the function "bubbleSort" and start it.
    void bubbleSort()
    {
        int i, j;
        int n = array.Length;
        bool swapped;
        for (i = 0; i < n- 1; i++)
        {
            swapped = false;
            for (j = 0; j < n - i - 1; j++)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j+1]) = (array[j+1], array[j]);
                    swapped = true;
                    statusChange = threadPipeline.swapChange;
                }
            }
            if (swapped == false)
                break;
        }
        //You may debug log your Array here in case you want to. It will only be called one the bubble algorithm has finished sorting the array
    }

    void InsertSort()
    {
        for (int i = 1; i < arrayInsert.Length; ++i)
        {
            float key = arrayInsert[i];
            int j = i - 1;

            while (j >= 0 && arrayInsert[j] > key)
            {
                arrayInsert[j + 1] = arrayInsert[j];
                j = j - 1;
            }

            statusChangeInsert = threadPipeline.swapChange;
            arrayInsert[j + 1] = key;
        }
    }

    void logArray(float[] arr)
    {
        string text = "";

        //TO DO 1
        //Simply show in the console what's inside our array.

        for(int i = 0; i < arr.Length; i++)
        {
            text = arr[i].ToString();
            Debug.Log(text);
        }
    }
    
    void spawnObjs(List<GameObject> objects, GameObject pref, int offset = 0)
    {
        //TO DO 2
        //We should be storing our objects in a list so we can access them later on.

        for (int i = 0; i < array.Length; i++)
        {
            //We have to separate the objs accordingly to their width, in which case we divide their position by 1000.
            //If you decide to make your objs wider, don't forget to up this value
            GameObject gm = Instantiate(pref, new Vector3((float)i / 1000, this.gameObject.GetComponent<Transform>().position.y - offset, 0), Quaternion.identity);
            objects.Add(gm);
        }

    }

    //TO DO 3
    //We'll just change the height of every obj in our list to match the values of the array.
    //To avoid calling this function once everything is sorted, keep track of new changes to the list.
    //If there weren't, you might as well stop calling this function

    threadPipeline updateHeights(float[] arr, List<GameObject> objects)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            objects[i].gameObject.transform.localScale = new Vector3(objects[i].gameObject.transform.localScale.x, arr[i], objects[i].gameObject.transform.localScale.z);
        }

        threadPipeline status = threadPipeline.noChange;

        return status;
    }
}
