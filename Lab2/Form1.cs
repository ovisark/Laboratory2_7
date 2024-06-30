using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace Lab2
{
    public partial class Form1 : Form
    {
        // Список для хранения самых длинных слов
        private List<string> longestWords = new List<string>();
        // Список для хранения индексов самых длинных слов
        private List<int> longestWordIndices = new List<int>();
        // Индекс текущего выделенного слова с максимальной длиной
        private int currentHighlightIndex = 0;
        public Form1()
        {
            InitializeComponent();
            button4.Enabled = false;

            // Подписка на событие KeyDown формы
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyPreview = true; // Нужно для того, чтобы форма могла перехватывать нажатия клавиш
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Создание контекстного меню
            ContextMenu contextMenu1 = new ContextMenu();

            // Создание пунктов меню
            MenuItem menuItem1 = new MenuItem();
            MenuItem menuItem2 = new MenuItem();
            MenuItem menuItem3 = new MenuItem();

            // Добавление пунктов меню в контекстное меню
            contextMenu1.MenuItems.AddRange(new MenuItem[] { menuItem1, menuItem2, menuItem3 });

            // Настройка пунктов меню
            menuItem1.Index = 0;
            menuItem1.Text = "Открыть";
            menuItem2.Index = 1;
            menuItem2.Text = "Сохранить";
            menuItem3.Index = 2;
            menuItem3.Text = "Сохранить как";

            // Присвоение контекстного меню к richTextBox1
            richTextBox1.ContextMenu = contextMenu1;

            // Подписка на события клика по пунктам меню
            menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
        }

        private void menuItem1_Click(object sender, System.EventArgs e)
        {

            // Диалоговое окно открытия файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Чтение содержимого файла
                    string filePath = openFileDialog.FileName;
                    string fileContent = System.IO.File.ReadAllText(filePath);
                    richTextBox1.Text = fileContent;

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        private void menuItem2_Click(object sender, EventArgs e)
        {
            // Диалоговое окно сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Сохранение содержимого richTextBox1 в файл
                    string filePath = saveFileDialog.FileName;
                    System.IO.File.WriteAllText(filePath, richTextBox1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
                }
            }
        }
        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            // Диалоговое окно сохранения файла (аналогично menuItem2_Click)
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;
                    System.IO.File.WriteAllText(filePath, richTextBox1.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            richTextBox1.Clear();
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }
        int result1 = -1, result2 = -1;
        private void button2_Click(object sender, EventArgs e)
        {

            textBox3.Text += "Поиск первого слова" + Environment.NewLine;
            string FWord = textBox1.Text;
            result1 = FindWordPosition(FWord);
            if (result1 != -1)
            {
                textBox3.Text += "Позиция первого слова: " + result1 + Environment.NewLine + Environment.NewLine;
                HighlightWord(FWord, Color.Red);
                button2.Enabled = false;
                button4.Enabled = button3.Enabled == false;
            }
            else
            {
                textBox3.Text += "Слово не найдено " + Environment.NewLine + Environment.NewLine;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text += "Поиск второго слова" + Environment.NewLine;
            string FWord = textBox2.Text;
            result2 = FindWordPosition(FWord);
            if (result2 != -1)
            {
                textBox3.Text += "Позиция второго слова: " + result2 + Environment.NewLine + Environment.NewLine;
                HighlightWord(FWord, Color.Green);
                button3.Enabled = false;
                button4.Enabled = button2.Enabled == false;
            }
            else
            {
                textBox3.Text += "Слово не найдено " + Environment.NewLine + Environment.NewLine;
            }
        }


        int FindWordPosition(string FWord)
        {
            // Поиск позиции слова в тексте
            string[] words = richTextBox1.Text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Equals(FWord, StringComparison.OrdinalIgnoreCase))
                {
                    return i + 1; // Позиция слова (1-based index)
                }
            }
            return -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (result1 != -1 && result2 != -1)
            {
                string word1 = textBox1.Text;
                string word2 = textBox2.Text;
                richTextBox1.Text = SwapWords(richTextBox1.Text, word1, word2);
                textBox3.Text += "Произошла замена слов" + Environment.NewLine;
                button4.Enabled = false;
            }
        }

        // Обработчик события KeyDown для выделения следующего самого длинного слова
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            HighlightNextLongestWord();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Выделение первого самого длинного слова при открытии файла
            HighlightNextLongestWord();
            textBox3.Clear();
            FindLongestWordsAndIndices();
            if (longestWords.Count > 0)
            {
                textBox3.Text += $"Самые длинные слова: {string.Join(", ", longestWords)}, количество символов: {longestWords[0].Length}\n";
                HighlightNextLongestWord(); // Выделяем первое самое длинное слово после обновления списка
            }
        }

        string SwapWords(string text, string word1, string word2)
        {
            // Разделяем текст на строки
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Проходим по каждой строке и заменяем слова
            for (int i = 0; i < lines.Length; i++)
            {
                // Разделяем строку на слова
                string[] words = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Ищем индексы слов для замены
                int index1 = Array.FindIndex(words, w => w.Equals(word1, StringComparison.OrdinalIgnoreCase));
                int index2 = Array.FindIndex(words, w => w.Equals(word2, StringComparison.OrdinalIgnoreCase));

                // Если оба слова найдены в строке, меняем их местами
                if (index1 != -1 && index2 != -1)
                {
                    string temp = words[index1];
                    words[index1] = words[index2];
                    words[index2] = temp;
                }

                // Соединяем слова обратно в строку
                lines[i] = string.Join(" ", words);
            }

            // Соединяем строки обратно в текст с сохранением переносов строк
            return string.Join(Environment.NewLine, lines);
        }

        // Выделение слова в richTextBox1 определенным цветом
        void HighlightWord(string FWord, Color color)
        {
            int startIndex = 0;
            while ((startIndex = richTextBox1.Text.IndexOf(FWord, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                richTextBox1.Select(startIndex, FWord.Length);
                richTextBox1.SelectionBackColor = color;
                startIndex += FWord.Length;
            }
        }

        private void HighlightNextLongestWord()
        {
            // Если список индексов самых длинных слов пуст, завершаем метод
            if (longestWordIndices.Count == 0)
                return;

            // Снимаем выделение со всех слов в richTextBox1, устанавливая им белый цвет фона
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;

            // Определяем индекс слова для выделения на основе текущего индекса выделения и количества индексов в списке longestWordIndices
            int index = currentHighlightIndex % longestWordIndices.Count;

            // Получаем слово для выделения из текста richTextBox1 по индексу в longestWordIndices
            string wordToHighlight = richTextBox1.Text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)[longestWordIndices[index]];

            // Находим начальную позицию слова в тексте richTextBox1
            int startIndex = richTextBox1.Text.IndexOf(wordToHighlight, StringComparison.OrdinalIgnoreCase);

            // Если слово найдено, выделяем его желтым цветом
            if (startIndex != -1)
            {
                richTextBox1.Select(startIndex, wordToHighlight.Length);
                richTextBox1.SelectionBackColor = Color.Yellow;
            }

            // Увеличиваем текущий индекс для следующего вызова метода
            currentHighlightIndex++;
        }

        // Метод для поиска всех слов с максимальной длиной и их индексов
        private void FindLongestWordsAndIndices()
        {
            // Очистка списков самых длинных слов и их индексов перед началом поиска
            longestWords.Clear();
            longestWordIndices.Clear();

            // Разделение текста на слова по разделителям (пробел, новая строка, возврат каретки) и удаление пустых элементов
            string[] words = richTextBox1.Text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Поиск самого длиного слова в тексте
            int maxWordLength = words.Max(w => w.Length);

            // Добавление в список самых длинных слов и их индексов
            for (int i = 0; i < words.Length; i++)
            {
                // Если длина текущего слова равна максимальной найденной длине
                if (words[i].Length == maxWordLength)
                {
                    // Добавление слова в список самых длинных слов
                    longestWords.Add(words[i]);
                    // Добавление индекса слова в список индексов самых длинных слов
                    longestWordIndices.Add(i);
                }
            }
        }
    }
}
