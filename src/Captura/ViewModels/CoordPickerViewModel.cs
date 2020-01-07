using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using Reactive.Bindings;
using System.Text.RegularExpressions;

// ReSharper disable MemberCanBePrivate.Global

namespace Captura
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CoordPickerViewModel : NotifyPropertyChanged
    {
        public CoordSettings CoordSettings;

        class CoordinateDesc
        {
            public Rectangle rect;
            public string name = "";
        }

        List<CoordinateDesc> _coordDescList = new List<CoordinateDesc>();

        string _template;

        public CoordPickerViewModel(Settings Settings)
        {
            CoordSettings = Settings.Coord;
            _template = CoordSettings.Template;

            ClearCommand = new ReactiveCommand()
                .WithSubscribe(() => Clear());
            UndoCommand = new ReactiveCommand()
                .WithSubscribe(() => Undo());
        }

        public string Template
        {
            get => _template;
            set
            {
                Set(ref _template, value);
                CoordSettings.Template = value;
                RaisePropertyChanged(nameof(TemplateOutput));
            }
        }

        public string TemplateOutput
        {
            get
            {
                string result = "";

                if (!string.IsNullOrEmpty(Template))
                {
                    foreach (CoordinateDesc item in _coordDescList)
                    {
                        if (_coordDescList.First() != item)
                            result += System.Environment.NewLine;

                        Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                        {
                            { "top",    item.rect.Top.ToString()    },
                            { "bottom", item.rect.Bottom.ToString() },
                            { "left",   item.rect.Left.ToString()   },
                            { "right",  item.rect.Right.ToString()  },
                            { "width",  item.rect.Width.ToString()  },
                            { "height", item.rect.Height.ToString() },
                            { "name",   item.name                   }
                        };

                        result += Regex.Replace(Template, @"\{(.+?)\}", m => parameters.TryGetValue(m.Groups[1].Value, out var val) ? val : m.ToString());
                    }
                }
                return result;
            }
        }

        public void Clear()
        {
            if (_coordDescList.Any())
            {
                _coordDescList.Clear();
                RaisePropertyChanged(nameof(TemplateOutput));
            }
        }

        public void Undo()
        {
            if (_coordDescList.Any())
            {
                _coordDescList.RemoveAt(_coordDescList.Count - 1);
                RaisePropertyChanged(nameof(TemplateOutput));
            }
        }

        public void PushCoord(Rectangle rect)
        {
            _coordDescList.Add(new CoordinateDesc { rect = rect });
            RaisePropertyChanged(nameof(TemplateOutput));
        }

        public string CoordinatesTemplateHelp { get; } = @"Selected region is defined by its width, height, and upper-left corner.
Available template variables:
{top} - the y-coordinate of the top edge
{bottom} - the y-coordinate that is the sum of the {top} and {height} values
{left} - the x-coordinate of the left edge
{right} - the x-coordinate that is the sum of {left} and {width} values
{width} - the width of the selected region
{height} - the height of the selected region";

        public ICommand ClearCommand { get; }
        public ICommand UndoCommand { get; }
    }
}