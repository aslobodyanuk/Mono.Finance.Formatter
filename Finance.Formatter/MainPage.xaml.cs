using Finance.Formatter.Core;

namespace Finance.Formatter;

public partial class MainPage : ContentPage
{
    private readonly FilePickerService _filePickerService;
    private readonly ProcessingService _processingService;
    private readonly ExportService _exportService;

    public MainPage(FilePickerService filePickerService, ProcessingService processingService, ExportService exportService)
	{
		InitializeComponent();
        _filePickerService = filePickerService;
        _processingService = processingService;
        _exportService = exportService;
    }

    private async void Page_Loaded(object sender, EventArgs e)
    {

    }

    private void OnCounterClicked(object sender, EventArgs e)
	{

	}

    private async void OnPickFileButtonClicked(object sender, EventArgs e)
    {
        var files = await _filePickerService.SelectFinanceFilesAsync();
        var processed = _processingService.ProcessData(files);
        _exportService.ExportTransactions(processed, "C:\\Temp\\output2.xlsx");
    }
}

