using System.Net;

namespace Captura
{
    public class CoordSettings : PropertyStore
    {
        public string Template
        {
            get => Get("[{top}, {bottom}, {left}, {right}],");
            set => Set(value);
        }
    }
}
