using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SocialPanelManager : MonoBehaviour
{
    /// <summary>
    /// DMBox'ların bulunduğu ScrollView içeriği.
    /// </summary>
    public GameObject DMUsersScrollViewContent;

    /// <summary>
    /// DMBox'larda ki kullanıcıları arayıp bulabilmesi için kullanılan InputField.
    /// </summary>
    public TMP_InputField DMSearchFriendInputField;

    /// <summary>
    /// Yeni arkadaş ekleme butonu.
    /// </summary>
    public Button AddNewFriendButton;

    /// <summary>
    /// DMBox prefab'ı. Bu prefab, yeni DM kutuları oluşturmak için kullanılır.
    /// </summary>
    [SerializeField] private SocialFriendDMBox SocialFriendDMBox;

    [SerializeField] private GameObject AddNewFriendsPage;
    [SerializeField] private GameObject DMChatPanel;
    [SerializeField] private GameObject DMHomePanel;

    /// <summary>
    /// DMBox'ların listesi. Bu liste, DM kutularını dinamik olarak oluşturmak için kullanılır.
    /// </summary>
    public List<SocialFriendDMBox> DMBoxes { get; private set; } = new List<SocialFriendDMBox>();

    public Color DefaultDMBoxSearchInputFieldTextColor = Color.white;
    public Color NotFoundDMBoxSearchInputFieldTextColor = Color.red;

    void Start()
    {
        DMSearchFriendInputField.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                FilterDMBoxes(string.Empty);
                ColorizeSearchInputFieldText(true);
            }
            else
            {
                ColorizeSearchInputFieldText(FilterDMBoxes(value) > 0);
            }

        });
        AddNewFriendButton.onClick.AddListener(RedirectToAddNewFriendsPage);
    }

    // Yeni arkadaşlar ekleme sayfasına yönlendirir.
    private void RedirectToAddNewFriendsPage()
    {
        SetActiveAllSocialPanels(false);

        // Yeni arkadaş ekleme sayfasını aktif hale getirir.
        AddNewFriendsPage.SetActive(true);
    }

    /// <summary>
    /// DM kutularını arama metnine göre filtreler ve görünürlüklerini ayarlar.
    /// Filtreleme işlemi, DM kutularının kullanıcı adlarının arama metni ile eşleşip eşleşmediğine dayanır.
    /// </summary>
    /// <param name="searchText">Aranacak metin</param>
    /// <returns>Filtrelenen DM kutularının sayısı</returns>
    public int FilterDMBoxes(string searchText)
    {
        // Filtrelenen DM kutularının sayısını döner.
        int filteredCount = 0;

        // DM kutularını arama metnine göre filtreler.
        foreach (var dmBox in DMBoxes)
        {
            // Eğer kutunun kullanıcı adı arama metni ile eşleşiyorsa, kutuyu gösterir.
            if (string.IsNullOrWhiteSpace(searchText) || dmBox.DMBoxUsername.text.ToLower().Contains(searchText.ToLower()))
            {
                dmBox.gameObject.SetActive(true);
                filteredCount++;
            }
            else
            {
                dmBox.gameObject.SetActive(false);
            }
        }

        return filteredCount;
    }

    // DM Kutuları filtrelendiğinde kutuların olup olmamasına göre kullanıcı arama input field'inin yazı rengini günceller.
    private void ColorizeSearchInputFieldText(bool isFound)
    {
        // Arama metni bulunursa, varsayılan renk kullanılır; bulunamazsa kırmızı renk kullanılır.
        if (isFound)
        {
            DMSearchFriendInputField.textComponent.color = DefaultDMBoxSearchInputFieldTextColor;
        }
        else
        {
            DMSearchFriendInputField.textComponent.color = NotFoundDMBoxSearchInputFieldTextColor;
        }
    }

    /// <summary>
    /// Yeni DM Kutusu oluşturur ve DMUsersScrollViewContent içine ekler.
    /// </summary>
    /// <returns>Oluşturulan DM Kutusu.</returns>
    public SocialFriendDMBox AddNewDMBox()
    {
        // Yeni bir DMBox oluşturur ve DMUsersScrollViewContent içine ekler.
        SocialFriendDMBox newDMBox = Instantiate(SocialFriendDMBox, DMUsersScrollViewContent.transform);

        // Yeni DMBox'ın kullanıcı bilgilerini ayarlar. (Örnek olarak, boş bir kullanıcı adı ve resim kullanılıyor.)
        newDMBox.gameObject.SetActive(true);
        DMBoxes.Add(newDMBox);
        return newDMBox;
    }

    /// <summary>
    /// DMBox listesinden belirtilen DMBox'ı siler.
    /// Eğer DMBox listede yoksa, hiçbir şey yapmaz ve false döner.
    /// </summary>
    /// <param name="box">Silinecek kutu</param>
    /// <returns>Kutu silinirse true, yoksa false</returns>
    public bool DelDMBox(SocialFriendDMBox box)
    {
        if (!DMBoxes.Contains(box)) return false;

        DMBoxes.Remove(box);
        return true;
    }

    // Tüm sosyal panelleri aktif veya pasif hale getirir.
    private void SetActiveAllSocialPanels(bool isActive)
    {
        // Tüm sosyal panelleri aktif veya pasif hale getirir.
        AddNewFriendsPage.SetActive(isActive);
        DMChatPanel.SetActive(isActive);
        DMHomePanel.SetActive(isActive);
    }
}