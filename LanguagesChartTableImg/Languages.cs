using System;
using System.ComponentModel;

namespace LanguagesChartTableImg
{
    public enum ENumGroups { Скандинавская, Германская, Индийсякая, Славянская, Иранская};
    [Serializable]
    public class Languages
    {
        [DisplayName("Язык"), Category("Сводка")]
        public string NameL { get; set; }
        
        [DisplayName("Короткое название"), Category("Дополнительно")]
        public string ShortName { get; set; }
        
        [DisplayName("Кол-во говорящих"), Category("Сводка")]
        public uint SpeakersAmount { get; set; }
        
        [DisplayName("Основная страна"), Category("Сводка")]
        public string Country { get; set; }

        [DisplayName("Картинка"), Category("Техническое"), ReadOnly(true), DesignOnly(true),]
        public string Image { get; set; }
        
        [DisplayName("Группа"), Category("Дополнительно")]
        public ENumGroups GroupLang{ get; set; }
        
        [Browsable(false)]
        public string StrType
        {
            get => GroupLang.ToString();
            set => GroupLang.ToString();
        }
        public Languages()
        {
            Image = "../../../pics/click.png";
        }
        
        public Languages(string name, string shortName, uint speakersAmount, string image, string country)
        {
            NameL = name;
            ShortName = shortName;
            SpeakersAmount = speakersAmount;    
            Image = image;
            Country = country;
        }
    }
}