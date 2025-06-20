using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SocialFriendDMBox : MonoBehaviour, IComparable<TextMeshProUGUI>
{
    /// <summary>
    /// Kutu butonu, Kutuya tıklayabilmeyi sağlar.
    /// </summary>
    public Button DMBoxButton;

    /// <summary>
    /// Kullanıcı resminin daire içinde gösterilmesini sağlar.
    /// </summary>
    public Image DMBoxImageCircle;

    /// <summary>
    /// Kullanıcı resmi.
    /// </summary>
    public Image DMBoxUserImage;

    /// <summary>
    /// Kullanıcı adı.
    /// </summary>
    public TextMeshProUGUI DMBoxUsername;

    /// <summary>
    /// Kutunun kullanıcı bilgilerini ayarlar.
    /// </summary>
    /// <param name="username">Kullanıcı adı</param>
    /// <param name="userImage">Kullanıcı resmi</param>
    public void SetUserToBox(string username, Sprite userImage)
    {
        // Kullanıcı adını ve resmini kutuya ayarlar.
        DMBoxUsername.text = username;
        DMBoxUserImage.sprite = userImage;
    }

    public int CompareTo(TextMeshProUGUI other)
    {
        if (other == null)
        {
            return 1; // Diğer nesne null ise, bu nesne daha büyük kabul edilir.
        }

        // Kullanıcı adlarını karşılaştırır, büyük/küçük harf duyarsızdır.
        return string.Compare(DMBoxUsername.text, other.text, StringComparison.OrdinalIgnoreCase);
    }
}