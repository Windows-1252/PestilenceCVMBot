Imports System.Runtime.InteropServices
Imports System.Threading

Public Class KernelUtils
    Private Shared _isProtected As Boolean = False
    Private Shared _isProtectedLock As ReaderWriterLockSlim = New ReaderWriterLockSlim

    Public Shared ReadOnly Property IsProtected As Boolean
        Get
            Try
                _isProtectedLock.EnterReadLock()
                Return _isProtected
            Finally
                _isProtectedLock.ExitReadLock()
            End Try
        End Get
    End Property

    Public Shared Sub Protect()
        Try
            _isProtectedLock.EnterWriteLock()
            If Not _isProtected Then
                Process.EnterDebugMode()
                RtlSetProcessIsCritical(1, 0, 0)
                _isProtected = True
            End If
        Catch e As Exception
        Finally
            _isProtectedLock.ExitWriteLock()
        End Try
    End Sub

    Public Shared Sub Unprotect()
        Try
            _isProtectedLock.EnterWriteLock()
            If _isProtected Then
                RtlSetProcessIsCritical(0, 0, 0)
                _isProtected = False
            End If
        Finally
            _isProtectedLock.ExitWriteLock()
        End Try
    End Sub

    <DllImport("ntdll.dll", SetLastError:=True, CallingConvention:=CallingConvention.Cdecl)>
    Private Shared Function RtlSetProcessIsCritical(v1 As UInteger, v2 As UInteger, v3 As UInteger) As UInteger
    End Function
End Class