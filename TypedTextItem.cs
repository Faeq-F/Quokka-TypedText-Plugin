using Quokka;
using Quokka.ListItems;
using System.Windows.Media.Imaging;

namespace Plugin_TypedText {
  class TypedTextItem : ListItem {

    string query;

    public TypedTextItem(string query, bool differentDesc) {
      this.Name = $"You typed `{query}`";
      this.Description = "Hit the enter key to copy the text";
      this.Icon = new BitmapImage(new Uri(
          Environment.CurrentDirectory + "\\PlugBoard\\Plugin_TypedText\\Plugin\\text.png"));
      this.query = query;
      if (differentDesc) {
        this.Description = "This is a different description";
      } else {
        this.Description = "Hit the enter key to copy the text";
      }
    }

    public override void Execute() {
      System.Windows.Clipboard.SetText(query);
      App.Current.MainWindow.Close();
    }
  }

  class OtherTypedTextItem : ListItem {

    string query;

    public OtherTypedTextItem(string query) {
      this.Name = "This is the other typed text item";
      this.Description = "You typed " + query;
      this.Icon = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Config\\Resources\\information.png"));
      this.query = query;
    }

    public override void Execute() {
      System.Windows.Clipboard.SetText(query);
      App.Current.MainWindow.Close();
    }
  }
}
