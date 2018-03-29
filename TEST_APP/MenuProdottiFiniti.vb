Imports Screens

Public Class MenuProdottiFiniti
    Inherits Form

    Private WithEvents btCaricoInCella As New Button
    Private WithEvents btSpostamento As New Button
    Private WithEvents btPrelievoMaster As New Button
    Private WithEvents btRettifica As New Button
    Private WithEvents btResoCliente As New Button
    Private WithEvents btConsultazioneGiacenze As New Button
    Private WithEvents btEsci As New Button

    Private WithEvents my_timer As New Timer

    Private Sub InitializeComponents()

        btCaricoInCella.Name = "btCaricoInCella"
        btCaricoInCella.Text = "&1 CARICO IN CELLA"
        btCaricoInCella.Top = 2
        btCaricoInCella.Left = 1
        btCaricoInCella.Height = 1
        btCaricoInCella.Width = 25
        btCaricoInCella.TabIndex = 1
        Me.Controls.Add(btCaricoInCella)

        btPrelievoMaster.Name = "btPrelievoMaster"
        btPrelievoMaster.Text = "&2 PREL. MASTER/SCAMPOLO"
        btPrelievoMaster.Top = 4
        btPrelievoMaster.Left = 1
        btPrelievoMaster.Height = 1
        btPrelievoMaster.Width = 25
        btPrelievoMaster.TabIndex = 2
        Me.Controls.Add(btPrelievoMaster)

        btSpostamento.Name = "btSpostamento"
        btSpostamento.Text = "&3 SPOSTAMENTO"
        btSpostamento.Top = 6
        btSpostamento.Left = 1
        btSpostamento.Height = 1
        btSpostamento.Width = 25
        btSpostamento.TabIndex = 3
        Me.Controls.Add(btSpostamento)

        btRettifica.Name = "btRettifica"
        btRettifica.Text = "&4 RETTIFICA"
        btRettifica.Top = 8
        btRettifica.Left = 1
        btRettifica.Height = 1
        btRettifica.Width = 25
        btRettifica.TabIndex = 4
        Me.Controls.Add(btRettifica)

        btResoCliente.Name = "btResoCliente"
        btResoCliente.Text = "&5 RESO DA CLIENTE"
        btResoCliente.Top = 10
        btResoCliente.Left = 1
        btResoCliente.Height = 1
        btResoCliente.Width = 25
        btResoCliente.TabIndex = 5
        Me.Controls.Add(btResoCliente)

        btConsultazioneGiacenze.Name = "btConsultazioneGiacenze"
        btConsultazioneGiacenze.Text = "&6 GIACENZE"
        btConsultazioneGiacenze.Top = 12
        btConsultazioneGiacenze.Left = 1
        btConsultazioneGiacenze.Height = 1
        btConsultazioneGiacenze.Width = 25
        btConsultazioneGiacenze.TabIndex = 6
        Me.Controls.Add(btConsultazioneGiacenze)

        my_timer.Name = "my_timer"
        my_timer.Interval = 1000
        Me.Controls.Add(my_timer)

        btEsci.Name = "btEsci"
        btEsci.Text = "<< MENU PRINCIPALE"
        btEsci.Top = 18
        btEsci.Left = 1
        btEsci.Height = 1
        btEsci.Width = 24
        btEsci.TabIndex = 7
        btEsci.BackColor = Color.DarkRed
        Me.Controls.Add(btEsci)

        Me.Name = "MenuProdottiFiniti"
        Me.Text = "PRODOTTI FINITI"
        Me.BackColor = Color.Black
        Me.ForeColor = Color.White
        Me.CancelButton = btEsci
        Me.Width = 28
        Me.Height = 20

    End Sub

    Public Sub New()
        InitializeComponents()
    End Sub

    Private Sub Utilita_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'my_timer.Start()
    End Sub

    Private Sub btSpostamento_Click(sender As Screens.Button, e As System.EventArgs) Handles btSpostamento.Click
        'Dim K As New TrasferimentoMagazzino
        'K.Text = "SPOSTAMENTO"
        'K.CausaleTrasferimento = "CM"
        'K.AttivaRettificaSuTrasferimento = False
        'K.MagazzinoDest = ""
        'Application.Show(K)
    End Sub

    Private Sub my_timer_Tick(sender As Screens.Timer, e As System.EventArgs) Handles my_timer.Tick
        Me.Text = DateTime.Now.ToString("HH:mm:ss")
    End Sub

    Private Sub btCaricoInCella_Click(sender As Screens.Button, e As System.EventArgs) Handles btCaricoInCella.Click
        'Dim K As New TrasferimentoMagazzino
        'K.Text = "CARICO IN CELLA"
        'K.CausaleTrasferimento = "PF"
        'K.MagazzinoDest = "S5"
        'K.AttivaRettificaSuTrasferimento = False
        'Application.Show(K)
    End Sub

    Private Sub btPrelievoMaster_Click(sender As Screens.Button, e As System.EventArgs) Handles btPrelievoMaster.Click
        'Dim K As New PrelievoProduzione
        'K.Text = "PRELIEVO MASTER/SCAMPOLO"
        'K.CausaleTrasferimento = "PM"
        'K.MagazzinoDest = "S4"
        'K.AttivaRettificaSuTrasferimento = False

        'Application.Show(K)
    End Sub

    Private Sub btResoCliente_Click(sender As Screens.Button, e As System.EventArgs) Handles btResoCliente.Click
        'Dim K As New TrasferimentoMagazzino
        'K.Text = "RESO DA CLIENTE"
        'K.CausaleTrasferimento = "RDC"
        'K.MagazzinoDest = "S5"
        'K.AttivaRettificaSuTrasferimento = True
        'Application.Show(K)
    End Sub


    Private Sub btRettifica_Click(sender As Screens.Button, e As System.EventArgs) Handles btRettifica.Click
        'Dim K As New Rettifica
        'Application.Show(K)

        Dim app = Application.Current

    End Sub

    Private Sub btConsultazioneGiacenze_Click(sender As Screens.Button, e As System.EventArgs) Handles btConsultazioneGiacenze.Click
        'Dim K As New GiacenzaArticoloPF
        'Application.Show(K)
    End Sub

    Private Sub btEsci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btEsci.Click
        Me.Close()
    End Sub

End Class

