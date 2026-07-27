using Quokka.ListItems;
using Quokka.PluginArch;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PluginTypedText
{
  internal sealed class TypedTextItem : ListItem
  {

    private readonly string query;

    public TypedTextItem(string query, bool differentDesc)
    {
      Name = $"You typed `{query}`";
      Description = differentDesc ? "This is a different description" : "Hit the enter key to copy the text";
      UiDispatcher.BeginInvoke(() => Icon = new BitmapImage(new Uri(
            Environment.CurrentDirectory + "\\PlugBoard\\PluginTypedText\\Plugin\\text.png")));
      this.query = query;
    }

    public override void Execute()
    {
      Clipboard.SetText(query);
      Application.Current.MainWindow.Close();
    }
  }

  internal sealed class OtherTypedTextItem : ListItem
  {

    private readonly string query;

    public OtherTypedTextItem(string query)
    {
      Name = "This is the other typed text item";
      Description = "You typed " + query;
      Icon = IconCache.GetOrAdd(
        Environment.CurrentDirectory + "\\Config\\Resources\\information.png"
      );
      this.query = query;
    }

    public override void Execute()
    {
      Clipboard.SetText(query);
      Application.Current.MainWindow.Close();
    }
  }
}
