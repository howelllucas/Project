using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System;

/*Scene:N/A
 *Object:DialogSavePictureManager
 *Description:Klasa koja sadrzi globalne promenljive i logiku neophodnu za funkcionisanje PopUpDialogSavePicture dijaloga
 *
 *
 */
public class DialogSavePictureManager : MonoBehaviour
{

    /// <summary>
    /// Stanje dijaloga.
    /// true=otvoren
    /// false=zatvoren
    /// </summary>
    public  bool dialogState = false;
    /// <summary>
    /// Rezultat dijaloga
    /// true=yes
    /// false=no
    /// </summary>
    public  bool dialogResult = false;
    /// <summary>
    /// PopUpDialogSavePicture dijalog
    /// </summary>
    public GameObject savePictureDialog;
    /// <summary>
    /// Teskura koju dijalog prikazuje
    /// </summary>
    public RawImage dialogContent;
    /// <summary>
    /// Naslov dijaloga
    /// </summary>
    public Text dialogTitle;


    public AdvancedMobilePaint.AdvancedMobilePaint PaintEngine;

    //	/// <summary>
    //	/// The tmp.
    //	/// </summary>
    //	public Texture2D tmp;

    DateTime currentDateTime;
    string fileNameByDate;
    // koristi se za cuvanje stringa koji se dodaje na kraju slike. String je sacinjen od trenuntnog datuma i vremena
    string imageFinalPath;
    Vector2 previewImageSize;
    Texture2D textureToSave;
    Rect paintAreaRect;
    Vector3[] fourCornersArray = new Vector3[4];

    //	void Awake()
    //	{
    //		savePictureDialog = GameObject.Find ("Canvas/PopUps/PopUpDialogSavePicture");
    //		dialogContent = GameObject.Find ("Canvas/PopUps/PopUpDialogSavePicture/AnimationHolder/Body/ContentHolder/TextBG/RawImage").GetComponent<RawImage>();
    //		dialogTitle = GameObject.Find ("Canvas/PopUps/PopUpDialogSavePicture/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text> ();
    //		transform.name = "DialogSavePictureManager";
    //	}
    //	// Use this for initialization
    //	void Start () {
    //
    //		//dialogContent.texture = tmp;
    //		//dialogTitle.text = title;
    //		//GameObject.Find ("Canvas").GetComponent<MenuManager> ().ShowPopUpMenu (savePictureDialog);
    //		//dialogState = true;
    //	}
    ////	
    ////	// Update is called once per frame
    ////	void Update () {
    ////	
    ////	}



    //Poziva je DialogBackPressResult klasa i setuje rezultat dijaloga
    public void SetDialogResult(bool result)
    {
        dialogResult = result;
    }
    //Poziva je DialogBackPressResult klasa i setuje stanje dijaloga na zatvoren
    public void SetDialogState(bool state)
    {
        dialogState = state;

        //if(dialogResult)
        //	SavePicture(tmp,"blah","test");
    }

    /// <summary>
    /// Prikazuje PopUpDialogSavePicture popUp dijalog
    /// </summary>
    /// <param name="title">Naslov dijaloga</param>
    /// <param name="content">Texture2D tekstura koju dijalog treba da prikaze</param>
    public void ShowDialog(string title, Texture2D content)
    {
        dialogContent.texture = content;
        dialogTitle.text = title;
        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMenu(savePictureDialog);
        dialogState = true;
    }

    public void TakeScreenShot()
    {
        currentDateTime = DateTime.Now;
        fileNameByDate = currentDateTime.Year.ToString() + currentDateTime.Month.ToString() + currentDateTime.Day.ToString() + "_" + currentDateTime.Hour.ToString() + currentDateTime.Minute.ToString() + currentDateTime.Second.ToString();
        SoundManager.Instance.Play_TakePhotoSound();
        StopCoroutine("ScreenShot");
        StartCoroutine("ScreenShot");
    }

