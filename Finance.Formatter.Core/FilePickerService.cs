using Finance.Formatter.Models;

namespace Finance.Formatter.Core
{
    public class FilePickerService
    {
        public async Task<IEnumerable<string>> SelectFinanceFilesAsync()
        {
            var options = GetPickOptions();
            var result = await FilePicker.Default.PickMultipleAsync(options);
            var validResults = result?.Where(x => x.FileName.EndsWith(StaticResources.CSV, StringComparison.OrdinalIgnoreCase))
                ?.Select(x => x.FileName);

            return validResults ?? Enumerable.Empty<string>();
        }

        private PickOptions GetPickOptions()
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { StaticResources.CSV_MIME, StaticResources.CSV_MIME_EXCEL } },
                    { DevicePlatform.WinUI, new[] { StaticResources.CSV } }
                });

            return new PickOptions()
            {
                PickerTitle = StaticResources.FILE_PICKER_TEXT,
                FileTypes = customFileType,
            };
        }
    }
}