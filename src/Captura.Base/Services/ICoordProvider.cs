using System;
using System.Drawing;

namespace Captura.Models
{
    public interface ICoordProvider
    {
        Rectangle SelectedRegion { get; set; }
    }
}
