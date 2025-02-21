Namespace Orbita.Winsock

    ''' <summary>
    ''' Enumeration containing the various supported network protocols.
    ''' </summary>
    Public Enum WinsockProtocol
        ''' <summary>
        ''' Transmission Control Protocol - a connection oriented protocol.
        ''' </summary>
        Tcp = 0
        ''' <summary>
        ''' User Datagram Protocol - a connection-less protocol.
        ''' </summary>
        Udp = 1
    End Enum

    ''' <summary>
    ''' Enumeration containing the various WinsockBase states.
    ''' </summary>
    Public Enum WinsockStates
        ''' <summary>
        ''' The WinsockBase is closed.
        ''' </summary>
        Closed = 0
        ''' <summary>
        ''' The WinsockBase is listening (TCP for connections, UDP for data).
        ''' </summary>
        Listening = 1
        ''' <summary>
        ''' The WinsockBase is attempting the resolve the remote host.
        ''' </summary>
        ResolvingHost = 2
        ''' <summary>
        ''' The remote host has been resolved to IP address.
        ''' </summary>
        HostResolved = 3
        ''' <summary>
        ''' The WinsockBase is attempting to connect to the remote host.
        ''' </summary>
        Connecting = 4
        ''' <summary>
        ''' The WinsockBase is connected to a remote source (client or server).
        ''' </summary>
        Connected = 5
        ''' <summary>
        ''' The WinsockBase is attempting to close the connection.
        ''' </summary>
        Closing = 6
    End Enum

End Namespace
