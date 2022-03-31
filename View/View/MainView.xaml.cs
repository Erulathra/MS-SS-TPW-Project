using MS_SS_TPW_Project.Logic;

namespace Presentation.View
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var name = (new MainLogic()).GetGreeting("World");
            greetingLabel.Content = name;
        }
    }
}
