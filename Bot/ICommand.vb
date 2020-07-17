Public Interface ICommand

    ReadOnly Property Name As String
    Sub Execute(args As String, sender As String, guac As Guacamole)

End Interface