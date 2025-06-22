using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Kullanıcı kayıt işlemlerini yöneten sınıf.
/// </summary>
public static class APIRegister
{
    /// <summary>
    /// Asenkron olarak kullanıcı kayıt işlemi yapar.
    /// </summary>
    /// <param name="username">Kullanıcı adı.</param>
    /// <param name="email">Kullanıcı email adresi.</param>
    /// <param name="password">Kullanıcı şifresi.</param>
    /// <param name="onResult">Kayıt işlemi sonlanınca çağrılacak geri çağırma fonksiyonu.</param>
    /// <param name="timeout">[Opsiyonel] İstek zaman aşımı süresi (sn)</param>
    public static async void RegisterAsync(string username, string email, string password, Action<APIRegisterResponse> onResult, ushort timeout = 10)
    {
        string url = ApiManager.BaseUrl + ApiManager.RegisterUrl;
        string jsonData = JsonConvert.SerializeObject(new { username, email, password });
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
                    APIRegisterResponse errorResponse = new APIRegisterResponse
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
                APIRegisterResponse response = JsonUtility.FromJson<APIRegisterResponse>(jsonResponse);
                response.Code = request.responseCode;
                onResult?.Invoke(response);
                return;
            }
            catch (UnityWebRequestException e)
            {
                APIRegisterResponse errorResponse = new APIRegisterResponse
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
                APIRegisterResponse errorResponse = new APIRegisterResponse
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