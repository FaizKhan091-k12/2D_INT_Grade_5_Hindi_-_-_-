using System.Collections;
using System.Reflection;
using UnityEngine;
using DG.Tweening;
public class MainMenuBehaviour : MonoBehaviour
{
   public bool isTesting;
   [SerializeField] private Transform tittleTop;
   [SerializeField] Transform playButton;
   [SerializeField] Transform mainLevel;
   [SerializeField] private Transform midPanel;
   [SerializeField] private Transform topPanel;
   [SerializeField] Transform[] leftbuttons, rightbuttons;
   [SerializeField] private Transform hintPanelBG, hintPanel;
   void Start()
   {
      if (isTesting) return;
      hintPanel.localScale = Vector3.zero;
      hintPanelBG.gameObject.SetActive(false);
      tittleTop.localScale = Vector3.zero;
      playButton.localScale = Vector3.zero;
      mainLevel.localScale = Vector3.zero;
      midPanel.localScale = Vector3.zero;
      topPanel.localScale = Vector3.zero;
      foreach (var VARIABLE in leftbuttons)
      {
         VARIABLE.localScale = Vector3.zero;
      }
      foreach (var VARIABLE in rightbuttons)
      {
         VARIABLE.localScale = Vector3.zero;
      }
      Initialize();

   }

   void Initialize()
   {
      tittleTop.DOScale(Vector3.one, 0.5f)
         .SetEase(Ease.OutBack);
      Invoke(nameof(PopPlay),.2f);
   }

   void PopPlay()
   {
      playButton.DOScale(Vector3.one, 0.5f)
         .SetEase(Ease.OutBack);
      Invoke(nameof(PlayButtonAnim),.75f);
   }

   void PlayButtonAnim()
   {
      playButton.GetComponent<UIHoverClickEffect>().enableIdlePulse = true;
      AudioManager.instance.PlayStartSound();
   }

   public void PlayButtonClick()
   {
     
      playButton.DOScale(Vector3.zero, 0.12f)
         .SetEase(Ease.InOutBack);
      tittleTop.DOScale(Vector3.zero, 0.12f)
         .SetEase(Ease.InOutBack);
      mainLevel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
      
      Invoke(nameof(MidPanel),.2f);
      
   }

   void MidPanel()
   {
      midPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
      Invoke(nameof(TopPanel), .2f);
   }

   void TopPanel()
   {
      topPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
      Invoke(nameof(Buttons), .25f);
   }

   void Buttons()
   {
      StartCoroutine(LeftButtons());
   }
   IEnumerator LeftButtons()
   {
      for (int i = 0; i < leftbuttons.Length; i++)
      {
         leftbuttons[i].DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
         yield return new WaitForSeconds(.1f);
      }

      for (int i = 0; i < rightbuttons.Length; i++)
      {
         rightbuttons[i].DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
         yield return new WaitForSeconds(.1f);
      }
      
      AudioManager.instance.PlayLevelSound();
   }

   public void OpenHintPanel()
   {
      hintPanelBG.gameObject.SetActive(true);
      hintPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
   }

   public void CloseHintPanel()
   {
      hintPanel.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack).OnComplete(() => HintBG());
      // Invoke(nameof(HintBG),.5f);
   }

   void HintBG()
   {
      hintPanelBG.gameObject.SetActive(false);

   }

}
