using Quokka;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.Windows.Media.Imaging;

namespace PluginTypedText
{
  class TypedTextItem : ListItem
  {

    readonly string query;

    public TypedTextItem(string query, bool differentDesc)
    {
      Name = $"You typed `{query}`";
      Description = "Hit the enter key to copy the text";
      UiDispatcher.BeginInvoke(() =>
      {
        Icon = new BitmapImage(new Uri(
            Environment.CurrentDirectory + "\\PlugBoard\\PluginTypedText\\Plugin\\text.png"));
      });
      this.query = query;
      if (differentDesc)
      {
        this.Description = "This is a different description";
      }
      else
      {
        this.Description = "Hit the enter key to copy the text";
      }
    }

    public override void Execute()
    {
      System.Windows.Clipboard.SetText(query);
      App.Current.MainWindow.Close();
    }
  }

  class OtherTypedTextItem : ListItem
  {

    readonly string query;

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
      System.Windows.Clipboard.SetText(query);
      App.Current.MainWindow.Close();
    }
  }
}
