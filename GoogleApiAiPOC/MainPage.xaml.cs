using ApiAiSDK;
using ApiAiSDK.Model;
using BotHandlerUWP.Usercontrols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoogleApiAiPOC 
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public List<string> messagesFromBot { get; set; }
        private AIConfiguration config;
        private AIDataService dataService;
        public MainPage()
        {
            this.InitializeComponent();
            this.KeyDown += MainPage_KeyDown;
            Init();
        }

        private void Init(string accessToken = "Paste your default 'Client access token' from api.ai settings here")
        {
            messagesFromBot = new List<string>();
            config = new AIConfiguration(accessToken, SupportedLanguage.English);
            dataService = new AIDataService(config);
        }

        private void MainPage_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SendConversation();
            }
        }

        async void Interactive(string input)
        {
            try
            {

                await ReadBotMessagesAsync(input);
                foreach (var msg in messagesFromBot)
                {
                    if (msg != null)
                        AddTextToGrid(msg, 1, "ms-appx:///Assets/6422482.png", 0);

                }
                await Task.Delay(100);
                double off = ScrollRow.ActualHeight;
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    var Succes = Scroller.ChangeView(null, off, null);
                });
               
            }
            catch (Exception err)
            {
            }
        }

        private async Task<string> ReadBotMessagesAsync(string input)
        {
            bool messageReceived = false;
            while (!messageReceived)
            {
      
                var request = new AIRequest(input);
                var aiResponse = await dataService.RequestAsync(request);
                messagesFromBot.Clear();
                messagesFromBot.Add(aiResponse.Result.Fulfillment.Speech.ToString());
                messageReceived = true;
            }

            return messagesFromBot.FirstOrDefault();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendConversation();
        }

        private void SendConversation()
        {
            string input = chatText.Text;

            if (string.IsNullOrWhiteSpace(input) == false)
            {
                AddTextToGrid(input, 2, "ms-appx:///Assets/avatar.png", 3);
                Interactive(input);
                chatText.Text = string.Empty;
            }
        }

        private void AddTextToGrid(string mesg, int col, string source, int ImgCol)
        {
            //Adding profile picture
            ProfilePicture picture = new ProfilePicture(source);
            ChatGrid.Children.Add(picture);
            Grid.SetColumn(picture, ImgCol);


            RichTextBlock textOutPut = new RichTextBlock()
            {
                Margin = new Thickness(10, 0, 10, 0),
                IsTextScaleFactorEnabled = true,
                TextWrapping = TextWrapping.WrapWholeWords,
                MaxWidth = 250
            };
            if (ImgCol == 0)
            {
                textOutPut.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                textOutPut.HorizontalAlignment = HorizontalAlignment.Right;
            }

            Paragraph paragraph = new Paragraph();
            Run run = new Run() { FontSize = 15, FontWeight = FontWeights.SemiLight };
            run.Text = mesg;
            paragraph.Inlines.Add(run);
            RowDefinition def = new RowDefinition();
            def.Height = new GridLength(10);
            ChatGrid.RowDefinitions.Add(def);
            def.Height = GridLength.Auto;
            def = new RowDefinition();
            def.Height = GridLength.Auto;

            ChatGrid.RowDefinitions.Add(def);
            textOutPut.Blocks.Add(paragraph);
            ChatGrid.Children.Add(textOutPut);
            Grid.SetRow(textOutPut, ChatGrid.RowDefinitions.Count);
            Grid.SetRow(picture, ChatGrid.RowDefinitions.Count);
            Grid.SetColumn(textOutPut, col);
            
        }

        private void NewConversation_Click(object sender, RoutedEventArgs e)
        {
            var accessCode = "";
            if (!string.IsNullOrEmpty(accessCodeText.Text))
            {
                accessCode = accessCodeText.Text;
                Init(accessCode);
            }
            else
            {
                Init();
            }
            ChatGrid.Children.Clear();
            
        }
    }
}
