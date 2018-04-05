Imports System
Imports System.Drawing
Imports System.Net
Imports Screens
Imports Screens.Hosting
Imports Screens.Hosting.WebTerm

Module Program
    Sub Main(args As String())

        Dim host = New WebTermHost

        host.Main = Sub(term)
                        Dim application = New Application(term)
                        application.ScreenSize = New Size(29, 20)
                        application.BlackAndWhite = False
                        application.Run(New MenuProdottiFiniti())
                    End Sub

        host.StartHost()

        'Dim host = New TelnetHost()
        'AddHandler host.SessionConnected, AddressOf connect
        'AddHandler host.SessionDisconnected, AddressOf disconnect

        'host.Main = Sub(term)
        '                Dim application = New Application(term)
        '                application.ScreenSize = New Size(29, 20)
        '                application.BlackAndWhite = False
        '                application.Run(New MenuProdottiFiniti())
        '            End Sub


        'host.StartHost()

        'Console.WriteLine("SERVER RUNNING!!")

        'Do
        '    Threading.Thread.Sleep(20000)

        'Loop

    End Sub

    Private Sub connect(h As TelnetHost, e As SessionEventArgs)
        Console.WriteLine("Session connected from IP {0}", e.Session.Connection.RemoteAddress.ToString())
    End Sub

    Private Sub disconnect(h As TelnetHost, e As SessionEventArgs)
        Console.WriteLine("Session disconnected at IP {0}", e.Session.Connection.RemoteAddress.ToString())
    End Sub


End Module
