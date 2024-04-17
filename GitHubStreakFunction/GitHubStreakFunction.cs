using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Linq;

namespace GitHubStreakFunction
{
    public static class Function1
    {

        [FunctionName("GitHubStreakFunction")]
        public static async Task Run([TimerTrigger("0 10,17,22 * * *")] TimerInfo myTimer, ILogger log)
        {
            string appEmail = "*****";
            string appPassword = "*****";
            string recipient = "*****";
            string user = "*****";
            string token = "*****";

            log.LogInformation($"GitHub Streak Timer trigger function executed at: {DateTime.Now}");

            var reposUrl = $"https://api.github.com/users/{user}/repos";

            var httpClient = new HttpClient();
            HttpRequestMessage reposRequest = new HttpRequestMessage(HttpMethod.Get, reposUrl);
            reposRequest.Headers.Add("User-Agent", "request");
            reposRequest.Headers.Add("Authorization", $"Bearer {token}");
            var repos = await httpClient.SendAsync(reposRequest);

            var content = await repos.Content.ReadAsStringAsync();
            var repositories = JsonSerializer.Deserialize<List<Repository>>(content);

            var totalCommits = new List<Commits>();
            foreach(var repo in repositories)
            {
                var commitUrl = $"https://api.github.com/repos/{user}/{repo.Name}/commits";
                HttpRequestMessage commitRequest = new HttpRequestMessage(HttpMethod.Get, commitUrl);
                commitRequest.Headers.Add("User-Agent", "request");
                commitRequest.Headers.Add("Authorization", $"Bearer {token}");
                var repoCommits = await httpClient.SendAsync(commitRequest);

                var commitsContent = await repoCommits.Content.ReadAsStringAsync();
                var commits = JsonSerializer.Deserialize<List<Commits>>(commitsContent);
                totalCommits.AddRange(commits);
            }
            var orderedCommits = totalCommits.OrderBy(tc => tc.Commit.Author.Date);

            var committedToday = orderedCommits.LastOrDefault().Commit.Author.Date >= DateTime.Now.Date;

            //for(int i = orderedCommits.Count() - 1; i >= 0; i--)
            //{
            //    //Have to keep track of day and keep looking back
            //    //We need to ignore current day to give ourselves a chance to continue the streak
            //}

            var message = string.Empty;
            if (committedToday)
                message = "Congrats! You've done some more work. Be proud and keep it up!";
            else
                message = "HEY! You want to keep up your streak don't you? Get on and commit something already. It's ok if you don't but you should.";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Credentials = new NetworkCredential(appEmail, appPassword),
                EnableSsl = true,
            };

            smtpClient.Send(appEmail, recipient, "GitHub Streak", message);

            log.LogInformation($"GitHub Streak Timer trigger function completed at: {DateTime.Now}");

        }
    }
}
