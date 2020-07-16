Public Module MainModule

    Private WithEvents Tray As NotifyIcon

    Sub Main(ByVal args() As String)
        Tray = New NotifyIcon()
        Tray.Icon = SystemIcons.Error
        Tray.Text = "Pestilence"
        Tray.Visible = True



        Application.Run()
    End Sub

    Private Sub NewMethod(sender As Object, e As MouseEventArgs) Handles Tray.MouseClick
        If e.Button = MouseButtons.Middle Then
            Application.Exit()
        End If
    End Sub

End Module