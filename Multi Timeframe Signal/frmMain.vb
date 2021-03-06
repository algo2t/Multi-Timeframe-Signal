Imports System.Threading
Imports System.IO
Imports Utilities.DAL
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports Syncfusion.WinForms.DataGrid
Imports Syncfusion.WinForms.DataGrid.Events
Imports Syncfusion.WinForms.Input.Enums
Imports Syncfusion.WinForms.DataGridConverter
Imports NLog
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmMain
    Private canceller As CancellationTokenSource

#Region "Common Delegates"
    Delegate Sub SetSFGridDataBind_Delegate(ByVal [grd] As SfDataGrid, ByVal [value] As Object)
    Public Async Sub SetSFGridDataBind_ThreadSafe(ByVal [grd] As Syncfusion.WinForms.DataGrid.SfDataGrid, ByVal [value] As Object)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [grd].InvokeRequired Then
            Dim MyDelegate As New SetSFGridDataBind_Delegate(AddressOf SetSFGridDataBind_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[grd], [value]})
        Else
            While True
                Try
                    [grd].DataSource = [value]
                    Exit While
                Catch sop As System.InvalidOperationException
                End Try
                Await Task.Delay(500, canceller.Token).ConfigureAwait(False)
            End While
        End If
    End Sub

    Delegate Sub SetSFGridFreezFirstColumn_Delegate(ByVal [grd] As SfDataGrid)
    Public Sub SetSFGridFreezFirstColumn_ThreadSafe(ByVal [grd] As Syncfusion.WinForms.DataGrid.SfDataGrid)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [grd].InvokeRequired Then
            Dim MyDelegate As New SetSFGridFreezFirstColumn_Delegate(AddressOf SetSFGridFreezFirstColumn_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[grd]})
        Else
            [grd].FrozenColumnCount = 8
            'Await Task.Delay(500).ConfigureAwait(False)
        End If
    End Sub

    Delegate Sub SetObjectEnableDisable_Delegate(ByVal [obj] As Object, ByVal [value] As Boolean)
    Public Sub SetObjectEnableDisable_ThreadSafe(ByVal [obj] As Object, ByVal [value] As Boolean)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [obj].InvokeRequired Then
            Dim MyDelegate As New SetObjectEnableDisable_Delegate(AddressOf SetObjectEnableDisable_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[obj], [value]})
        Else
            [obj].Enabled = [value]
        End If
    End Sub

    Delegate Sub SetObjectVisible_Delegate(ByVal [obj] As Object, ByVal [value] As Boolean)
    Public Sub SetObjectVisible_ThreadSafe(ByVal [obj] As Object, ByVal [value] As Boolean)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [obj].InvokeRequired Then
            Dim MyDelegate As New SetObjectVisible_Delegate(AddressOf SetObjectVisible_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[obj], [value]})
        Else
            [obj].Visible = [value]
        End If
    End Sub

    Delegate Sub SetLabelText_Delegate(ByVal [label] As Label, ByVal [text] As String)
    Public Sub SetLabelText_ThreadSafe(ByVal [label] As Label, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [label].InvokeRequired Then
            Dim MyDelegate As New SetLabelText_Delegate(AddressOf SetLabelText_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[label], [text]})
        Else
            [label].Text = [text]
        End If
    End Sub

    Delegate Function GetLabelText_Delegate(ByVal [label] As Label) As String
    Public Function GetLabelText_ThreadSafe(ByVal [label] As Label) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [label].InvokeRequired Then
            Dim MyDelegate As New GetLabelText_Delegate(AddressOf GetLabelText_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[label]})
        Else
            Return [label].Text
        End If
    End Function

    Delegate Sub SetLabelTag_Delegate(ByVal [label] As Label, ByVal [tag] As String)
    Public Sub SetLabelTag_ThreadSafe(ByVal [label] As Label, ByVal [tag] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [label].InvokeRequired Then
            Dim MyDelegate As New SetLabelTag_Delegate(AddressOf SetLabelTag_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[label], [tag]})
        Else
            [label].Tag = [tag]
        End If
    End Sub

    Delegate Function GetLabelTag_Delegate(ByVal [label] As Label) As String
    Public Function GetLabelTag_ThreadSafe(ByVal [label] As Label) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [label].InvokeRequired Then
            Dim MyDelegate As New GetLabelTag_Delegate(AddressOf GetLabelTag_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[label]})
        Else
            Return [label].Tag
        End If
    End Function
    Delegate Sub SetToolStripLabel_Delegate(ByVal [toolStrip] As StatusStrip, ByVal [label] As ToolStripStatusLabel, ByVal [text] As String)
    Public Sub SetToolStripLabel_ThreadSafe(ByVal [toolStrip] As StatusStrip, ByVal [label] As ToolStripStatusLabel, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [toolStrip].InvokeRequired Then
            Dim MyDelegate As New SetToolStripLabel_Delegate(AddressOf SetToolStripLabel_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[toolStrip], [label], [text]})
        Else
            [label].Text = [text]
        End If
    End Sub

    Delegate Function GetToolStripLabel_Delegate(ByVal [toolStrip] As StatusStrip, ByVal [label] As ToolStripLabel) As String
    Public Function GetToolStripLabel_ThreadSafe(ByVal [toolStrip] As StatusStrip, ByVal [label] As ToolStripLabel) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [toolStrip].InvokeRequired Then
            Dim MyDelegate As New GetToolStripLabel_Delegate(AddressOf GetToolStripLabel_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[toolStrip], [label]})
        Else
            Return [label].Text
        End If
    End Function

    Delegate Function GetDateTimePickerValue_Delegate(ByVal [dateTimePicker] As DateTimePicker) As Date
    Public Function GetDateTimePickerValue_ThreadSafe(ByVal [dateTimePicker] As DateTimePicker) As Date
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [dateTimePicker].InvokeRequired Then
            Dim MyDelegate As New GetDateTimePickerValue_Delegate(AddressOf GetDateTimePickerValue_ThreadSafe)
            Return Me.Invoke(MyDelegate, New DateTimePicker() {[dateTimePicker]})
        Else
            Return [dateTimePicker].Value
        End If
    End Function

    Delegate Function GetNumericUpDownValue_Delegate(ByVal [numericUpDown] As NumericUpDown) As Integer
    Public Function GetNumericUpDownValue_ThreadSafe(ByVal [numericUpDown] As NumericUpDown) As Integer
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [numericUpDown].InvokeRequired Then
            Dim MyDelegate As New GetNumericUpDownValue_Delegate(AddressOf GetNumericUpDownValue_ThreadSafe)
            Return Me.Invoke(MyDelegate, New NumericUpDown() {[numericUpDown]})
        Else
            Return [numericUpDown].Value
        End If
    End Function

    Delegate Function GetComboBoxIndex_Delegate(ByVal [combobox] As ComboBox) As Integer
    Public Function GetComboBoxIndex_ThreadSafe(ByVal [combobox] As ComboBox) As Integer
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [combobox].InvokeRequired Then
            Dim MyDelegate As New GetComboBoxIndex_Delegate(AddressOf GetComboBoxIndex_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[combobox]})
        Else
            Return [combobox].SelectedIndex
        End If
    End Function

    Delegate Function GetComboBoxItem_Delegate(ByVal [ComboBox] As ComboBox) As String
    Public Function GetComboBoxItem_ThreadSafe(ByVal [ComboBox] As ComboBox) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [ComboBox].InvokeRequired Then
            Dim MyDelegate As New GetComboBoxItem_Delegate(AddressOf GetComboBoxItem_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[ComboBox]})
        Else
            Return [ComboBox].SelectedItem.ToString
        End If
    End Function

    Delegate Function GetTextBoxText_Delegate(ByVal [textBox] As TextBox) As String
    Public Function GetTextBoxText_ThreadSafe(ByVal [textBox] As TextBox) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [textBox].InvokeRequired Then
            Dim MyDelegate As New GetTextBoxText_Delegate(AddressOf GetTextBoxText_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[textBox]})
        Else
            Return [textBox].Text
        End If
    End Function

    Delegate Function GetCheckBoxChecked_Delegate(ByVal [checkBox] As CheckBox) As Boolean
    Public Function GetCheckBoxChecked_ThreadSafe(ByVal [checkBox] As CheckBox) As Boolean
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [checkBox].InvokeRequired Then
            Dim MyDelegate As New GetCheckBoxChecked_Delegate(AddressOf GetCheckBoxChecked_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[checkBox]})
        Else
            Return [checkBox].Checked
        End If
    End Function

    Delegate Function GetRadioButtonChecked_Delegate(ByVal [radioButton] As RadioButton) As Boolean
    Public Function GetRadioButtonChecked_ThreadSafe(ByVal [radioButton] As RadioButton) As Boolean
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [radioButton].InvokeRequired Then
            Dim MyDelegate As New GetRadioButtonChecked_Delegate(AddressOf GetRadioButtonChecked_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[radioButton]})
        Else
            Return [radioButton].Checked
        End If
    End Function

    Delegate Sub SetDatagridBind_Delegate(ByVal [datagrid] As DataGridView, ByVal [data] As Object)
    Public Sub SetDatagridBind_ThreadSafe(ByVal [datagrid] As DataGridView, ByVal [data] As Object)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [datagrid].InvokeRequired Then
            Dim MyDelegate As New SetDatagridBind_Delegate(AddressOf SetDatagridBind_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[datagrid], [data]})
        Else
            [datagrid].DataSource = [data]
            [datagrid].Refresh()
        End If
    End Sub