    IEnumerator ScreenShot()
    {	

        yield return new WaitForEndOfFrame();
		
        //ScreenShot dela ekrana
        paintAreaRect = new Rect();
        PaintEngine.transform.parent.GetComponent<RectTransform>().GetWorldCorners(fourCornersArray);
        Vector2 rectPos = new Vector2(Camera.main.WorldToScreenPoint(fourCornersArray[0]).x, Camera.main.WorldToScreenPoint(fourCornersArray[0]).y);
        Vector2 rectSize = new Vector2(Camera.main.WorldToScreenPoint(fourCornersArray[2]).x - Camera.main.WorldToScreenPoint(fourCornersArray[0]).x, Camera.main.WorldToScreenPoint(fourCornersArray[1]).y - Camera.main.WorldToScreenPoint(fourCornersArray[0]).y);
        paintAreaRect.position = rectPos;
        paintAreaRect.size = rectSize;
        Debug.Log("Rect paras: " + paintAreaRect);
        textureToSave = new Texture2D((int)paintAreaRect.width, (int)paintAreaRect.height, TextureFormat.RGB24, false);
        textureToSave.ReadPixels(paintAreaRect, 0, 0, false);
        textureToSave.Apply();
        yield return new WaitForEndOfFrame();
		
        ShowDialog("Save picture", textureToSave);
    }

    public void CallSavePicture()
    {
        SavePicture(textureToSave, "Mandala_Images", "Mandala_" + fileNameByDate);
    }

    /// <summary>
    /// Saves the picture to device galery.
    /// </summary>
    /// <param name="texture">Tekstura koju zelimo da sacuvamo </param>
    /// <param name="directoryName">Ime direktrojuma koji zelimo da sacuvamo sliku.</param>
    /// <param name="pictureName">Ime slike bez ekstenzije.</param>
    void SavePicture(Texture2D texture, string directoryName, string pictureName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/" + directoryName))
            Directory.CreateDirectory(Application.persistentDataPath + "/" + directoryName);
        //Debug.Log (Application.persistentDataPath + "/test");
        byte[] bytes = texture.EncodeToPNG();

        File.WriteAllBytes(Application.persistentDataPath + "/" + directoryName + "/" + pictureName + ".png", bytes);

        #if UNITY_ANDROID &&!UNITY_EDITOR
        string path2 = Share.ReturnGalleryFolder();
        //Debug.Log("NAIL path2 "+path2);
        if (path2 != "")
        {
            try
            {
                if (!Directory.Exists(path2 + directoryName))
                    Directory.CreateDirectory(path2 + directoryName);
				
                path2 += directoryName + "/" + pictureName + ".png";
                string path = Application.persistentDataPath + "/" + directoryName + "/" + pictureName + ".png";
                File.Copy(path, path2, false);
                imageFinalPath = path2;
                GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageTitleText("PICTURE SAVED");
                GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageCustomMessageText("Picture has been saved to your gallery.");
                Debug.Log("Mandala " + "picture saved at: " + imageFinalPath);
                //ShowPopUp("PICTURE SAVED"," ");
            }
            catch (Exception e)
            { 	
                Debug.Log("NAIL PATH 2 " + e.Message);
                GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageTitleText("PICTURE NOT SAVED");
                GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageCustomMessageText("Please grant the storage permission and try again!");
                //ShowPopUp("PICTURE  NOT SAVED"," ");
            }
			
        }
        else
        {
            Invoke("WaitAndShowFailure", 2f);

        }
        //MenuShare.anchoredPosition = hidePos;
        //bMenuShare = false;
		
        Share.RefreshGalleryFolder(path2);

        #endif

        #if UNITY_IOS && !UNITY_EDITOR
        OtherMessagesBinding.sendMessage("SaveToGallery###" + Application.persistentDataPath + "/"+directoryName+"/"+pictureName +".png");
//        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageTitleText("PICTURE SAVED");
//        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageCustomMessageText("PICTURE IS SAVED TO YOUR GALERY");
        #endif


        #if UNITY_EDITOR
        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageTitleText("PICTURE SAVED");
        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageCustomMessageText(imageFinalPath);
        Debug.Log("picture saved");
        #endif

    }

