Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports Microsoft.VisualBasic
Namespace Orbita.Winsock

    ''' <summary>
    ''' WinsockBase designer class provides designer time support for the WinsockBase component.
    ''' </summary>
    Public Class WinsockDesigner
        Inherits System.ComponentModel.Design.ComponentDesigner

        Private lists As DesignerActionListCollection

        ''' <summary>
        ''' Creates a new instance of the WinsockDesigner class.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Initializes this instance of the WinsockDesigner class.
        ''' </summary>
        ''' <param name="component">The base component of the designer.</param>
        Public Overrides Sub Initialize(ByVal component As System.ComponentModel.IComponent)
            MyBase.Initialize(component)
        End Sub

        ''' <summary>
        ''' Gets the Verb collection.
        ''' </summary>
        ''' <remarks>
        ''' The Verb collection is used to display links at the
        ''' bottom of the description in the Properties pane.
        ''' </remarks>
        Public Overrides ReadOnly Property Verbs() As System.ComponentModel.Design.DesignerVerbCollection
            Get
                Return New DesignerVerbCollection()
            End Get
        End Property

        ''' <summary>
        ''' Gets the Action list collection.
        ''' </summary>
        ''' <remarks>
        ''' The Action list collection is used for the Smart Tag
        ''' popup to provide easy access to various properties/actions.
        ''' </remarks>
        Public Overrides ReadOnly Property ActionLists() As DesignerActionListCollection
            Get
                If lists Is Nothing Then
                    lists = New DesignerActionListCollection()
                    lists.Add(New WinsockActionList(Me.Component))
                End If
                Return lists
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Provides the action list for the WinsockBase component during design time.
    ''' </summary>
    Public Class WinsockActionList
        Inherits DesignerActionList

        Private _wsk As OWinsockBase
        Private designerActionUISvc As DesignerActionUIService = Nothing

        ''' <summary>
        ''' Initializes a new instance of the WinsockActionList class.
        ''' </summary>
        ''' <param name="component">The component used in initialization.</param>
        Public Sub New(ByVal component As IComponent)
            MyBase.New(component)
            Me._wsk = CType(component, OWinsockBase)

            Me.designerActionUISvc = CType(GetService(GetType(DesignerActionUIService)), DesignerActionUIService)
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating if Legacy support should be used or not.
        ''' </summary>
        ''' <remarks>Legacy support is to support older winsock style connections.</remarks>
        Public Property LegacySupport() As Boolean
            Get
                Return _wsk.LegacySupport
            End Get
            Set(ByVal value As Boolean)
                GetPropertyByName("LegacySupport").SetValue(_wsk, value)
                Me.designerActionUISvc.Refresh(Me.Component)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the winsock protocol to use when communicating with the remote computer.
        ''' </summary>
        Public Property Protocol() As WinsockProtocol
            Get
                Return _wsk.Protocol
            End Get
            Set(ByVal value As WinsockProtocol)
                GetPropertyByName("Protocol").SetValue(_wsk, value)
                Me.designerActionUISvc.Refresh(Me.Component)
            End Set
        End Property

        ''' <summary>
        ''' Builds and retrieves the Action list itself.
        ''' </summary>
        Public Overrides Function GetSortedActionItems() As DesignerActionItemCollection
            Dim items As New DesignerActionItemCollection()

            ' create the headers
            items.Add(New DesignerActionHeaderItem("Appearance & Behavior"))
            items.Add(New DesignerActionHeaderItem("Events"))
            items.Add(New DesignerActionHeaderItem("About"))

            ' add the properties
            items.Add(New DesignerActionPropertyItem("LegacySupport", "Legacy Support", "Appearance & Behavior", "Enables legacy (VB6) send/receive support."))
            items.Add(New DesignerActionPropertyItem("Protocol", "Protocol", "Appearance & Behavior", "Specifies whether the component should use the TCP or UDP protocol."))

            ' add the events
            items.Add(New DesignerActionMethodItem(Me, "TriggerConnectedEvent", "Connected", "Events", "Takes you to the handler for the Connected event.", False))
            items.Add(New DesignerActionMethodItem(Me, "TriggerConnectionRequestEvent", "ConnectionRequest", "Events", "Takes you to the handler for the ConnectionRequest event.", False))
            items.Add(New DesignerActionMethodItem(Me, "TriggerDataArrivalEvent", "DataArrival", "Events", "Takes you to the handler for the DataArrival event.", False))
            items.Add(New DesignerActionMethodItem(Me, "TriggerDisconnectedEvent", "Disconnected", "Events", "Takes you to the handler for the Disconnected event.", False))
            items.Add(New DesignerActionMethodItem(Me, "TriggerErrorReceivedEvent", "ErrorReceived", "Events", "Takes you to the handler for the ErrorReceived event.", False))
            items.Add(New DesignerActionMethodItem(Me, "TriggerStateChangedEvent", "StateChanged", "Events", "Takes you to the handler for the StateChanged event.", False))

            ' add support items
            Dim ver As String = String.Format("{0}.{1}.{2}", My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build)
            items.Add(New DesignerActionMethodItem(Me, "ShowAbout", "About WinsockBase " & ver, "About", "Displays the about box.", False))
            items.Add(New DesignerActionMethodItem(Me, "LaunchWebSite", "Kolkman Koding Website", "About", "Opens the author's website.", False))

            Return items
        End Function

        ''' <summary>
        ''' Gets the property information by the given name.
        ''' </summary>
        ''' <param name="propName">The name of the property to get.</param>
        Private Function GetPropertyByName(ByVal propName As String) As PropertyDescriptor
            Dim prop As PropertyDescriptor
            prop = TypeDescriptor.GetProperties(_wsk)(propName)
            If prop Is Nothing Then
                Throw New ArgumentException("Invalid property.", propName)
            Else
                Return prop
            End If
        End Function

        ''' <summary>
        ''' Shows the about box.
        ''' </summary>
        Public Sub ShowAbout()
            Dim f As New frmAbout()
            f.ShowDialog()
        End Sub

        ''' <summary>
        ''' Launches the author's website.
        ''' </summary>
        Public Sub LaunchWebSite()
            System.Diagnostics.Process.Start("http://www.k-koding.com")
        End Sub

        Public Sub TriggerConnectedEvent()
            CreateAndShowEvent("Connected")
        End Sub
        Public Sub TriggerConnectionRequestEvent()
            CreateAndShowEvent("ConnectionRequest")
        End Sub
        Public Sub TriggerDataArrivalEvent()
            CreateAndShowEvent("DataArrival")
        End Sub
        Public Sub TriggerDisconnectedEvent()
            CreateAndShowEvent("Disconnected")
        End Sub
        Public Sub TriggerErrorReceivedEvent()
            CreateAndShowEvent("ErrorReceived")
        End Sub
        Public Sub TriggerStateChangedEvent()
            CreateAndShowEvent("StateChanged")
        End Sub

        Private Sub CreateAndShowEvent(ByVal eventName As String)
            Dim evService As IEventBindingService = CType(Me.Component.Site.GetService(GetType(System.ComponentModel.Design.IEventBindingService)), IEventBindingService)
            Dim ev As EventDescriptor = GetEvent(evService, eventName)
            If ev IsNot Nothing Then
                CreateEvent(evService, ev)
                Me.designerActionUISvc.HideUI(Me.Component)
                evService.ShowCode(Me.Component, ev)
            End If
        End Sub
        Private Sub CreateEvent(ByRef evService As IEventBindingService, ByVal ev As EventDescriptor)
            Dim epd As PropertyDescriptor = evService.GetEventProperty(ev)
            Dim strEventName As String = Me.Component.Site.Name & "_" & ev.Name
            Dim existing As Object = epd.GetValue(Me.Component)
            'Only create if there isn't already a handler
            If existing Is Nothing Then
                epd.SetValue(Me.Component, strEventName)
            End If
        End Sub

        Private Function GetEvent(ByRef evService As IEventBindingService, ByVal eventName As String) As EventDescriptor
            If evService Is Nothing Then Return Nothing
            ' Attempt to obtain a PropertyDescriptor for a 
            ' component event named "testEvent".
            Dim edc As EventDescriptorCollection = TypeDescriptor.GetEvents(Me.Component)
            If edc Is Nothing Or edc.Count = 0 Then
                Return Nothing
            End If
            Dim ed As EventDescriptor = Nothing
            ' Search for an event named "Connected".
            Dim edi As EventDescriptor
            For Each edi In edc
                If edi.Name = eventName Then
                    ed = edi
                    Exit For
                End If
            Next edi
            If ed Is Nothing Then
                Return Nothing
            End If
            Return ed
        End Function

    End Class

End Namespace

