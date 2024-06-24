using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Newsletter.EmailOctopus
{
    public class EmailOctopusService : MonoBehaviour, INewsletterService
    {
        [SerializeField] private string _listId;
        [SerializeField] private string _apiKey;

        private void Awake()
        {
            GetComponent<NewsletterController>().SetService(this);
        }

        public async UniTask SubscribeAsync(string email, CancellationToken cancellationToken = default)
        {
            using var httpClient = new HttpClient();
            var model = new SubscribeRequestModel
            {
                ApiKey = _apiKey,
                EmailAddress = email,
                Status = "SUBSCRIBED",
                Fields = new Dictionary<string, string>(),
                Tags = Array.Empty<string>(),
            };
            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            });
            Debug.Log(json);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = $"https://emailoctopus.com/api/1.6/lists/{_listId}/contacts";
            var result = await httpClient.PostAsync(uri, content, cancellationToken);
            Debug.Log(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }
    }
}