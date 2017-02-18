using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace BusinessAccountingUniverse.Localization
{
    internal class Localize
    {
        public const string ApplicationName = "Business Accounting Universe";
        private const string DefaultLanguage = "en";
        private DataSet _languageDataset;

        public Localize()
        {
            GetXmlResource();
        }

        private string CurrentLanguage
        {
            get
            {
                var language = Application.CurrentCulture.TwoLetterISOLanguageName;

                if (language.Length == 0)
                {
                    language = DefaultLanguage;
                }

                return language.ToLower();
            }
        }

        private void GetXmlResource()
        {
            var assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                return;
            }

            var localizationFile = $"{assembly.GetName().Name}.Localization.{CurrentLanguage}.xml";
            var xmlStream = assembly.GetManifestResourceStream(localizationFile);


            if (_languageDataset == null)
                _languageDataset = new DataSet();

            if (xmlStream != null)
            {
                var importDataset = new DataSet();
                importDataset.ReadXml(xmlStream);

                _languageDataset.Merge(importDataset);
                xmlStream.Close();
            }
        }

        public string GetStringById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }

            if (_languageDataset?.Tables["Localization"] == null)
            {
                return $"%{id}%";
            }

            var message = _languageDataset.Tables["Localization"].Select($"Id='{id}'");
            if (message.Length <= 0)
            {
                return $"%{id}%";
            }

            return message[0]["String"].ToString();
        }
    }
}
