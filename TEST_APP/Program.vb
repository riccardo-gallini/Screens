Imports System
Imports System.Drawing
Imports System.Net
Imports Screens
Imports Screens.Hosting
Imports Screens.Hosting.Telnet
Imports Screens.Hosting.WebTerm

Module Program
    Sub Main(args As String())

        'Dim host = WebTermHost.Instance

        'host.Main = Sub(term) 
        '                Dim application = New Application(term)
        '                application.ScreenSize = New Size(29, 20)
        '                application.BlackAndWhite = False
        '                application.Run(New MenuProdottiFiniti())
        '            End Sub

        'host.StartHost()

        'Dim host = New TelnetHost()
        'AddHandler host.SessionConnected, AddressOf connect
        'AddHandler host.SessionDisconnected, AddressOf disconnect

        'host.Main = Sub(term)
        '                Dim application = New Application(term)
        '                application.ScreenSize = New Size(29, 20)
        '                application.BlackAndWhite = False
        '                application.Run(New MenuProdottiFiniti())
        '            End Sub

        Dim host = WebTermHost.Instance
        'Dim host = New LocalConsole.ConsoleHost()
        AddHandler host.SessionConnected, AddressOf connect
        AddHandler host.SessionDisconnected, AddressOf disconnect

        'host.Main = Sub(term)
        '                Dim application = New Application(term)
        '                application.ScreenSize = New Size(29, 20)
        '                application.BlackAndWhite = False
        '                application.Run(New MenuProdottiFiniti())
        '            End Sub

        host.Main = Sub(term)
                        Dim application = New Application(term)
                        application.ScreenSize = New Size(29, 20)
                        application.BlackAndWhite = False
                        application.Run(New TEST_SNAKE.SnakeWorld())
                    End Sub

        host.StartHost()

                        Console.WriteLine("SERVER RUNNING!!")

                        Do
                            Threading.Thread.Sleep(20000)

                        Loop

                    End Sub

    Private Sub connect(h As IHost, e As SessionEventArgs)
        Console.WriteLine("Session connected from IP {0}", e.Session.RemoteAddress)
    End Sub

    Private Sub disconnect(h As IHost, e As SessionEventArgs)
        Console.WriteLine("Session disconnected at IP {0}", e.Session.RemoteAddress)
    End Sub


End Module
