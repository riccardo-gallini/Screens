Imports System
Imports System.Drawing
Imports System.Net
Imports Screens
Imports Screens.Hosting

Module Program
    Sub Main(args As String())

        Dim decoder = New ANSI_Decoder()
        decoder.KeyReady = Sub(key) Console.WriteLine(key.SpecialKey)


        'decoder.Decode(New Byte() {27, 91, 66, 27, 79, 80, 27, 11, 27, 91, 66})
        'decoder.Decode(New Byte() {27, 91, 27, 91, 66, 65, 65})


        'Dim host = New ConsoleHost()

        'host.Run(
        '        Sub(application)
        '            application.ScreenSize = New Size(29, 20)
        '            application.BlackAndWhite = False
        '            application.Run(New MenuProdottiFiniti())
        '        End Sub)

        'host.StartHost()

        'Do
        '    Threading.Thread.Sleep(20000)
        'Loop

        Dim host = New TelnetHost()

        AddHandler host.SessionConnected, AddressOf handleNewSession

        host.Run(
                Sub(application)
                    application.ScreenSize = New Size(29, 20)
                    application.BlackAndWhite = False
                    application.Run(New MenuProdottiFiniti())
                End Sub)

        host.StartHost()

        Console.WriteLine("SERVER RUNNING!!")

        Do
            Threading.Thread.Sleep(20000)

        Loop

    End Sub

    Private Sub handleNewSession(h As TelnetHost, e As SessionConnectedEventArgs)
        Console.WriteLine("Session connected from {0}", e.Session.Connection.RemoteAddress.ToString())
    End Sub


End Module
