using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DictionaryApp.Models;

namespace DictionaryApp.ViewModels
{
    public class DictionaryViewModel : INotifyPropertyChanged
    {
        private Dictionary<string, string> dictionary;
        private string keyInput;
        private string valueInput;
        private string searchKey;
        private string removeKey;
        private string resultMessage;

        public ObservableCollection<KeyValuePairStruct<string, string>> DictionaryItems { get; set; }

        public string KeyInput
        {
            get => keyInput;
            set
            {
                keyInput = value;
                OnPropertyChanged();
            }
        }

        public string ValueInput
        {
            get => valueInput;
            set
            {
                valueInput = value;
                OnPropertyChanged();
            }
        }

        public string SearchKey
        {
            get => searchKey;
            set
            {
                searchKey = value;
                OnPropertyChanged();
            }
        }

        public string RemoveKey
        {
            get => removeKey;
            set
            {
                removeKey = value;
                OnPropertyChanged();
            }
        }

        public string ResultMessage
        {
            get => resultMessage;
            set
            {
                resultMessage = value;
                OnPropertyChanged();
            }
        }

        public int Count => dictionary.Count;
        public bool IsEmpty => dictionary.IsEmpty;
        public string Keys => string.Join(", ", dictionary.Keys);
        public string Values => string.Join(", ", dictionary.Values);

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand ContainsCommand { get; }
        public ICommand ClearCommand { get; }

        public DictionaryViewModel()
        {
            dictionary = new Dictionary<string, string>();
            DictionaryItems = new ObservableCollection<KeyValuePairStruct<string, string>>();

            AddCommand = new RelayCommand(AddEntry, CanAddEntry);
            RemoveCommand = new RelayCommand(RemoveEntry, CanRemoveEntry);
            ContainsCommand = new RelayCommand(CheckContains, CanCheckContains);
            ClearCommand = new RelayCommand(ClearDictionary, CanClearDictionary);

            UpdateDisplay();
        }

        private void AddEntry()
        {
            try
            {
                dictionary.Add(KeyInput, ValueInput);
                UpdateDisplay();
                ResultMessage = $"Элемент добавлен: {KeyInput} = {ValueInput}";
                KeyInput = string.Empty;
                ValueInput = string.Empty;
            }
            catch (Exception ex)
            {
                ResultMessage = $"Ошибка: {ex.Message}";
            }
        }

        private bool CanAddEntry()
        {
            return !string.IsNullOrWhiteSpace(KeyInput) && !string.IsNullOrWhiteSpace(ValueInput);
        }

        private void RemoveEntry()
        {
            try
            {
                if (dictionary.Remove(RemoveKey))
                {
                    UpdateDisplay();
                    ResultMessage = $"Элемент с ключом '{RemoveKey}' удалён";
                    RemoveKey = string.Empty;
                }
                else
                {
                    ResultMessage = $"Ключ '{RemoveKey}' не найден";
                }
            }
            catch (Exception ex)
            {
                ResultMessage = $"Ошибка: {ex.Message}";
            }
        }

        private bool CanRemoveEntry()
        {
            return !string.IsNullOrWhiteSpace(RemoveKey);
        }

        private void CheckContains()
        {
            bool exists = dictionary.ContainsKey(SearchKey);
            if (exists)
            {
                ResultMessage = $"Ключ '{SearchKey}' существует в словаре. Значение: {dictionary[SearchKey]}";
            }
            else
            {
                ResultMessage = $"Ключ '{SearchKey}' не найден в словаре";
            }
        }

        private bool CanCheckContains()
        {
            return !string.IsNullOrWhiteSpace(SearchKey);
        }

        private void ClearDictionary()
        {
            dictionary.Clear();
            UpdateDisplay();
            ResultMessage = "Словарь очищен";
        }

        private bool CanClearDictionary()
        {
            return !dictionary.IsEmpty;
        }

        private void UpdateDisplay()
        {
            DictionaryItems.Clear();
            foreach (var pair in dictionary.Pairs)
            {
                DictionaryItems.Add(pair);
            }

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Вспомогательный класс для команд
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}