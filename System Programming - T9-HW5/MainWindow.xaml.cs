using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace System_Programming___T9_HW5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int clickCount = 0; // Счётчик для отслеживания нажатий
        private string lastButton = "";
        //позволяет безопасно обновлять элементы интерфейса непосредственно из обработчиков DispatcherTimer,
        //так как он запускает события в том же UI-потоке, в котором работают все элементы WPF
        private DispatcherTimer clickTimer;

        private List<string> dictionary;
        private const string DictionaryFilePath = "Dictionary.txt";
        public MainWindow()
        {
            InitializeComponent();

            clickTimer = new DispatcherTimer();
            clickTimer.Interval = TimeSpan.FromSeconds(1); // интервал в 1 секунду
            clickTimer.Tick += ClickTimer_Tick;
            dictionary = LoadDictionary(DictionaryFilePath);
        }

        private void ClickTimer_Tick(object? sender, EventArgs e)
        {
            clickTimer.Stop(); 
            clickCount = 0;    
            lastButton = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string character = "1";
            HandleButtonClick("1", new string[] { character });
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string[] characters = { "2", "А", "Б", "В", "Г", "A", "B", "C" }; 
            HandleButtonClick("2", characters);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string[] characters = { "3", "Д", "Е", "Ж", "З", "D", "E", "F" }; 
            HandleButtonClick("3", characters);
        }

        
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string[] characters = { "4", "И", "Й", "К", "Л", "G", "H", "I" }; 
            HandleButtonClick("4", characters);
        }

        
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string[] characters = { "5", "М", "Н", "О", "П", "J", "K", "L" }; 
            HandleButtonClick("5", characters);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string[] characters = { "6", "Р", "С", "Т", "У", "M", "N", "O" };
            HandleButtonClick("6", characters);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            string[] characters = { "7", "Ф", "Х", "С", "Ч", "P", "Q", "R", "S" };
            HandleButtonClick("7", characters);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            string[] characters = { "8", "Ш", "Щ", "Ъ", "Ы", "T", "U", "V" };
            HandleButtonClick("8", characters);
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            string[] characters = { "9", "Ь", "Э", "Ю", "Я", "W", "X", "Y", "Z" };
            HandleButtonClick("9", characters);
        }

        private void Button_Click_Star(object sender, RoutedEventArgs e)
        {
            string character = "*";
            HandleButtonClick("*", new string[] { character });
        }

        private void Button_Click_0(object sender, RoutedEventArgs e)
        {
            string[] characters = { "0", "+" };
            HandleButtonClick("0", characters);
        }

        private void Button_Click_Hash(object sender, RoutedEventArgs e)
        {
            string character = "#";
            HandleButtonClick("#", new string[] { character });
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // фокус на TextBox
            Keyboard.Focus(InputTextBox);

            if (InputTextBox.Text.Length > 0)
            {
                // Если курсор не в начале текста, удаляем символ слева от курсора
                if (InputTextBox.CaretIndex > 0)
                {
                    int caretPos = InputTextBox.CaretIndex;
                    InputTextBox.Text = InputTextBox.Text.Remove(caretPos - 1, 1);
                    // Сдвигаем курсор обратно на позицию после удаления
                    InputTextBox.CaretIndex = caretPos - 1;
                }
                // Если курсор в начале, по-прежнему удаляем последний символ
                else if (InputTextBox.CaretIndex == 0)
                {
                    InputTextBox.Text = InputTextBox.Text.Remove(InputTextBox.Text.Length - 1);
                }
            }
        }

        private void HandleButtonClick(string button, string[] characters)
        {
            // Если это новая кнопка, фиксируем последний символ и сбрасываем состояние
            if (lastButton != button && lastButton != "")
            {
                // Не сбрасываем lastButton сразу, фиксируем его и сбрасываем только после добавления символа
                clickCount = 0; // Сбрасываем счётчик, чтобы начать с первой буквы
            }

            // Если это та же кнопка, что и раньше, переключаем символ
            if (lastButton == button)
            {
                clickCount = (clickCount + 1) % characters.Length;
            }
            else
            {
                // Если новая кнопка, сбрасываем счётчик
                clickCount = 0;
                lastButton = button;
            }

            // символ в TextBox
            AddCharacterToTextBox(characters[clickCount]);
            clickTimer.Stop();
            clickTimer.Start();
        }

        private void AddCharacterToTextBox(string character)
        {
            // Если это повторное нажатие на ту же кнопку, заменяем последний символ
            if (lastButton != "" && InputTextBox.Text.Length > 0 && clickCount > 0)
            {
                // Удаляем последний символ и заменяем его новым
                InputTextBox.Text = InputTextBox.Text.Remove(InputTextBox.Text.Length - 1);
            }

            // Добавляем выбранный символ
            InputTextBox.Text += character;
        }

        private List<string> LoadDictionary(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath).ToList();
            }
            catch (IOException ex)
            {
                MessageBox.Show("Ошибка при загрузке словаря: " + ex.Message);
                return new List<string>();
            }
        }

        private void MoveCursorLeft(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(InputTextBox);
            if (InputTextBox.CaretIndex > 0)
            {
                InputTextBox.CaretIndex--; 
            }
        }

        private void MoveCursorRight(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(InputTextBox);
            if (InputTextBox.CaretIndex < InputTextBox.Text.Length)
            {
                InputTextBox.CaretIndex++; 
            }
        }

        private void MoveCursorUp(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(InputTextBox);
            int currentLineIndex = InputTextBox.GetLineIndexFromCharacterIndex(InputTextBox.CaretIndex);
            if (currentLineIndex > 0) // Проверяем, что не на первой строке
            {
                int previousLineStartIndex = InputTextBox.GetCharacterIndexFromLineIndex(currentLineIndex - 1);
                int caretPositionInCurrentLine = InputTextBox.CaretIndex - InputTextBox.GetCharacterIndexFromLineIndex(currentLineIndex);
                int previousLineLength = InputTextBox.GetLineLength(currentLineIndex - 1);

                // Перемещаем на предыдущую строку
                InputTextBox.CaretIndex = previousLineStartIndex + Math.Min(caretPositionInCurrentLine, previousLineLength);
            }
        }

        private void MoveCursorDown(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(InputTextBox);
            int currentLineIndex = InputTextBox.GetLineIndexFromCharacterIndex(InputTextBox.CaretIndex);
            if (currentLineIndex < InputTextBox.LineCount - 1) // Проверяем, что не на последней строке
            {
                int nextLineStartIndex = InputTextBox.GetCharacterIndexFromLineIndex(currentLineIndex + 1);
                int caretPositionInCurrentLine = InputTextBox.CaretIndex - InputTextBox.GetCharacterIndexFromLineIndex(currentLineIndex);
                int nextLineLength = InputTextBox.GetLineLength(currentLineIndex + 1);

                // Перемещаем на следующую строку
                InputTextBox.CaretIndex = nextLineStartIndex + Math.Min(caretPositionInCurrentLine, nextLineLength);
            }
        }

        private async void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string currentText = InputTextBox.Text;

            // Текст на слова
            // StringSplitOptions.RemoveEmptyEntries - для управления тем, как строка разбивается на подстроки.
            // Он определяет, нужно ли исключать пустые строки из результата при разделении строки.
            string[] words = currentText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Последнее слово (если оно есть)
            string lastWord = words.LastOrDefault();

            // Поиск похожих слов в словаре
            if (!string.IsNullOrEmpty(lastWord))
            {
                // Запуск поиска в отдельном потоке
                var suggestions = await Task.Run(() =>
                {
                    return dictionary
                        .Where(word => word.StartsWith(lastWord, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                });

                // Если есть предложения, показываем первое
                if (suggestions.Count > 0)
                {
                    SuggestedWord.Text = suggestions[0];
                }
                else
                {
                    SuggestedWord.Text = "";
                }
            }
            else
            {
                SuggestedWord.Text = ""; // Если нет слова, очищаем предложение
            }
        }


        private void SuggestedWord_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Добавляем предложенное слово в текстовое поле
            if (!string.IsNullOrEmpty(SuggestedWord.Text))
            {
                string currentText = InputTextBox.Text;
                string[] words = currentText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Если последнее слово не пустое, добавляем предложенное слово
                if (words.Length > 0)
                {
                    // Удаляем последнее слово и добавляем предложенное
                    currentText = currentText.TrimEnd(); // Удаляем пробел в конце
                    int lastSpaceIndex = currentText.LastIndexOf(' ');
                    if (lastSpaceIndex >= 0)
                    {
                        currentText = currentText.Substring(0, lastSpaceIndex + 1) + SuggestedWord.Text + " ";
                    }
                    else
                    {
                        currentText = SuggestedWord.Text + " "; // Если нет пробелов, просто добавляем слово
                    }

                    InputTextBox.Text = currentText;
                    InputTextBox.CaretIndex = InputTextBox.Text.Length; // Перемещаем курсор в конец
                }

                SuggestedWord.Text = ""; 
            }
        }

        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            string inputText = InputTextBox.Text;

            if (!string.IsNullOrEmpty(inputText))
            {
                string[] newWords = inputText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> addedWords = new List<string>();

                foreach (string word in newWords)
                {
                    // Проверяем, если слово уже есть в словаре
                    if (!dictionary.Contains(word, StringComparer.OrdinalIgnoreCase))
                    {
                        // Если нет, добавляем его в список добавляемых слов
                        addedWords.Add(word);
                        dictionary.Add(word); 
                    }
                }

                if (addedWords.Count > 0)
                {
                    // Сохраняем новые слова в файл
                    File.AppendAllLines(DictionaryFilePath, addedWords);
                    // Перезагружаем словарь из файла
                    dictionary = LoadDictionary(DictionaryFilePath);
                    MessageBox.Show($"Слова добавлены в словарь: {string.Join(", ", addedWords)}.");
                }
                else
                {
                    MessageBox.Show("Все слова уже есть в словаре.");
                }
            }
            else
            {
                MessageBox.Show("Текст пуст.");
            }
        }

    }
}
