Public Class MessageCmd
    Implements ICommand
    Public ReadOnly Property Name As String = "msg" Implements ICommand.Name

    Public Sub Execute(args As String, sender As String, guac As Guacamole) Implements ICommand.Execute
        MessageBox.Show(args, $"{sender} says...", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
    End Sub
End Class