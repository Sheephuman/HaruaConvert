namespace HaruaConvert.Json
{
    public sealed class AppSettingsSelectorAccessor
    {
        public ParamSelectorSettings GetOrCreate(AppSettings settings, int index)
        {
            while (settings.Selectors.Count <= index)
            {
                settings.Selectors.Add(new ParamSelectorSettings());
            }

            return settings.Selectors[index];
        }
    }
}
