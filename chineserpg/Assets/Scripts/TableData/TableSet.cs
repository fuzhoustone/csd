using System.Collections;
using System.IO;
using UnityEngine;

public class TableSet : MonoBehaviour
{
    public TextAsset storyRel;
    public TextAsset storyBgSceneRel;
    public TextAsset bgScenePic;
    public TextAsset roleFacePic;
    public TextAsset talkOption;

    private void Start()
    {
       initData();
    }

    public void initData()
    {
        using (var stream = new MemoryStream(storyRel.bytes))
        {
            StoryRelationTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(storyBgSceneRel.bytes))
        {
            StoryBgSceneRelationTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(bgScenePic.bytes))
        {
            bgScenePicTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(roleFacePic.bytes))
        {
            roleFacePicTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(talkOption.bytes))
        {
            talkOptionTab._instance().Load(stream);
        }
    }
       
 
}
