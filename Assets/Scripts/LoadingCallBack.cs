using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingCallBalk : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText; // 加载文本组件
    [SerializeField] private float loadingDelay = 0.1f; // 加载延迟时间

    private void Start()
    {
        Debug.Log("LoadingCallBalk.Start() 开始");
        StartCoroutine(LoadWithDelay());
    }

    private IEnumerator LoadWithDelay()
    {
        string targetScene = LoadingManager.State.ToString();
        Debug.Log($"目标场景: {targetScene}");

        AsyncOperation ao = SceneManager.LoadSceneAsync(targetScene);

        if (ao == null)
        {
            Debug.LogError($"场景 {targetScene} 加载失败！检查 Build Settings");
            yield break;
        }

        Debug.Log($"ao.isDone: {ao.isDone}, ao.progress: {ao.progress}");

        ao.allowSceneActivation = false;

        int i = 0;
        while (!ao.isDone)
        {
            loadingText.text = new string('.', i % 6);
            Debug.Log($"加载中... progress: {ao.progress}");
            yield return new WaitForSeconds(loadingDelay);
            i++;
            Debug.Log($"间隔结束，继续加载... 间隔时间为：{loadingDelay}");
            if (ao.progress >= 0.9f)
            {
                Debug.Log($"加载完成... progress: {ao.progress}");
                Debug.Log("设置 allowSceneActivation = true");
                ao.allowSceneActivation = true;
            }
            Debug.Log($"循环结束... progress: {ao.progress}");
            Debug.Log($"循环结束... ao.isDone: {ao.isDone}");
        }
    }
}
