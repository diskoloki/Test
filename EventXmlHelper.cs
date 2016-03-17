using System;
using System.Xml;
using Beeline.Model;

namespace Beeline.TfsServices
{
    /// <summary>
    /// Содержит методы обработки XML сообщения от TFS.
    /// </summary>
    internal class EventXmlHelper
    {
        internal enum FieldSection
        {
            CoreFields,
            ChangedFields
        }

        internal enum FieldType
        {
            IntegerField,
            StringField
        }

        internal enum ValueType
        {
            NewValue,
            OldValue
        }

        /// <summary>
        /// Возвращает значение из XML сообщения.
        /// </summary>
        /// <param name="eventXml">Сообщение в формате XML.</param>
        /// <param name="section">Секция данных (измененные...)</param>
        /// <param name="fieldType">Тип поля.</param>
        /// <param name="valueType">Тип значения (новое, старое)</param>
        /// <param name="refName">Имя поля.</param>
        internal static T GetWorkItemValue<T>(string eventXml, FieldSection section, FieldType fieldType, ValueType valueType, string refName)
        {
            var path = string.Format("/WorkItemChangedEvent/{0}/{1}s/Field[ReferenceName='{3}']/{2}", section, fieldType, valueType, refName);

            var doc = new XmlDocument();
            doc.LoadXml(eventXml);

            var node = doc.SelectSingleNode(path);

            object text;
            if (node == null)
            {
                if (typeof(T) == typeof(int))
                {
                    text = 0;
                }
                else if (typeof(T) == typeof(string))
                {
                    text = string.Empty;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                text = node.InnerText;
            }

            return (T)Convert.ChangeType(text, typeof(T));
        }

        /// <summary>
        /// Возвращает ID WorkItem из XML сообщения.
        /// </summary>
        /// <param name="eventXml">Сообщение в формате XML.</param>
        internal static int GetWorkItemId(string eventXml)
        {
            var workItemId = GetWorkItemValue<int>(eventXml,
                                                   FieldSection.CoreFields,
                                                   FieldType.IntegerField,
                                                   ValueType.NewValue, TaskFieldName.Id);
            return workItemId;
        }

        internal static string GetProjectName(string eventXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(eventXml);
            return doc.SelectSingleNode("/WorkItemChangedEvent/PortfolioProject").InnerText;
        }

        /// <summary>
        /// Извлекает URL для подключения к TFS из XML строки.
        /// </summary>
        /// <param name="tfsIdentityXml">XML строка содержащая строку подключения к TFS.</param>
        internal static string ExtractUrlFromIdentityXml(string tfsIdentityXml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(tfsIdentityXml);

            if (doc.FirstChild == null || doc.FirstChild.Attributes == null)
                throw new InvalidOperationException("url not found");

            return doc.FirstChild.Attributes["url"].Value;
        }
    }
}