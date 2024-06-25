Imports System
Imports System.Text
Imports System.IO
Imports System.IO.Compression


Module Program

    Private br As BinaryReader
    Private des As String
    Private source As String
    Private buffer As Byte()
    Private subfiles As New List(Of FileData)()

    Sub Main(args As String())

        If args.Count = 0 Then
            Console.WriteLine("UnPack Tool - 2CongLc.Vn")
        Else
            source = args(0)
        End If

        If File.Exists(source) Then

            des = Path.GetDirectoryName(source) & "\" & Path.GetFileNameWithoutExtension(source) & "\"
            br = New BinaryReader(File.OpenRead(source))

            br.BaseStream.Position = 8

            subfiles.Add(New FileData)
            For i As Int32 = 0 To subfiles(0).size - 2
                subfiles.Add(New FileData)
            Next

            Dim unknow As Int32 = br.ReadInt32
            Dim dataoffs As Int64 = br.BaseStream.Position


            For Each fd As FileData In subfiles

                Console.WriteLine("File Offset : {0} - File Size : {1} - File Name : {2}", fd.offset, fd.size, fd._path)

                If fd.isFolder = 1 Then
                    Directory.CreateDirectory(des + fd._path)
                Else
                    br.BaseStream.Position = fd.offset + dataoffs
                    buffer = br.ReadBytes(fd.size)

                    Using bw As BinaryWriter = New BinaryWriter(File.Create(des & fd._path))
                        bw.Write(buffer)
                    End Using

                End If

            Next
            Console.WriteLine("Unpack Done!!!")

        End If
        Console.ReadLine()
    End Sub

    ' cấu trúc dữ liệu block
    Class FileData
        Public size As Int32 = br.ReadInt32
        Public isFolder As Byte = br.ReadByte
        Public name As String = New String(br.ReadChars(br.ReadInt32)).TrimEnd(ChrW(0))
        Public _path As String = New String(br.ReadChars(br.ReadInt32)).TrimEnd(ChrW(0))
        Public offset As Int32 = br.ReadInt32
    End Class

End Module
