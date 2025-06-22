using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Kullanıcı giriş işlemlerini yöneten sınıf.
/// </summary>
public static class APILogin
{
    /// <summary>
    /// Asenkron olarak kullanıcı giriş işlemi yapar.
    /// </summary>
    /// <param name="email">Kullanıcı email adresi.</param>
    /// <param name="password">Kullanıcı şifresi.</param>
    /// <param name="onResult">Giriş işlemi sonlanınca çağrılacak geri çağırma fonksiyonu.</param>
    /// <param name="timeout">[Opsiyonel] İstek zaman aşımı süresi (sn)</param>
    public static async void LoginAsync(string email, string password, Action<APILoginResponse> onResult, ushort timeout = 10)
    {
        string url = ApiManager.BaseUrl + ApiManager.LoginUrl;
        string jsonData = JsonConvert.SerializeObject(new { email, password });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = timeout;
            request.SetRequestHeader("Content-Type", "application/json");

            try
            {
                await request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    APILoginResponse errorResponse = new APILoginResponse
                    {
                        Error = $"Error: {request.error}",
                        Code = request.responseCode,
                    };
#if UNITY_EDITOR
                    Debug.LogError(errorResponse.Error);
#endif
                    onResult?.Invoke(errorResponse);
                    return;
                }

                string jsonResponse = request.downloadHandler.text;
                APILoginResponse response = JsonUtility.FromJson<APILoginResponse>(jsonResponse);
                response.Code = request.responseCode;
                onResult?.Invoke(response);
                return;
            }
            catch (UnityWebRequestException e)
            {
                APILoginResponse errorResponse = new APILoginResponse
                {
                    Error = $"Network error: {e.Message}",
                    Code = request.responseCode,
                };
#if UNITY_EDITOR
                Debug.LogError(errorResponse.Error);
#endif
                onResult?.Invoke(errorResponse);
                return;
            }
            catch (Exception e)
            {
                APILoginResponse errorResponse = new APILoginResponse
                {
                    Error = $"Unexpected error: {e.Message}",
                    Code = request.responseCode,
                };
#if UNITY_EDITOR
                Debug.LogError(errorResponse.Error);
#endif
                onResult?.Invoke(errorResponse);
            }
        }
    }
}