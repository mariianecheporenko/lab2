using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using DormApp.Processors;
using DormApp.Strategies;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using DormApp.Dialogs;

namespace DormApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly XmlProcessor _processor = new XmlProcessor();
        private string _currentXmlPath = "";

        private readonly Dictionary<string, string> _attributeLocalization = new Dictionary<string, string>
        {
            { "faculty", "Факультет" },
            { "room", "Кімната" },
            { "FullName", "ПІБ" },
            { "Department", "Спеціальність" },
            { "Chair", "Кафедра" },
            { "Course", "Курс" }
        
        };
        public MainWindow()
        {
            InitializeComponent();

            // При закритті вікна — підтвердження
            this.Closing += MainWindow_Closing;
        }

        private async void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true; // скасовуємо стандартне закриття поки не підтверджено
            var dlg = new ConfirmDialog("Чи дійсно ви хочете завершити роботу з програмою?");
            var res = await dlg.ShowDialog<bool>(this);
            if (res)
            {
                // Закриваємо остаточно
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
            }
        }

        private async void BtnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            // Відкриваємо FileDialog для вибору XML
            var ofd = new OpenFileDialog();
            ofd.Filters.Add(new FileDialogFilter() { Name = "XML files", Extensions = { "xml" } });
            ofd.AllowMultiple = false;
            var res = await ofd.ShowAsync(this);
            if (res != null && res.Length > 0)
            {
                _currentXmlPath = res[0];
                TxtFilePath.Text = _currentXmlPath;
                TxtInfo.Text = "Завантажено файл. Зчитую атрибути...";
                    try
                {
                    // Заповнюємо список атрибутів (динамічно)
                    var attributes = _processor.GetAttributeNames(_currentXmlPath);
                    var localizedAttributes = attributes
                        .Select(attr => _attributeLocalization.GetValueOrDefault(attr, attr))
                        .ToList();
                    CmbAttribute.ItemsSource = localizedAttributes;

                    CmbAttribute.SelectedIndex = localizedAttributes.Any() ? 0 : -1;

                    TxtInfo.Text = $"Знайдено атрибутів: {attributes.Count}";
                }
                catch (Exception ex)
                {
                    TxtInfo.Text = "Помилка при читанні XML: " + ex.Message;
                }

            }
        }

        private string GetInternalAttributeName(string? displayedName)
        {
            if (string.IsNullOrEmpty(displayedName)) return "";

            // Шукаємо у словнику "навпаки" (за значенням)
            return _attributeLocalization
                       .FirstOrDefault(kvp => kvp.Value == displayedName)
                       .Key // Якщо знайшли, повертаємо ключ (англ. "faculty")
                   ?? displayedName; // Якщо не знайшли, повертаємо як є
        }

        private void CmbAttribute_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            string displayedName = CmbAttribute.SelectedItem as string ?? "";
            string internalName = GetInternalAttributeName(displayedName);

            if (!string.IsNullOrEmpty(internalName) && !string.IsNullOrEmpty(_currentXmlPath))
            {
                try
                {
                    var values = _processor.GetValuesForAttribute(_currentXmlPath, internalName); 

                    CmbAttributeValue.ItemsSource = values;
                    CmbAttributeValue.SelectedIndex = values.Any() ? 0 : -1;
        
                    TxtInfo.Text = $"Знайдено значень для '{displayedName}': {values.Count}"; 
                }
                catch (Exception ex)
                {
                    TxtInfo.Text = "Помилка: " + ex.Message;
                }
            }
        }


        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentXmlPath))
            {
                TxtInfo.Text = "Спочатку завантаж файл (Load XML).";
                return;
            }

            string chosenStrategy = (CmbStrategy.SelectedItem as ComboBoxItem)?.Content?.ToString()
                ?? "XmlReader (SAX-like)";

            IXmlParserStrategy strategy = chosenStrategy switch
            {
                var s when s.Contains("XmlReader") => new XmlReaderStrategy(),
                var s when s.Contains("XmlDocument") => new XmlDocumentStrategy(),
                var s when s.Contains("LINQ") => new LinqToXmlStrategy(),
                _ => new XmlReaderStrategy()
            };

            _processor.SetStrategy(strategy);

            string displayedAttribute = CmbAttribute.SelectedItem as string ?? "";
            string attribute = GetInternalAttributeName(displayedAttribute);
            string attrValue = CmbAttributeValue.SelectedItem as string ?? "";
            string keyword = TxtKeyword.Text?.Trim() ?? "";

            try
            {
                var results = await Task.Run(() => _processor.ParseAndSearch(_currentXmlPath, attribute, attrValue, keyword));
                TxtResults.Text = string.Join(Environment.NewLine + Environment.NewLine, results.Select(r => r.ToReadableString()));
                TxtInfo.Text = $"Знайдено {results.Count} записів ({strategy.GetType().Name}).";
            }
            catch (Exception ex)
            {
                TxtInfo.Text = "Помилка під час пошуку: " + ex.Message;
            }
        }

        private async void BtnTransform_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentXmlPath))
            {
                TxtInfo.Text = "Спочатку завантаж файл (Load XML).";
                return;
            }

            // Відкрити XSL файл
            var ofd = new OpenFileDialog();
            ofd.Filters.Add(new FileDialogFilter() { Name = "XSL files", Extensions = { "xsl", "xslt" } });
            var res = await ofd.ShowAsync(this);
            if (res == null || res.Length == 0) return;
            string xslPath = res[0];

            // Зберегти HTML результат
            var sfd = new SaveFileDialog();
            sfd.Filters.Add(new FileDialogFilter() { Name = "HTML", Extensions = { "html", "htm" } });
            sfd.InitialFileName = "result.html";
            var savePath = await sfd.ShowAsync(this);
            if (string.IsNullOrEmpty(savePath)) return;

            try
            {
                _processor.TransformXmlToHtml(_currentXmlPath, xslPath, savePath);
                TxtInfo.Text = $"Перетворення завершено. Збережено у {savePath}";
            }
            catch (Exception ex)
            {
                TxtInfo.Text = "Помилка перетворення: " + ex.Message;
            }
        }

        private async void BtnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filters.Add(new FileDialogFilter() { Name = "Text", Extensions = { "txt" } });
            sfd.InitialFileName = "results.txt";
            var savePath = await sfd.ShowAsync(this);
            if (string.IsNullOrEmpty(savePath)) return;

            await File.WriteAllTextAsync(savePath, TxtResults.Text);
            TxtInfo.Text = "Результати збережно до " + savePath;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxtResults.Text = "";
            TxtKeyword.Text = "";
            CmbAttributeValue.SelectedIndex = -1;
            TxtInfo.Text = "Очищено.";
        }

        private async void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ConfirmDialog("Чи дійсно ви хочете завершити роботу з програмою?");
            var res = await dlg.ShowDialog<bool>(this);
            if (res)
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
            }
        }
    }
}
