using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;

namespace GitHubStreakFunction
{
    public static class Function1
    {

        [FunctionName("GitHubStreakFunction")]
        public static async Task Run([TimerTrigger("*/30 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            string appEmail = "*****";
            string appPassword = "*****";
            string recipient = "*****";
            string org = "*****";
            string token = "*****";
            string repo = "*****";

            log.LogInformation($"GitHub Streak Timer trigger function executed at: {DateTime.Now}");

            var url = $"https://api.github.com/repos/{org}/{repo}";
            var headers = new Dictionary<string, string>();
            headers.Add("User-Agent", "request");
            headers.Add("bearer", token);
            var httpClient = new HttpClient();
            var repos = await httpClient.GetAsync(url);

            var content = repos.Content.ReadAsStringAsync();
            var t = 0;


            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Credentials = new NetworkCredential(appEmail, appPassword),
                EnableSsl = true,
            };

            smtpClient.Send(appEmail, recipient, "GitHub Streak", "Hey just letting you know about github streak. We haven't implemented it yet though.");

            log.LogInformation($"GitHub Streak Timer trigger function completed at: {DateTime.Now}");

        }
    }
}
