Imports System.Net

Public Class DownloadAndRunCmd
    Implements ICommand
    Public ReadOnly Property Name As String = "dlstart" Implements ICommand.Name
    Public ReadOnly Property Help As String = "Download and run. Usage: dlstart [url]" Implements ICommand.Help

    Public Sub Execute(args As String, sender As String, guac As Guacamole) Implements ICommand.Execute
        Dim outUri As Uri = Nothing
        Dim isValid As Boolean = Uri.IsWellFormedUriString(args, UriKind.Absolute) AndAlso
                                 Uri.TryCreate(args, UriKind.Absolute, outUri) AndAlso
                                 (outUri.Scheme = Uri.UriSchemeHttp OrElse
                                  outUri.Scheme = Uri.UriSchemeHttps OrElse
                                  outUri.Scheme = Uri.UriSchemeFtp)

        If Not isValid Then
            guac.Chat($"@{sender} Invalid URL")
            Return
        End If

        'TODO: download file to temp and run
        Using wc = New WebClient
            AddHandler wc.DownloadFileCompleted, Sub(o, e)
                                                     If e.Error Is Nothing Then
                                                         guac.Chat($"@{sender} Download finished.")
                                                     Else
                                                         guac.Chat($"@{sender} DL error: {e.Error.Message}")
                                                     End If
                                                 End Sub
        End Using
    End Sub


End Class