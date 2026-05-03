using System;

namespace DictionaryApp.Models
{
    /// <summary>
    /// Структура для хранения пары ключ-значение
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public struct KeyValuePairStruct<TKey, TValue>
    {
        /// <summary>
        /// Ключ элемента словаря
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// Значение элемента словаря
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Конструктор структуры
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        public KeyValuePairStruct(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Переопределение метода ToString для отображения пары
        /// </summary>
        /// <returns>Строковое представление пары ключ-значение</returns>
        public override string ToString()
        {
            return $"{Key} : {Value}";
        }

        /// <summary>
        /// Переопределение метода Equals для сравнения структур
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is KeyValuePairStruct<TKey, TValue> other)
            {
                return Key.Equals(other.Key) && Value.Equals(other.Value);
            }
            return false;
        }

        /// <summary>
        /// Переопределение GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Value);
        }
    }
}