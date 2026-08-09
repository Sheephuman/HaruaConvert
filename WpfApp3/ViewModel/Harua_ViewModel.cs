using HaruaConvert.HaruaService;
using HaruaConvert.Json;
using HaruaConvert.mainUI.QueryCreateWindow.ViewModel;
using HaruaConvert.ViewModel.ffmpegOptions.CheckBox;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using WpfApp3.Parameter;
using static HaruaConvert.Parameter.ParamField;

namespace HaruaConvert.Parameter
{
    public class Harua_ViewModel : BindableBase
    {
        public Harua_ViewModel(MainWindow main)
        {
            _main = main;
        }

        private readonly MainWindow _main;
        private ISettingsService _settingsService;

        public Harua_ViewModel(ISettingsService settingsService, MainWindow main)
        {
            _main = main ?? throw new ArgumentNullException(nameof(main));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            LoadInitialData(_settingsService.Store);
        }

        public void LoadInitialData(AppSettingsStore store)
        {
            var settings = store.Current;

            ClassShearingMenbers.ffmpegQuery = string.IsNullOrEmpty(settings.FfmpegQuery.BaseQuery)
                ? ClassShearingMenbers.defaultQuery
                : settings.FfmpegQuery.BaseQuery;

            MainParams = new ObservableCollection<MainBindingParam>
            {
                new MainBindingParam
                {
                    StartQuery = ClassShearingMenbers.ffmpegQuery,
                    OutputPath = MainTab_OutputDirectory,
                    BackImageOpacity = settings.Appearance.BackImageOpacity,
                    endString = settings.FfmpegQuery.EndStrings,
                    SourcePathText = "フォルダ:" + (string.IsNullOrEmpty(settings.Directories.Convert)
                        ? "Source File"
                        : settings.Directories.Convert),
                    invisibleText = string.Empty,
                    placement = string.Empty,
                    ffmpegOptionsStateModel = new ffmpegDetailsOptionsStateModel
                    {
                        IsNoAudio = settings.CheckState.TryGetValue("NoAudio", out var noAudio) && noAudio,
                    },
                },
            };
        }

        private ObservableCollection<MainBindingParam> _mainParam = new ObservableCollection<MainBindingParam>();

        public BackColorViewModels BackColorView
        {
            get => field;
            set;
        } = new BackColorViewModels();

        public ObservableCollection<MainBindingParam> MainParams
        {
            get => _mainParam;
            set => SetProperty(ref _mainParam, value);
        }

        public string OutputPath { get; set; }

        public string SourcePathText
        {
            get { return field; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(field, "_sourcePathText is null");
                }

                SetProperty(ref field, value);
            }
        }
    }
}