    void WaitAndShowFailure()
    {

        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageTitleText("PICTURE NOT SAVED");
        GameObject.Find("Canvas").GetComponent<MenuManager>().ShowPopUpMessageCustomMessageText("Galery folder could not be found.");
    }

    /// <summary>
    /// Loads the texture from file on application persistent data path.
    /// </summary>
    /// <returns>The texture from file.</returns>
    /// <param name="directory">Directory (can include subpath).</param>
    /// <param name="imageName">Image name (no extension).</param>
    /// <param name="width">Texture width.</param>
    /// <param name="height">Texture height.</param>
    public Texture2D  LoadTextureFromFile(string directory, string imageName, int width, int height)
    {
        string path = Application.persistentDataPath + "/" + directory + "/" + imageName + ".png";
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Bilinear;
        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
			
            texture.LoadImage(bytes);
            texture.Apply();
        }
        return texture;
    }

    /// <summary>
    /// Loads the texture from file on application persistent data path..
    /// </summary>
    /// <param name="directory">Directory (can include subpath).</param>
    /// <param name="imageName">Image name (no extension).</param>
    /// <param name="target">Target texture.</param>
    public void  LoadTextureFromFile(string directory, string imageName, Texture2D target)
    {
        string path = Application.persistentDataPath + "/" + directory + "/" + imageName + ".png";
        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
			
            target.LoadImage(bytes);
            target.Apply();
        }
    }

    /// <summary>
    /// Cuva sliku unutar persistentDataPath foldera (ne vidi se u galeriji).
    /// </summary>
    /// <param name="texture">Textura koju zelimo da sacuvamo za deljenje.</param>
    /// <param name="directoryName">Ime foldera u koji ce se cuvati slike za deljenje.</param>
    /// <param name="pictureName">Ime slike.</param>
    void SaveImageForSharing(Texture2D texture, string directoryName, string pictureName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/" + directoryName))
            Directory.CreateDirectory(Application.persistentDataPath + "/" + directoryName);
        //Debug.Log (Application.persistentDataPath + "/test");
        byte[] bytes = texture.EncodeToPNG();
		
        File.WriteAllBytes(Application.persistentDataPath + "/" + directoryName + "/" + pictureName + ".png", bytes);

        imageFinalPath = Application.persistentDataPath + "/" + directoryName + "/" + pictureName + ".png";

    }

    /// <summary>
    /// Used for starting the ShateImage procedure. Called from inspector.
    /// </summary>
    public void ShareImage()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR 
        SaveImageForSharing(textureToSave, "TempImages", "Mandala_" + fileNameByDate);

        Share.ShareScreenshot(imageFinalPath);
        #elif UNITY_IOS && !UNITY_EDITOR
        SaveImageForSharing(textureToSave, "TempImages", "Mandala_" + fileNameByDate);
       
        GameObject shareButton=savePictureDialog.transform.Find("AnimationHolder/Body/ButtonsHolder/ButtonShare").gameObject;
        Vector3 [] tmp= new Vector3[4];

        shareButton.GetComponent<RectTransform>().GetWorldCorners(tmp);
        //Debug.Log (tmp[0].ToString()+" "+tmp[1].ToString()+" "+tmp[2].ToString()+" "+tmp[3].ToString() );
        Vector3 center= new Vector3(tmp[0].x+Mathf.Abs(tmp[0].x-tmp[3].x)/2f
        ,tmp[0].y+Mathf.Abs(tmp[0].y-tmp[1].y)/2f
        ,tmp[0].z);
        //Debug.Log ("CENTER "+ center.ToString());
        Vector2 centerInPixels=RectTransformUtility.WorldToScreenPoint(Camera.main,center);
        OtherMessagesBinding.sendMessage("ShareImage###"+(int)centerInPixels.x+"###"+(int)centerInPixels.y+"###"+imageFinalPath);
        Debug.Log("ShareImage###"+(int)centerInPixels.x+"###"+(int)centerInPixels.y+"###"+imageFinalPath);
        #endif
    }

}




























