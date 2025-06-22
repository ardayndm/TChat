/// <summary>
/// Kullanıcı giriş yanıtını temsil eden yapı.
/// </summary>
public struct APIRegisterResponse
{
    public APIRegisterResponse(string token, ulong userId, string username, string email, string error = "", long code = 0)
    {
        Token = token ?? string.Empty;
        UserId = userId;
        Username = username ?? string.Empty;
        Email = email ?? string.Empty;
        Error = error ?? string.Empty;
        Code = code;
    }

    /// <summary>
    /// Kullanıcının kimlik doğrulama jetonu.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Kullanıcının kimliği.
    /// </summary>
    public ulong UserId { get; set; }

    /// <summary>
    /// Kullanıcının kullanıcı adı.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Kullanıcının e-posta adresi.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Oluşan hata mesajı, eğer varsa.
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// Sonuç kodu.
    /// </summary>
    public long Code { get; set; }
}