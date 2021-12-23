    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

namespace GrowSense.Installer.GitHub
{
  public class ReleaseInfo
  {

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("assets_url")]
    public Uri AssetsUrl { get; set; }

    [JsonProperty("upload_url")]
    public string UploadUrl { get; set; }

    [JsonProperty("html_url")]
    public Uri HtmlUrl { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("author")]
    public Author Author { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("tag_name")]
    public string TagName { get; set; }

    [JsonProperty("target_commitish")]
    public string TargetCommitish { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("draft")]
    public bool Draft { get; set; }

    [JsonProperty("prerelease")]
    public bool Prerelease { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("published_at")]
    public DateTimeOffset PublishedAt { get; set; }

    [JsonProperty("assets")]
    public Asset[] Assets { get; set; }

    [JsonProperty("tarball_url")]
    public Uri TarballUrl { get; set; }

    [JsonProperty("zipball_url")]
    public Uri ZipballUrl { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }
  }

  public partial class Asset
  {
    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("uploader")]
    public Author Uploader { get; set; }

    [JsonProperty("content_type")]
    public string ContentType { get; set; }

    [JsonProperty("state")]
    public State State { get; set; }

    [JsonProperty("size")]
    public long Size { get; set; }

    [JsonProperty("download_count")]
    public long DownloadCount { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonProperty("browser_download_url")]
    public Uri BrowserDownloadUrl { get; set; }
  }

  public partial class Author
  {
    [JsonProperty("login")]
    public string Login { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("avatar_url")]
    public Uri AvatarUrl { get; set; }

    [JsonProperty("gravatar_id")]
    public string GravatarId { get; set; }

    [JsonProperty("url")]
    public Uri Url { get; set; }

    [JsonProperty("html_url")]
    public Uri HtmlUrl { get; set; }

    [JsonProperty("followers_url")]
    public Uri FollowersUrl { get; set; }

    [JsonProperty("following_url")]
    public string FollowingUrl { get; set; }

    [JsonProperty("gists_url")]
    public string GistsUrl { get; set; }

    [JsonProperty("starred_url")]
    public string StarredUrl { get; set; }

    [JsonProperty("subscriptions_url")]
    public Uri SubscriptionsUrl { get; set; }

    [JsonProperty("organizations_url")]
    public Uri OrganizationsUrl { get; set; }

    [JsonProperty("repos_url")]
    public Uri ReposUrl { get; set; }

    [JsonProperty("events_url")]
    public string EventsUrl { get; set; }

    [JsonProperty("received_events_url")]
    public Uri ReceivedEventsUrl { get; set; }

    [JsonProperty("type")]
    public TypeEnum Type { get; set; }

    [JsonProperty("site_admin")]
    public bool SiteAdmin { get; set; }
  }

  public enum State { Uploaded };

  public enum TypeEnum { Bot, User };

  internal static class Converter
  {
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
      MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
      DateParseHandling = DateParseHandling.None,
      Converters =
            {
                StateConverter.Singleton,
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
  }
  
  internal class StateConverter : JsonConverter
  {
    public override bool CanConvert(Type t) => t == typeof(State) || t == typeof(State?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null) return null;
      var value = serializer.Deserialize<string>(reader);
      if (value == "uploaded")
      {
        return State.Uploaded;
      }
      throw new Exception("Cannot unmarshal type State");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
      if (untypedValue == null)
      {
        serializer.Serialize(writer, null);
        return;
      }
      var value = (State)untypedValue;
      if (value == State.Uploaded)
      {
        serializer.Serialize(writer, "uploaded");
        return;
      }
      throw new Exception("Cannot marshal type State");
    }

    public static readonly StateConverter Singleton = new StateConverter();
  }

  internal class TypeEnumConverter : JsonConverter
  {
    public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null) return null;
      var value = serializer.Deserialize<string>(reader);
      switch (value)
      {
        case "Bot":
          return TypeEnum.Bot;
        case "User":
          return TypeEnum.User;
      }
      throw new Exception("Cannot unmarshal type TypeEnum");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
      if (untypedValue == null)
      {
        serializer.Serialize(writer, null);
        return;
      }
      var value = (TypeEnum)untypedValue;
      switch (value)
      {
        case TypeEnum.Bot:
          serializer.Serialize(writer, "Bot");
          return;
        case TypeEnum.User:
          serializer.Serialize(writer, "User");
          return;
      }
      throw new Exception("Cannot marshal type TypeEnum");
    }

    public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
  }
}