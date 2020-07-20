Public Class MessageBoxCmd
    Implements ICommand
    Public ReadOnly Property Name As String = "msg" Implements ICommand.Name
    Public ReadOnly Property Help As String = "Show message box. Usage: msg [message]" Implements ICommand.Help

    'TODO: separate thread for msgboxes
    Public Sub Execute(args As String, sender As String, guac As Guacamole) Implements ICommand.Execute
        MessageBox.Show(args, $"{sender} says...", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
    End Sub
End Class