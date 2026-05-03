using System;
using System.Collections.Generic;
using System.Linq;

namespace DictionaryApp.Models
{
    /// <summary>
    /// Обобщённый класс словаря
    /// </summary>
    /// <typeparam name="TKey">Тип ключа (должен поддерживать сравнение)</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public class Dictionary<TKey, TValue>
    {
        private List<KeyValuePairStruct<TKey, TValue>> items;

        /// <summary>
        /// Конструктор - инициализирует пустой список
        /// </summary>
        public Dictionary()
        {
            items = new List<KeyValuePairStruct<TKey, TValue>>();
        }

        /// <summary>
        /// Свойство: количество элементов
        /// </summary>
        public int Count => items.Count;

        /// <summary>
        /// Свойство: есть ли элементы
        /// </summary>
        public bool IsEmpty => items.Count == 0;

        /// <summary>
        /// Свойство: массив ключей
        /// </summary>
        public TKey[] Keys => items.Select(i => i.Key).ToArray();

        /// <summary>
        /// Свойство: массив значений
        /// </summary>
        public TValue[] Values => items.Select(i => i.Value).ToArray();

        /// <summary>
        /// Свойство: массив пар ключ-значение
        /// </summary>
        public KeyValuePairStruct<TKey, TValue>[] Pairs => items.ToArray();

        /// <summary>
        /// Индексатор: возвращает элемент по ключу
        /// </summary>
        /// <param name="key">Ключ элемента</param>
        /// <returns>Значение по указанному ключу</returns>
        public TValue this[TKey key]
        {
            get
            {
                if (!ContainsKey(key))
                    throw new KeyNotFoundException($"Ключ '{key}' не найден в словаре.");
                
                return items.First(i => i.Key.Equals(key)).Value;
            }
            set
            {
                var index = items.FindIndex(i => i.Key.Equals(key));
                if (index >= 0)
                {
                    var pair = items[index];
                    pair.Value = value;
                    items[index] = pair;
                }
                else
                {
                    throw new KeyNotFoundException($"Ключ '{key}' не найден в словаре.");
                }
            }
        }

        /// <summary>
        /// Метод: добавить пару ключ и значение
        /// </summary>
        /// <param name="key">Ключ (должен быть уникальным)</param>
        /// <param name="value">Значение (может повторяться)</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если ключ уже существует</exception>
        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
                throw new InvalidOperationException($"Элемент с ключом '{key}' уже существует.");

            items.Add(new KeyValuePairStruct<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Метод: удалить элемент по ключу
        /// </summary>
        /// <param name="key">Ключ элемента для удаления</param>
        /// <returns>true, если элемент был удалён, иначе false</returns>
        public bool Remove(TKey key)
        {
            var item = items.FirstOrDefault(i => i.Key.Equals(key));
            return items.Remove(item);
        }

        /// <summary>
        /// Метод: проверить, есть ли элемент с заданным ключом
        /// </summary>
        /// <param name="key">Ключ для проверки</param>
        /// <returns>true, если элемент с таким ключом есть, иначе false</returns>
        public bool ContainsKey(TKey key)
        {
            return items.Any(i => i.Key.Equals(key));
        }

        /// <summary>
        /// Метод: очистить словарь
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }
    }
}