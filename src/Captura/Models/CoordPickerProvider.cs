using System;
using System.Drawing;
using System.Windows;
using Captura.Models;

namespace Captura
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CoordPickerProvider : ICoordProvider
    {
        readonly CoordPickerViewModel _coordPickerViewModel;

        public CoordPickerProvider(CoordPickerViewModel CoordPickerViewModel,
            IPlatformServices PlatformServices)
        {
            _coordPickerViewModel = CoordPickerViewModel;
        }

        public Rectangle SelectedRegion
        {
            get => new Rectangle();
            set => _coordPickerViewModel.PushCoord(value);
        }
    }
}