#End Region

#Region "Event Handlers"
    Private Sub OnHeartbeat(message As String)
        SetLabelText_ThreadSafe(lblProgress, message)
    End Sub
    Private Sub OnDocumentDownloadComplete()
        'OnHeartbeat("Document download compelete")
    End Sub
    Private Sub OnDocumentRetryStatus(currentTry As Integer, totalTries As Integer)
        OnHeartbeat(String.Format("Try #{0}/{1}: Connecting...", currentTry, totalTries))
    End Sub
    Public Sub OnWaitingFor(ByVal elapsedSecs As Integer, ByVal totalSecs As Integer, ByVal msg As String)
        OnHeartbeat(String.Format("{0}, waiting {1}/{2} secs", msg, elapsedSecs, totalSecs))
    End Sub
#End Region

#Region "Logging and Status Progress"
    Public Shared logger As Logger = LogManager.GetCurrentClassLogger
#End Region

#Region "SF Data Grid"
    Private Sub sfdgvMain_AutoGeneratingColumn(sender As Object, e As AutoGeneratingColumnArgs) Handles sfdgvMain.AutoGeneratingColumn
        sfdgvMain.Style.HeaderStyle.BackColor = Color.DeepSkyBlue
        sfdgvMain.Style.HeaderStyle.TextColor = Color.White

        sfdgvMain.Style.CheckBoxStyle.CheckedBackColor = Color.White
        sfdgvMain.Style.CheckBoxStyle.CheckedTickColor = Color.LightSkyBlue
        If e.Column.CellType = "DateTime" Then
            CType(e.Column, GridDateTimeColumn).Pattern = DateTimePattern.Custom
            CType(e.Column, GridDateTimeColumn).Format = "HH:mm:ss"
        End If
        If e.Column.MappingName = "OriginatingInstrument" Then
            e.Column.Width = 100
        ElseIf e.Column.MappingName = "LastUpdateTime" Then
            e.Column.Width = 65
        ElseIf e.Column.MappingName = "ATR" Then
            e.Column.Width = 45
        ElseIf e.Column.MappingName = "Slab" Then
            e.Column.Width = 45
        ElseIf e.Column.MappingName = "OverallEMA" Then
            e.Column.Width = 45
        ElseIf e.Column.MappingName = "OverallAroon" Then
            e.Column.Width = 45
        ElseIf e.Column.MappingName = "OverallSupertrend" Then
            e.Column.Width = 45
        ElseIf e.Column.MappingName = "OverallFractal" Then
            e.Column.Width = 45
        ElseIf e.Column.MappingName.Contains("Minute") Then
            e.Column.Width = 20
        End If
    End Sub

    Private Sub sfdgvMain_QueryCellStyle(sender As Object, e As QueryCellStyleEventArgs) Handles sfdgvMain.QueryCellStyle
        If e.Column.MappingName.Contains("EMA") OrElse
            e.Column.MappingName.Contains("Aroon") OrElse
            e.Column.MappingName.Contains("Supertrend") OrElse
            e.Column.MappingName.Contains("Fractal") Then
            If e.DisplayText = "White" Then
                e.Style.BackColor = Color.White
                e.Style.TextColor = Color.White
            ElseIf e.DisplayText = "Green" Then
                e.Style.BackColor = Color.Green
                e.Style.TextColor = Color.Green
            ElseIf e.DisplayText = "Red" Then
                e.Style.BackColor = Color.Red
                e.Style.TextColor = Color.Red
            End If
        End If
        If e.Column.MappingName.Contains("Minute") Then
            If e.DisplayText = "-4" Then
                e.Style.BackColor = Color.FromArgb(255, 0, 0)
                e.Style.TextColor = Color.FromArgb(255, 0, 0)
            ElseIf e.DisplayText = "-3" Then
                e.Style.BackColor = Color.FromArgb(236, 69, 69)
                e.Style.TextColor = Color.FromArgb(236, 69, 69)
            ElseIf e.DisplayText = "-2" Then
                e.Style.BackColor = Color.FromArgb(255, 92, 92)
                e.Style.TextColor = Color.FromArgb(255, 92, 92)
            ElseIf e.DisplayText = "-1" Then
                e.Style.BackColor = Color.FromArgb(255, 133, 133)
                e.Style.TextColor = Color.FromArgb(255, 133, 133)
            ElseIf e.DisplayText = "0" Then
                e.Style.BackColor = Color.FromArgb(255, 255, 255)
                e.Style.TextColor = Color.FromArgb(255, 255, 255)
            ElseIf e.DisplayText = "1" Then
                e.Style.BackColor = Color.FromArgb(0, 255, 0)
                e.Style.TextColor = Color.FromArgb(0, 255, 0)
            ElseIf e.DisplayText = "2" Then
                e.Style.BackColor = Color.FromArgb(40, 206, 40)
                e.Style.TextColor = Color.FromArgb(40, 206, 40)
            ElseIf e.DisplayText = "3" Then
                e.Style.BackColor = Color.FromArgb(26, 166, 26)
                e.Style.TextColor = Color.FromArgb(26, 166, 26)
            ElseIf e.DisplayText = "4" Then
                e.Style.BackColor = Color.FromArgb(0, 128, 0)
                e.Style.TextColor = Color.FromArgb(0, 128, 0)
            End If
        End If
    End Sub

    Private Sub sfdgvMain_FilterPopupShowing(sender As Object, e As FilterPopupShowingEventArgs) Handles sfdgvMain.FilterPopupShowing
        Dim eFilterPopupShowingEventArgsCommon As FilterPopupShowingEventArgs = e

        eFilterPopupShowingEventArgsCommon.Control.BackColor = ColorTranslator.FromHtml("#EDF3F3")

        'Customize the appearance of the CheckedListBox

        sfdgvMain.Style.CheckBoxStyle.CheckedBackColor = Color.White
        sfdgvMain.Style.CheckBoxStyle.CheckedTickColor = Color.LightSkyBlue
        eFilterPopupShowingEventArgsCommon.Control.CheckListBox.Style.CheckBoxStyle.CheckedBackColor = Color.White
        eFilterPopupShowingEventArgsCommon.Control.CheckListBox.Style.CheckBoxStyle.CheckedTickColor = Color.LightSkyBlue

        'Customize the appearance of the Ok and Cancel buttons
        eFilterPopupShowingEventArgsCommon.Control.CancelButton.BackColor = Color.DeepSkyBlue
        eFilterPopupShowingEventArgsCommon.Control.OkButton.BackColor = eFilterPopupShowingEventArgsCommon.Control.CancelButton.BackColor
        eFilterPopupShowingEventArgsCommon.Control.CancelButton.ForeColor = Color.White
        eFilterPopupShowingEventArgsCommon.Control.OkButton.ForeColor = eFilterPopupShowingEventArgsCommon.Control.CancelButton.ForeColor
    End Sub
