using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoginManager : MonoBehaviour
{
    public TMP_InputField EmailInputField;
    public TMP_InputField PasswordInputField;
    public TextMeshProUGUI EmailErrorText;
    public TextMeshProUGUI PasswordErrorText;
    public TextMeshProUGUI GlobalErrorText;
    public Button LoginButton;
    public Button RegisterNowButton;
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    void Start()
    {
        // Butonlara tıklama olaylarını bağla
        LoginButton.onClick.AddListener(OnLoginButtonClicked);
        RegisterNowButton.onClick.AddListener(OnRegisterNowButtonClicked);

        // Hataları temizle
        ClearErrors();
    }

    // Kayıt ol butonuna tıklandığında çağrılır.
    private void OnRegisterNowButtonClicked()
    {
        ClearAll();
        RegisterPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }

    // Giriş yap butonuna tıklandığında çağrılır.
    private void OnLoginButtonClicked()
    {
        if (!ValidateEmail(EmailInputField.text))
        {
            ClearErrors();
            EmailErrorText.text = "Geçersiz e-posta adresi.";
        }
        else if (!ValidatePassword(PasswordInputField.text))
        {
            ClearErrors();
            PasswordErrorText.text = "Şifre en az 7 karakter olmalıdır.";
        }
        else
        {
            ClearErrors();

            // Giriş işlemini başlat
            APILogin.LoginAsync(EmailInputField.text, PasswordInputField.text, OnLoginResponse);
        }
    }

    // Giriş işlemi asenkron olarak tamamlandığında çağrılır.
    private void OnLoginResponse(APILoginResponse response)
    {
        Debug.Log(response.ToString());
    }


    // Tüm hata mesajlarını temizler.
    private void ClearErrors()
    {
        EmailErrorText.text = string.Empty;
        PasswordErrorText.text = string.Empty;
        GlobalErrorText.text = string.Empty;
    }

    // Tüm hata mesajlarını ve girilen verileri temizler.
    private void ClearAll()
    {
        EmailInputField.text = string.Empty;
        PasswordInputField.text = string.Empty;
        EmailErrorText.text = string.Empty;
        PasswordErrorText.text = string.Empty;
        GlobalErrorText.text = string.Empty;
    }

    // Email kalıbını kontrol eder.
    private static bool ValidateEmail(string email)
    {
        // Basit bir regex ile e-posta formatını kontrol et
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains(".") || !System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern))
        {
            return false;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    // Şifre kalıbını kontrol eder.
    private static bool ValidatePassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Length > 6;
    }
}
