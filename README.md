# Mono.Finance.Formatter
Designed to format csv exports from Monobank into one spreadsheet with a report.

Warning: Currently MAUI project does not even start because of some dependencies missing, in order to use a project you need to use Console app.

# How to setup ?
1. Build Finance.Formatter.Console project
2. Check out appSettings.json for some configuration, mainly TemplatePath which should point to Template.xlsx that is included in Finance.Formatter.Core project.
3. Set correct UahToUsdMultiplier and EurToUsdMultiplier to convert UAH -> USD and EUR -> USD
4. Configure ignored keywords to automatically remove transactions that contain select text in the description

# How to use ?
1. Export Monobank transactions from card or fop account
2. Use English language and CSV format for the export (other seem to have various bugs in them)
3. You can use whatever date times you want for the export
4. Drag the files onto Finance.Formatter.Console.exe in the Windows explorer (you should see filepaths of those files in the console)
5. Wait for the excel to be opened
6. Have fun with your finance report