#End Region

    Private Async Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Await Task.Run(AddressOf ExportToExcelAsync).ConfigureAwait(False)
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        Dim frm As New frmSettings
        frm.ShowDialog()
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        canceller.Cancel()
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetObjectEnableDisable_ThreadSafe(btnStop, False)
        SetObjectEnableDisable_ThreadSafe(btnExport, False)

        rdbWithAPI.Checked = My.Settings.WithAPI
        rdbWithoutAPI.Checked = My.Settings.WithoutAPI
        rdbLocal.Checked = My.Settings.DBLocal
        rdbRemote.Checked = My.Settings.DBRemote
    End Sub

    Private Async Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        SetLabelText_ThreadSafe(lblNumberOfStock, String.Format("Number Of Stock: {0}", 0))
        SetLabelText_ThreadSafe(lblDuration, String.Format("Last Iteration Duration: {0}", 0))
        SetLabelText_ThreadSafe(lblTime, String.Format("Last Iteration Time: {0}", "00:00:00"))
        SetSFGridDataBind_ThreadSafe(sfdgvMain, Nothing)

        SetObjectEnableDisable_ThreadSafe(btnSettings, False)
        SetObjectEnableDisable_ThreadSafe(btnStart, False)
        SetObjectEnableDisable_ThreadSafe(btnStop, True)
        SetObjectEnableDisable_ThreadSafe(btnExport, True)
        SetObjectEnableDisable_ThreadSafe(grpHistoricalMode, False)
        SetObjectEnableDisable_ThreadSafe(grpDatabaseMode, False)

        My.Settings.WithAPI = rdbWithAPI.Checked
        My.Settings.WithoutAPI = rdbWithoutAPI.Checked
        My.Settings.DBLocal = rdbLocal.Checked
        My.Settings.DBRemote = rdbRemote.Checked
        If rdbLocal.Checked Then
            My.Settings.ServerName = "localhost"
        ElseIf rdbRemote.Checked Then
            My.Settings.ServerName = "103.57.246.210"
        End If
        My.Settings.Save()

        canceller = New CancellationTokenSource
        Await Task.Run(AddressOf StartProcessingAsync).ConfigureAwait(False)
    End Sub

    Private Async Function StartProcessingAsync() As Task
        Try
            If GetRadioButtonChecked_ThreadSafe(rdbWithAPI) Then Throw New NotImplementedException

            OnHeartbeat("Validating user settings")
            Dim settings As SignalSettings = Nothing
            If File.Exists(SignalSettings.SettingsFilename) Then
                Dim fs As Stream = New FileStream(SignalSettings.SettingsFilename, FileMode.Open)
                Dim bf As BinaryFormatter = New BinaryFormatter()
                settings = CType(bf.Deserialize(fs), SignalSettings)
                fs.Close()
            Else
                Throw New ApplicationException("Settings file not found. Please complete your settings properly.")
            End If
            If settings IsNot Nothing Then
                canceller.Token.ThrowIfCancellationRequested()
                Dim tradingDate As Date = Now.Date
                Dim workableStockList As List(Of InstrumentDetails) = Nothing
                Dim allFutureStockList As Dictionary(Of String, List(Of Date)) = Await GetFutureStockListAsync(tradingDate.Date).ConfigureAwait(False)
                canceller.Token.ThrowIfCancellationRequested()
                Dim cashStockList As Dictionary(Of String, String) = Await GetCashStockListAsync(tradingDate.Date).ConfigureAwait(False)

                canceller.Token.ThrowIfCancellationRequested()
                OnHeartbeat("Getting banned stock list. Please wait...")
                Dim bannedStockFileName As String = Path.Combine(My.Application.Info.DirectoryPath, String.Format("Bannned Stocks {0}.csv", tradingDate.ToString("ddMMyyyy")))
                For Each runningFile In Directory.GetFiles(My.Application.Info.DirectoryPath, "Bannned Stocks *.csv")
                    If Not runningFile.Contains(tradingDate.ToString("ddMMyyyy")) Then File.Delete(runningFile)
                Next
                Dim bannedStockList As List(Of String) = Nothing
                If settings.FetchBannedStock Then
                    Using bannedStock As New BannedStockDataFetcher(bannedStockFileName, canceller)
                        'AddHandler bannedStock.Heartbeat, AddressOf OnHeartbeat
                        bannedStockList = Await bannedStock.GetBannedStocksData(tradingDate).ConfigureAwait(False)
                    End Using
                End If

                canceller.Token.ThrowIfCancellationRequested()
                OnHeartbeat("Getting ATR stock list. Please wait...")
                Dim atrStockList As Dictionary(Of String, Decimal()) = Nothing
                Using stkSlctn As New StockSelection(canceller, settings, tradingDate)
                    'AddHandler stkSlctn.Heartbeat, AddressOf OnHeartbeat
                    atrStockList = Await stkSlctn.GetInstrumentData(allFutureStockList, cashStockList, bannedStockList).ConfigureAwait(False)
                End Using

                If allFutureStockList IsNot Nothing AndAlso allFutureStockList.Count > 0 AndAlso
                    cashStockList IsNot Nothing AndAlso cashStockList.Count > 0 AndAlso
                    atrStockList IsNot Nothing AndAlso atrStockList.Count > 0 Then
                    Dim ctr As Integer = 0
                    For Each runningStock In allFutureStockList
                        canceller.Token.ThrowIfCancellationRequested()
                        ctr += 1
                        OnHeartbeat(String.Format("Getting cash stock for {0}. #{1}/{2}", runningStock.Key, ctr, allFutureStockList.Count))
                        Dim cashStockName As String = runningStock.Key
                        If runningStock.Key = "BANKNIFTY" Then cashStockName = "NIFTY BANK"
                        If runningStock.Key = "NIFTY" Then cashStockName = "NIFTY 50"
                        If cashStockList.ContainsKey(cashStockName) AndAlso atrStockList.ContainsKey(cashStockName) Then
                            canceller.Token.ThrowIfCancellationRequested()
                            'Dim workingInstrument As InstrumentDetails = New InstrumentDetails(settings) With {
                            '                                                .OriginatingInstrument = runningStock.Key,
                            '                                                .CashTradingSymbol = cashStockName,
                            '                                                .CashInstrumentToken = cashStockList(cashStockName),
                            '                                                .ATR = atrStockList(cashStockName)(0),
                            '                                                .Slab = atrStockList(cashStockName)(1)
                            '                                            }
                            Dim workingInstrument As InstrumentDetails = New InstrumentDetails(settings) With {
                                                                                .OriginatingInstrument = runningStock.Key,
                                                                                .CashTradingSymbol = cashStockName,
                                                                                .CashInstrumentToken = cashStockList(cashStockName),
                                                                                .ATR = 1,
                                                                                .Slab = 2
                                                                            }

                            If workableStockList Is Nothing Then workableStockList = New List(Of InstrumentDetails)
                            workableStockList.Add(workingInstrument)
                        End If
                    Next
                End If
                OnHeartbeat("Processing Data")
                If workableStockList IsNot Nothing AndAlso workableStockList.Count > 0 Then
                    SetLabelText_ThreadSafe(lblNumberOfStock, String.Format("Number Of Stock: {0}", workableStockList.Count))
                    Dim dashboardList As BindingList(Of InstrumentDetails) = New BindingList(Of InstrumentDetails)(workableStockList)
                    SetSFGridDataBind_ThreadSafe(sfdgvMain, dashboardList)
                    SetSFGridFreezFirstColumn_ThreadSafe(sfdgvMain)

                    Dim sw As Stopwatch = New Stopwatch
                    While True
                        canceller.Token.ThrowIfCancellationRequested()
                        Try
                            sw.Reset()
                            sw.Start()
                            Dim numberOfParallelHit As Integer = settings.MainStockParallelHit
                            For i As Integer = 0 To workableStockList.Count - 1 Step numberOfParallelHit
                                canceller.Token.ThrowIfCancellationRequested()
                                Dim numberOfData As Integer = If(workableStockList.Count - i > numberOfParallelHit, numberOfParallelHit, workableStockList.Count - i)
                                Dim tasks As IEnumerable(Of Task(Of Boolean)) = Nothing
                                tasks = workableStockList.GetRange(i, numberOfData).Select(Async Function(x)
                                                                                               Try
                                                                                                   Dim dataFtchr As DataFetcher = New DataFetcher(canceller,
                                                                                                                                          x,
                                                                                                                                          GetRadioButtonChecked_ThreadSafe(rdbWithAPI),
                                                                                                                                          GetRadioButtonChecked_ThreadSafe(rdbWithoutAPI),
                                                                                                                                          tradingDate,
                                                                                                                                          settings)
                                                                                                   Await dataFtchr.StartFetchingAsync().ConfigureAwait(False)
                                                                                               Catch ex As Exception
                                                                                                   Throw ex
                                                                                               End Try
                                                                                               Return True
                                                                                           End Function)

                                Dim mainTask As Task = Task.WhenAll(tasks)
                                Await mainTask.ConfigureAwait(False)
                                If mainTask.Exception IsNot Nothing Then
                                    Throw mainTask.Exception
                                End If
                            Next
                            sw.Stop()
                            SetLabelText_ThreadSafe(lblDuration, String.Format("Last Iteration Duration (sec): {0}", TimeSpan.FromSeconds(sw.Elapsed.TotalSeconds).ToString("hh\:mm\:ss")))
                            SetLabelText_ThreadSafe(lblTime, String.Format("Last Iteration Time: {0}", Now.ToString("HH:mm:ss")))
                        Catch cex As TaskCanceledException
                            Throw cex
                        Catch aex As AggregateException
                            Throw aex
                        Catch ex As Exception
                            Throw ex
                        End Try
                        Await Task.Delay(1000, canceller.Token).ConfigureAwait(False)
                    End While
                End If
            End If
        Catch cex As OperationCanceledException
            MessageBox.Show(cex.Message, "Multi Timeframe Signal", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Catch ex As Exception
            MessageBox.Show(ex.ToString, "Multi Timeframe Signal", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetObjectEnableDisable_ThreadSafe(btnSettings, True)
            SetObjectEnableDisable_ThreadSafe(btnStart, True)
            SetObjectEnableDisable_ThreadSafe(btnStop, False)
            SetObjectEnableDisable_ThreadSafe(grpHistoricalMode, True)
            SetObjectEnableDisable_ThreadSafe(grpDatabaseMode, True)
            OnHeartbeat("Process Complete")
        End Try
    End Function

    Private Async Function ExportToExcelAsync() As Task
        Await Task.Delay(0).ConfigureAwait(False)
        If sfdgvMain IsNot Nothing AndAlso sfdgvMain.DataSource IsNot Nothing Then
            Dim options = New ExcelExportingOptions()
            Dim excelEngine = sfdgvMain.ExportToExcel(sfdgvMain.View, options)
            Dim workBook = excelEngine.Excel.Workbooks(0)
            Dim filename As String = Path.Combine(My.Application.Info.DirectoryPath, String.Format("Multi Timeframe Signal {0}.xlsx", Now.ToString("ddMMyyyy HH_mm_ss")))
            workBook.SaveAs(filename)
            If MessageBox.Show("Export successful. Do you want to open?", "Multi Timeframe Signal Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                Process.Start(filename)
            End If
        Else
            MessageBox.Show("Can not export as there is no data", "Multi Timeframe Signal Export", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Function

#Region "Database stock selection"
    Public Async Function GetFutureStockListAsync(ByVal tradingDate As Date) As Task(Of Dictionary(Of String, List(Of Date)))
        Dim ret As Dictionary(Of String, List(Of Date)) = Nothing

        Using sqlHlpr As New MySQLDBHelper(My.Settings.ServerName, "local_stock", "3306", "rio", "speech123", canceller)
            AddHandler sqlHlpr.Heartbeat, AddressOf OnHeartbeat

            Dim queryString As String = "SELECT `TRADING_SYMBOL`,`EXPIRY` FROM `active_instruments_futures` WHERE `AS_ON_DATE`='{0}' AND `SEGMENT`='NFO-FUT'"
            queryString = String.Format(queryString, tradingDate.ToString("yyyy-MM-dd"))

            Dim dt As DataTable = Await sqlHlpr.RunSelectAsync(queryString).ConfigureAwait(False)
            canceller.Token.ThrowIfCancellationRequested()

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim allStock As Dictionary(Of String, Date) = Nothing
                For i = 0 To dt.Rows.Count - 1
                    canceller.Token.ThrowIfCancellationRequested()
                    If Not IsDBNull(dt.Rows(i).Item(0)) AndAlso Not IsDBNull(dt.Rows(i).Item(1)) Then
                        Dim tradingSymbol As String = dt.Rows(i).Item(0).ToString.ToUpper
                        Dim expiry As Date = Convert.ToDateTime(dt.Rows(i).Item(1))

                        Dim pattern As String = "([0-9][0-9]JAN)|([0-9][0-9]FEB)|([0-9][0-9]MAR)|([0-9][0-9]APR)|([0-9][0-9]MAY)|([0-9][0-9]JUN)|([0-9][0-9]JUL)|([0-9][0-9]AUG)|([0-9][0-9]SEP)|([0-9][0-9]OCT)|([0-9][0-9]NOV)|([0-9][0-9]DEC)"
                        If Regex.Matches(tradingSymbol, pattern).Count <= 1 Then
                            If allStock Is Nothing Then allStock = New Dictionary(Of String, Date)
                            allStock.Add(tradingSymbol, expiry)
                        End If
                    End If
                Next
                If allStock IsNot Nothing AndAlso allStock.Count > 0 Then

                    For Each runningStock In allStock
                        canceller.Token.ThrowIfCancellationRequested()
                        If ret Is Nothing Then ret = New Dictionary(Of String, List(Of Date))
                        Dim intrumentName As String = runningStock.Key.Remove(runningStock.Key.Count - 8)
                        If Not ret.ContainsKey(intrumentName) Then
                            Dim allInstrumentDetails As IEnumerable(Of KeyValuePair(Of String, Date)) = allStock.Where(Function(x)
                                                                                                                           Return x.Key.Remove(x.Key.Count - 8) = intrumentName
                                                                                                                       End Function)
                            If allInstrumentDetails IsNot Nothing AndAlso allInstrumentDetails.Count > 0 Then
                                Dim expiryList As List(Of Date) = Nothing
                                For Each runningInstrument In allInstrumentDetails
                                    If expiryList Is Nothing Then expiryList = New List(Of Date)
                                    expiryList.Add(runningInstrument.Value)
                                Next
                                ret.Add(intrumentName, expiryList)
                            End If
                        End If
                    Next
                End If
            End If
        End Using
        Return ret
    End Function

    Public Async Function GetCashStockListAsync(ByVal tradingDate As Date) As Task(Of Dictionary(Of String, String))
        Dim ret As Dictionary(Of String, String) = Nothing

        Using sqlHlpr As New MySQLDBHelper(My.Settings.ServerName, "local_stock", "3306", "rio", "speech123", canceller)
            AddHandler sqlHlpr.Heartbeat, AddressOf OnHeartbeat

            Dim queryString As String = "SELECT DISTINCT(`INSTRUMENT_TOKEN`),`TRADING_SYMBOL` FROM `active_instruments_cash` WHERE `AS_ON_DATE`='{0}'"
            queryString = String.Format(queryString, tradingDate.ToString("yyyy-MM-dd"))

            Dim dt As DataTable = Await sqlHlpr.RunSelectAsync(queryString).ConfigureAwait(False)
            canceller.Token.ThrowIfCancellationRequested()

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    canceller.Token.ThrowIfCancellationRequested()
                    If Not IsDBNull(dt.Rows(i).Item(0)) AndAlso Not IsDBNull(dt.Rows(i).Item(1)) Then
                        If ret Is Nothing Then ret = New Dictionary(Of String, String)
                        ret.Add(dt.Rows(i).Item(1), dt.Rows(i).Item(0))
                    End If
                Next
            End If
        End Using
        Return ret
    End Function
#End Region

End Class