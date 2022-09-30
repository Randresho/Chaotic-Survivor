using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;


//Build your Credits.txt with an ! to denote headers, # to denote comments, everything else will be considered a name.
//Place your Credits.txt in Assets/Resources
public class CreditsManager : MonoBehaviour
{
    [SerializeField] private string path = "Credits";
    [SerializeField] private int animationCreditNumber = 0;
    [SerializeField] private Font m_font;
    [SerializeField] private Color headerColor = Color.blue;
    [SerializeField] private Color nameColor = Color.white;
    [SerializeField] private int headerSize = 35;
    [SerializeField] private int nameSize = 20;
    [SerializeField] private float scrollSpeed = 8f;
    [SerializeField] private int spaceDivisions = 15;
    [SerializeField] private float distanceScreenToDestroy = 1;
    [SerializeField] private bool scrollCredits = false;

    [SerializeField] List<string> headersLists = new List<string>();
    [SerializeField] List<List<string>> titles = new List<List<string>>();
    [SerializeField] List<GameObject> creditsText = new List<GameObject>();

    [SerializeField] private UiManager uiManager;

    // Start is called before the first frame update
    void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();

        bool newStart = false;
        TextAsset theList = (TextAsset)Resources.Load(path, typeof(TextAsset));
        string[] linesFromfile = theList.text.Split("\n"[0]);
        foreach (string line in linesFromfile)
        {
            string firstCharacter = line.Substring(0,1);
            bool isIgnore = firstCharacter.Equals("#");
            bool isHeader = firstCharacter.Equals("!");
            if(isIgnore)
            {
                //Do Nothing
            }
            else if(isHeader)
            {
                newStart = true;
                headersLists.Add(line.Substring(1));
            }
            else
            {
                if(newStart)
                {
                    titles.Add(new List<string>());
                    newStart = false;
                }
                titles[titles.Count - 1].Add(line);
            }
        }

        if(m_font == null)
        {
            m_font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        }
    }    

    void StartCredits()
    {
        Vector3 lastPosition = new Vector3(Screen.width * 0.5f, 0, 0);
        for (int i = 0; i < headersLists.Count; i++)
        {
            GameObject newObj = newText(headersLists[i], true);
            Vector3 nextPosition = new Vector3(Screen.width * 0.5f, lastPosition.y - (Screen.height / spaceDivisions), 0);
            newObj.transform.position = nextPosition;
            lastPosition = nextPosition;
            creditsText.Add(newObj);
            for (int j = 0; j < titles[i].Count; j++)
            {
                nextPosition = new Vector3(Screen.width * 0.5f, lastPosition.y - (Screen.height / spaceDivisions), 0);
                GameObject oObj = newText(titles[i][j], false);
                oObj.transform.position = nextPosition;
                creditsText.Add(oObj);
                lastPosition = nextPosition;
            }

        }
    }


    public GameObject newText(string labelText, bool isHeader)
    {
        GameObject textOb = new GameObject(labelText);
        textOb.transform.SetParent(this.transform);
        Text myText;
        myText = textOb.AddComponent<Text>();
        myText.text = labelText;
        myText.font = m_font;
        myText.horizontalOverflow = HorizontalWrapMode.Overflow;
        myText.alignment = TextAnchor.MiddleCenter;
        if(isHeader)
        {
            myText.fontStyle = FontStyle.Bold;
            myText.color = headerColor;
            myText.fontSize = headerSize;
        }
        else
        {
            myText.color = nameColor;
            myText.fontSize = nameSize;
        }
        textOb.transform.localScale = new Vector3(1f, 1f, 1f);
        textOb.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.25f, 0f);

        return textOb;
    }
    public void StartCredits(bool active)
    {
        //StartCoroutine(awakeCredits());
        StartCoroutine(startCredits(active));
    }
    IEnumerator awakeCredits()
    {
        yield return new WaitForSeconds(0.5f);
        Awake();
    }
    IEnumerator startCredits(bool active)
    {
        StartCredits();
        yield return new WaitForSeconds(1);
        scrollCredits = active;
    }

    public void ActiveCredits(bool active)
    {
        scrollCredits = active;
    }

    public void DestroyCredits()
    {
        //headersLists.Clear();
        for (int i = 0; i < creditsText.Count; i++)
        {
            Destroy (creditsText[i]);
            creditsText[i] = null;
        }
        creditsText.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(scrollCredits)
        {
            for (int i = 0; i < creditsText.Count; i++)
            {
                if (creditsText[i] != null)
                {
                    creditsText[i].transform.position = new Vector3(creditsText[i].transform.position.x, creditsText[i].transform.position.y + scrollSpeed, 0f);
                    if (creditsText[i].transform.position.y > Screen.height * distanceScreenToDestroy)
                    {
                        Destroy(creditsText[i]);
                        creditsText[i] = null;
                        creditsText.RemoveAt(i);
                        if(creditsText.Count == 0)
                        {
                            Debug.Log("La Lista esta vacia");
                            uiManager.ActiveAnimation(animationCreditNumber);
                            ActiveCredits(false);
                            DestroyCredits();
                            scrollCredits = false;
                        }
                    }
                }
            }
        }
    }
}
