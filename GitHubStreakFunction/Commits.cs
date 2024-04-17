using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GitHubStreakFunction
{
    public class Commits
    {
        [JsonPropertyName("committer")]
        public Committer Committer { get; set; }
        [JsonPropertyName("commit")]
        public Commit Commit { get; set; }
        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }
    }

    public class Committer
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }
    }

    public class Commit
    {
        [JsonPropertyName("author")]
        public Author Author { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }


    public class Author
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}
