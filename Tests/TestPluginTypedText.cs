using Quokka.ListItems;
using System.Collections.ObjectModel;
using System.IO;

namespace PluginTypedText.Tests
{
  /// <summary>
  /// Integration and unit tests for the <see cref="TypedText"/> plugin.
  /// </summary>
  public class TestPluginTypedText : IDisposable
  {
    private readonly string _tempPath;
    private readonly string _originalCurrentDirectory;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestPluginTypedText"/> class and sets up the sandboxed environment.
    /// </summary>
    public TestPluginTypedText()
    {
      _originalCurrentDirectory = Environment.CurrentDirectory;

      // Clean up legacy directories from previous runs
      try
      {
        foreach (string dir in Directory.GetDirectories(_originalCurrentDirectory, "TempTypedText_*"))
        {
          try
          {
            Directory.Delete(dir, recursive: true);
          }
          catch
          {
            // Ignore if locked
          }
        }
      }
      catch
      {
      }

      string uniqueId = Guid.NewGuid().ToString("N");
      _tempPath = Path.Combine(_originalCurrentDirectory, "TempTypedText_" + uniqueId);
      Directory.CreateDirectory(_tempPath);

      // Create target directory structure matching expected plugin path
      string targetPluginDir = Path.Combine(_tempPath, "PlugBoard", "PluginTypedText", "Plugin");
      Directory.CreateDirectory(targetPluginDir);

      // Write a custom settings.json with usePlugin enabled for testing
      const string customSettingsJson = /*lang=json,strict*/ @"
      {
        ""usePlugin"": true,
        ""Show2ItemsSpecialCommand"": ""2TypedText"",
        ""Show3ItemsSpecialCommand"": ""3TypedText"",
        ""Show4ItemsSpecialCommand"": ""4TypedText"",
        ""ItemSignifier"": ""TT "",
        ""ShowDifferentDescriptionFlag"": "" --Desc"",
        ""FuzzySearchThreshold"": 80
      }";
      File.WriteAllText(Path.Combine(targetPluginDir, "settings.json"), customSettingsJson);

      // Redirect CurrentDirectory
      Environment.CurrentDirectory = _tempPath;

      if (System.Windows.Application.Current == null)
      {
        _ = new System.Windows.Application();
      }
    }

    /// <summary>
    /// Verifies that the plugin correctly configures its command signifiers and special commands.
    /// </summary>
    [Fact]
    public void TestTypedText_ConfiguresSignifiersAndSpecialCommands()
    {
      TypedText typedText = new();
      typedText.CommandSignifiers().Should().ContainSingle().Which.Should().Be("TT ");
      typedText.SpecialCommands().Should().BeEquivalentTo("2TypedText", "3TypedText", "4TypedText");
    }

    /// <summary>
    /// Verifies that the plugin evaluates normal query changes and processes the description flag.
    /// </summary>
    [Fact]
    public void TestTypedText_ProcessesNormalQueryChanges()
    {
      TypedText typedText = new();

      // Normal query returns a ListItem with name and description matching the query
      Collection<ListItem> results = typedText.OnQueryChange("hello world");
      results.Should().NotBeEmpty();
      ListItem item = results[0];
      item.Name.Should().Be("You typed `hello world`");
      item.Description.Should().Be("Hit the enter key to copy the text");

      // Query with the description flag modifies the query and description
      Collection<ListItem> resultsWithFlag = typedText.OnQueryChange("hello world --Desc");
      resultsWithFlag.Should().NotBeEmpty();
      ListItem itemWithFlag = resultsWithFlag[0];
      itemWithFlag.Name.Should().Be("You typed `hello world`");
      itemWithFlag.Description.Should().Be("This is a different description");
    }

    /// <summary>
    /// Verifies that the plugin returns the correct number of items for special commands.
    /// </summary>
    [Fact]
    public void TestTypedText_EvaluatesSpecialCommands()
    {
      TypedText typedText = new();

      Collection<ListItem> twoItems = typedText.OnSpecialCommand("2TypedText");
      twoItems.Count.Should().BeGreaterThanOrEqualTo(2);

      Collection<ListItem> threeItems = typedText.OnSpecialCommand("3TypedText");
      threeItems.Count.Should().BeGreaterThanOrEqualTo(3);
    }

    /// <summary>
    /// Verifies that the plugin processes signifier commands.
    /// </summary>
    [Fact]
    public void TestTypedText_EvaluatesSignifiers()
    {
      TypedText typedText = new();
      Collection<ListItem> results = typedText.OnSignifier("TT hello");
      results.Should().NotBeEmpty();
      results[0].Name.Should().Be("You typed `hello`");
    }

    /// <summary>
    /// Restores the original current directory and cleans up the sandbox files.
    /// </summary>
    public void Dispose()
    {
      Environment.CurrentDirectory = _originalCurrentDirectory;
      try
      {
        if (Directory.Exists(_tempPath))
        {
          Directory.Delete(_tempPath, recursive: true);
        }
      }
      catch
      {
        // Ignore locked files
      }
      GC.SuppressFinalize(this);
    }
  }
}
